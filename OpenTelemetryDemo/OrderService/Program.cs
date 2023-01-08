using System.Diagnostics.Metrics;
using Dal.Ado;
using Dal.Common;
using Logic;
using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Extensions.Hosting;

const string SERVICE_NAME = "OrderService";

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

var app = builder.Build();

ConfigureMiddleware(app, app.Environment);
ConfigureEndpoints(app);

// resolve this issue
app.Run();

// Add service to container
void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment env) {

  builder.Services.AddControllers(options => options.ReturnHttpNotAcceptable = true)
    .AddNewtonsoftJson();

  services.AddSingleton<IConnectionFactory, DefaultConnectionFactory>(x => (DefaultConnectionFactory)DefaultConnectionFactory.FromConfiguration("orderConnection"));
  services.AddSingleton<IOrderDao, AdoOrderDao>();
  services.AddSingleton<IOrderLogic, OrderLogic>();

  #region OpenTelemetrySetup

  services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder => {
      tracerProviderBuilder
        .AddJaegerExporter()
        .AddSource(SERVICE_NAME)
        .SetResourceBuilder(
          ResourceBuilder.CreateDefault()
            .AddService(serviceName: SERVICE_NAME))
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddNpgsql();
    });
  
  services.AddOpenTelemetry()
    .WithMetrics(builder => {
      builder
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(SERVICE_NAME))
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddRuntimeInstrumentation()
        .AddPrometheusExporter();
    });

    
    #endregion

  services.AddAuthorization();

  services.AddRouting(options => options.LowercaseUrls = true);
  
  services.AddOpenApiDocument(settings => settings.PostProcess = doc => doc.Info.Title = "Order API");
  
  services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
}

// Configure the HTTP request pipeline.
void ConfigureMiddleware(IApplicationBuilder app, IHostEnvironment env) {
  app.UseCors();

  app.UseHttpsRedirection();
  app.UseAuthorization();
  
  app.UseOpenApi();
  app.UseSwaggerUi3(settings => settings.Path = "/swagger/order");
}

// Configure the routing system
void ConfigureEndpoints(IEndpointRouteBuilder app) {
  app.MapControllers();
}
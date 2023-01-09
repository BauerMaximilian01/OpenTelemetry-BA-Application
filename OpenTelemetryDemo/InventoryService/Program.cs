using Dal.Ado;
using Dal.Common;
using InventoryService.Controllers;
using Logic;
using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

const string SERVICE_NAME = "InventoryService";

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

var app = builder.Build();

ConfigureMiddleware(app, app.Environment);
ConfigureEndpoints(app);

app.Run();

// Add service to container
void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment env) {

  var meters = new OtelMetrics(SERVICE_NAME);
  
  builder.Services.AddControllers(options => options.ReturnHttpNotAcceptable = true)
    .AddNewtonsoftJson();

  services.AddSingleton<IConnectionFactory, DefaultConnectionFactory>(x => (DefaultConnectionFactory)DefaultConnectionFactory.FromConfiguration("inventoryConnection"));
  services.AddSingleton<IInventoryDao, AdoInventoryDao>();
  services.AddSingleton<IInventoryLogic, InventoryLogic>();
  services.AddSingleton(meters);

  #region OpenTelemetrySetup

  services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder => {
      tracerProviderBuilder 
        .SetResourceBuilder(
          ResourceBuilder.CreateDefault()
            .AddService(serviceName: SERVICE_NAME, serviceVersion: "1.0.0"))
        .AddHttpClientInstrumentation()
        .AddSource(SERVICE_NAME)
        .AddAspNetCoreInstrumentation()
        .AddNpgsql()
        .AddJaegerExporter();
    })
    .WithMetrics(metricBuilder => {
      metricBuilder
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(SERVICE_NAME))
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddMeter(meters.meterName)
        .AddRuntimeInstrumentation()
        .AddPrometheusExporter();
    })
    .StartWithHost();

  #endregion
  
  services.AddAuthorization();

  services.AddRouting(options => options.LowercaseUrls = true);
  
  services.AddOpenApiDocument(settings => settings.PostProcess = doc => doc.Info.Title = "Inventory API");
  
  services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
}

// Configure the HTTP request pipeline.
void ConfigureMiddleware(IApplicationBuilder app, IHostEnvironment env) {
  app.UseOpenTelemetryPrometheusScrapingEndpoint(context => context.Connection.LocalPort == 1234);
  
  app.UseCors();
  
  app.UseAuthorization();
  
  app.UseOpenApi();
  app.UseSwaggerUi3(settings => settings.Path = "/swagger/inventory");
}

// Configure the routing system
void ConfigureEndpoints(IEndpointRouteBuilder app) {
  app.MapControllers();
}
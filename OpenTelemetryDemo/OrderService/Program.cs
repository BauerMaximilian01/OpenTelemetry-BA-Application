using Dal.Ado;
using Dal.Common;
using Logic;
using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

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

  var meters = new OtelMetrics(SERVICE_NAME);
  
  builder.Services.AddControllers(options => options.ReturnHttpNotAcceptable = true)
    .AddNewtonsoftJson();

  services.AddSingleton<IConnectionFactory, DefaultConnectionFactory>(x => (DefaultConnectionFactory)DefaultConnectionFactory.FromConfiguration("orderConnection"));
  services.AddSingleton<IOrderDao, AdoOrderDao>();
  services.AddSingleton<IOrderLogic, OrderLogic>();
  services.AddSingleton(meters);

  #region OpenTelemetrySetup

  services.AddOpenTelemetry()
    .WithMetrics(metricBuilder => {
      metricBuilder
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(SERVICE_NAME))
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddMeter(meters.meterName)
        .AddRuntimeInstrumentation()
        .AddView(
          instrumentName: "orders-price",
          new ExplicitBucketHistogramConfiguration { Boundaries = new double[] { 10, 15, 30, 45, 60, 75, 80 } })
        .AddView(
          instrumentName: "orders-number-of-products",
          new ExplicitBucketHistogramConfiguration { Boundaries = new double[] { 1, 2, 5 } })
        .AddPrometheusExporter();
    })
    .WithTracing(tracerProviderBuilder => {
      tracerProviderBuilder
        .SetResourceBuilder(
          ResourceBuilder.CreateDefault()
            .AddService(serviceName: SERVICE_NAME))
        .AddHttpClientInstrumentation()
        .AddSource(SERVICE_NAME)
        .AddAspNetCoreInstrumentation()
        .AddNpgsql()
        .AddJaegerExporter();
    })
    .StartWithHost();


    #endregion

  services.AddAuthorization();

  services.AddRouting(options => options.LowercaseUrls = true);
  
  services.AddOpenApiDocument(settings => settings.PostProcess = doc => doc.Info.Title = "Order API");
  
  services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
}

// Configure the HTTP request pipeline.
void ConfigureMiddleware(IApplicationBuilder app, IHostEnvironment env) {
  app.UseOpenTelemetryPrometheusScrapingEndpoint(context => context.Connection.LocalPort == 9464);
  
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
using Dal.Ado;
using Dal.Common;
using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders.Thrift;
using Logic;
using OpenTracing;
using OpenTracing.Util;
using Prometheus;


const string SERVICE_NAME = "InventoryService";

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

var app = builder.Build();

ConfigureMiddleware(app, app.Environment);
ConfigureEndpoints(app);

app.Run();

// Add service to container
void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment env) {

  var metrics = new PrometheusMetrics();
  
  builder.Services.AddControllers(options => options.ReturnHttpNotAcceptable = true)
    .AddNewtonsoftJson();

  services.AddSingleton<IConnectionFactory, DefaultConnectionFactory>(x => (DefaultConnectionFactory)DefaultConnectionFactory.FromConfiguration("inventoryConnection"));
  services.AddSingleton<IInventoryDao, AdoInventoryDao>();
  services.AddSingleton<IInventoryLogic, InventoryLogic>();
  services.AddSingleton(metrics);

  #region InstrumentationTracing

  services.AddSingleton(provider => {

    var reporter = new RemoteReporter.Builder()
      .WithSender(new HttpSender("http://localhost:14268/api/traces"))
      .Build();
    
    ITracer tracer = new Tracer.Builder(SERVICE_NAME)
      .WithReporter(reporter)
      .Build();

    GlobalTracer.Register(tracer);

    return tracer;
  });


  services.AddOpenTracing();
  #endregion
  
  services.AddAuthorization();

  services.AddRouting(options => options.LowercaseUrls = true);
  
  services.AddOpenApiDocument(settings => settings.PostProcess = doc => doc.Info.Title = "Inventory API");
  
  services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
}

// Configure the HTTP request pipeline.
void ConfigureMiddleware(IApplicationBuilder app, IHostEnvironment env) {

  app.UseCors();
  
  #region InstrumentationMetrics
  
  app.UseHttpMetrics();
  app.UseMetricServer();
  
  #endregion
  
  app.UseAuthorization();
  
  app.UseOpenApi();
  app.UseSwaggerUi3(settings => settings.Path = "/swagger/inventory");
}

// Configure the routing system
void ConfigureEndpoints(IEndpointRouteBuilder app) {
  app.MapControllers();
}
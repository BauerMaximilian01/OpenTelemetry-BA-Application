using Dal.Ado;
using Dal.Common;
using Logic;
using Microsoft.Extensions.Options;
using Prometheus;

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

  var metrics = new PrometheusMetrics();
  
  builder.Services.AddControllers(options => options.ReturnHttpNotAcceptable = true)
    .AddNewtonsoftJson();

  services.AddSingleton<IConnectionFactory, DefaultConnectionFactory>(x => (DefaultConnectionFactory)DefaultConnectionFactory.FromConfiguration("orderConnection"));
  services.AddSingleton<IOrderDao, AdoOrderDao>();
  services.AddSingleton<IOrderLogic, OrderLogic>();
  services.AddSingleton(metrics);

  services.AddAuthorization();

  services.AddRouting(options => options.LowercaseUrls = true);
  
  services.AddOpenApiDocument(settings => settings.PostProcess = doc => doc.Info.Title = "Order API");
  
  services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
}

// Configure the HTTP request pipeline.
void ConfigureMiddleware(IApplicationBuilder app, IHostEnvironment env) {
  app.UseCors();

  #region PrometheusInstrumentation
  
  app.UseHttpMetrics();
  app.UseMetricServer();
  
  #endregion

  app.UseHttpsRedirection();
  app.UseAuthorization();

  app.UseOpenApi();
  app.UseSwaggerUi3(settings => settings.Path = "/swagger/order");
}

// Configure the routing system
void ConfigureEndpoints(IEndpointRouteBuilder app) {
  app.MapControllers();
}
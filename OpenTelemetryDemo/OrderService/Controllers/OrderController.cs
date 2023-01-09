using CaaS.Api.Controllers;
using Domain;
using Logic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace OpenTelemetryDemo.Controllers;

[Route("[controller]")]
[ApiController]
[EnableCors]
[ApiConventionType(typeof(WebApiConventions))]
public class OrderController : ControllerBase {
  private readonly IOrderLogic logic;
  private PrometheusMetrics metrics;
  
  public OrderController(IOrderLogic logic, PrometheusMetrics metrics) {
    this.logic = logic;
    this.metrics = metrics;
  }

  [HttpGet("/orders")]
  public async Task<ActionResult<IEnumerable<Order?>>> GetOrders() {
    return Ok(await logic.GetAllOrders());
  }

  [HttpGet("/orders/{orderId}")]
  public async Task<ActionResult<Order?>> GetOrder(int orderId) {
    var order = await logic.GetOrderById(orderId);
    if (order is not null) {
      return Ok(await logic.GetOrderById(orderId));  
    }
    
    return NotFound("Order not found");
  }
  
  [HttpPost("/orders")]
  public async Task<ActionResult<Order?>> CreateOrder([FromBody] Order order) {
    if (await logic.CreateOrderAsync(order)) {
      var result = await logic.GetOrderById(order.Id);

      if (result is not null) {
        metrics.TotalOrdersInc();
        
        metrics.OrdersPriceHistogram.Observe(order.Total);
        metrics.NumberOfFilmsPerOrder.Observe(order.Quantity);
        
        return CreatedAtAction(
          actionName: nameof(GetOrder),
          routeValues: new { orderId = result.Id },
          value: result
        );
      }
    }
    
    return Conflict("Not enough in stock");
  }
  
  [HttpDelete("/orders/{orderId}")]
  public async Task<ActionResult<bool>> DeleteOrder(int orderId) {
    if (await logic.DeleteOrder(orderId)) {
      metrics.TotalOrdersDec();
      return NoContent();
    }
    
    return NotFound("Order not found");
  }
}
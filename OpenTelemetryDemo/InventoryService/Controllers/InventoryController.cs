using CaaS.Api.Controllers;
using Domain;
using Logic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Controllers;

[Route("[controller]")]
[ApiController]
[EnableCors]
[ApiConventionType(typeof(WebApiConventions))]
public class InventoryController : ControllerBase {
  private readonly IInventoryLogic logic;
  
  public InventoryController(IInventoryLogic logic) {
    this.logic = logic;
  }

  [HttpGet("/verify/{productId}/{quantity}")]
  public async Task<ActionResult<bool>> VerifyProduct(int productId, int quantity) {
    if (await logic.VerifyItem(productId, quantity)) {
      return Ok();
    }

    return Conflict();
  }

  [HttpGet("/products/{productId}")]
  public async Task<ActionResult<Product?>> GetProduct(int productId) {
    return await logic.GetProduct(productId);
  }
  
  [HttpGet("/products")]
  public async Task<ActionResult<IEnumerable<InventoryProduct>>> GetProducts() {
    return Ok(await logic.GetProducts()); 
  }
  
  [HttpPost("/products/{productId}/{quantity}")]
  public async Task<ActionResult<bool>> ClaimProduct(int productId, int quantity) {
    if (await logic.ClaimProduct(productId, quantity)) {
      return Ok();
    }
    
    return Conflict();
  }
}
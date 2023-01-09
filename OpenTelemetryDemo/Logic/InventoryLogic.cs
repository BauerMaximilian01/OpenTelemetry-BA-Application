using Dal.Ado;
using Domain;
using Logic.Extensions;

namespace Logic; 

public class InventoryLogic : IInventoryLogic {
  private readonly IInventoryDao inventoryDao;
  private OtelMetrics meter;
  
  public InventoryLogic(IInventoryDao inventoryDao, OtelMetrics meter) {
    this.inventoryDao = inventoryDao;
    this.meter = meter;
  }
  
  public async Task<bool> VerifyItem(int productId, int quantity) {
    var product = await inventoryDao.GetProduct(productId);
    if (product is not null) {
      return product.AvailableQuantity >= quantity;
    }

    return false;
  }

  public async Task<Product?> GetProduct(int productId) {
    var product = await inventoryDao.GetProduct(productId);
    return product is not null ? product.ToProduct() : null;
  }
  
  public async Task<IEnumerable<InventoryProduct?>> GetProducts() {
    var products = await inventoryDao.GetProducts();
    return products.OrderBy(x => x.Name);
  }

  public async Task<bool> ClaimProduct(int productId, int quantity) {
    var product = await inventoryDao.GetProduct(productId);
    if (product is not null) {
      if (product.AvailableQuantity >= quantity) {
        
        meter.DecrementTotalInventory(quantity);
        meter.UpdateProduct();
        
        product.AvailableQuantity -= quantity;
        await inventoryDao.UpdateProduct(product);
        return true;
      }
    }

    return false;
  }
}
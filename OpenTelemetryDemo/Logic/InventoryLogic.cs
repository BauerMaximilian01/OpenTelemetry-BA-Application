using Dal.Ado;
using Domain;
using Logic.Extensions;

namespace Logic; 

public class InventoryLogic : IInventoryLogic {
  private readonly IInventoryDao inventoryDao;
  private PrometheusMetrics metrics;
  
  public InventoryLogic(IInventoryDao inventoryDao, PrometheusMetrics metrics) {
    this.inventoryDao = inventoryDao;
    this.metrics = metrics;
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
        metrics.UpdatedProductsInc();
        metrics.TotalInventoryDec(quantity);
        
        product.AvailableQuantity -= quantity;
        await inventoryDao.UpdateProduct(product);
        return true;
      }
    }

    return false;
  }
}
using Domain;

namespace Logic; 

public interface IInventoryLogic {
  Task<bool> VerifyItem(int productId, int quantity);
  Task<Product?> GetProduct(int productId);
  Task<IEnumerable<InventoryProduct?>> GetProducts();
  Task<bool> ClaimProduct(int productId, int quantity);
}
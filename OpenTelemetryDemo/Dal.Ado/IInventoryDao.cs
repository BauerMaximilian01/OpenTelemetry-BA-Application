using Domain;

namespace Dal.Ado; 

public interface IInventoryDao {
  Task<InventoryProduct?> GetProduct(int id);
  Task<IEnumerable<InventoryProduct>> GetProducts();
  Task<bool> UpdateProduct(InventoryProduct product);
}
using System.Data;
using Dal.Common;
using Domain;

namespace Dal.Ado; 

public class AdoInventoryDao : IInventoryDao {
  private readonly AdoTemplate template;
  private string LastInsertIdQuery { get; }

  public AdoInventoryDao(IConnectionFactory connectionFactory) {
    this.template = new AdoTemplate(connectionFactory);
  }
  
  public async Task<InventoryProduct?> GetProduct(int id) {
    return await template.QuerySingleAsync("SELECT * FROM product WHERE product_id = @id", MapRowToInventoryProduct, new QueryParameter("@id", id));
  }

  public async Task<IEnumerable<InventoryProduct>> GetProducts() {
    return await template.QueryAsync("SELECT * FROM product", MapRowToInventoryProduct);
  }

  public async Task<bool> UpdateProduct(InventoryProduct product) {
    return (await template.ExecuteAsync("UPDATE product SET name=@name, price=@price, available_quantity=@aq WHERE product_id = @id", 
      new QueryParameter("@name", product.Name),
      new QueryParameter("@price", product.Price),
      new QueryParameter("@aq", product.AvailableQuantity),
      new QueryParameter("@id", product.Id))) == 1;
  }
  
  private InventoryProduct MapRowToInventoryProduct(IDataRecord row) {
    return new InventoryProduct((int)row["product_id"], (string)row["name"], Decimal.ToDouble((decimal)row["price"]), (int)row["available_quantity"]);
  }
}
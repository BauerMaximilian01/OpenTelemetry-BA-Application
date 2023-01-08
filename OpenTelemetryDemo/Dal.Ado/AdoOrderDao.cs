using System.Data;
using Dal.Common;
using Domain;

namespace Dal.Ado;

public class AdoOrderDao : IOrderDao {
  private readonly AdoTemplate template;
  private string LastInsertIdQuery { get; }

  public AdoOrderDao(IConnectionFactory connectionFactory) {
    this.template = new AdoTemplate(connectionFactory);
    LastInsertIdQuery = "SELECT MAX(order_id) FROM orders";
  }
  
  public async Task CreateOrder(Order order) {
    const string INSERT = "INSERT INTO orders(order_date, username, product_id, quantity, total) " +
                          "VALUES (@date, @username, @product_id, @quantity, @total)";

    order.Id = Convert.ToInt32(await template.ExecuteScalarAsync<IConvertible>($"{INSERT}; {LastInsertIdQuery}", 
      new QueryParameter("@date", order.Date),
      new QueryParameter("@username", order.Username), new QueryParameter("@product_id", order.Product.Id),
      new QueryParameter("@quantity", order.Quantity), new QueryParameter("@total", order.Total)));
  }

  // c.Id = Convert.ToInt32(await template.ExecuteScalarAsync<IConvertible>($"{SQLINSERT};{LastInsertIdQuery}",
  // new QueryParameter("@fn", c.FirstName), new QueryParameter("@ln", c.LastName),
  // new QueryParameter("@mail", c.EMail), new QueryParameter("@shop", c.ShopName)));
  
  public async Task<Order?> GetOrder(int orderId) {
    return await template.QuerySingleAsync("SELECT * FROM orders WHERE order_id = @id", MapRowToOrder, new QueryParameter("@id", orderId));
  }

  public async Task<bool> DeleteOrder(int orderId) {
    return (await template.ExecuteAsync("DELETE FROM orders WHERE order_id = @id", new QueryParameter("@id", orderId))) == 1;
  }

  public async Task<IEnumerable<Order?>> GetOrders() {
    return await template.QueryAsync("SELECT * FROM orders", MapRowToOrder);
  }

  private Order MapRowToOrder(IDataRecord row) {
    return new Order((int)row["order_id"], (DateTime)row["order_date"], (string)row["username"],
      new Product((int)row["product_id"], "", 0), (int)row["quantity"], Decimal.ToDouble((decimal)row["total"]));
  }
}
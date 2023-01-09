using Domain;

namespace Dal.Ado;

public interface IOrderDao {
  Task CreateOrder(Order order);
  Task<Order?> GetOrder(int orderId);
  Task<bool> DeleteOrder(int orderId);
  Task<IEnumerable<Order?>> GetOrders();
}
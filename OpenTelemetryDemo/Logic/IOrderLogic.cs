using Domain;

namespace Logic; 

public interface IOrderLogic {
  Task<bool> CreateOrderAsync(Order order);
  Task<Order?> GetOrderById(int id);
  Task<IEnumerable<Order?>> GetAllOrders();
  Task<bool> DeleteOrder(int id);
}
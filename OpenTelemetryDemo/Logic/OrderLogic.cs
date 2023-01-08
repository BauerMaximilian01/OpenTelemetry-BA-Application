using System.Net;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Dal.Ado;
using Domain;

namespace Logic;

public class OrderLogic : IOrderLogic {
  private readonly IOrderDao orderDao;
  private const string URL = "https://localhost:7153/";
  
  public OrderLogic(IOrderDao orderDao) {
    this.orderDao = orderDao;
  }
  
  public async Task<bool> CreateOrderAsync(Order order) {
    if (await VerifyProductQuantity(order.Product.Id, order.Quantity)) {
      var product = await GetProductFromInventory(order.Product.Id);
      await ClaimProduct(order.Product.Id, order.Quantity);
      int prevId = order.Id;

      order.Total = product.Price * order.Quantity;
      order.Product = product;
      await orderDao.CreateOrder(order);
      
      return prevId != order.Id;
    }

    return false;
  }

  public async Task<Order?> GetOrderById(int id) {
    var order = await orderDao.GetOrder(id);
    if (order is not null) {
      order.Product = await GetProductFromInventory(order.Product.Id) ?? new Product(0, "Not Found", 0.0);
    }
    
    return order;
  }

  public async Task<IEnumerable<Order?>> GetAllOrders() {
    var orders = await orderDao.GetOrders();
    
      foreach (var order in orders) {
        if (order is not null) {
          order.Product = await GetProductFromInventory(order.Product.Id) ?? new Product(0, "Not Found", 0.0);
        }
      }

      return orders;
  }
  
  public async Task<bool> DeleteOrder(int id) {
    return await orderDao.DeleteOrder(id);
  }

  private async Task<bool> VerifyProductQuantity(int id, int quantity) {
    using (var client = new HttpClient()) {
      client.BaseAddress = new Uri($"{URL}");
      client.DefaultRequestHeaders.Accept.Clear();
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      
      var response = await client.GetAsync($"verify/{id}/{quantity}");
      return response.IsSuccessStatusCode;
    }
  }

  private async Task<Product?> GetProductFromInventory(int id) {
    using (var client = new HttpClient()) {
      client.BaseAddress = new Uri($"{URL}");
      client.DefaultRequestHeaders.Accept.Clear();
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

      var response = await client.GetAsync($"products/{id}");
      var content = await response.Content.ReadAsStringAsync();
      return JsonSerializer.Deserialize<Product>(content,
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
  }

  private async Task<bool> ClaimProduct(int id, int quantity) {
    using (var client = new HttpClient()) {
      client.BaseAddress = new Uri($"{URL}");
      client.DefaultRequestHeaders.Accept.Clear();
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

      var response = await client.PostAsync($"products/{id}/{quantity}", null);
      return response.IsSuccessStatusCode;
    }
  }
}
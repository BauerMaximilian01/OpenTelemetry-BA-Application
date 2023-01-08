namespace Domain; 

[Serializable]
public class InventoryProduct {
  public InventoryProduct(int id, string name, double price, int quantity) {
    Id = id;
    Name = name;
    Price = price;
    AvailableQuantity = quantity;
  }

  public int Id { get; set; }
  public string Name { get; set; }
  public double Price { get; set; }
  public int AvailableQuantity { get; set; }
}
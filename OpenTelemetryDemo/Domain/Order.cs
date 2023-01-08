namespace Domain;

[Serializable]
public class Order {
    public Order(int id, DateTime date, string name, Product product, int quantity, double total) {
        Id = id;
        Date = date;
        Username = name;
        Product = product;
        Quantity = quantity;
        Total = total;
    }
    
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Username { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public double Total { get; set; }
}
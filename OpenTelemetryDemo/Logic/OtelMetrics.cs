using System.Diagnostics.Metrics;

namespace Logic; 

public class OtelMetrics {
  public string meterName { get; }
  
  private int totalOrders = 0;
  private int totalInventory = 0;
  private ObservableGauge<int> TotalOrdersGauge { get; }
  private Histogram<int> NumberOfFilmsPerOrder { get; }
  private ObservableGauge<int> TotalInventoryCount { get; }
  private Counter<int> ProductsUpdated { get; }
  private Histogram<double> OrdersPriceHistogramm { get; }


  public OtelMetrics(string meterName) {
    var meter = new Meter(meterName, "1.0.0");
    this.meterName = meterName;

    if (this.meterName.Contains("Order")) {
      totalOrders = 22;
      TotalOrdersGauge =
        meter.CreateObservableGauge<int>("total_orders", () => new[] { new Measurement<int>(totalOrders) });
      NumberOfFilmsPerOrder =
        meter.CreateHistogram<int>("orders-number-of-products", "Product", "Number of products per order");
      OrdersPriceHistogramm = meter.CreateHistogram<double>("orders-price", "Euros", "Price distribution of film orders");
    }
    else {
      totalInventory = 21;
      TotalInventoryCount =
        meter.CreateObservableGauge<int>("total_inventory_count", () => new[] { new Measurement<int>(totalInventory) });
      ProductsUpdated = meter.CreateCounter<int>("products-updated", "Product");
    }

  }
  
  public void IncrementTotalOrders() => totalOrders++;
  public void DecrementTotalOrders() => totalOrders--;
  public void DecrementTotalInventory(int amount) => totalInventory -= amount;
  public void UpdateProduct() => ProductsUpdated.Add(1); 

  public void RecordNumberOfFilms(int amount) => NumberOfFilmsPerOrder.Record(amount);
  public void RecordOrderPrice(double price) => OrdersPriceHistogramm.Record(price);
}
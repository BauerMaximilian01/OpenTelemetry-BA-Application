using Prometheus;
using Metrics = Prometheus.Metrics;

namespace Logic; 

public class PrometheusMetrics {
  private readonly Gauge UpdatedProducts = Metrics.CreateGauge("updated_products_count", "Number of updated products.");
  private readonly Gauge TotalOrdersGauge = Metrics.CreateGauge("total_orders_gauge", "Number of total Orders.");
  private readonly Gauge TotalInventoryGauge = Metrics.CreateGauge("total_inventory_gauge", "Number of total Inventory Quantity.");
  public readonly Histogram OrdersPriceHistogram = Metrics
    .CreateHistogram("orders_price_histogram", "Histogram of prices of orders.",
      new HistogramConfiguration
      {
        Buckets = Histogram.LinearBuckets(start: 10, width: 5, count: 7)
      });
  
  public readonly Histogram NumberOfFilmsPerOrder = Metrics
    .CreateHistogram("number_films_histogram", "Histogram for number of products per order.",
      new HistogramConfiguration
      {
        Buckets = Histogram.LinearBuckets(start: 1, width: 2, count: 3)
      });

  public PrometheusMetrics() {
    TotalOrdersGauge.Set(22);
    TotalInventoryGauge.Set(21);
  }
  
  public void UpdatedProductsInc() => UpdatedProducts.Inc();

  public void TotalOrdersInc() => TotalOrdersGauge.Inc();
  public void TotalOrdersDec() => TotalOrdersGauge.Dec();
  
  public void TotalInventoryDec(int amount) => TotalInventoryGauge.Dec(amount);
}
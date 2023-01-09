using Domain;

namespace Logic.Extensions; 

public static class InventoryProductExtension {
  public static Product ToProduct(this InventoryProduct inventoryProduct) {
    return new Product(inventoryProduct.Id, inventoryProduct.Name, inventoryProduct.Price);
  }
}
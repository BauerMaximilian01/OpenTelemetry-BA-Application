using Microsoft.AspNetCore.Mvc;

namespace CaaS.Api.Controllers;
public static class StatusInfo {
  public static ProblemDetails CustomerAlreadyExists(string customerMail)  =>
    new ProblemDetails { Title = "Conflicting customer E-Mail", Detail = $"Customer with E-Mail {customerMail} already exists." };

  public static ProblemDetails InvalidCustomerIdMail(string customerIdMail) =>
    new ProblemDetails { Title = "Invalid customer ID or E-Mail", Detail = $"Customer ID or E-Mail {customerIdMail} does not exist." };
  
  public static ProblemDetails InvalidCustomerOrOrder(int cid, int oid) =>
    new ProblemDetails { Title = "Invalid customer or order", Detail = $"Customer {cid} or order {oid} does not exist." };
  
  public static ProblemDetails InvalidCartOrInvalidCustomer(string mail, int cartId) =>
    new ProblemDetails { Title = "Invalid cart or invalid customer", Detail = $"Customer {mail} does not exist or cart {cartId} is invalid." };

  public static ProblemDetails InvalidCart(int cartId) =>
    new ProblemDetails { Title = "Invalid cart ID", Detail = $"Cart {cartId} does not exist" };

  public static ProblemDetails InvalidCartItem(int cartId) =>
    new ProblemDetails{ Title = "Invalid Cart Item", Detail = $"Cart {cartId} does not contain the specified item." };
  
  public static ProblemDetails InvalidProductId(int productId) =>
    new ProblemDetails{ Title = "Invalid Product ID", Detail = $"Product {productId} does not exist." };
  
  public static ProblemDetails InvalidShopName(string shopName) =>
    new ProblemDetails{ Title = "Invalid Shop name", Detail = $"Shop {shopName} does not exist." };
}
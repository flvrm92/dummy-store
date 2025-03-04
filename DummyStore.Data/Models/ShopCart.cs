﻿
namespace DummyStore.Data.Models;
public class ShopCart : BaseEntity
{
  public Guid UserId { get; set; }
  public User? User { get; set; }

  public Guid ProductId { get; set; }
  public Product? Product { get; set; }

  public int Quantity { get; set; }
}

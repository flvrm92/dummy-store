
namespace DummyStore.Data.Models;

public class Product : BaseEntity
{
  public string Name { get; set; } = string.Empty;
  public double Price { get; set; } = 0f;
}

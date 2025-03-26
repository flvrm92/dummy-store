
namespace DummyStore.Data.Models;

public sealed class Product : BaseEntity
{
  public string Name { get; set; } = string.Empty;
  public double Price { get; set; } = 0f;
  public List<string> Tags { get; set; } = [];

}

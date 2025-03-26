
namespace DummyStore.Data.Events;

public class ShoppingCart(Guid id, Guid userId)
{
  public Guid Id { get; private set; } = id;
  public Guid UserId { get; private set; } = userId;

  public Dictionary<Guid, int> Products { get; private set; } = [];
  public bool IsCheckedOut { get; private set; }

  public void Apply(IShoppingCartEvent @event) 
  {
    switch (@event)
    {
      case ShoppingCartCreated e:
        Apply(e);
        break;
      case ProductAdded e:
        Apply(e);
        break;
      case ProductRemoved e:
        Apply(e);
        break;
      case ShoppingCartCheckedOut e:
        Apply(e);
        break;
      default:
        throw new InvalidOperationException("Unknown event");
    }
  }

  private void Apply(ShoppingCartCreated e)
  {
    Id = e.Id;
    UserId = e.UserId;
  }

  private void Apply(ProductAdded e)
  {
    if (Products.ContainsKey(e.ProductId))
      Products[e.ProductId] += e.Quantity;
    else
      Products[e.ProductId] = e.Quantity;
  }

  private void Apply(ProductRemoved e)
  {
    if (Products.TryGetValue(e.ProductId, out var quantity) && quantity > e.Quantity)
      Products[e.ProductId] -= e.Quantity;
    else
      Products.Remove(e.ProductId);
  }

  private void Apply(ShoppingCartCheckedOut e)
  {
    IsCheckedOut = true;
  }
}

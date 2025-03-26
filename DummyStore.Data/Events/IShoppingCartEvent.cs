
namespace DummyStore.Data.Events;

public interface IShoppingCartEvent
{
  Guid Id { get; }
  Guid UserId { get; }
  DateTime Timestamp { get; }
}

public sealed record ShoppingCartCreated(Guid Id, Guid UserId, DateTime Timestamp) : IShoppingCartEvent;
public sealed record ProductAdded(Guid Id, Guid UserId, Guid ProductId, int Quantity, DateTime Timestamp) : IShoppingCartEvent;

public sealed record ProductRemoved(Guid Id, Guid UserId, Guid ProductId, int Quantity, DateTime Timestamp) : IShoppingCartEvent;

public sealed record ShoppingCartCheckedOut(Guid Id, Guid UserId, DateTime Timestamp) : IShoppingCartEvent;

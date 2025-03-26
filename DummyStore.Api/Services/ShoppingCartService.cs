using DummyStore.Api.Extensions;
using DummyStore.Data.Events;
using Marten;

namespace DummyStore.Api.Services;

public class ShoppingCartService(IDocumentSession session) : IShoppingCartService
{
  public async Task AddProduct(Guid Id, Guid UserId, Guid ProductId, int Quantity)
  {
    var @event = new ProductAdded(Id, UserId, ProductId, Quantity, DateTime.UtcNow);
    session.Events.Append(Id, @event);
    await session.SaveChangesAsync();
  }

  public async Task CheckoutCart(Guid Id, Guid UserId)
  {
    var @event = new ShoppingCartCheckedOut(Id, UserId, DateTime.UtcNow);
    session.Events.Append(Id, @event);
    await session.SaveChangesAsync();
  }

  public async Task CreateCart(Guid Id, Guid UserId)
  {
    var @event = new ShoppingCartCreated(Id, UserId, DateTime.UtcNow);
    
    await session.Add<ShoppingCart>(Id, @event);    
    await session.SaveChangesAsync();
  }

  public async Task RemoveProduct(Guid Id, Guid UserId, Guid ProductId, int Quantity)
  {
    var @event = new ProductRemoved(Id, UserId, ProductId, Quantity, DateTime.UtcNow);
    session.Events.Append(Id, @event);
    await session.SaveChangesAsync();
  }

  public async Task<ShoppingCart?> GetCart(Guid id)
  {
    var events = await session.Events.FetchStreamAsync(id);
    if (events is null or { Count: 0 }) return null;

    var cart = new ShoppingCart(id, Guid.Empty);
    foreach (var @event in events)
      cart.Apply((IShoppingCartEvent)@event.Data);

    return cart;
  }
}

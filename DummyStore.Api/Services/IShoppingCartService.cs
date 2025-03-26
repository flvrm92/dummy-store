using DummyStore.Api.Dtos;
using DummyStore.Data.Events;

namespace DummyStore.Api.Services;

public interface IShoppingCartService
{
  Task CreateCart(Guid Id, Guid UserId);
  Task AddProduct(Guid Id, Guid UserId, Guid ProductId, int Quantity);
  Task RemoveProduct(Guid Id, Guid UserId, Guid ProductId, int Quantity);
  Task CheckoutCart(Guid Id, Guid UserId);
  Task<ShoppingCart?> GetCart(Guid id);
}

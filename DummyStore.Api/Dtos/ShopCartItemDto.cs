namespace DummyStore.Api.Dtos;

public sealed record ShopCartItemDto (Guid Id, Guid ShopCartId, Guid ProductId, int Quantity);

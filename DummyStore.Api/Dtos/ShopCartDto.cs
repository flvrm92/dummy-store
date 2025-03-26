namespace DummyStore.Api.Dtos;

public sealed record ShopCartDto(Guid Id, Guid UserId);
public sealed record ShopCartProductDto(Guid Id, Guid UserId, Guid ProductId, int Quantity);

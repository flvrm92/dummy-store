using DummyStore.Api.Dtos;
using DummyStore.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DummyStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShopCartController(IShoppingCartService shoppingCartService) : Controller
{

  [HttpGet("{id}")]
  public async Task<ActionResult> GetById(Guid id)
  {
    var cart = await shoppingCartService.GetCart(id);
    return cart is not null ? Ok(cart) : NotFound();
  }

  [HttpPost]
  public async Task<ActionResult> Create(ShopCartDto input)
  {
    if (input is null) return BadRequest();

    await shoppingCartService.CreateCart(input.Id, input.UserId);
    return Created();
  }

  [HttpPut("AddProduct")]
  public async Task<IActionResult> AddProduct(ShopCartProductDto input)
  {
    await shoppingCartService.AddProduct(input.Id, input.UserId, input.ProductId, input.Quantity);
    return Ok();
  }

  [HttpDelete("RemoveProduct")]
  public async Task<IActionResult> RemoveProduct(ShopCartProductDto input)
  {
    await shoppingCartService.RemoveProduct(input.Id, input.UserId, input.ProductId, input.Quantity);
    return Ok();
  }

  [HttpPost("Checkout")]
  public async Task<IActionResult> Checkout(ShopCartDto input)
  {
    await shoppingCartService.CheckoutCart(input.Id, input.UserId);
    return Ok();
  }  
}

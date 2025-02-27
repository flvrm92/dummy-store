using DummyStore.Data.Context;
using DummyStore.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DummyStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShopCartController(DummyStoreContext context) : Controller
{
  [HttpGet]
  public async Task<ActionResult<IReadOnlyCollection<ShopCart>>> GetAll()
   => Ok(await context.ShopCarts.ToListAsync());

  [HttpGet("{id}")]
  public async Task<ActionResult<ShopCart>> GetById(Guid id)
  {
    var shopCart = await context.ShopCarts.FindAsync(id);
    if (shopCart == null)
      return NotFound();
    return Ok(shopCart);
  }

  [HttpPost]
  public async Task<ActionResult<ShopCart>> Create(ShopCart shopCart)
  {
    await context.ShopCarts.AddAsync(shopCart);
    await context.SaveChangesAsync();
    return CreatedAtAction(nameof(GetById), new { id = shopCart.Id }, shopCart);
  }
}

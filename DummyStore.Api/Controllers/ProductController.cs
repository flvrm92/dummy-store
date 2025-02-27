using DummyStore.Data.Context;
using DummyStore.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DummyStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController (DummyStoreContext context) : Controller
{
  [HttpGet]
  public async Task<ActionResult<IReadOnlyCollection<Product>>> GetAll()
   => Ok(await context.Products.ToListAsync());

  [HttpGet("{id}")]
  public async Task<ActionResult<Product>> GetById(Guid id)
  {
    var product = await context.Products.FindAsync(id);
    if (product == null)
      return NotFound();
    return Ok(product);
  }

  [HttpPost]
  public async Task<ActionResult<Product>> Create(Product product)
  {
    await context.Products.AddAsync(product);
    await context.SaveChangesAsync();
    return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
  }  
}

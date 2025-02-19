using DummyStore.Data.Context;
using DummyStore.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DummyStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(DummyStoreContext context) : Controller
{
  [HttpGet]
  public async Task<ActionResult<IReadOnlyCollection<User>>> GetAll()
  {
    return Ok(await context.Users.ToListAsync());
  }


  [HttpGet("{id}")]
  public async Task<ActionResult<User>> GetById(int id)
  {
    var user = await context.Users.FindAsync(id);
    if (user == null)
      return NotFound();
    return Ok(user);
  }

  [HttpPost]
  public async Task<ActionResult<User>> Create(User user)
  {
    await context.Users.AddAsync(user);
    await context.SaveChangesAsync();
    return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
  }
}

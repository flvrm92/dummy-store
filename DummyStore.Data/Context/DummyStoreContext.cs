using DummyStore.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DummyStore.Data.Context;
public class DummyStoreContext(DbContextOptions<DummyStoreContext> options) : DbContext(options)
{
  public DbSet<Product> Products { get; set; }
  public DbSet<User> Users { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Product>()
      .ToTable("Product")
      .HasKey(p => p.Id);

    modelBuilder.Entity<User>()
      .ToTable("User")
      .HasKey(u => u.Id);

    base.OnModelCreating(modelBuilder);
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    if (optionsBuilder.IsConfigured)
      return;    
  }  
}

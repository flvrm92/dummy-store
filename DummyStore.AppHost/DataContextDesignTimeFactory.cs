using DummyStore.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DummyStore.AppHost;
public sealed class DataContextDesignTimeFactory : IDesignTimeDbContextFactory<DummyStoreContext>
{
  public DummyStoreContext CreateDbContext(string[] args)
  {
    var optionsBuilder = new DbContextOptionsBuilder<DummyStoreContext>();
    optionsBuilder.UseNpgsql("dummystore");

    return new DummyStoreContext(optionsBuilder.Options);
  }
}

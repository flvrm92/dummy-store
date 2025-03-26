using DummyStore.Data.Context;
using DummyStore.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics;

namespace DummyStore.MigrationService;

public class Worker(IServiceProvider serviceProvider,
  IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
  public const string ActivitySourceName = "DummyStore.MigrationService";
  private static readonly ActivitySource ActivitySource = new(ActivitySourceName);

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    using var activity = ActivitySource.StartActivity("MigrationService", ActivityKind.Client);
    try
    {
      using var scope = serviceProvider.CreateScope();
      var context = scope.ServiceProvider.GetRequiredService<DummyStoreContext>();

      await EnsureDatabaseAsync(context, stoppingToken);
      await RunMigrationAsync(context, stoppingToken);
      await SeedDataAsync(context, stoppingToken);
    }
    catch (Exception ex)
    {
      activity?.AddException(ex);
      throw;
    }

    hostApplicationLifetime.StopApplication();
  }

  private static async Task EnsureDatabaseAsync(DummyStoreContext context, CancellationToken cancellationToken)
  {
    var dbCreator = context.GetService<IRelationalDatabaseCreator>();

    var strategy = context.Database.CreateExecutionStrategy();
    await strategy.ExecuteAsync(async () =>
    {
      if (!await dbCreator.ExistsAsync(cancellationToken))
        await dbCreator.CreateAsync(cancellationToken);
    });
  }

  private static async Task RunMigrationAsync(DummyStoreContext context, CancellationToken cancellationToken)
  {
    var strategy = context.Database.CreateExecutionStrategy();
    await strategy.ExecuteAsync(async () =>
    {
      await context.Database.MigrateAsync(cancellationToken);
    });
  }

  private static async Task SeedDataAsync(DummyStoreContext context, CancellationToken cancellationToken)
  {
    var strategy = context.Database.CreateExecutionStrategy();
    await strategy.ExecuteAsync(async () =>
    {
      // Seed the database
      await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

      if (!await context.Users.AnyAsync(cancellationToken))
        await context.Users.AddRangeAsync(GetUsers(), cancellationToken);

      if (!await context.Products.AnyAsync(cancellationToken))
        await context.Products.AddRangeAsync(GetProducts(), cancellationToken);

      await context.SaveChangesAsync(cancellationToken);
      await transaction.CommitAsync(cancellationToken);
    });
  }


  public static List<User> GetUsers() => [
     new() { Name = "Alice" },
      new() { Name = "Bob" },
      new() { Name = "Charlie" },
      new() { Name = "David" },
      new() { Name = "Eve" },
      new() { Name = "Frank" },
      new() { Name = "Grace" },
      new() { Name = "Hank" },
      new() { Name = "Ivy" },
      new() { Name = "Jack" }
   ];

  private static List<Product> GetProducts() => [
    new() { Name = "Product 1", Price = 10.0 },
    new() { Name = "Product 2", Price = 20.0 },
    new() { Name = "Product 3", Price = 30.0 },
    new() { Name = "Product 4", Price = 40.0 },
    new() { Name = "Product 5", Price = 50.0 },
    new() { Name = "Product 6", Price = 60.0 },
    new() { Name = "Product 7", Price = 70.0 },
    new() { Name = "Product 8", Price = 80.0 },
    new() { Name = "Product 9", Price = 90.0 },
    new() { Name = "Product 10", Price = 100.0 }
  ];

}

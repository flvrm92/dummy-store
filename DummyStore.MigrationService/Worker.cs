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
    User user = new()
    {
      Name = "John Doe"
    };

    Product product = new()
    {
      Name = "Product 1",
      Price = 100
    };

    var strategy = context.Database.CreateExecutionStrategy();
    await strategy.ExecuteAsync(async () =>
    {
      // Seed the database
      await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

      if (!await context.Users.AnyAsync(cancellationToken))
        await context.Users.AddAsync(user, cancellationToken);

      if (!await context.Products.AnyAsync(cancellationToken))
        await context.Products.AddAsync(product, cancellationToken);

      await context.SaveChangesAsync(cancellationToken);
      await transaction.CommitAsync(cancellationToken);
    });
  }
}

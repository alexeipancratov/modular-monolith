using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RiverBooks.OrderProcessing.Domain;
using RiverBooks.SharedKernel;

namespace RiverBooks.OrderProcessing.Infrastructure.Data;

internal class OrderProcessingDbContext(
  DbContextOptions<OrderProcessingDbContext> options,
  IDomainEventDispatcher domainEventDispatcher) : DbContext(options)
{
  private readonly IDomainEventDispatcher _domainEventDispatcher = domainEventDispatcher;
  
  public DbSet<Order> Orders { get; set; } = null!;

  protected override void OnModelCreating(ModelBuilder builder)
  {
    builder.HasDefaultSchema("OrderProcessing");

    builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    base.OnModelCreating(builder);
  }

  protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
  {
    configurationBuilder.Properties<decimal>().HavePrecision(18, 6);
  }
  
  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
  {
    int result = await base.SaveChangesAsync(cancellationToken);
    
    // TODO: ignore domain events if no dispatcher provided

    var entitiesWithEvents = ChangeTracker.Entries<IHaveDomainEvents>()
      .Select(e => e.Entity)
      .Where(e => e.DomainEvents.Count != 0)
      .ToArray();

    await _domainEventDispatcher.DispatchAndClearEvents(entitiesWithEvents);

    return result;
  }
}

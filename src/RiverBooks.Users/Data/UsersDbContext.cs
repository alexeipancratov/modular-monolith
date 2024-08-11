using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RiverBooks.Users.DomainEvents.Infrastructure;

namespace RiverBooks.Users.Data;

public class UsersDbContext(
  DbContextOptions<UsersDbContext> options,
  IDomainEventDispatcher domainEventDispatcher) : IdentityDbContext(options)
{
  private readonly IDomainEventDispatcher _domainEventDispatcher = domainEventDispatcher;
  
  public DbSet<ApplicationUser> ApplicationUsers { get; set; }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    builder.HasDefaultSchema("Users");

    builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    base.OnModelCreating(builder);
  }

  protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
  {
    configurationBuilder.Properties<decimal>().HavePrecision(18, 6);
  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
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

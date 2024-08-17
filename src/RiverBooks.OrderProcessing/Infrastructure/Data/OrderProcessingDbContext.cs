using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RiverBooks.OrderProcessing.Domain;

namespace RiverBooks.OrderProcessing.Infrastructure.Data;

internal class OrderProcessingDbContext(DbContextOptions<OrderProcessingDbContext> options) : DbContext(options)
{
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
}
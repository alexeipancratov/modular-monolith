using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace RiverBooks.Books.Data;

internal class BookDbContext(DbContextOptions<BookDbContext> options) : DbContext(options)
{
  public DbSet<Book> Books { get; set; } = null!;

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasDefaultSchema("Books");

    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }

  protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
  {
    configurationBuilder.Properties<decimal>()
      .HavePrecision(18, 6);
  }
}

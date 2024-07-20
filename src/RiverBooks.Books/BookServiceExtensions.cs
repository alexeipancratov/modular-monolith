using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RiverBooks.Books.Data;
using Serilog;

namespace RiverBooks.Books;

public static class BookServiceExtensions
{
  public static IServiceCollection AddBookModuleServices(this IServiceCollection services,
    IConfiguration configuration,
    ILogger logger, List<Assembly> mediatRAssemblies)
  {
    // Data access
    string? connectionString = configuration.GetConnectionString("BooksConnectionString");
    services.AddDbContext<BookDbContext>(options => options.UseSqlServer(connectionString));
    services.AddScoped<IBookRepository, EfBookRepository>();
    
    // Application
    services.AddScoped<IBookService, BookService>();
    mediatRAssemblies.Add(typeof(BookServiceExtensions).Assembly);
    
    logger.Information("{Module} module services registered.", "Books");
    
    return services;
  }
}

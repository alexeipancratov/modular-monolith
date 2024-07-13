using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RiverBooks.Books.Data;

namespace RiverBooks.Books;

public static class BookServiceExtensions
{
  public static IServiceCollection AddBookServices(this IServiceCollection services, IConfiguration configuration)
  {
    // Data access
    string? connectionString = configuration.GetConnectionString("BooksConnectionString");
    services.AddDbContext<BookDbContext>(options => options.UseSqlServer(connectionString));
    services.AddScoped<IBookRepository, EfBookRepository>();
    
    // Application
    services.AddScoped<IBookService, BookService>();
    
    return services;
  }
}

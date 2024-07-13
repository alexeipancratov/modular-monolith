using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace RiverBooks.Users;

public static class UserModuleExtensions
{
  public static IServiceCollection AddUserModuleServices(this IServiceCollection services,
    IConfiguration configuration,
    ILogger logger)
  {
    string? connectionString = configuration.GetConnectionString("UsersConnectionString");
    services.AddDbContext<UsersDbContext>(config => config.UseSqlServer(connectionString));

    services.AddIdentityCore<ApplicationUser>()
      .AddEntityFrameworkStores<UsersDbContext>();
    
    logger.Information("{Module} module services registered.", "Users");
    
    return services;
  }
}

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RiverBooks.Users.Data;
using RiverBooks.Users.Data.Repositories;
using RiverBooks.Users.DomainEvents.Infrastructure;
using Serilog;

namespace RiverBooks.Users;

public static class UserModuleExtensions
{
  public static IServiceCollection AddUserModuleServices(this IServiceCollection services,
    IConfiguration configuration,
    ILogger logger, List<Assembly> mediatRAssemblies)
  {
    string? connectionString = configuration.GetConnectionString("UsersConnectionString");
    services.AddDbContext<UsersDbContext>(config => config.UseSqlServer(connectionString));

    services.AddIdentityCore<ApplicationUser>()
      .AddEntityFrameworkStores<UsersDbContext>();
    
    mediatRAssemblies.Add(typeof(UserModuleExtensions).Assembly);
    
    // Add User module services.
    services.AddScoped<IApplicationUserRepository, EfApplicationUserRepository>();

    // TODO: Register it as an app wide dependency.
    services.AddScoped<IDomainEventDispatcher, MediatrDomainEventDispatcher>();
    
    logger.Information("{Module} module services registered.", "Users");
    
    return services;
  }
}

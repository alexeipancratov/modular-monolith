using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RiverBooks.SharedKernel;
using RiverBooks.Users.Domain;
using RiverBooks.Users.Infrastructure.Data;
using RiverBooks.Users.Infrastructure.Data.Repositories;
using RiverBooks.Users.Interfaces;
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
    services.AddScoped<IReadOnlyUserStreetAddressRepository, EfUserStreetAddressRepository>();
    
    logger.Information("{Module} module services registered.", "Users");
    
    return services;
  }
}

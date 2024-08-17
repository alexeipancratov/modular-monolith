using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RiverBooks.OrderProcessing.MaterializedViews;
using Serilog;

namespace RiverBooks.OrderProcessing;

public static class OrderProcessingModuleExtensions
{
  public static IServiceCollection AddOrderProcessingModuleServices(this IServiceCollection services,
    IConfiguration configuration,
    ILogger logger, List<Assembly> mediatRAssemblies)
  {
    string? connectionString = configuration.GetConnectionString("OrderProcessingConnectionString");
    services.AddDbContext<OrderProcessingDbContext>(config => config.UseSqlServer(connectionString));
    
    mediatRAssemblies.Add(typeof(OrderProcessingModuleExtensions).Assembly);
    
    // Add Order Processing module services.
    services.AddScoped<IOrderRepository, EfOrderRepository>();
    // NOTE: here ReadThroughOrderAddressCache needs to override RedisOrderAddressCache since it's its decorator
    // with an additional logic.
    services.AddScoped<RedisOrderAddressCache>();
    services.AddScoped<IOrderAddressCache, ReadThroughOrderAddressCache>();
    
    logger.Information("{Module} module services registered.", "OrderProcessing");
    
    return services;
  }
}

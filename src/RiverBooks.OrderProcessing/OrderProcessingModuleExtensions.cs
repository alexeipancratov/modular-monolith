using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
    
    logger.Information("{Module} module services registered.", "OrderProcessing");
    
    return services;
  }
}

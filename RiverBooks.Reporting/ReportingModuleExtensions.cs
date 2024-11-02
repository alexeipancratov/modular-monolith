using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RiverBooks.Reporting.Services;
using Serilog;

namespace RiverBooks.Reporting;

public static class ReportingModuleExtensions
{
  public static IServiceCollection AddReportingModuleServices(this IServiceCollection services,
    IConfiguration configuration,
    ILogger logger, List<Assembly> mediatRAssemblies)
  {
    // string? connectionString = configuration.GetConnectionString("OrderProcessingConnectionString");
    // services.AddDbContext<OrderProcessingDbContext>(config => config.UseSqlServer(connectionString));
    
    // Services
    services.AddScoped<ITopSellingBooksReportService, TopSellingBooksReportService>();
    
    mediatRAssemblies.Add(typeof(ReportingModuleExtensions).Assembly);
    
    logger.Information("{Module} module services registered.", "Reporting");
    
    return services;
  }
}

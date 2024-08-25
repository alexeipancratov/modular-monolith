using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace RiverBooks.EmailSending;

public static class EmailSendingModuleServicesExtensions
{
  public static IServiceCollection AddEmailSendingModuleServices(this IServiceCollection services,
    IConfiguration configuration,
    ILogger logger, List<Assembly> mediatRAssemblies)
  {
    mediatRAssemblies.Add(typeof(EmailSendingModuleServicesExtensions).Assembly);
    
    logger.Information("{Module} module services registered.", "EmailSending");
    
    return services;
  }
}

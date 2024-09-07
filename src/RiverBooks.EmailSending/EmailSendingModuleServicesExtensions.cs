using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RiverBooks.EmailSending.Services;
using RiverBooks.EmailSending.Services.Interfaces;
using Serilog;

namespace RiverBooks.EmailSending;

public static class EmailSendingModuleServicesExtensions
{
  public static IServiceCollection AddEmailSendingModuleServices(this IServiceCollection services,
    IConfiguration configuration,
    ILogger logger, List<Assembly> mediatRAssemblies)
  {
    // Add module services.
    services.AddTransient<ISendEmailService, MimeKitSendEmailService>();
    
    mediatRAssemblies.Add(typeof(EmailSendingModuleServicesExtensions).Assembly);
    
    logger.Information("{Module} module services registered.", "EmailSending");
    
    return services;
  }
}

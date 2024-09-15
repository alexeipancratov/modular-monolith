using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using RiverBooks.EmailSending.Domain;
using RiverBooks.EmailSending.Services;
using RiverBooks.EmailSending.Services.Interfaces;
using ILogger = Serilog.ILogger;

namespace RiverBooks.EmailSending;

public static class EmailSendingModuleServicesExtensions
{
  public static IServiceCollection AddEmailSendingModuleServices(this IServiceCollection services,
    IConfiguration configuration,
    ILogger logger, List<Assembly> mediatRAssemblies)
  {
    // Configure MongoDB
    services.Configure<MongoDbSettings>(configuration.GetSection("MongoDb"));
    services.AddMongoDb(configuration);
    
    // Add module services.
    services.AddTransient<ISendEmailService, MimeKitSendEmailService>();
    services.AddTransient<IOutboxService, MongoDbOutboxService>();
    services.AddTransient<ISendEmailsFromOutboxService, DefaultSendEmailsFromOutboxService>();
    
    mediatRAssemblies.Add(typeof(EmailSendingModuleServicesExtensions).Assembly);

    services.AddHostedService<EmailSendingBackgroundService>();
    
    logger.Information("{Module} module services registered.", "EmailSending");
    
    return services;
  }
  
  public static IServiceCollection AddMongoDb(this IServiceCollection services,
    IConfiguration configuration)
  {
    // Register the MongoDB client as a singleton
    services.AddSingleton<IMongoClient>(serviceProvider =>
    {
      var settings = configuration.GetSection("MongoDB").Get<MongoDbSettings>();
      return new MongoClient(settings!.ConnectionString);
    });

    // Register the MongoDB database as a singleton
    services.AddSingleton(serviceProvider =>
    {
      var settings = configuration.GetSection("MongoDB").Get<MongoDbSettings>();
      var client = serviceProvider.GetService<IMongoClient>();
      return client!.GetDatabase(settings!.DatabaseName);
    });

    //// Optionally, register specific collections here as scoped or singleton services
    //// Example for a 'EmailOutboxEntity' collection
    services.AddTransient(serviceProvider =>
    {
      var database = serviceProvider.GetService<IMongoDatabase>();
      return database!.GetCollection<EmailOutboxEntity>("EmailOutboxEntityCollection");
    });

    return services;
  }
}

using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace RiverBooks.SharedKernel;

public static class BehaviorExtensions
{
  public static IServiceCollection AddMediatrLoggingBehavior(this IServiceCollection services)
  {
    services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

    return services;
  }

  /// <summary>
  /// Adds custom Fluent Validation behavior for MediatR commands.
  /// </summary>
  /// <remarks>Please also register validators!</remarks>
  public static IServiceCollection AddMediatrFluentValidationBehavior(this IServiceCollection services)
  {
    services.AddScoped(typeof(IPipelineBehavior<,>), typeof(FluentValidationBehavior<,>));
    
    return services;
  }

  public static IServiceCollection AddValidatorsFromAssemblyContaining<T>(this IServiceCollection services)
  {
    var assembly = typeof(T).Assembly;
    
    var allValidatorTypes = assembly.GetTypes()
      .Where(t => t.GetInterfaces()
        .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>)))
      .ToList();

    foreach (var validatorType in allValidatorTypes)
    {
      var implementedTypes = validatorType.GetInterfaces()
        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>));

      foreach (var implementedType in implementedTypes)
      {
        services.AddTransient(implementedType, validatorType);
      }
    }
    
    return services;
  }
}

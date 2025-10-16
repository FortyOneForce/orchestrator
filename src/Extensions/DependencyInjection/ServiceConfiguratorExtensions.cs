using FortyOne.OrchestratR.Extensions.DependencyInjection;
using FortyOne.OrchestratR.Extensions.Formatters;
using FortyOne.OrchestratR.Extensions.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

#pragma warning disable IDE0130
namespace FortyOne.OrchestratR.DependencyInjection;
#pragma warning restore IDE0130

/// <summary>
/// Provides extension methods for configuring services related to OrchestratR.
/// </summary>
public static class ServiceConfiguratorExtensions
{
    /// <summary>
    /// Adds default request interceptors for exception handling and logging to the service configurator.
    /// </summary>
    public static IServiceConfigurator AddDefaultRequestInterceptors(
        this IServiceConfigurator serviceConfigurator,
        Action<IDefaultInterceptorOptions>? options = null)
    {
        ArgumentNullException.ThrowIfNull(serviceConfigurator);

        serviceConfigurator.AddRequestInterceptor(typeof(DefaultExceptionInterceptor<,>), ServiceLifetime.Transient);
        serviceConfigurator.AddRequestInterceptor(typeof(DefaultLoggingInterceptor<,>), ServiceLifetime.Transient);
        serviceConfigurator.AddRequestInterceptor(typeof(DefaultTransactionInterceptor<,>), ServiceLifetime.Transient);
        serviceConfigurator.AddRequestInterceptor(typeof(DefaultCacheInterceptor<,>), ServiceLifetime.Transient);

        serviceConfigurator.Services.TryAddSingleton<IErrorFormatter>(new DefaultErrorFormatter());

        var optionsInstance = new DefaultInterceptorOptions();
        options?.Invoke(optionsInstance);

        serviceConfigurator.Services.AddSingleton(optionsInstance);
        
        return serviceConfigurator;
    }

    /// <summary>
    /// Sets a custom error formatter for transforming errors in the OrchestratR pipeline.
    /// </summary>
    public static IServiceConfigurator AddErrorFormatter(
        this IServiceConfigurator serviceConfigurator,
        IErrorFormatter formatter)
    {
        ArgumentNullException.ThrowIfNull(serviceConfigurator);
        ArgumentNullException.ThrowIfNull(formatter);

        if (serviceConfigurator.Services.Any(sd => sd.ServiceType == typeof(IErrorFormatter)))
        {
            serviceConfigurator.Services.Replace(ServiceDescriptor.Singleton<IErrorFormatter>(formatter));
        }
        else
        {
            serviceConfigurator.Services.AddSingleton<IErrorFormatter>(formatter);
        }

        return serviceConfigurator;
    }
}

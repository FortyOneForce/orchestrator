using FortyOne.OrchestratR.Extensions;
using FortyOne.OrchestratR.HandlerProxies;
using Microsoft.Extensions.DependencyInjection;

namespace FortyOne.OrchestratR.DependencyInjection;

/// <summary>
/// Provides extension methods for <see cref="IServiceCollection"/> to register OrchestratR services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the OrchestratR mediator and all its required services with the dependency injection container.
    /// </summary>
    public static IServiceCollection AddOrchestrator(this IServiceCollection services, Action<IServiceConfigurator> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        EnsureOrchestratorNotRegistered(services);

        var configurator = new ServiceConfigurator();
        configure(configurator);

        RegisterCoreServices(services);
        RegisterInterceptors(services, configurator);
        RegisterConfiguresServices(services, configurator);

        return services;
    }

    private static void EnsureOrchestratorNotRegistered(IServiceCollection services)
    {
        var existingDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IOrchestrator));
        if (existingDescriptor != null)
        {
            throw new InvalidOperationException("OrchestratR has already been registered. Multiple registrations are not allowed.");
        }
    }

    private static void RegisterCoreServices(IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddTransient<IRequestOrchestrator, Orchestrator>();
        services.AddTransient<INotificationOrchestrator, Orchestrator>();
        services.AddTransient<ICommandOrchestrator, Orchestrator>();
        services.AddTransient<IOrchestrator, Orchestrator>();
        
    }

    private static void RegisterInterceptors(IServiceCollection services, ServiceConfigurator configurator)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configurator);

        var interceptorRegistry = new InterceptorRegistry();

        foreach (var item in configurator.InterceptorTypes)
        {
            services.Add(new ServiceDescriptor(item.InterceptorType, item.InterceptorType, item.ServiceLifetime));
            interceptorRegistry.AddInterceptorType(item.InterceptorType);
        }

        services.AddSingleton(interceptorRegistry);
    }

    private static void RegisterConfiguresServices(IServiceCollection services, ServiceConfigurator configurator)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configurator);

        foreach(var assembly in configurator.Assemblies)
        {
            var handlerTypes = assembly.GetTypes()
                .Where(type => type.IsConcreteAssignableTo<IHandlerBase>());

            foreach(var handlerType in handlerTypes)
            {
                var lifetime = configurator.HandlerTypeLifetimeSelector(handlerType);

                if (handlerType.TryGetNotificationHandlerInterfaces(out var notificationHandlerInterfaces))
                {
                    if (configurator.HandlerTypeFilterPredicate(handlerType, HandlerKind.NotificationHandler))
                    {
                        foreach (var notificationHandlerInterface in notificationHandlerInterfaces)
                        {
                            var notificationType = notificationHandlerInterface.GetGenericArguments()[0];
                            var handlerInterfaceType = typeof(INotificationHandler<>).MakeGenericType(notificationType);
                            var proxyType = typeof(NotificationHandlerProxy<>).MakeGenericType(notificationType);

                            services.Add(new ServiceDescriptor(handlerInterfaceType, handlerType, lifetime));
                            services.AddTransient(proxyType);
                        }
                    }
                }

                if (handlerType.TryGetRequstHandlerInterfaces(out var requestHandlerInterfaces))
                {
                    if (configurator.HandlerTypeFilterPredicate(handlerType, HandlerKind.RequestHandler))
                    {
                        foreach (var requestHandlerInterface in requestHandlerInterfaces)
                        {
                            var interfaceGenericAurguments = requestHandlerInterface.GetGenericArguments();
                            services.Add(new ServiceDescriptor(requestHandlerInterface, handlerType, lifetime));

                            if (interfaceGenericAurguments.Length == 1)
                            {
                                var requestType = interfaceGenericAurguments[0];
                                var proxyType = typeof(RequestHandlerPropxy<>).MakeGenericType(requestType);

                                services.AddSingleton(proxyType);
                            }
                            else if (interfaceGenericAurguments.Length == 2)
                            {
                                var requestType = interfaceGenericAurguments[0];
                                var responseType = interfaceGenericAurguments[1];
                                var proxyType = typeof(RequestHandlerProxy<,>).MakeGenericType(requestType, responseType);

                                services.AddSingleton(proxyType);
                            }
                        }
                    }
                }
            }
        }
    }
}

using FortyOne.OrchestratR.Extensions;
using FortyOne.OrchestratR.Proxies;
using Microsoft.Extensions.DependencyInjection;

namespace FortyOne.OrchestratR.DependencyInjection;

public static class ServiceCollectionExtensions
{
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

        services.AddTransient<IOrchestrator, Orchestrator>();
        services.AddTransient<RequestExecutor>();
        services.AddTransient<NotificationPublisher>();
    }

    private static void RegisterInterceptors(IServiceCollection services, ServiceConfigurator configurator)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configurator);

        var interceptorFactory = new InterceptorFactory();

        foreach (var item in configurator.InterceptorTypes)
        {
            services.Add(new ServiceDescriptor(item.InterceptorType, item.InterceptorType, item.ServiceLifetime));
            interceptorFactory.AddInterceptorType(item.InterceptorType);
        }

        services.AddSingleton(interceptorFactory);
    }

    private static void RegisterConfiguresServices(IServiceCollection services, ServiceConfigurator configurator)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configurator);

        foreach(var registeredAssembly in configurator.Assemblies)
        {
            var assembly = registeredAssembly.Key;
            var serviceLifetimeSelector = registeredAssembly.Value;

            var handlerTypes = assembly.GetTypes()
                .Where(type => type.IsConcreteAssignableTo<IHandlerBase>());

            foreach(var handlerType in handlerTypes)
            {
                var lifetime = serviceLifetimeSelector(handlerType);

                if (handlerType.TryGetEventHandlerInterfaces(out var eventHandlerInterfaces))
                {
                    foreach(var eventHandlerInterface in eventHandlerInterfaces)
                    {
                        services.Add(new ServiceDescriptor(eventHandlerInterface, handlerType, lifetime));
                    }
                }

                if (handlerType.TryGetActionHandlerInterfaces(out var actionHandlerInterfaces))
                {
                    foreach(var actionHandlerInterface in actionHandlerInterfaces)
                    {
                        var interfaceGenericAurguments = actionHandlerInterface.GetGenericArguments();
                        services.Add(new ServiceDescriptor(actionHandlerInterface, handlerType, lifetime));

                        if (interfaceGenericAurguments.Length == 1)
                        {
                            var requestType = interfaceGenericAurguments[0];
                            var proxyType = typeof(HandlerPropxy<>).MakeGenericType(requestType);

                            services.AddTransient(proxyType);
                        }
                        else if (interfaceGenericAurguments.Length == 2)
                        {
                            var requestType = interfaceGenericAurguments[0];
                            var responseType = interfaceGenericAurguments[1];
                            var proxyType = typeof(HandlerWithResponseProxy<,>).MakeGenericType(requestType, responseType);

                            services.AddTransient(proxyType);
                        }
                    }
                }
            }
        }
    }
}

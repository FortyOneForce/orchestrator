using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FortyOne.OrchestratR.DependencyInjection;

internal sealed class OrchestratorConfigurator : IOrchestratorConfigurator
{
    public ActionExecutionChain ActionExecutionChain { get; }
    public IServiceCollection Services { get; }


    public OrchestratorConfigurator(ActionExecutionChain actionExecutionChain, IServiceCollection services)
    {
        ActionExecutionChain = actionExecutionChain;
        Services = services;
    }

    public IOrchestratorConfigurator RegisterHandlersFromAssembly(Assembly assembly, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        return RegisterHandlersFromAssembly(assembly, _ => serviceLifetime);
    }

    public IOrchestratorConfigurator RegisterHandlersFromAssembly(Assembly assembly, Func<Type, ServiceLifetime> serviceLifetimeSelector)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        ArgumentNullException.ThrowIfNull(serviceLifetimeSelector);

        var handlerTypes = assembly.GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract && !type.IsInterface)
            .Select(type => new
            {
                ImplementationType = type,
                HandlerInterfaceTypes = type.GetInterfaces().Where(i => i.IsGenericType && i.IsAssignableTo(typeof(IHandlerBase)))
            })
            .Where(x => x.HandlerInterfaceTypes.Any());

        foreach(var handlerType in handlerTypes)
        {
            var serviceLifetime = serviceLifetimeSelector(handlerType.ImplementationType);

            foreach(var interfaceType in handlerType.HandlerInterfaceTypes)
            {
                var genericTypeDefinition = interfaceType.GetGenericTypeDefinition();
                var genericArguments = interfaceType.GetGenericArguments();

                if (genericTypeDefinition == typeof(IActionHandler<>))
                {
                    var serviceType = typeof(IActionHandler<>).MakeGenericType(genericArguments[0]);
                    Services.Add(new ServiceDescriptor(serviceType, handlerType.ImplementationType, serviceLifetime));
                }
                else if (genericTypeDefinition == typeof(IActionHandler<,>))
                {
                    var serviceType = typeof(IActionHandler<,>).MakeGenericType(genericArguments[0], genericArguments[1]);
                    Services.Add(new ServiceDescriptor(serviceType, handlerType.ImplementationType, serviceLifetime));
                }
                else if (genericTypeDefinition == typeof(IEventHandler<>))
                {
                    var serviceType = typeof(IEventHandler<>).MakeGenericType(genericArguments[0]);
                    Services.Add(new ServiceDescriptor(serviceType, handlerType.ImplementationType, serviceLifetime));
                }
            }
        }

        return this;
    }

    public IOrchestratorConfigurator UseActionExecutionInterceptor(Type interceptorType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        ArgumentNullException.ThrowIfNull(interceptorType);

        ActionExecutionChain.AddInterceptor(interceptorType, serviceLifetime);

        Services.Add(new ServiceDescriptor(interceptorType, interceptorType, serviceLifetime));

        return this;
    }
}

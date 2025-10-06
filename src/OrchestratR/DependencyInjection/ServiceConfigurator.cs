using FortyOne.OrchestratR.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FortyOne.OrchestratR.DependencyInjection;

internal sealed class ServiceConfigurator : IServiceConfigurator
{
    public Dictionary<Assembly, Func<Type, ServiceLifetime>> Assemblies { get; } = new();
    public List<(Type InterceptorType, ServiceLifetime ServiceLifetime)> InterceptorTypes { get; } = new();

    public IServiceConfigurator RegisterServicesFromAssembly(Assembly assembly, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        return RegisterServicesFromAssembly(assembly, _ => serviceLifetime);
    }

    public IServiceConfigurator RegisterServicesFromAssembly(Assembly assembly, Func<Type, ServiceLifetime> serviceLifetimeSelector)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        ArgumentNullException.ThrowIfNull(serviceLifetimeSelector);

        Assemblies[assembly] = serviceLifetimeSelector;

        return this;
    }

    public IServiceConfigurator AddRequestInterceptor(Type interceptorType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        ArgumentNullException.ThrowIfNull(interceptorType);

        if (InterceptorTypes.Any(x => x.InterceptorType == interceptorType))
        {
            throw new ArgumentException($"The request interceptor '{interceptorType.FullName}' has already been registered.");
        }

        if (!interceptorType.IsRequestInterceptor())
        {
            throw new ArgumentException($"The type '{interceptorType.FullName}' is not a valid request interceptor. It must implement the IRequestInterceptor<TRequest, TResponse> interface.");
        }

        InterceptorTypes.Add((interceptorType, serviceLifetime));

        return this;
    }
}

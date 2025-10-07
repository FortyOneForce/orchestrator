using FortyOne.OrchestratR.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FortyOne.OrchestratR.DependencyInjection;

internal sealed class ServiceConfigurator : IServiceConfigurator
{
    private Func<Type, HandlerKind, bool>? _handlerTypeFilterPredicate = null;
    private Func<Type, ServiceLifetime>? _handlerTypeLifetimeSelector = null;

    public HashSet<Assembly> Assemblies { get; } = new();
    public List<(Type InterceptorType, ServiceLifetime ServiceLifetime)> InterceptorTypes { get; } = new();
    public Func<Type, ServiceLifetime> HandlerTypeLifetimeSelector => _handlerTypeLifetimeSelector ?? (_ => ServiceLifetime.Transient);
    public Func<Type, HandlerKind, bool> HandlerTypeFilterPredicate => _handlerTypeFilterPredicate ?? ((_,_) => true);

    public IServiceConfigurator RegisterServicesFromAssembly(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        Assemblies.Add(assembly);

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

    public IServiceConfigurator WithHandlerTypeFilter(Func<Type, HandlerKind, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        if (_handlerTypeFilterPredicate != null)
        {
            throw new InvalidOperationException("Type filter has already been configured. Only one type filter can be set per configurator.");
        }

        _handlerTypeFilterPredicate = predicate;

        return this;
    }

    public IServiceConfigurator WithHandlerTypeLifetime(Func<Type, ServiceLifetime> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        if (_handlerTypeLifetimeSelector != null)
        {
            throw new InvalidOperationException("Type lifetime selector has already been configured. Only one type lifetime selector can be set per configurator.");
        }

        _handlerTypeLifetimeSelector = selector;

        return this;
    }
}

using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FortyOne.OrchestratR.DependencyInjection;

/// <summary>
/// Provides a fluent interface for configuring the OrchestratR mediator during service registration.
/// </summary>
public interface IServiceConfigurator
{
    /// <summary>
    /// Registers all handlers found in the specified assembly.
    /// </summary>
    IServiceConfigurator RegisterServicesFromAssembly(Assembly assembly);

    /// <summary>
    /// Configures the service lifetime for all handler types.
    /// </summary>
    IServiceConfigurator WithHandlerTypeLifetime(Func<Type, ServiceLifetime> selector);

    /// <summary>
    /// Configures a filter to determine which types should be considered as handlers.
    /// </summary>
    IServiceConfigurator WithHandlerTypeFilter(Func<Type, HandlerKind, bool> predicate);

    /// <summary>
    /// Adds a request pipeline interceptor.
    /// </summary>
    IServiceConfigurator AddRequestInterceptor(Type interceptorType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient);
}

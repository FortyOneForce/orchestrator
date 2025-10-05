using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FortyOne.OrchestratR.DependencyInjection;

/// <summary>
/// Provides a fluent API for configuring the orchestrator and registering its components.
/// </summary>
/// <remarks>
/// This interface is used during application startup to:
/// <list type="bullet">
///   <li>Register handlers from assemblies</li>
///   <li>Configure the action execution pipeline with interceptors</li>
///   <li>Access the service collection for additional registrations</li>
/// </list>
/// </remarks>
public interface IOrchestratorConfigurator
{
    /// <summary>
    /// Gets the service collection being configured.
    /// </summary>
    /// <value>The <see cref="IServiceCollection"/> instance used to register orchestrator services.</value>
    IServiceCollection Services { get; }

    /// <summary>
    /// Registers all handlers from the specified assembly with a uniform service lifetime.
    /// </summary>
    /// <param name="assembly">The assembly to scan for handler implementations.</param>
    /// <param name="serviceLifetime">The service lifetime to apply to all discovered handlers. Defaults to Transient.</param>
    /// <returns>The same <see cref="IOrchestratorConfigurator"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="assembly"/> is null.</exception>
    IOrchestratorConfigurator RegisterHandlersFromAssembly(Assembly assembly, ServiceLifetime serviceLifetime = ServiceLifetime.Transient);

    /// <summary>
    /// Registers all handlers from the specified assembly with custom service lifetime selection.
    /// </summary>
    /// <param name="assembly">The assembly to scan for handler implementations.</param>
    /// <param name="serviceLifetimeSelector">A function that determines the service lifetime for each handler type.</param>
    /// <returns>The same <see cref="IOrchestratorConfigurator"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="assembly"/> or <paramref name="serviceLifetimeSelector"/> is null.</exception>
    /// <remarks>
    /// This overload allows different service lifetimes to be applied to different handlers based on their type.
    /// The selector function receives the handler implementation type and should return an appropriate service lifetime.
    /// </remarks>
    IOrchestratorConfigurator RegisterHandlersFromAssembly(Assembly assembly, Func<Type, ServiceLifetime> serviceLifetimeSelector);

    /// <summary>
    /// Adds an interceptor to the action execution pipeline.
    /// </summary>
    /// <param name="interceptorType">The generic type definition of the interceptor.</param>
    /// <param name="serviceLifetime">The service lifetime to apply to the interceptor. Defaults to Transient.</param>
    /// <returns>The same <see cref="IOrchestratorConfigurator"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="interceptorType"/> is null.</exception>
    /// <exception cref="ArgumentException">
    /// Thrown when:
    /// <list type="bullet">
    ///   <li>The interceptor type is not a generic type</li>
    ///   <li>The interceptor type is abstract or an interface</li>
    ///   <li>The interceptor type does not implement IActionExecutionInterceptor&lt;TRequest,TResponse&gt;</li>
    ///   <li>The interceptor type implements the interface more than once</li>
    ///   <li>The interceptor type does not have exactly 2 generic type parameters</li>
    ///   <li>The interceptor type has already been registered</li>
    /// </list>
    /// </exception>
    /// <remarks>
    /// Interceptors are executed in the order they are registered, forming a pipeline around action handlers.
    /// The interceptor type must be an open generic type that implements IActionExecutionInterceptor&lt;TRequest,TResponse&gt;.
    /// </remarks>
    IOrchestratorConfigurator UseActionExecutionInterceptor(Type interceptorType, ServiceLifetime serviceLifetime = ServiceLifetime.Transient);
}

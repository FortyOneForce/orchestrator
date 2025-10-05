using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace FortyOne.OrchestratR;

/// <summary>
/// Manages the registration and resolution of interceptors in the action execution pipeline.
/// </summary>
internal sealed class ActionExecutionChain
{
    /// <summary>
    /// Represents metadata about a registered interceptor type and its constraints.
    /// </summary>
    private record Interceptor(Type ServiceType, ServiceLifetime ServiceLifetime, Type[] RequestConstraints, Type[] ResponseConstraints);
    

    private readonly List<Interceptor> _interceptors = [];
    private readonly ConcurrentDictionary<(Type RequestType, Type ResponseType), Type[]> _visitedInterceptors = new();

    /// <summary>
    /// Adds an interceptor type to the execution chain with the specified service lifetime.
    /// </summary>
    public void AddInterceptor(Type interceptorType, ServiceLifetime serviceLifetime)
    {
        ArgumentNullException.ThrowIfNull(interceptorType);

        if (_interceptors.Any(x => x.ServiceType == interceptorType))
        {
            throw new ArgumentException(
                $"The action execution interceptor '{interceptorType.FullName}' has already been registered.");
        }

        if (!interceptorType.IsGenericType)
        {
            throw new ArgumentException(
                $"The action execution interceptor '{interceptorType.FullName}' must be a generic type implementing IActionExecutionInterceptor<TRequest, TResponse>.");
        }

        if (interceptorType.IsAbstract || interceptorType.IsInterface)
        {
            throw new ArgumentException(
                $"The action execution interceptor '{interceptorType.FullName}' cannot be abstract or an interface.");
        }

        var interfaceTypes = interceptorType
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IActionExecutionInterceptor<,>))
                .ToList();

        if (!interfaceTypes.Any())
        {
            throw new ArgumentException(
                $"The action execution interceptor '{interceptorType.FullName}' must implement IActionExecutionInterceptor<TRequest, TResponse>.");
        }

        if (interfaceTypes.Count > 1)
        {
            throw new ArgumentException(
                $"The action execution interceptor '{interceptorType.FullName}' cannot implement IActionExecutionInterceptor<TRequest, TResponse> more than once.");
        }

        var genericArguments = interceptorType.GetGenericArguments();
        if (genericArguments.Length != 2)
        {
            throw new ArgumentException(
                $"The action execution interceptor '{interceptorType.FullName}' must have exactly 2 generic type parameters: TRequest and TResponse.");
        }

        var interceptor = new Interceptor(
            interceptorType,
            serviceLifetime,
            genericArguments[0].GetGenericParameterConstraints(),
            genericArguments[1].GetGenericParameterConstraints());

        _interceptors.Add(interceptor);
    }

    /// <summary>
    /// Returns the applicable interceptor types for the specified request and response types, caching the results for performance.
    /// </summary>
    public Type[] GetInterceptorsFor(Type requestType, Type responseType)
    {
        ArgumentNullException.ThrowIfNull(requestType);
        ArgumentNullException.ThrowIfNull(responseType);

        return _visitedInterceptors.GetOrAdd((requestType, responseType), (key) =>
        {
            return _interceptors
                .Where(x =>
                    x.RequestConstraints.All(c => c.IsAssignableFrom(key.RequestType)) &&
                    x.ResponseConstraints.All(c => c.IsAssignableFrom(key.ResponseType)))
                .Select(x => x.ServiceType)
                .ToArray();
        });
    }

}

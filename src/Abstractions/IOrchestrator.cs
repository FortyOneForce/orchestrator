#pragma warning disable IDE0130
namespace FortyOne.OrchestratR;
#pragma warning restore IDE0130

/// <summary>
/// Provides a central orchestrator for all request-response and publish-subscribe operations within the OrchestratR library.
/// </summary>
public interface IOrchestrator : 
    IRequestOrchestrator, 
    INotificationOrchestrator
{
}

/// <summary>
/// Provides a central orchestrator for request-response operations within the OrchestratR library.
/// </summary>
public interface IRequestOrchestrator
{
    /// <summary>
    /// Sends a request to its corresponding handler and returns a response.
    /// </summary>
    Task SendAsync(IRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a request to its corresponding handler and returns a response, allowing for middleware configuration.
    /// </summary>
    Task SendAsync(IRequest request, Action<IRequestExecutionParameters> middleware, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a request to its corresponding handler and returns a response of type TResponse.
    /// </summary>
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a request to its corresponding handler and returns a response of type TResponse, allowing for middleware configuration.
    /// </summary>
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, Action<IRequestExecutionParameters> middleware, CancellationToken cancellationToken = default);
}

/// <summary>
/// Provides a central orchestrator for publish-subscribe operations within the OrchestratR library.
/// </summary>
public interface INotificationOrchestrator
{
    /// <summary>
    /// Publishes a notification to all registered handlers.
    /// </summary>
    Task NotifyAsync(INotification notification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes a notification to all registered handlers, allowing for middleware configuration.
    /// </summary>
    Task NotifyAsync(INotification notification, Action<INotificationExecutionParameters> middleware, CancellationToken cancellationToken = default);
}
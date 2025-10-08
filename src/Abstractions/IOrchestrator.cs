namespace FortyOne.OrchestratR;

/// <summary>
/// Provides a central orchestrator for all request-response and publish-subscribe operations within the OrchestratR library.
/// </summary>
public interface IOrchestrator
{
    /// <summary>
    /// Executes a request that does not return a value.
    /// </summary>
    Task ExecuteAsync(IRequest request, CancellationToken cancellationToken = default);
    /// <summary>
    /// Executes a request that does not return a value with specified processing options.
    /// </summary>
    Task ExecuteBatchAsync(IEnumerable<IRequest> requests, RequestProcessingOptions options, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a request that returns a value of type <typeparamref name="TResponse"/>.
    /// </summary>
    Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes multiple requests that return values of type <typeparamref name="TResponse"/> with specified processing options.
    /// </summary>
    Task<BatchProcessingResponse<TResponse>> ExecuteBatchAsync<TResponse>(IEnumerable<IRequest<TResponse>> requests, RequestProcessingOptions options, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes multiple requests that return values of type <typeparamref name="TResponse"/> with specified processing options and a failure predicate.
    /// </summary>
    Task<BatchProcessingResponse<TResponse>> ExecuteBatchAsync<TResponse>(IEnumerable<IRequest<TResponse>> requests, RequestProcessingOptions options, Func<TResponse, bool> failurePredicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a command to its associated handler for execution.
    /// </summary>
    Task SendAsync(ICommand request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a command to its associated handler and returns the result.
    /// </summary>
    Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a query to retrieve data from the system.
    /// </summary>
    Task<TResponse> QueryAsync<TResponse>(IQuery<TResponse> request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes a notification to all registered handlers.
    /// </summary>
    Task NotifyAsync(INotification notification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes a notification to all registered handlers with specified batch execution options.
    /// </summary>
    Task NotifyAsync(INotification notification, NotificationProcessingOptions options, CancellationToken cancellationToken = default);
}

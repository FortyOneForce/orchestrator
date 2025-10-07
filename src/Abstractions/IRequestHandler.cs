namespace FortyOne.OrchestratR;

/// <summary>
/// Defines a handler for processing requests that don't return a value.
/// </summary>
public interface IRequestHandler<in TRequest> : IHandlerBase where TRequest : IRequest
{
    /// <summary>
    /// Handles a request of type <typeparamref name="TRequest"/>.
    /// </summary>
    Task HandleAsync(TRequest request, CancellationToken cancellationToken);
}

/// <summary>
/// Defines a handler for processing requests that return a value of type <typeparamref name="TResponse"/>.
/// </summary>
public interface IRequestHandler<in TRequest, TResponse> : IHandlerBase where TRequest: IRequest<TResponse>
{
    /// <summary>
    /// Handles a request of type <typeparamref name="TRequest"/> and returns a result of type <typeparamref name="TResponse"/>.
    /// </summary>
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
}

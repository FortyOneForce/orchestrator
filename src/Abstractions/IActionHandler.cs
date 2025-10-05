namespace FortyOne.OrchestratR;

/// <summary>
/// Defines a handler for processing action requests that don't return a response.
/// </summary>
/// <typeparam name="TRequest">The type of request to be handled.</typeparam>
/// <remarks>
/// Action handlers represent the core business logic processors in the orchestration system.
/// Each handler is responsible for processing a specific type of request and performing
/// the associated operation. Handlers are executed at the end of the pipeline after
/// all interceptors have been processed.
/// </remarks>
public interface IActionHandler<TRequest> : IHandlerBase where TRequest : IActionRequest
{
    /// <summary>
    /// Processes the specified request asynchronously.
    /// </summary>
    /// <param name="request">The request to process.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <remarks>
    /// Implementations should contain the core business logic for processing the request.
    /// This method is called after all registered interceptors in the pipeline have been executed.
    /// </remarks>
    Task HandleAsync(TRequest request, CancellationToken cancellationToken);
}

/// <summary>
/// Defines a handler for processing action requests that return a response.
/// </summary>
/// <typeparam name="TRequest">The type of request to be handled.</typeparam>
/// <typeparam name="TResponse">The type of response returned by the handler.</typeparam>
/// <remarks>
/// Action handlers represent the core business logic processors in the orchestration system.
/// Each handler is responsible for processing a specific type of request and returning
/// a response. Handlers are executed at the end of the pipeline after all interceptors
/// have been processed.
/// </remarks>
public interface IActionHandler<TRequest, TResponse> : IHandlerBase where TRequest: IActionRequest<TResponse>
{
    /// <summary>
    /// Processes the specified request asynchronously and returns a response.
    /// </summary>
    /// <param name="request">The request to process.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="Task{TResponse}"/> representing the asynchronous operation with the result of type <typeparamref name="TResponse"/>.
    /// </returns>
    /// <remarks>
    /// Implementations should contain the core business logic for processing the request and generating a response.
    /// This method is called after all registered interceptors in the pipeline have been executed.
    /// </remarks>
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
}

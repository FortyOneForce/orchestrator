namespace FortyOne.OrchestratR;

/// <summary>
/// Defines the base marker interface for action execution interceptors in the orchestration pipeline.
/// </summary>
/// <remarks>
/// This interface serves as the base type for all action interceptors, allowing for
/// non-generic collection of interceptors in the execution pipeline. Concrete implementations 
/// should implement the generic version <see cref="IActionExecutionInterceptor{TRequest,TResponse}"/>.
/// </remarks>
public interface IActionExecutionInterceptor
{
}

/// <summary>
/// Defines an interceptor for the action execution pipeline with strongly-typed request and response.
/// </summary>
/// <typeparam name="TRequest">The type of request being processed.</typeparam>
/// <typeparam name="TResponse">The type of response returned from the pipeline.</typeparam>
/// <remarks>
/// Interceptors form a middleware pipeline around action handlers, enabling cross-cutting concerns
/// such as logging, validation, error handling, and performance monitoring. Each interceptor can
/// execute code before and after the next element in the pipeline, and can modify the request context
/// or the response.
/// </remarks>
public interface IActionExecutionInterceptor<TRequest, TResponse> : IActionExecutionInterceptor
{
    /// <summary>
    /// Processes the request in the execution pipeline and invokes the next interceptor or handler.
    /// </summary>
    /// <param name="context">The execution context containing the request and pipeline information.</param>
    /// <param name="next">A delegate representing the next interceptor or final handler in the pipeline.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="Task{TResponse}"/> representing the asynchronous operation with the result of type <typeparamref name="TResponse"/>.
    /// </returns>
    /// <remarks>
    /// Implementations should invoke the <paramref name="next"/> delegate to continue the pipeline execution.
    /// Code can be executed before calling next (pre-processing) and after calling next (post-processing).
    /// Interceptors can short-circuit the pipeline by not calling next and returning their own response.
    /// </remarks>
    Task<TResponse> HandleAsync(IActionExecutionContext<TRequest> context, ActionExecutionDelegate<TResponse> next, CancellationToken cancellationToken);
}

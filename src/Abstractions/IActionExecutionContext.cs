namespace FortyOne.OrchestratR;

/// <summary>
/// Defines the execution context for request processing pipeline in the orchestration system.
/// </summary>
/// <remarks>
/// The execution context provides access to the current service provider, request metadata,
/// and the interceptor chain. This context is passed through the entire request processing 
/// pipeline, allowing interceptors to access and potentially modify the execution environment.
/// </remarks>
public interface IActionExecutionContext
{
    /// <summary>
    /// Gets the service provider associated with the current execution scope.
    /// </summary>
    /// <value>The <see cref="IServiceProvider"/> that can be used to resolve dependencies during execution.</value>
    IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Gets a value indicating whether the current execution context is running within a DI scope.
    /// </summary>
    /// <value><c>true</c> if the context is executing within a scoped lifetime; otherwise, <c>false</c>.</value>
    bool IsScoped { get; }

    /// <summary>
    /// Gets the runtime type of the request being processed.
    /// </summary>
    /// <value>The <see cref="Type"/> of the request object.</value>
    Type RequestType { get; }

    /// <summary>
    /// Gets the runtime type of the expected response from the request.
    /// </summary>
    /// <value>The <see cref="Type"/> of the expected response.</value>
    Type ResponseType { get; }

    /// <summary>
    /// Gets the collection of interceptors in the execution pipeline.
    /// </summary>
    /// <value>
    /// An array of <see cref="IActionExecutionInterceptor"/> instances that form the execution pipeline.
    /// </value>
    IActionExecutionInterceptor[] Interceptors { get; } 
}

/// <summary>
/// Extends the <see cref="IActionExecutionContext"/> with strongly-typed request access.
/// </summary>
/// <typeparam name="TRequest">The type of the request being processed.</typeparam>
/// <remarks>
/// This generic interface provides type-safe access to the request object being processed
/// through the pipeline, enabling interceptors to interact with the request without casting.
/// </remarks>
public interface IActionExecutionContext<TRequest> : IActionExecutionContext
{
    /// <summary>
    /// Gets the strongly-typed request being processed.
    /// </summary>
    /// <value>The request object of type <typeparamref name="TRequest"/>.</value>
    TRequest Request { get; }
}

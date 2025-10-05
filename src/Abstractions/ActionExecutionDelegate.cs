namespace FortyOne.OrchestratR;

/// <summary>
/// Represents a delegate that encapsulates the next action in the execution pipeline.
/// </summary>
/// <typeparam name="TResponse">The type of the response returned from the delegate execution.</typeparam>
/// <returns>A <see cref="Task"/> that represents the asynchronous operation and contains the execution result of type <typeparamref name="TResponse"/>.</returns>
/// <remarks>
/// This delegate is used in the execution pipeline to chain interceptors and handlers.
/// Each interceptor can execute code before and after invoking the next delegate in the chain.
/// </remarks>
public delegate Task<TResponse> ActionExecutionDelegate<TResponse>();

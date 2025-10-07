namespace FortyOne.OrchestratR;

/// <summary>
/// Represents a delegate that encapsulates the next step in the request processing pipeline.
/// </summary>
public delegate Task<TResponse> NextDelegate<TResponse>();

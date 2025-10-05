namespace FortyOne.OrchestratR;

/// <summary>
/// Serves as the base marker interface for all action requests in the orchestration system.
/// </summary>
/// <remarks>
/// This interface establishes a common type hierarchy for request objects,
/// allowing for generic handling and processing in the orchestration pipeline.
/// It does not define any members but serves as a type constraint for the request handling system.
/// </remarks>
public interface IActionRequestBase
{
}

/// <summary>
/// Represents an action request that does not expect a specific response.
/// </summary>
/// <remarks>
/// Implementations of this interface represent commands or actions in a CQRS-like pattern
/// that perform operations without returning specific data. These requests are typically
/// processed by <see cref="IActionHandler{TRequest}"/> implementations.
/// </remarks>
public interface IActionRequest : IActionRequestBase
{
}

/// <summary>
/// Represents an action request that expects a response of type <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TResponse">The type of response expected from processing this request.</typeparam>
/// <remarks>
/// Implementations of this interface represent queries or commands in a CQRS-like pattern
/// that return specific data after processing. These requests are typically handled by
/// <see cref="IActionHandler{TRequest, TResponse}"/> implementations.
/// </remarks>
public interface IActionRequest<TResponse> : IActionRequestBase
{
}

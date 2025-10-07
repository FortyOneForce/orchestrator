namespace FortyOne.OrchestratR;

/// <summary>
/// Represents a command in the CQRS pattern that performs an operation but doesn't return a value.
/// Commands are typically used to change state within the system and can be processed by a single handler.
/// </summary>
public interface ICommand : IRequest, ICommandBase
{
}

/// <summary>
/// Represents a command in the CQRS pattern that performs an operation and returns a response of type <typeparamref name="TResponse"/>.
/// Commands are typically used to change state within the system and can be processed by a single handler.
/// </summary>
public interface ICommand<out TResponse> : IRequest<TResponse>, ICommandBase
{
}

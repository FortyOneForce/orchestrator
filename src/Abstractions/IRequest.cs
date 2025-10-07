namespace FortyOne.OrchestratR;

/// <summary>
/// Represents a base request in the mediator pattern that doesn't return a value.
/// </summary>
public interface IRequest : IRequestBase
{
}

/// <summary>
/// Represents a base request in the mediator pattern that returns a response of type <typeparamref name="TResponse"/>.
/// </summary>
public interface IRequest<out TResponse> : IRequestBase
{
}
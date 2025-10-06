namespace FortyOne.OrchestratR;

public interface IRequest : IRequestBase
{
}

public interface IRequest<out TResponse> : IRequestBase
{
}
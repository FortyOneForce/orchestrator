namespace FortyOne.OrchestratR;

public interface ICommand : IRequest, ICommandBase
{
}

public interface ICommand<out TResponse> : IRequest<TResponse>, ICommandBase
{
}

namespace FortyOne.OrchestratR;

public interface IQuery<out TResponse> : IRequest<TResponse>, IQueryBase
{
}

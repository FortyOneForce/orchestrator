namespace FortyOne.OrchestratR;

/// <summary>
/// Represents a query in the CQRS pattern used to retrieve data from the system.
/// </summary>
public interface IQuery<out TResponse> : IRequest<TResponse>, IQueryBase
{
}

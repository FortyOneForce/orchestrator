#pragma warning disable IDE0130
namespace FortyOne.OrchestratR;
#pragma warning restore IDE0130

public interface IRequestOrchestrator
{
    Task ExecuteAsync(IRequest request, CancellationToken cancellationToken = default);
    Task ExecuteAsync(IRequest request, Action<IRequestExecutionMiddleware> middleware, CancellationToken cancellationToken = default);


    Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request, Action<IRequestExecutionMiddleware> middleware, CancellationToken cancellationToken = default);
}

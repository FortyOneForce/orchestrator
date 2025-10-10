#pragma warning disable IDE0130
namespace FortyOne.OrchestratR;
#pragma warning restore IDE0130

public interface IRequestOrchestrator
{
    Task SendAsync(IRequest request, CancellationToken cancellationToken = default);
    Task SendAsync(IRequest request, Action<IRequestExecutionMiddleware> middleware, CancellationToken cancellationToken = default);


    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, Action<IRequestExecutionMiddleware> middleware, CancellationToken cancellationToken = default);
}

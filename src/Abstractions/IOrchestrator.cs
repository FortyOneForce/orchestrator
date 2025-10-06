namespace FortyOne.OrchestratR;


public interface IOrchestrator
{
    Task ExecuteAsync(IRequest request, CancellationToken cancellationToken = default);
    Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    Task SendAsync(ICommand request, CancellationToken cancellationToken = default);
    Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> request, CancellationToken cancellationToken = default);
    Task<TResponse> QueryAsync<TResponse>(IQuery<TResponse> request, CancellationToken cancellationToken = default);
    Task NotifyAsync(INotification notification, CancellationToken cancellationToken = default);
}

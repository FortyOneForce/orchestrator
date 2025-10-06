namespace FortyOne.OrchestratR;

internal class Orchestrator : IOrchestrator
{
    private readonly RequestExecutor _requestExecutor;
    private readonly NotificationPublisher _notificationPublisher;
    public Orchestrator(
        RequestExecutor requestExecutor,
        NotificationPublisher notificationPublisher)
    {
        _requestExecutor = requestExecutor;
        _notificationPublisher = notificationPublisher;
    }

    public Task ExecuteAsync(IRequest request, CancellationToken cancellationToken = default)
        => _requestExecutor.ExecuteAsync(request, cancellationToken);

    public Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        => _requestExecutor.ExecuteAsync(request, cancellationToken);

    public Task<TResponse> QueryAsync<TResponse>(IQuery<TResponse> request, CancellationToken cancellationToken = default)
        => _requestExecutor.ExecuteAsync(request, cancellationToken);
    
    public Task SendAsync(ICommand request, CancellationToken cancellationToken = default)
        => _requestExecutor.ExecuteAsync(request, cancellationToken);

    public Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> request, CancellationToken cancellationToken = default)
        => _requestExecutor.ExecuteAsync(request, cancellationToken);

    public Task NotifyAsync(INotification notification, CancellationToken cancellationToken = default)
        => _notificationPublisher.NotifyAsync(notification, cancellationToken);
}

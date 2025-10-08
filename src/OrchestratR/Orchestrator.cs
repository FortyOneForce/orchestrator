
using System.Transactions;

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
    public async Task ExecuteBatchAsync(IEnumerable<IRequest> requests, RequestProcessingOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(requests);

        var isTransactional = options.HasFlag(RequestProcessingOptions.Transactional);
        using var transaction = isTransactional ? new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled) : null;

        foreach(var request in requests)
        {
            await _requestExecutor.ExecuteAsync(request, cancellationToken);
        }

        transaction?.Complete();
    }

    public Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        => _requestExecutor.ExecuteAsync(request, cancellationToken);

    public async Task<BatchProcessingResponse<TResponse>> ExecuteBatchAsync<TResponse>(IEnumerable<IRequest<TResponse>> requests, RequestProcessingOptions options, CancellationToken cancellationToken = default)
        => await ExecuteBatchAsync(requests, options, _ => false, cancellationToken);

    public async Task<BatchProcessingResponse<TResponse>> ExecuteBatchAsync<TResponse>(IEnumerable<IRequest<TResponse>> requests, RequestProcessingOptions options, Func<TResponse, bool> failurePredicate, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(requests);
        ArgumentNullException.ThrowIfNull(failurePredicate);

        var result = new BatchProcessingResponse<TResponse>();

        var isTransactional = options.HasFlag(RequestProcessingOptions.Transactional);
        using var transaction = isTransactional ? new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled) : null;

        foreach (var request in requests)
        {
            var response = await _requestExecutor.ExecuteAsync(request, cancellationToken);

            if (failurePredicate(response))
            {
                result.FailedResponse = response;
                return result;
            }

            result.Add(response);
        }

        transaction?.Complete();

        return result;
    }

    public Task<TResponse> QueryAsync<TResponse>(IQuery<TResponse> request, CancellationToken cancellationToken = default)
        => _requestExecutor.ExecuteAsync(request, cancellationToken);
    
    public Task SendAsync(ICommand request, CancellationToken cancellationToken = default)
        => _requestExecutor.ExecuteAsync(request, cancellationToken);

    public Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> request, CancellationToken cancellationToken = default)
        => _requestExecutor.ExecuteAsync(request, cancellationToken);

    public Task NotifyAsync(INotification notification, CancellationToken cancellationToken = default)
        => NotifyAsync(notification, NotificationProcessingOptions.None, cancellationToken);

    public Task NotifyAsync(INotification notification, NotificationProcessingOptions options, CancellationToken cancellationToken = default)
        => _notificationPublisher.NotifyAsync(notification, options, cancellationToken);

    
}

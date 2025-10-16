using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace FortyOne.OrchestratR;

internal sealed class Orchestrator : IOrchestrator
{
    private readonly static ConcurrentDictionary<Type, Type> _proxyTypeCache = new();
    private readonly static ConcurrentDictionary<Type, object> _requestProxyInstances = new();
    private readonly static ConcurrentDictionary<Type, bool> _notHandledNotifications = new();

    private readonly IServiceProvider _serviceProvider;
    public Orchestrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    #region [ Creators ]

    private RequestExecutionParameters? CreateRequestExecutionParameters(Action<IRequestExecutionParameters>? action)
    {
        if (action is null)
        {
            return null;
        }

        var instance = new RequestExecutionParameters();
        action(instance);

        return instance;
    }

    private NotificationExecutionParameters? CreateNotificationExecutionParameters(Action<INotificationExecutionParameters>? action)
    {
        if (action is null)
        {
            return null;
        }

        var instance = new NotificationExecutionParameters();
        action(instance);

        return instance;
    }

    private CancellationTokenSource CreateCancellationTokenSource(CancellationToken cancellationToken, TimeSpan? timeout)
    {
        var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        if (timeout != null && timeout != TimeSpan.Zero)
        {
            cts.CancelAfter(timeout.Value);
        }

        return cts;
    }

    #endregion

    #region [ IRequestOrchestrator Members ]

    // IRequest

    public Task SendAsync(IRequest request, CancellationToken cancellationToken = default)
        => DispatchRequest(request, null, cancellationToken);

    public Task SendAsync(IRequest request, Action<IRequestExecutionParameters> parameters, CancellationToken cancellationToken = default)
        => DispatchRequest(request, CreateRequestExecutionParameters(parameters), cancellationToken);

    // IRequest<TResponse>

    public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        => DispatchRequest(request, null, cancellationToken);

    public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, Action<IRequestExecutionParameters> parameters, CancellationToken cancellationToken = default)
        => DispatchRequest(request, CreateRequestExecutionParameters(parameters), cancellationToken);

    #endregion

    #region [ INotificationOrchestrator Members ]

    public Task NotifyAsync(INotification notification, CancellationToken cancellationToken = default)
        => DispatchNotification(notification, null, cancellationToken);

    public Task NotifyAsync(INotification notification, Action<INotificationExecutionParameters> parameters, CancellationToken cancellationToken = default)
        => DispatchNotification(notification, CreateNotificationExecutionParameters(parameters), cancellationToken);

    #endregion

    #region [ Dispatch Methods ] 

    private async Task DispatchNotification<TNotification>(
        TNotification notification,
        NotificationExecutionParameters? parameters,
        CancellationToken cancellationToken = default) where TNotification : INotification
    {
        ArgumentNullException.ThrowIfNull(notification);

        using var cts = CreateCancellationTokenSource(cancellationToken, parameters?.Timeout);

        var notificationType = notification.GetType();
        if (_notHandledNotifications.ContainsKey(notificationType))
        {
            return;
        }

        var enumerableProxyType = _proxyTypeCache.GetOrAdd(notificationType, (key) => typeof(IEnumerable<>).MakeGenericType(typeof(NotificationHandlerProxy<>).MakeGenericType(key)));
        var proxyInstances = ((IEnumerable<INotificationHandlerProxy>)_serviceProvider.GetRequiredService(enumerableProxyType)).ToArray();


        if (proxyInstances.Length == 0)
        {
            _notHandledNotifications.TryAdd(notificationType, true);
            return;
        }

        if (parameters?.SequentialExecution == true)
        {
            for (int i = 0; i < proxyInstances.Length; i++)
            {
                await proxyInstances[i].ProxyHandleAsync(notification, parameters, cts.Token);
            }
        }
        else
        {
            await Task.WhenAll(proxyInstances.Select(i => i.ProxyHandleAsync(notification, parameters, cts.Token)));
        }
    }

    private async Task DispatchRequest(IRequest request, RequestExecutionParameters? parameters, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var cts = CreateCancellationTokenSource(cancellationToken, parameters?.Timeout);

        var requestType = request.GetType();
        var proxyInstance = (IRequestHandlerProxy)_requestProxyInstances.GetOrAdd(requestType, (key) =>
        {
            var proxyType = typeof(RequestHandlerProxy<>).MakeGenericType(key);
            return _serviceProvider.GetRequiredService(proxyType);
        });

        await proxyInstance.ProxyHandleAsync(_serviceProvider, request, parameters, cts.Token);
    }

    private async Task<TResponse> DispatchRequest<TResponse>(IRequest<TResponse> request, RequestExecutionParameters? parameters, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);


        using var cts = CreateCancellationTokenSource(cancellationToken, parameters?.Timeout);

        var requestType = request.GetType();
        var proxyInstance = (IRequestHandlerProxy<TResponse>)_requestProxyInstances.GetOrAdd(requestType, (key) =>
        {
            var proxyType = typeof(RequestHandlerProxy<,>).MakeGenericType(key, typeof(TResponse));
            return _serviceProvider.GetRequiredService(proxyType);
        });

        var response = await proxyInstance.ProxyHandleAsync(_serviceProvider, request, parameters, cts.Token);
        return response;
    }

    #endregion
}

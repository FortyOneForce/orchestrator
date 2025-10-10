using FortyOne.OrchestratR.HandlerProxies;
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

    private RequestExecutionMiddleware? CreateRequestMiddleware(Action<IRequestExecutionMiddleware>? action)
    {
        if (action is null)
        {
            return null;
        }

        var instance = new RequestExecutionMiddleware();
        action(instance);

        return instance;
    }

    private NotificationExecutionMiddleware? CreateNotificationMiddleware(Action<INotificationExecutionMiddleware>? action)
    {
        if (action is null)
        {
            return null;
        }

        var instance = new NotificationExecutionMiddleware();
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

    public Task ExecuteAsync(IRequest request, CancellationToken cancellationToken = default)
        => DispatchRequest(request, null, cancellationToken);

    public Task ExecuteAsync(IRequest request, Action<IRequestExecutionMiddleware> middleware, CancellationToken cancellationToken = default)
        => DispatchRequest(request, CreateRequestMiddleware(middleware), cancellationToken);

    // IRequest<TResponse>

    public Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        => DispatchRequest(request, null, cancellationToken);

    public Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request, Action<IRequestExecutionMiddleware> middleware, CancellationToken cancellationToken = default)
        => DispatchRequest(request, CreateRequestMiddleware(middleware), cancellationToken);

    #endregion

    #region [ INotificationOrchestrator Members ]

    public Task NotifyAsync(INotification notification, CancellationToken cancellationToken = default)
        => DispatchNotification(notification, null, cancellationToken);

    public Task NotifyAsync(INotification notification, Action<INotificationExecutionMiddleware> middleware, CancellationToken cancellationToken = default)
        => DispatchNotification(notification, CreateNotificationMiddleware(middleware), cancellationToken);

    #endregion

    #region [ Dispatch Methods ] 

    private async Task DispatchNotification<TNotification>(
        TNotification notification,
        NotificationExecutionMiddleware? middleware,
        CancellationToken cancellationToken = default) where TNotification : INotification
    {
        ArgumentNullException.ThrowIfNull(notification);

        using var cts = CreateCancellationTokenSource(cancellationToken, middleware?.Timeout);

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

        if (middleware?.SequentialExecution == true)
        {
            for (int i = 0; i < proxyInstances.Length; i++)
            {
                await proxyInstances[i].ProxyHandleAsync(notification, middleware, cts.Token);
            }
        }
        else
        {
            await Task.WhenAll(proxyInstances.Select(i => i.ProxyHandleAsync(notification, middleware, cts.Token)));
        }
    }

    private async Task DispatchRequest(IRequest request, RequestExecutionMiddleware? middleware, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var cts = CreateCancellationTokenSource(cancellationToken, middleware?.Timeout);

        var requestType = request.GetType();
        var proxyInstance = (IRequestHandlerPropxy)_requestProxyInstances.GetOrAdd(requestType, (key) =>
        {
            var proxyType = typeof(RequestHandlerPropxy<>).MakeGenericType(key);
            return _serviceProvider.GetRequiredService(proxyType);
        });

        await proxyInstance.ProxyHandleAsync(_serviceProvider, request, middleware, cts.Token);
    }

    private async Task<TResponse> DispatchRequest<TResponse>(IRequest<TResponse> request, RequestExecutionMiddleware? middleware, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var cts = CreateCancellationTokenSource(cancellationToken, middleware?.Timeout);

        var requestType = request.GetType();
        var proxyInstance = (IRequestHandlerProxy<TResponse>)_requestProxyInstances.GetOrAdd(requestType, (key) =>
        {
            var proxyType = typeof(RequestHandlerProxy<,>).MakeGenericType(key, typeof(TResponse));
            return _serviceProvider.GetRequiredService(proxyType);
        });


        return await proxyInstance.ProxyHandleAsync(_serviceProvider, request, middleware, cts.Token);
    }

    #endregion
}

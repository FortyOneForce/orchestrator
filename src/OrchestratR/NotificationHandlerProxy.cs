namespace FortyOne.OrchestratR;

internal interface INotificationHandlerProxy
{
    Task ProxyHandleAsync(
        INotification notification,
        INotificationExecutionParameters? parameters,
        CancellationToken cancellationToken);
}

internal class NotificationHandlerProxy<TNotification> : INotificationHandlerProxy
    where TNotification : INotification
{
    private readonly INotificationHandler<TNotification> _notificationHandler;

    public NotificationHandlerProxy(INotificationHandler<TNotification> notificationHandler)
    {
        _notificationHandler = notificationHandler;
    }

    public Task ProxyHandleAsync(INotification notification, INotificationExecutionParameters? parameters, CancellationToken cancellationToken)
    {
        return _notificationHandler.HandleAsync((TNotification)notification, cancellationToken);
    }
}

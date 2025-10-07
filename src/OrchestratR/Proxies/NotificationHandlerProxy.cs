namespace FortyOne.OrchestratR.Proxies
{
    internal class NotificationHandlerProxy<TNotification> : INotificationHandler<TNotification>, INotificationHandlerProxy
        where TNotification : INotification
    {
        private readonly INotificationHandler<TNotification> _notificationHandler;
        public NotificationHandlerProxy(INotificationHandler<TNotification> notificationHandler)
        {
            _notificationHandler = notificationHandler;
        }

        public Task ProxyHandleAsync(INotification notification, CancellationToken cancellationToken)
        {
            return HandleAsync((TNotification)notification, cancellationToken);
        }

        public Task HandleAsync(TNotification notification, CancellationToken cancellationToken)
        {
            return _notificationHandler.HandleAsync(notification, cancellationToken);
        }
    }
}

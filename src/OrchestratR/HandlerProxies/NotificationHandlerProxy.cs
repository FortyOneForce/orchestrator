
namespace FortyOne.OrchestratR.HandlerProxies
{
    internal class NotificationHandlerProxy<TNotification> : INotificationHandlerProxy
        where TNotification : INotification
    {
        private readonly INotificationHandler<TNotification> _notificationHandler;

        public NotificationHandlerProxy(INotificationHandler<TNotification> notificationHandler)
        {
            _notificationHandler = notificationHandler;
        }

        public Task ProxyHandleAsync(INotification notification, INotificationExecutionMiddleware? middleware, CancellationToken cancellationToken)
        {
            return _notificationHandler.HandleAsync((TNotification)notification, cancellationToken);
        }
    }
}

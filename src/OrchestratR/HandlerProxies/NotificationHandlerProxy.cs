
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

        public async Task ProxyHandleAsync(ExecutionNode? parentNode, INotification notification, INotificationExecutionMiddleware? middleware , CancellationToken cancellationToken)
        {
            var node = parentNode?.AddExecution(ExecutionOperation.FinalHandlerExecution, _notificationHandler.GetType());
            var failed = true;

            try
            {
                await _notificationHandler.HandleAsync((TNotification)notification, cancellationToken);
                failed = false;
            }
            finally
            {
                node?.Complete(failed);
            }
        }
    }
}

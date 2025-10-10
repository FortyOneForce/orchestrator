namespace FortyOne.OrchestratR.HandlerProxies
{
    internal interface INotificationHandlerProxy
    {
        Task ProxyHandleAsync(
            INotification notification, 
            INotificationExecutionMiddleware? middleware, 
            CancellationToken cancellationToken);
    }
}

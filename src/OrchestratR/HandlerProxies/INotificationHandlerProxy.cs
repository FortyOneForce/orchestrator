namespace FortyOne.OrchestratR.HandlerProxies
{
    internal interface INotificationHandlerProxy
    {
        Task ProxyHandleAsync(
            ExecutionNode? parentNode,
            INotification notification, 
            INotificationExecutionMiddleware? middleware, 
            CancellationToken cancellationToken);
    }
}

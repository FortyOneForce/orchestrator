namespace FortyOne.OrchestratR.Proxies
{
    internal interface INotificationHandlerProxy
    {
        Task ProxyHandleAsync(INotification notification, CancellationToken cancellationToken);
    }
}

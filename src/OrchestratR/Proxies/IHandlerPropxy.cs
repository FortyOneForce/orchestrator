namespace FortyOne.OrchestratR.Proxies
{
    internal interface IHandlerPropxy
    {
        Task ProxyHandleAsync(IRequest request, CancellationToken cancellationToken);
    }
}

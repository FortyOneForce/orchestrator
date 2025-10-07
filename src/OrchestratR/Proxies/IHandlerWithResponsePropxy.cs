namespace FortyOne.OrchestratR.Proxies
{

    internal interface IHandlerWithResponsePropxy<TResponse>
    {
        Task<TResponse> ProxyHandleAsync(IRequest<TResponse> request, CancellationToken cancellationToken);
    }
}

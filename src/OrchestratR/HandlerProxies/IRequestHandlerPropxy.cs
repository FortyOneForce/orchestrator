namespace FortyOne.OrchestratR.HandlerProxies
{
    internal interface IRequestHandlerPropxy
    {
        Task ProxyHandleAsync(IServiceProvider serviceProvider, IRequest request, IRequestExecutionMiddleware? middleware, CancellationToken cancellationToken);
    }

    internal interface IRequestHandlerProxy<TResponse>
    {
        Task<TResponse> ProxyHandleAsync(IServiceProvider serviceProvider, IRequest<TResponse> request, IRequestExecutionMiddleware? middleware, CancellationToken cancellationToken);
    }
}

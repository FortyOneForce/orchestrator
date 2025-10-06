
namespace FortyOne.OrchestratR.Proxies
{

    internal class HandlerWithResponseProxy<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>, IHandlerWithResponsePropxy<TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly InterceptorFactory _interceptorFactory;
        private readonly IRequestHandler<TRequest, TResponse> _requestHandler;
        public HandlerWithResponseProxy(
            InterceptorFactory interceptorFactory,
            IRequestHandler<TRequest, TResponse> actionHandler)
        {
            _interceptorFactory = interceptorFactory;
            _requestHandler = actionHandler;
        }
        public Task<TResponse> ProxyHandleAsync(IRequest<TResponse> request, CancellationToken cancellationToken)
        {
            return HandleAsync((TRequest)request, cancellationToken);
        }

        public async Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken)
        {
            return await _requestHandler.HandleAsync(request, cancellationToken);
        }
    }
}

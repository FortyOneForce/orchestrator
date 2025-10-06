
namespace FortyOne.OrchestratR.Proxies
{
    internal class HandlerPropxy<TRequest> : IRequestHandler<TRequest>, IHandlerPropxy
        where TRequest : IRequest
    {
        private readonly InterceptorFactory _interceptorFactory;
        private readonly IRequestHandler<TRequest> _requestHandler;
        public HandlerPropxy(
            InterceptorFactory interceptorFactory,
            IRequestHandler<TRequest> actionHandler)
        {
            _interceptorFactory = interceptorFactory;
            _requestHandler = actionHandler;
        }

        public Task ProxyHandleAsync(IRequest request, CancellationToken cancellationToken)
        {
            return HandleAsync((TRequest)request, cancellationToken);
        }

        public async Task HandleAsync(TRequest request, CancellationToken cancellationToken)
        {
            await _requestHandler.HandleAsync(request, cancellationToken);
        }
    }
}


using Microsoft.Extensions.DependencyInjection;

namespace FortyOne.OrchestratR.Proxies
{
    internal class HandlerPropxy<TRequest> : IRequestHandler<TRequest>, IHandlerPropxy
        where TRequest : IRequest
    {
        private readonly InterceptorRegistry _interceptorRegistry;
        private readonly IRequestHandler<TRequest> _requestHandler;
        private readonly IServiceProvider _serviceProvider;
        public HandlerPropxy(
            InterceptorRegistry interceptorRegistry,
            IRequestHandler<TRequest> requestHandler,
            IServiceProvider serviceProvider)
        {
            _interceptorRegistry = interceptorRegistry;
            _requestHandler = requestHandler;
            _serviceProvider = serviceProvider;
        }

        public Task ProxyHandleAsync(IRequest request, CancellationToken cancellationToken)
        {
            return HandleAsync((TRequest)request, cancellationToken);
        }

        public Task HandleAsync(TRequest request, CancellationToken cancellationToken)
        {
            var reversedInterceptorTypes = _interceptorRegistry.GetReversedInterceptors<TRequest, VoidResponse>();
            if (reversedInterceptorTypes.Length == 0)
            {
                return _requestHandler.HandleAsync(request, cancellationToken);
            }
             
            NextDelegate<VoidResponse> next = async () =>
            {
                await _requestHandler.HandleAsync(request, cancellationToken);
                return VoidResponse.NotTerminated;
            };

            for (int i = 0; i < reversedInterceptorTypes.Length; i++)
            {
                var interceptorType = reversedInterceptorTypes[i];

                var currentNext = next;

                next = () =>
                {
                    var interceptor = (IRequestInterceptor<TRequest, VoidResponse>)_serviceProvider.GetRequiredService(interceptorType);
                    return interceptor.HandleAsync(request, currentNext, cancellationToken);
                };

            }

            return next();
        }
    }
}

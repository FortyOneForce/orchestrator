
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace FortyOne.OrchestratR.Proxies
{

    internal class HandlerWithResponseProxy<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>, IHandlerWithResponsePropxy<TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly InterceptorRegistry _interceptorRegistry;
        private readonly IRequestHandler<TRequest, TResponse> _requestHandler;
        private readonly IServiceProvider _serviceProvider;
        public HandlerWithResponseProxy(
            InterceptorRegistry interceptorRegistry,
            IRequestHandler<TRequest, TResponse> actionHandler,
            IServiceProvider serviceProvider)
        {
            _interceptorRegistry = interceptorRegistry;
            _requestHandler = actionHandler;
            _serviceProvider = serviceProvider;
        }
        public Task<TResponse> ProxyHandleAsync(IRequest<TResponse> request, CancellationToken cancellationToken)
        {
            return HandleAsync((TRequest)request, cancellationToken);
        }

        public Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken)
        {
            var reversedInterceptorTypes = _interceptorRegistry.GetReversedInterceptors<TRequest, TResponse>();
            if (reversedInterceptorTypes.Length == 0)
            {
                return _requestHandler.HandleAsync(request, cancellationToken);
            }

            NextDelegate<TResponse> next = () => _requestHandler.HandleAsync(request, cancellationToken);

            for(int i = 0; i < reversedInterceptorTypes.Length; i++)
            {
                var interceptorType = reversedInterceptorTypes[i];

                var currentNext = next;

                next = () =>
                {
                    var interceptor = (IRequestInterceptor<TRequest, TResponse>)_serviceProvider.GetRequiredService(interceptorType);
                    return interceptor.HandleAsync(request, currentNext, cancellationToken);
                };

            }

            return next();
        }
    }
}

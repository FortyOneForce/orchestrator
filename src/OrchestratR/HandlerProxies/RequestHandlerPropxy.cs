
using Microsoft.Extensions.DependencyInjection;

namespace FortyOne.OrchestratR.HandlerProxies
{
    internal class RequestHandlerPropxy<TRequest> : IRequestHandlerPropxy
        where TRequest : IRequest
    {
        private readonly InterceptorRegistry _interceptorRegistry;
        public RequestHandlerPropxy(InterceptorRegistry interceptorRegistry)
        {
            _interceptorRegistry = interceptorRegistry;
        }

        public Task ProxyHandleAsync(IServiceProvider serviceProvider, IRequest request, IRequestExecutionMiddleware? middleware, CancellationToken cancellationToken)
        {
            var handler = serviceProvider.GetRequiredService<IRequestHandler<TRequest>>();

            var reversedInterceptorTypes = _interceptorRegistry.GetReversedInterceptors<TRequest, VoidResponse>();
            if (reversedInterceptorTypes.Length == 0)
            {
                return handler.HandleAsync((TRequest)request, cancellationToken);
            }

            NextDelegate<VoidResponse> next = async () =>
            {
                await handler.HandleAsync((TRequest)request, cancellationToken);
                return VoidResponse.NotTerminated;
            };

            for (int i = 0; i < reversedInterceptorTypes.Length; i++)
            {
                var interceptorType = reversedInterceptorTypes[i];
                var currentNext = next;

                next = () =>
                {
                    var interceptor = (IRequestInterceptor<TRequest, VoidResponse>)serviceProvider.GetRequiredService(interceptorType);
                    return interceptor.HandleAsync((TRequest)request, currentNext, cancellationToken);
                };
            }

            return next();
        }
    }

    internal class RequestHandlerProxy<TRequest, TResponse> : IRequestHandlerProxy<TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly InterceptorRegistry _interceptorRegistry;

        public RequestHandlerProxy(InterceptorRegistry interceptorRegistry)
        {
            _interceptorRegistry = interceptorRegistry;
        }
        public Task<TResponse> ProxyHandleAsync(IServiceProvider serviceProvider, IRequest<TResponse> request, IRequestExecutionMiddleware? middleware, CancellationToken cancellationToken)
        {
            var handler = serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();

            var reversedInterceptorTypes = _interceptorRegistry.GetReversedInterceptors<TRequest, TResponse>();
            if (reversedInterceptorTypes.Length == 0)
            {
                return handler.HandleAsync((TRequest)request, cancellationToken);
            }

            NextDelegate<TResponse> next = () =>
            {
                return handler.HandleAsync((TRequest)request, cancellationToken);  
            };

            for (int i = 0; i < reversedInterceptorTypes.Length; i++)
            {
                var interceptorType = reversedInterceptorTypes[i];

                var currentNext = next;

                next = () =>
                {

                    var interceptor = (IRequestInterceptor<TRequest, TResponse>)serviceProvider.GetRequiredService(interceptorType);
                    return interceptor.HandleAsync((TRequest)request, currentNext, cancellationToken);

                };

            }

            return next();
        }


    }
}

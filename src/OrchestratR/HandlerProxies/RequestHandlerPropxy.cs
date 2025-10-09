
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

        public async Task ProxyHandleAsync(ExecutionNode? parent,IServiceProvider serviceProvider, IRequest request, IRequestExecutionMiddleware? middleware, CancellationToken cancellationToken)
        {
            var handler = serviceProvider.GetRequiredService<IRequestHandler<TRequest>>();

            var reversedInterceptorTypes = _interceptorRegistry.GetReversedInterceptors<TRequest, VoidResponse>();
            if (reversedInterceptorTypes.Length == 0)
            {
                parent?.AddExecution(ExecutionOperation.InterceptorExecution, this.GetType()).Complete(ExecutionState.Skipped);
                var node = parent?.AddExecution(ExecutionOperation.FinalHandlerExecution, handler.GetType());
                var isFailed = true;
                try
                {
                    await handler.HandleAsync((TRequest)request, cancellationToken);
                    isFailed = false;
                }
                finally
                {
                    node?.Complete(isFailed);
                }

                return;
            }

            var loopNode = parent?.AddExecution(ExecutionOperation.SequentialExecution, this.GetType());

            NextDelegate<VoidResponse> next = async () =>
            {
                var node = loopNode?.AddExecution(ExecutionOperation.FinalHandlerExecution, handler.GetType());
                var isFailed = true;

                try
                {
                    await handler.HandleAsync((TRequest)request, cancellationToken);
                    isFailed = false;
                    return VoidResponse.NotTerminated;
                }
                finally
                {
                    node?.Complete(isFailed);
                }
            };

            for (int i = 0; i < reversedInterceptorTypes.Length; i++)
            {
                var interceptorType = reversedInterceptorTypes[i];
                var currentNext = next;

                next = async () =>
                {
                    var node = loopNode?.AddExecution(ExecutionOperation.InterceptorExecution, interceptorType);
                    var isFailed = true;

                    try
                    {
                        var interceptor = (IRequestInterceptor<TRequest, VoidResponse>)serviceProvider.GetRequiredService(interceptorType);
                        var response = await interceptor.HandleAsync((TRequest)request, currentNext, cancellationToken);

                        isFailed = false;

                        return response;
                    }
                    finally
                    {
                        node?.Complete(isFailed);
                    }
                };

            }

            var overalFailed = true;
            try
            {
                await next();
                overalFailed = false;
            }
            finally
            {
                loopNode?.Complete(overalFailed);
            }
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
        public async Task<TResponse> ProxyHandleAsync(ExecutionNode? parent, IServiceProvider serviceProvider, IRequest<TResponse> request, IRequestExecutionMiddleware? middleware, CancellationToken cancellationToken)
        {
            var handler = serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();

            var reversedInterceptorTypes = _interceptorRegistry.GetReversedInterceptors<TRequest, TResponse>();
            if (reversedInterceptorTypes.Length == 0)
            {
                parent?.AddExecution(ExecutionOperation.InterceptorExecution, this.GetType()).Complete(ExecutionState.Skipped);
                var node = parent?.AddExecution(ExecutionOperation.FinalHandlerExecution, handler.GetType());

                var isFailed = true;
                try
                {
                    var response = await handler.HandleAsync((TRequest)request, cancellationToken);
                    isFailed = false;
                    return response;
                }
                finally
                {
                    node?.Complete(isFailed);
                }
            }
            var loopNode = parent?.AddExecution(ExecutionOperation.SequentialExecution, this.GetType());

            NextDelegate<TResponse> next = async () =>
            {
                var node = loopNode?.AddExecution(ExecutionOperation.FinalHandlerExecution, handler.GetType());

                var isFailed = true;
                try
                {
                    var response = await handler.HandleAsync((TRequest)request, cancellationToken);
                    isFailed = false;
                    return response;
                }
                finally
                {
                    node?.Complete(isFailed);
                }
            };

            for (int i = 0; i < reversedInterceptorTypes.Length; i++)
            {
                var interceptorType = reversedInterceptorTypes[i];

                var currentNext = next;

                next = async () =>
                {
                    var node = loopNode?.AddExecution(ExecutionOperation.InterceptorExecution, interceptorType);
                    var isFailed = true;
                    try
                    {
                        var interceptor = (IRequestInterceptor<TRequest, TResponse>)serviceProvider.GetRequiredService(interceptorType);
                        var response = await interceptor.HandleAsync((TRequest)request, currentNext, cancellationToken);
                        isFailed = false;

                        return response;
                    }
                    finally
                    {
                        node?.Complete(isFailed);
                    }
                };

            }

            var overalFailed = true;
            try
            {
                var response = await next();
                overalFailed = false;
                return response;
            }
            finally
            {
                loopNode?.Complete(overalFailed);
            }
        }


    }
}

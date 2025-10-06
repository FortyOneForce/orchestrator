using FortyOne.OrchestratR;
using MediatR;

namespace Benchmarks.Interceptors
{
    public class ExceptionHandlingInterceptor<TRequest, TResponse> : IRequestInterceptor<TRequest, TResponse>, IPipelineBehavior<TRequest, TResponse>
   
    {
        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            return next();
        }

        public Task<TResponse> HandleAsync(TRequest request, NextDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            return next();
        }
    }
}

using FortyOne.OrchestratR.Extensions;
using FortyOne.OrchestratR.Proxies;
using Microsoft.Extensions.DependencyInjection;

namespace FortyOne.OrchestratR
{
    internal class RequestExecutor
    {
        private readonly IServiceProvider _serviceProvider;
        public RequestExecutor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task ExecuteAsync(IRequest request, CancellationToken cancellationToken = default)
        {
            var proxyType = request.GetProxyType();
            var proxyInstance = (IHandlerPropxy)_serviceProvider.GetRequiredService(proxyType);

             return proxyInstance.ProxyHandleAsync(request, cancellationToken);
        }

        public Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            var proxyType = request.GetProxyType();
            var proxyInstance = (IHandlerWithResponsePropxy<TResponse>)_serviceProvider.GetRequiredService(proxyType);
            
            return proxyInstance.ProxyHandleAsync(request, cancellationToken);
        }
    }
}

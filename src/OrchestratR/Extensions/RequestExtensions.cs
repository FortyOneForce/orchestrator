using FortyOne.OrchestratR.Proxies;
using System.Collections.Concurrent;

namespace FortyOne.OrchestratR.Extensions
{
    internal static class RequestExtensions
    {
        private readonly static ConcurrentDictionary<Type, Type> _proxyTypeCache = new();

        public static Type GetProxyType(this IRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var requestType = request.GetType();
            return _proxyTypeCache.GetOrAdd(requestType, _ => typeof(HandlerPropxy<>).MakeGenericType(requestType));
        }

        public static Type GetProxyType<TResponse>(this IRequest<TResponse> request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var requestType = request.GetType();
            return _proxyTypeCache.GetOrAdd(requestType, _ => typeof(HandlerWithResponseProxy<,>).MakeGenericType(requestType, typeof(TResponse)));
        }
    }
}


using FortyOne.OrchestratR.Extensions.Markers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace FortyOne.OrchestratR.Extensions.Interceptors
{
    internal class DefaultCacheInterceptor<TRequest, TResponse> : IRequestInterceptor<TRequest, TResponse> where TRequest : ICacheableRequest
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger _logger;

        public DefaultCacheInterceptor(
            IMemoryCache memoryCache,
            ILogger<DefaultCacheInterceptor<TRequest, TResponse>> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<TResponse> HandleAsync(TRequest request, NextDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var cacheKey = $"{request.GetType().Name}:{request.CacheKey}";

            if (string.IsNullOrWhiteSpace(request.CacheKey))
            {
                _logger.LogWarning("Request of type {RequestType} has an empty CacheKey. Skipping cache.", request.GetType().Name);

                return await next();
            }

            if (_memoryCache.TryGetValue<TResponse>(cacheKey, out var cachedResponse) && cachedResponse is not null)
            {
                _logger.LogDebug("Cache hit for key {CacheKey}", cacheKey);

                return cachedResponse;
            }

            _logger.LogDebug("Cache miss for key {CacheKey}", cacheKey);

            var response = await next();

            if (response is not null)
            {
                _logger.LogDebug("Caching response for key {CacheKey}", cacheKey);

                _memoryCache.Set(cacheKey, response, request.CacheDuration ?? TimeSpan.FromMinutes(5));
            }

            return response;
        }
    }
}

namespace FortyOne.OrchestratR.Extensions.Markers;

/// <summary>
/// Marker interface to indicate that a request's response should be cached.
/// </summary>
public interface ICacheableRequest
{
    /// <summary>
    /// Key to use for caching the response.
    /// </summary>
    string CacheKey { get; }

    /// <summary>
    /// Expiration duration for the cached response. If null, a default duration will be used.
    /// </summary>
    TimeSpan? CacheDuration { get; }
}

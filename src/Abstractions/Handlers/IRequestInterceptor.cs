#pragma warning disable IDE0130
namespace FortyOne.OrchestratR;
#pragma warning restore IDE0130

/// <summary>
/// Defines an interceptor that can intercept and modify the processing of requests in the request pipeline.
/// </summary>
public interface IRequestInterceptor<in TRequest, TResponse> 
{
    /// <summary>
    /// Handles an intercepted request, potentially executing code before and after the next pipeline step.
    /// </summary>
    Task<TResponse> HandleAsync(TRequest request, NextDelegate<TResponse> next, CancellationToken cancellationToken);
}

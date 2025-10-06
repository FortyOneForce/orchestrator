namespace FortyOne.OrchestratR;

public interface IRequestInterceptor<in TRequest, TResponse> 
{
    Task<TResponse> HandleAsync(TRequest request, NextDelegate<TResponse> next, CancellationToken cancellationToken);
}

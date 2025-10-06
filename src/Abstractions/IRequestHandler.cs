namespace FortyOne.OrchestratR;


public interface IRequestHandler<in TRequest> : IHandlerBase where TRequest : IRequest
{

    Task HandleAsync(TRequest request, CancellationToken cancellationToken);
}


public interface IRequestHandler<in TRequest, TResponse> : IHandlerBase where TRequest: IRequest<TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
}

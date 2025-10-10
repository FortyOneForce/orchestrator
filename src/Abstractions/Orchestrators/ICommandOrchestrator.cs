#pragma warning disable IDE0130


namespace FortyOne.OrchestratR;
#pragma warning restore IDE0130

public interface ICommandOrchestrator
{
    Task<Result> ExecuteAsync(ICommand request, CancellationToken cancellationToken = default);
    Task<Result> ExecuteAsync(ICommand request, Action<IRequestExecutionMiddleware> middleware, CancellationToken cancellationToken = default);


    Task<Result<TResponse>> ExecuteAsync<TResponse>(ICommand<TResponse> request, CancellationToken cancellationToken = default) where TResponse: class;
    Task<Result<TResponse>> ExecuteAsync<TResponse>(ICommand<TResponse> request, Action<IRequestExecutionMiddleware> middleware, CancellationToken cancellationToken = default) where TResponse: class;
}

using FortyOne.OrchestratR.Extensions;
using FortyOne.OrchestratR.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FortyOne.OrchestratR.Extensions.Interceptors;

internal sealed class DefaultLoggingInterceptor<TRequest, TResponse> : IRequestInterceptor<TRequest, TResponse>
{
    private readonly DefaultInterceptorOptions _options;
    private readonly ILogger _logger;
    public DefaultLoggingInterceptor(
        DefaultInterceptorOptions options,
        ILogger<DefaultLoggingInterceptor<TRequest, TResponse>> logger)
    {
        _options = options;
        _logger = logger;
    }

    public async Task<TResponse> HandleAsync(TRequest request, NextDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestType = request!.GetType();

        if (_options.InterceptorLogLevel < LogLevel.Warning)
        {
            _logger.Log(_options.InterceptorLogLevel, "Handling request of type '{RequestType}'", requestType);
        }

        try
        {
            var response = await next();

            if (response is Result result)
            {
                if (result.IsFailure)
                {
                    _logger.Log(_options.FailedResultLogLevel,
                        "Request of type '{RequestType}' failed. Error: {ErrorMessage} [Code: {ErrorCode}]{ExceptionInfo}{NestedErrors}",
                        requestType,
                        result.Error.Message,
                        !string.IsNullOrEmpty(result.Error.Code) ? result.Error.Code : "None",
                        result.Error.Exception != null ? " Exception: " + result.Error.Exception.Message : string.Empty,
                        result.Error.Errors.Length > 0 ? $" ({result.Error.Errors.Length} nested errors)" : string.Empty);
                }
            }

            return response;

        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling request of type '{RequestType}'", requestType);

            throw;
        }
    }
}

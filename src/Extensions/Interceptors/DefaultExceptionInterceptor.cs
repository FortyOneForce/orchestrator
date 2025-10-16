using FortyOne.OrchestratR.Extensions.Extensions;
using FortyOne.OrchestratR.Extensions.Formatters;

namespace FortyOne.OrchestratR.Extensions.Interceptors;

internal sealed class DefaultExceptionInterceptor<TRequest, TResponse> : IRequestInterceptor<TRequest, TResponse>
{
    private readonly IErrorFormatter _errorFormatter;
    public DefaultExceptionInterceptor(IErrorFormatter errorFormatter)
    {
        _errorFormatter = errorFormatter;
    }

    public async Task<TResponse> HandleAsync(TRequest request, NextDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            var response = await next();
            return response;
        }
        catch(Exception ex)
        {
            var requestType = request!.GetType();
            var responseType = typeof(TResponse);

            var errorMessage = $"An unexpected error occurred while handling the request of type '{requestType}'.";

            var error = _errorFormatter
                    .TransformError(Error.Create(errorMessage).WithException(ex), requestType, responseType)
                    .WithExtension("source", "DefaultExceptionInterceptor");

            if (responseType.IsResultType())
            {
                return Result.Failure(error).Cast<TResponse>();
            }
            else if (responseType.IsGenericResultType())
            {
                return Result.Failure(responseType, error).Cast<TResponse>();
            }
            else
            {
                throw;
            }
        }
    }
}

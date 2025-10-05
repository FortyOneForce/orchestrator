using FortyOne.OrchestratR;

namespace Samples.WebApp.Interceptors
{
    public class ExceptionHandlingInterceptor<TRequest, TResponse> : IActionExecutionInterceptor<TRequest, TResponse>
    {
        private readonly ILogger _logger;
        public ExceptionHandlingInterceptor(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(TRequest));
        }

        public async Task<TResponse> HandleAsync(IActionExecutionContext<TRequest> context, ActionExecutionDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while handling the request of type '{RequestType}'", context.RequestType.FullName);

                throw;
            }
        }
    }
}

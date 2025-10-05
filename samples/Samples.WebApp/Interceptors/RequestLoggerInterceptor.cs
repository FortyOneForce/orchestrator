using FortyOne.OrchestratR;
using Newtonsoft.Json;
using Samples.WebApp.RequestMarkers;

namespace Samples.WebApp.Interceptors
{
    public class RequestLoggerInterceptor<TRequest, TResponse> : IActionExecutionInterceptor<TRequest, TResponse>
    {
        private readonly ILogger _logger;
        public RequestLoggerInterceptor(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(TRequest));
        }

        public Task<TResponse> HandleAsync(IActionExecutionContext<TRequest> context, ActionExecutionDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (context.RequestType.IsAssignableTo(typeof(ILoggableRequest)))
            {
                var requestData = JsonConvert.SerializeObject(context.Request, Formatting.None, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                _logger.LogDebug("Processing request {RequestType} with values {RequestData}", context.RequestType.FullName, requestData);
            }
            else
            {
                _logger.LogDebug("Processing request {RequestType}", context.RequestType.FullName);
            }

            return next();
        }
    }
}

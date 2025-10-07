using FortyOne.OrchestratR;
using Newtonsoft.Json;
using Samples.WebApp.RequestMarkers;

namespace Samples.WebApp.Interceptors
{
    public class RequestLoggerInterceptor<TRequest, TResponse> : IRequestInterceptor<TRequest, TResponse>
    {
        private readonly ILogger _logger;
        public RequestLoggerInterceptor(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(TRequest));
        }

        public Task<TResponse> HandleAsync(TRequest request, NextDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request.GetType().IsAssignableTo(typeof(ILoggableRequest)))
            {
                var requestData = JsonConvert.SerializeObject(request, Formatting.None, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                _logger.LogDebug("Processing request {RequestType} with values {RequestData}", request.GetType().FullName, requestData);
            }
            else
            {
                _logger.LogDebug("Processing request {RequestType}", request.GetType().FullName);
            }

            return next();
        }
    }
}

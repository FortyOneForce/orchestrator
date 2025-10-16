using FortyOne.OrchestratR.Extensions.Markers;
using Microsoft.Extensions.Logging;
using System.Transactions;

namespace FortyOne.OrchestratR.Extensions.Interceptors
{
    internal class DefaultTransactionInterceptor<TRequest, TResponse> : IRequestInterceptor<TRequest, TResponse> where TRequest : ITransactionalRequest
    {
        private readonly ILogger _logger;
        public DefaultTransactionInterceptor(ILogger<DefaultTransactionInterceptor<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> HandleAsync(TRequest request, NextDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestTypeName = request.GetType().Name;
            var transactionId = Guid.NewGuid();
            var option = request.TransactionScopeOption ?? TransactionScopeOption.Required;

            using (_logger.BeginScope(new Dictionary<string, object> { { "RequestType", requestTypeName }, { "TransactionId", transactionId } }))
            {
                _logger.LogDebug("Starting transaction (ScopeOption={TransactionScopeOption}) for request {RequestType}", option, requestTypeName);


                try
                {
                    using var transaction = new TransactionScope(option, TransactionScopeAsyncFlowOption.Enabled);

                    var response = await next();

                    if (response is Result result && result.IsFailure)
                    {
                        _logger.LogWarning("Transaction rolled back for request {RequestType} due to failed result", requestTypeName);
                    }
                    else
                    {
                        _logger.LogDebug("Transaction committed successfully for request {RequestType}", requestTypeName);

                        transaction.Complete();
                    }

                    return response;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Transaction rolled back for request {RequestType} due to unhandled exception: {ExceptionType}", requestTypeName, ex.GetType().Name);

                    throw;
                }
            }
        }
    }
}

using System.Transactions;

namespace FortyOne.OrchestratR.Extensions.Markers
{
    /// <summary>
    /// Marker interface to indicate that a request should be processed within a transaction.
    /// </summary>
    public interface ITransactionalRequest
    {
        /// <summary>
        /// Gets the transaction scope option to be used for the request.
        /// </summary>
        TransactionScopeOption? TransactionScopeOption { get; }
    }
}

namespace FortyOne.OrchestratR
{
    /// <summary>
    /// Represents the response from a batch processing operation, containing a list of individual responses and information about any failures.
    /// </summary>
    public class BatchProcessingResponse<TResponse> : List<TResponse>
    {
        /// <summary>
        /// Gets or sets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess => FailedResponse == null;

        /// <summary>
        /// Gets a value indicating whether the operation partially succeeded.
        /// </summary>
        public bool IsPartialSuccess => FailedResponse != null && Count > 0;


        /// <summary>
        /// Gets or sets a value indicating whether the operation has failed.
        /// </summary>
        public bool IsFailed => FailedResponse != null;
        /// <summary>
        /// Gets or sets the response object representing a failed operation.
        /// </summary>
        public TResponse? FailedResponse { get; set; }
    }
}

namespace FortyOne.OrchestratR
{
    /// <summary>
    /// Represents options for processing requests.
    /// </summary>
    [Flags]
    public enum RequestProcessingOptions
    {
        /// <summary>
        /// Represents the absence of a value or state.
        /// </summary>
        None = 0,
        /// <summary>
        /// Specifies that the operation should be performed in a transactional manner.
        /// </summary>
        Transactional = 1
    }
}

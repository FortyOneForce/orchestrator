namespace FortyOne.OrchestratR
{
    /// <summary>
    /// Represents a response value for requests that don't return data but need to participate in the interceptor pipeline.
    /// </summary>
    public readonly struct VoidResponse
    {
        /// <summary>
        /// Gets a value indicating whether the pipeline execution is terminated.
        /// </summary>
        public bool IsTerminated { get; }

        private VoidResponse(bool isTerminated)
        {
            IsTerminated = isTerminated;
        }

        internal static readonly VoidResponse NotTerminated = new(false);

        /// <summary>
        /// Represents a response that indicates the pipeline execution should terminate.
        /// Can be used by interceptors to signal that no further processing is needed.
        /// </summary>
        public static readonly VoidResponse Terminated = new(true);
    }
}

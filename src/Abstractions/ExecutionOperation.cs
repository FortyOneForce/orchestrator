namespace FortyOne.OrchestratR;

/// <summary>
/// Represents the kind of executor in the OrchestratR library.
/// </summary>
public enum ExecutionOperation
{
    Unknown,
    Orchestration,
    ResolveProxy,
    SequentialExecution,
    ParallelExecution,
    ProxyHandlerExecution,
    FinalHandlerExecution,
    InterceptorExecution,
}

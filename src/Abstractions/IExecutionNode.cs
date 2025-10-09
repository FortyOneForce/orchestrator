namespace FortyOne.OrchestratR
{

    /// <summary>
    /// Represents a single frame in the execution stack, providing information about the executor type associated with
    /// the frame.
    /// </summary>
    public interface IExecutionNode
    {
        Type? ExecutorType { get; }
        long StartedAt { get; }
        long CompletedAt { get; }
        List<IExecutionNode> Executions { get; }


        ExecutionOperation Operation { get; }
        ExecutionState State { get; }
    }
}

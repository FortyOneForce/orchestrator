
using System.Collections.Concurrent;
using System.Diagnostics;

namespace FortyOne.OrchestratR;

internal sealed class ExecutionNode : IExecutionTree
{
    public Type? ExecutorType { get; private set; }
    public long StartedAt { get; private set; } 
    public long CompletedAt { get; private set; } 
    public List<IExecutionNode> Executions { get; }
    public ExecutionOperation Operation { get; private set; } 
    public ExecutionState State { get; private set; }
    public TimeSpan Duration => TimeSpan.FromTicks(CompletedAt - StartedAt);

    public ExecutionNode(): this(null)
    {
    }

    public ExecutionNode(Type? executorType): this(ExecutionOperation.Unknown, executorType)
    {
    }

    public ExecutionNode(ExecutionOperation operation, Type? executorType)
    {
        var timestamp = Stopwatch.GetTimestamp();

        StartedAt = timestamp;
        CompletedAt = timestamp;
        Operation = operation;
        State = ExecutionState.Unknown;
        Executions = new List<IExecutionNode>();
        ExecutorType = executorType;
    }

    public ExecutionNode Start()
    {
        if (State != ExecutionState.Unknown)
            return this;

        StartedAt = Stopwatch.GetTimestamp();
        CompletedAt = StartedAt;
        Operation = ExecutionOperation.Orchestration;
        ExecutorType = typeof(Orchestrator);
        State = ExecutionState.Started;

        return this;
    }

    public void Complete(ExecutionState state = ExecutionState.Completed)
    {
        State = state;
        CompletedAt = Stopwatch.GetTimestamp();
    }

    public void Complete(bool failed)
    {
        if (failed)
        {
            Complete(ExecutionState.Failed);
        }
        else
        {
            Complete(ExecutionState.Completed);
        }
    }

    public ExecutionNode AddExecution(ExecutionOperation operation, Type? executorType = null)
    {
        lock (Executions)
        {
            var node = new ExecutionNode(operation, executorType ?? ExecutorType);
            Executions.Add(node);
            return node;
        }
    }

    public override string ToString()
    {
        return $"Operation: {Operation}, State: {State}, Duration: {Duration}";
    }
}

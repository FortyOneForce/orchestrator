#pragma warning disable IDE0130
using System.Diagnostics;

namespace FortyOne.OrchestratR;
#pragma warning restore IDE0130

internal sealed class NotificationExecutionMiddleware : INotificationExecutionMiddleware
{
    public bool SequentialExecution { get; private set; } = false;
    public ExecutionNode? ExecutionRootNode { get; private set; }

    public INotificationExecutionMiddleware UseExecutionTree(out IExecutionTree executionRootNode)
    {
        ExecutionRootNode = new ExecutionNode();
        executionRootNode = ExecutionRootNode;

        return this;
    }

    public INotificationExecutionMiddleware UseSequentialExecution(bool sequentialExecution = true)
    {
        SequentialExecution = sequentialExecution;
        return this;
    }
}

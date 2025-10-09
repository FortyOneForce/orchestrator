#pragma warning disable IDE0130
using System.Collections.Concurrent;

namespace FortyOne.OrchestratR;
#pragma warning restore IDE0130

public interface INotificationExecutionMiddleware
{
    INotificationExecutionMiddleware UseSequentialExecution(bool sequentialExecution = true);
    INotificationExecutionMiddleware UseExecutionTree(out IExecutionTree executionRootNode);
}

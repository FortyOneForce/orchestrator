#pragma warning disable IDE0130
using System.Diagnostics;

namespace FortyOne.OrchestratR;
#pragma warning restore IDE0130

internal sealed class NotificationExecutionMiddleware : INotificationExecutionMiddleware
{
    public bool SequentialExecution { get; private set; } = false;
    public TimeSpan? Timeout { get; private set; }

    public INotificationExecutionMiddleware UseSequentialExecution(bool sequentialExecution = true)
    {
        SequentialExecution = sequentialExecution;
        return this;
    }

    public INotificationExecutionMiddleware UseTimeout(TimeSpan timeout)
    {
        Timeout = timeout;
        return this;
    }
}

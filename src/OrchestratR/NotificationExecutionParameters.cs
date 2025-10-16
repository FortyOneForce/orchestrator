namespace FortyOne.OrchestratR;


internal sealed class NotificationExecutionParameters : INotificationExecutionParameters
{
    public bool SequentialExecution { get; private set; } = false;
    public TimeSpan? Timeout { get; private set; }

    public INotificationExecutionParameters UseSequentialExecution(bool sequentialExecution = true)
    {
        SequentialExecution = sequentialExecution;
        return this;
    }

    public INotificationExecutionParameters UseTimeout(TimeSpan timeout)
    {
        Timeout = timeout;
        return this;
    }
}

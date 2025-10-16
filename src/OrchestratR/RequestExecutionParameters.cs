namespace FortyOne.OrchestratR;


internal sealed class RequestExecutionParameters : IRequestExecutionParameters
{
    public TimeSpan? Timeout { get; private set; }

    public IRequestExecutionParameters UseTimeout(TimeSpan timeout)
    {
        Timeout = timeout;
        return this;
    }
}

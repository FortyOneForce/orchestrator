#pragma warning disable IDE0130

namespace FortyOne.OrchestratR;
#pragma warning restore IDE0130

internal sealed class RequestExecutionMiddleware : IRequestExecutionMiddleware
{
    public TimeSpan? Timeout { get; private set; }

    public IRequestExecutionMiddleware UseTimeout(TimeSpan timeout)
    {
        Timeout = timeout;
        return this;
    }
}

#pragma warning disable IDE0130
namespace FortyOne.OrchestratR;
#pragma warning restore IDE0130

/// <summary>
/// Represents parameters for executing a request.
/// </summary>
public interface IRequestExecutionParameters
{
    /// <summary>
    /// Sets a timeout for the request execution.
    /// </summary>
    IRequestExecutionParameters UseTimeout(TimeSpan timeout);
}

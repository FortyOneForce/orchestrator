#pragma warning disable IDE0130
namespace FortyOne.OrchestratR;
#pragma warning restore IDE0130

/// <summary>
/// Represents execution parameters for notifications.
/// </summary>
public interface INotificationExecutionParameters
{
    /// <summary>
    /// Sets whether the notification should be executed sequentially.
    /// </summary>
    INotificationExecutionParameters UseSequentialExecution(bool sequentialExecution = true);

    /// <summary>
    /// Sets a timeout for the notification execution.
    /// </summary>
    INotificationExecutionParameters UseTimeout(TimeSpan timeout);
}

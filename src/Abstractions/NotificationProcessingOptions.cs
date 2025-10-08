namespace FortyOne.OrchestratR;

/// <summary>
/// Defines the notification strategies that can be applied to an operation.
/// </summary>
[Flags]
public enum NotificationProcessingOptions
{
    /// <summary>
    /// Represents the absence of a value or state.
    /// </summary>
    None = 0,
    /// <summary>
    /// Specifies that the operation should be performed in a transactional manner.
    /// </summary>
    Transactional = 1,
    /// <summary>
    /// Specifies that the operation should be performed sequentially.
    /// </summary>
    Sequential = 2,
    /// <summary>
    /// Specifies that the operation should continue execution even if an error occurs.
    /// </summary>
    ContinueOnError = 4
}

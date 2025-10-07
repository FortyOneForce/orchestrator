namespace FortyOne.OrchestratR;

/// <summary>
/// Defines a handler for a specific notification type in the publish-subscribe pattern.
/// </summary>
public interface INotificationHandler<in TNotification> : IHandlerBase where TNotification : INotification
{
    /// <summary>
    /// Handles a notification of type <typeparamref name="TNotification"/>.
    /// </summary>
    Task HandleAsync(TNotification notification, CancellationToken cancellationToken);
}

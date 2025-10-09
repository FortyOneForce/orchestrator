#pragma warning disable IDE0130
namespace FortyOne.OrchestratR;
#pragma warning restore IDE0130

public interface INotificationOrchestrator
{
    /// <summary>
    /// Publishes a notification to all registered handlers.
    /// </summary>
    Task NotifyAsync(INotification notification, CancellationToken cancellationToken = default);
    Task NotifyAsync(INotification notification, Action<INotificationExecutionMiddleware> middleware, CancellationToken cancellationToken = default);
}

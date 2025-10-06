namespace FortyOne.OrchestratR;

public interface INotificationHandler<in TNotification> : IHandlerBase where TNotification : INotification
{

    Task HandleAsync(TNotification notification, CancellationToken cancellationToken);
}

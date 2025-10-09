
namespace FortyOne.OrchestratR;

/// <summary>
/// Specifies the category of handler within the OrchestratR implementation.
/// Used during component registration and handler resolution to differentiate between 
/// request processing handlers and notification subscribers.
/// </summary>
public enum HandlerKind
{
    /// <summary>
    /// Represents handlers that process requests and return results.
    /// These handlers typically implement <see cref="IRequestHandler{TRequest}"/> or 
    /// <see cref="IRequestHandler{TRequest, TResponse}"/> interfaces and are executed
    /// through the request pipeline.
    /// </summary>
    RequestHandler,

    /// <summary>
    /// Represents handlers that respond to notifications through an event-based model.
    /// These handlers typically implement <see cref="INotificationHandler{TNotification}"/> interface
    /// and are executed when corresponding notifications are published.
    /// </summary>
    NotificationHandler
}

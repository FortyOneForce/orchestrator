namespace FortyOne.OrchestratR;

/// <summary>
/// Defines the core orchestration service that coordinates the processing of requests and publishing of events.
/// </summary>
/// <remarks>
/// <para>
/// The orchestrator serves as the central coordination point in the application, handling both command/query
/// processing through <c>SendAsync</c> methods and event distribution through the <c>PublishAsync</c> method.
/// </para>
/// <para>
/// This interface follows the mediator pattern, decoupling the sender of a request/event from its handlers.
/// It provides a simplified API for dispatching requests to their appropriate handlers and distributing
/// event signals to all registered subscribers.
/// </para>
/// </remarks>
public interface IOrchestrator
{
    /// <summary>
    /// Sends a request to be processed by a single handler with no expected return value.
    /// </summary>
    /// <param name="request">The request to be processed.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <remarks>
    /// The request will be routed to the appropriate <see cref="IActionHandler{TRequest}"/> implementation.
    /// If multiple handlers are registered for the same request type, only one will be invoked based on
    /// dependency injection resolution rules.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when no handler is registered for the request type.</exception>
    Task SendAsync(IActionRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a request to be processed by a single handler and returns the result.
    /// </summary>
    /// <typeparam name="TResponse">The type of response expected from the request.</typeparam>
    /// <param name="request">The request to be processed.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="Task{TResponse}"/> representing the asynchronous operation with the result of type <typeparamref name="TResponse"/>.
    /// </returns>
    /// <remarks>
    /// The request will be routed to the appropriate <see cref="IActionHandler{TRequest,TResponse}"/> implementation.
    /// If multiple handlers are registered for the same request type, only one will be invoked based on
    /// dependency injection resolution rules.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when no handler is registered for the request type.</exception>
    Task<TResponse> SendAsync<TResponse>(IActionRequest<TResponse> request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes an event signal to all registered handlers.
    /// </summary>
    /// <param name="signal">The event signal to publish.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="Task{Int32}"/> representing the asynchronous operation with the number of handlers that processed the signal.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The signal will be distributed to all registered <see cref="IEventHandler{TSignal}"/> implementations
    /// that can handle the specific signal type. Unlike requests, which are processed by a single handler,
    /// event signals are processed by all matching handlers concurrently.
    /// </para>
    /// <para>
    /// The returned integer represents the number of handlers that processed the event.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="signal"/> is null.</exception>
    Task<int> PublishAsync(IEventSignal signal, CancellationToken cancellationToken = default);
}

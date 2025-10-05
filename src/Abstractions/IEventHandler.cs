namespace FortyOne.OrchestratR;

/// <summary>
/// Defines a handler for processing event signals in the orchestration system.
/// </summary>
/// <typeparam name="TSignal">The type of event signal to be handled.</typeparam>
/// <remarks>
/// <para>
/// Event handlers implement the observer pattern in the orchestration system, allowing
/// for decoupled communication between components. Multiple handlers can subscribe to
/// the same event signal type, enabling a one-to-many distribution of events.
/// </para>
/// <para>
/// Unlike action handlers which process requests in a one-to-one relationship, event handlers
/// operate in a publish-subscribe model where multiple handlers may process the same signal
/// concurrently. The orchestrator dispatches event signals to all registered handlers of the
/// matching type.
/// </para>
/// <para>
/// Event handlers are typically used for side effects, notifications, or maintaining
/// eventual consistency across system components.
/// </para>
/// </remarks>
public interface IEventHandler<TSignal> : IHandlerBase where TSignal : IEventSignal
{
    /// <summary>
    /// Processes the specified event signal asynchronously.
    /// </summary>
    /// <param name="signal">The event signal to process.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <remarks>
    /// <para>
    /// Implementations should contain the logic for reacting to the specific event signal.
    /// Multiple handlers for the same signal type will be executed, potentially in parallel.
    /// </para>
    /// <para>
    /// Handlers should be designed to be idempotent and tolerant of concurrent execution,
    /// as the same event signal may be processed by multiple handlers simultaneously.
    /// </para>
    /// <para>
    /// Exceptions thrown by one handler do not prevent other handlers from processing the same signal,
    /// but may be propagated to the caller of the orchestrator's PublishAsync method.
    /// </para>
    /// </remarks>
    Task HandleAsync(TSignal signal, CancellationToken cancellationToken);
}

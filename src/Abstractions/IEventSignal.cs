namespace FortyOne.OrchestratR;

/// <summary>
/// Represents an event notification in the orchestration system.
/// </summary>
/// <remarks>
/// <para>
/// Event signals are used in the publish-subscribe pattern to notify interested components
/// about something that has occurred in the system. Unlike action requests which expect
/// a direct response, event signals are dispatched to multiple handlers and typically
/// represent notifications about past events.
/// </para>
/// <para>
/// Event signals should be immutable data containers that describe what happened, including
/// relevant data but no behavior. They are processed by implementations of 
/// <see cref="IEventHandler{TSignal}"/> which subscribe to specific event types.
/// </para>
/// <para>
/// This marker interface establishes a common type hierarchy for all event signals,
/// allowing the orchestrator to identify and route them to appropriate handlers.
/// </para>
/// </remarks>
public interface IEventSignal
{
}

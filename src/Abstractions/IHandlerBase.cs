namespace FortyOne.OrchestratR;

/// <summary>
/// Serves as the common base marker interface for all handler types in the orchestration system.
/// </summary>
/// <remarks>
/// <para>
/// This interface establishes a unified type hierarchy for both action handlers and event handlers,
/// enabling consistent registration, discovery, and management of handlers throughout the system.
/// </para>
/// <para>
/// While <see cref="IHandlerBase"/> doesn't define any members, it's extended by specialized handler
/// interfaces such as <see cref="IActionHandler{TRequest}"/>, <see cref="IActionHandler{TRequest,TResponse}"/>,
/// and <see cref="IEventHandler{TSignal}"/>, each providing specific handling behaviors.
/// </para>
/// <para>
/// The orchestration system uses this common base type for:
/// <list type="bullet">
///   <li>Uniform registration of handlers in the dependency injection container</li>
///   <li>Assembly scanning and automatic discovery of handler implementations</li>
///   <li>Runtime resolution of appropriate handlers based on request/signal types</li>
/// </list>
/// </para>
/// </remarks>
public interface IHandlerBase
{
}

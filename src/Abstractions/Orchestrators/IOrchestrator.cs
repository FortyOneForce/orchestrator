#pragma warning disable IDE0130
namespace FortyOne.OrchestratR;
#pragma warning restore IDE0130

/// <summary>
/// Provides a central orchestrator for all request-response and publish-subscribe operations within the OrchestratR library.
/// </summary>
public interface IOrchestrator : 
    IRequestOrchestrator, 
    INotificationOrchestrator
{
}

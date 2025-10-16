using Microsoft.Extensions.Logging;

#pragma warning disable IDE0130
namespace FortyOne.OrchestratR.DependencyInjection;
#pragma warning restore IDE0130

/// <summary>
/// Provides configuration options for default interceptor behaviors.
/// </summary>
public interface IDefaultInterceptorOptions
{
    /// <summary>
    /// Defines the default logging level for interceptor operations.
    /// </summary>
    LogLevel InterceptorLogLevel { get; set; }

    /// <summary>
    /// Defines the logging level for failed interceptor operations.
    /// </summary>
    LogLevel FailedResultLogLevel { get; set; }
}

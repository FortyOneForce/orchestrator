using FortyOne.OrchestratR.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FortyOne.OrchestratR.Extensions.DependencyInjection;

internal class DefaultInterceptorOptions : IDefaultInterceptorOptions
{
    public LogLevel InterceptorLogLevel { get; set; } = LogLevel.Information;
    public LogLevel FailedResultLogLevel { get; set; } = LogLevel.Warning;
}

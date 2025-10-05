using System.Reflection;

namespace FortyOne.OrchestratR;

/// <summary>
/// Contains metadata and runtime instances required for dynamic action handler invocation.
/// </summary>
internal sealed class ResolvedActionHandler
{
    public Type HandlerType { get; set; } = null!;
    public Type RequestType { get; set; } = null!;
    public Type ResponseType { get; set; } = null!;
    public object HandlerInstance { get; set; } = null!;
    public MethodInfo HandleAsyncMethod { get; set; } = null!;
}

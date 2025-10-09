#pragma warning disable IDE0130
namespace FortyOne.OrchestratR;
#pragma warning restore IDE0130

public interface IRequestExecutionMiddleware
{
    IRequestExecutionMiddleware UseExecutionTree(out IExecutionTree executionRootNode);
}

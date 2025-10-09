#pragma warning disable IDE0130
namespace FortyOne.OrchestratR;
#pragma warning restore IDE0130

internal sealed class RequestExecutionMiddleware : IRequestExecutionMiddleware
{
    public ExecutionNode? ExecutionRootNode { get; private set; }

    public IRequestExecutionMiddleware UseExecutionTree(out IExecutionTree executionRootNode)
    {
        ExecutionRootNode = new ExecutionNode();
        executionRootNode = ExecutionRootNode;

        return this;
    }
}

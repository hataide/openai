using ThreadingProject.WorkflowEngine.TreeEngine.Models;

namespace ThreadingProject.WorkflowEngine.NodeHandlers;

public interface INodeHandler<T>
{
    public ValueTask<(bool ShouldDescend, string Message)> ExecuteAsync(AgenticNodeModel<T> node, CancellationToken ct = default);
}

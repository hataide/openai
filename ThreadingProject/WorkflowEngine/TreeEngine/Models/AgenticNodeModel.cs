namespace ThreadingProject.WorkflowEngine.TreeEngine.Models;

public enum NodeType
{
    Analyst,
    AnalystOrchestrator,
    Developer,
}

public class AgenticNodeModel<T>
{
    public T Value { get; init; }
    public NodeType Type { get; init; }
    public List<AgenticNodeModel<T>> Children { get; init; } = new();

    public AgenticNodeModel(T value, NodeType nodeType)
        => (Value, Type) = (value, nodeType);

    public AgenticNodeModel<T> AddChild(T value, NodeType nodeType)
    {
        var child = new AgenticNodeModel<T>(value, nodeType);
        Children.Add(child);

        return child;
    }
}

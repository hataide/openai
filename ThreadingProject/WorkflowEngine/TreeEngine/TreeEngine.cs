using System.Runtime.CompilerServices;
using ThreadingProject.Models;
using ThreadingProject.WorkflowEngine.NodeHandlers;
using ThreadingProject.WorkflowEngine.TreeEngine.Models;

namespace ThreadingProject.WorkflowEngine.TreeEngine;

public static class TreeEngineAsync
{
    public static async IAsyncEnumerable<(AgenticNodeModel<T> Node,
                                      bool ShouldDescend,
                                      string Message)> TraverseAsync<T>(
    AgenticNodeModel<T> root,
    IReadOnlyDictionary<NodeType, INodeHandler<T>> handlers,
    Action<Exception, AgenticNodeModel<T>>? onError = null,
    Action<AgenticNodeModel<T>, List<AgenticNodeModel<T>>>? propagateContext = null, // NEW!
    [EnumeratorCancellation] CancellationToken ct = default)
    {
        var stack = new Stack<AgenticNodeModel<T>>();
        stack.Push(root);

        while (stack.Count > 0)
        {
            ct.ThrowIfCancellationRequested();
            var node = stack.Pop();

            // Pick the handler – fall back to the first handler in the map
            if (!handlers.TryGetValue(node.Type, out var handler))
                handler = handlers.Values.First();   // or throw, your choice

            (bool ok, string msg) result;
            try
            {
                result = await handler.ExecuteAsync(node, ct).ConfigureAwait(false);
            }
            catch (Exception ex) when (onError is not null)
            {
                onError(ex, node);
                continue;
            }

            yield return (node, result.ok, result.msg);

            if (result.ok)
            {
                propagateContext?.Invoke(node, node.Children);

                for (int i = node.Children.Count - 1; i >= 0; i--)
                    stack.Push(node.Children[i]);
            }                
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="children"></param>
    public static void PropagateAgenticContext(AgenticNodeModel<AgenticPropertiesModel> parent, List<AgenticNodeModel<AgenticPropertiesModel>> children)
    {
        var parentCtx = parent.Value.Memory.ContextParameters;

        foreach (var child in children)
        {
            var childCtx = child.Value.Memory.ContextParameters;

            void PropagateField(MemoryChunkModel parentField, MemoryChunkModel childField)
            {
                if (parentField.IsTransferable && string.IsNullOrEmpty(childField.Value))
                {
                    childField.Value = parentField.Value;
                    childField.IsTransferable = true;
                }
            }

            PropagateField(parentCtx.Architecture, childCtx.Architecture);
            PropagateField(parentCtx.Current, childCtx.Current);
            PropagateField(parentCtx.Directory, childCtx.Directory);
            PropagateField(parentCtx.Global, childCtx.Global);
            PropagateField(parentCtx.ReferenceCode, childCtx.ReferenceCode);
            PropagateField(parentCtx.Requirements, childCtx.Requirements);
            PropagateField(parentCtx.Rules, childCtx.Rules);
            PropagateField(parentCtx.Technology, childCtx.Technology);
        }
    }
}

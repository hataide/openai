using ThreadingProject.WorkflowEngine.NodeHandlers.DeveloperOrchestratorHandlers.ProcessTasks.Models;

namespace ThreadingProject.WorkflowEngine.NodeHandlers.DeveloperOrchestratorHandlers.ProcessTasks;

public class ProcessTasks : IProcessTasks
{
    // Simulated action: takes a string, returns (success, result string)
    public async Task<(bool success, string result)> ProcessStringAsync(string input)
    {
        // Example logic: succeed if length is even, fail otherwise
        await Task.Delay(50); // Simulate async work
        if (input.Length % 2 == 0)
            return (true, input.ToUpper());
        return (false, string.Empty);
    }

    public async Task<string> RunAsync(List<string> items)
    {
        // Track results
        List<ProcessingResult> results = new();

        // First pass
        foreach (var item in items)
        {
            var (success, result) = await ProcessStringAsync(item);
            results.Add(new ProcessingResult
            {
                Input = item,
                Result = result,
                Success = success
            });
        }

        // Gather failed inputs
        var failedItems = new List<ProcessingResult>();
        foreach (var r in results)
            if (!r.Success)
                failedItems.Add(r);

        // Second pass on failed
        foreach (var failed in failedItems)
        {
            var (success, result) = await ProcessStringAsync(failed.Input);
            if (success)
            {
                failed.Result = result;
                failed.Success = true;
            }
        }

        // Concatenate all result strings
        var finalResult = string.Concat(results.ConvertAll(r => r.Result));
        return finalResult;
    }
}

// ──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────

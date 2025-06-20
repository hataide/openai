namespace ThreadingProject.WorkflowEngine.NodeHandlers.DeveloperOrchestratorHandlers.ProcessTasks;

public interface IProcessTasks
{
    Task<(bool success, string result)> ProcessStringAsync(string input);
    Task<string> RunAsync(List<string> items);
}

namespace ThreadingProject.WorkflowEngine.NodeHandlers.DeveloperOrchestratorHandlers.ProcessTasks.Models;

public class ProcessingResult
{
    public string Input { get; set; } = string.Empty;
    public string Result { get; set; } = string.Empty;
    public bool Success { get; set; }
}

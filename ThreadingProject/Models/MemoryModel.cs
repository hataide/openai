namespace ThreadingProject.Models;

public class AgenticPropertiesModel
{
    public MemoryModel Memory { get; set; } = new();
    public string AgentType { get; set; } = string.Empty; // ; = "Single",
    public string Role { get; set; } = string.Empty; // ; = "Developer", 
}

public class MemoryModel
{
    public ContextParamenterModel ContextParameters { get; set; } = new();
    public string StartPrompt { get; set; } = string.Empty;
}

public record ContextParamenterModel
{
    public MemoryChunkModel Global { get; set; } = new();
    public MemoryChunkModel Requirements { get; set; } = new();
    public MemoryChunkModel Technology { get; set; } = new();
    public MemoryChunkModel Architecture { get; set; } = new();
    public MemoryChunkModel Rules { get; set; } = new();
    public MemoryChunkModel Current { get; set; } = new();
    public MemoryChunkModel ReferenceCode { get; set; } = new();
    public MemoryChunkModel Directory { get; set; } = new();
}

public record MemoryChunkModel
{
    public string Value { get; set; } = string.Empty;
    public bool IsTransferable { get; set; }
}

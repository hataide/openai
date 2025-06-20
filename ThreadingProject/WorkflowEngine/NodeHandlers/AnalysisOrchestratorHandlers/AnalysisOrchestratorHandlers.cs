using CodeCraftLab.Channel;
using CodeCraftLab.Channel.Models.Request;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using ThreadingProject.Models;
using ThreadingProject.WorkflowEngine.TreeEngine.Models;

namespace ThreadingProject.WorkflowEngine.NodeHandlers.AnalysisOrchestratorHandlers;

public class AnalysisOrchestratorHandlers : INodeHandler<AgenticPropertiesModel>
{
    private readonly IChannelCommunication _channel;

    public AnalysisOrchestratorHandlers(IChannelCommunication channel)
    {
        _channel = channel;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async ValueTask<(bool ShouldDescend, string Message)> ExecuteAsync(AgenticNodeModel<AgenticPropertiesModel> node, CancellationToken ct = default)
    {
        RequestDataModel request = new()
        {
            AISettings = new()
            {
                AgentType = node.Value.AgentType,
                AIModel = "oasis4o",
                AIRole = node.Value.Role,
                CommandMode = false,
                Expertise = "junior",
                Provider = "Azure",
            },
            Tools = null,
            MessageContext = new()
            {
                Architecture = node.Value.Memory.ContextParameters.Architecture.IsTransferable ? node.Value.Memory.ContextParameters.Architecture.Value : string.Empty,
                Current = node.Value.Memory.ContextParameters.Current.IsTransferable ? node.Value.Memory.ContextParameters.Current.Value : string.Empty,
                Global = node.Value.Memory.ContextParameters.Global.IsTransferable ? node.Value.Memory.ContextParameters.Global.Value : string.Empty,
                Requirements = node.Value.Memory.ContextParameters.Requirements.IsTransferable ? node.Value.Memory.ContextParameters.Requirements.Value : string.Empty,
                Rules = node.Value.Memory.ContextParameters.Rules.IsTransferable ? node.Value.Memory.ContextParameters.Rules.Value : string.Empty,
                Technology = node.Value.Memory.ContextParameters.Technology.IsTransferable ? node.Value.Memory.ContextParameters.Technology.Value : string.Empty,
                ReferenceCode = node.Value.Memory.ContextParameters.ReferenceCode.IsTransferable ? node.Value.Memory.ContextParameters.ReferenceCode.Value : string.Empty,
                Summary = "",
                Directory = node.Value.Memory.ContextParameters.Directory.IsTransferable ? node.Value.Memory.ContextParameters.Directory.Value : string.Empty,
                Images = null,
            },
            Messages = new()
            {
                new MessageModel()
                {
                    Content = node.Value.Memory.StartPrompt,
                    Role = "user",
                    Name = "",
                    Tool_Call_id = null,
                    ToolCalls = null,
                },
            }
        };

        var result = await _channel.ChatCompletion(request);

        if (result.Message.Content.Contains("[NOT_ENOUGH_REQUIREMENTS]"))
        {
            return (false, "[NOT_ENOUGH_REQUIREMENTS]");
        }

        if (result.Message.Content.Contains("[ERROR]"))
        {
            return (false, "[ERROR]");
        }

        if (result.Message.Content.Contains("[SUCCESS]"))
        {
            // process the result
            var extractedContent = ExtractContent(result.Message.Content);

            node.Value.Memory.ContextParameters.Current.Value = string.Join(" ", extractedContent);

            Console.WriteLine(JsonConvert.SerializeObject(result));
            Console.WriteLine();
            return (true, "[SUCCESS]");
        }

        return (false, "[ERROR]");
    }


    public static List<string> ExtractContent(string inputText)
    {
        List<string> extractedContent = new List<string>();

        // Pattern using [\s\S] to match any character including newlines.
        string pattern = @"\[RFB\]([\s\S]*?)\[RFE\]";

        MatchCollection matches = Regex.Matches(inputText, pattern); // No RegexOptions.Singleline needed here

        foreach (Match match in matches)
        {
            extractedContent.Add("\n[RFB]" + match.Groups[1].Value + "[RFE]\n");
        }

        return extractedContent;
    }
}


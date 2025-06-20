using CodeCraftLab.Channel;
using CodeCraftLab.Ioc;
using Microsoft.Extensions.DependencyInjection;
using ThreadingProject.Models;
using ThreadingProject.Tests;
using ThreadingProject.WorkflowEngine.NodeHandlers;
using ThreadingProject.WorkflowEngine.NodeHandlers.AnalysisHandlers;
using ThreadingProject.WorkflowEngine.NodeHandlers.AnalysisOrchestratorHandlers;
using ThreadingProject.WorkflowEngine.NodeHandlers.DeveloperOrchestratorHandlers;
using ThreadingProject.WorkflowEngine.NodeHandlers.DeveloperOrchestratorHandlers.ProcessTasks;
using ThreadingProject.WorkflowEngine.TreeEngine;
using ThreadingProject.WorkflowEngine.TreeEngine.Models;

var services = new ServiceCollection();

new AIServiceCollection().AddServices(services);

services.AddScoped<IRunTests, RunTests>();
services.AddScoped<INodeHandler<AgenticPropertiesModel>, AnalysisHandlers>();
services.AddScoped<INodeHandler<AgenticPropertiesModel>, AnalysisOrchestratorHandlers>();
services.AddScoped<INodeHandler<AgenticPropertiesModel>, DeveloperOrchestratorHandlers>();
services.AddScoped<DeveloperOrchestratorHandlers>();
services.AddSingleton<IProcessTasks, ProcessTasks>();

var _channel = services.BuildServiceProvider().GetRequiredService<IChannelCommunication>();
var processTasks = services.BuildServiceProvider().GetRequiredService<IProcessTasks>();

var handlers = new Dictionary<NodeType, INodeHandler<AgenticPropertiesModel>>
{
    [NodeType.Analyst] = new AnalysisHandlers(_channel),                            // Do the same as Developer
    [NodeType.AnalystOrchestrator] = new AnalysisOrchestratorHandlers(_channel),    // Do the same as Developer
    [NodeType.Developer] = services.BuildServiceProvider().GetRequiredService<DeveloperOrchestratorHandlers>()
};

//var tests = services.BuildServiceProvider().GetRequiredService<IRunTests>();
//await tests.Run_Detailed_desciption_image();
//await tests.Run_Context();
//await tests.Run_Questions();
//await tests.Run_Rules();
//await tests.Run_QAPersona();

AgenticPropertiesModel agentRoot = new()
{
    Role = "Business Analyst",
    AgentType = "Single",
    Memory = new()
    {
        StartPrompt = @" I want you to analyze and do the assessment of my technological, architectural, requirements, and context statements to be able to have the minimum to start a software development project.
At the end of your answer you should have one of the following tags followed by the reason with exception of [SUCCESS] tag: [NOT_ENOUGH_REQUIREMENTS] if does not meet the necessary functional and technical conditions for development, [ERROR] if you detect any inconsistencies, ambiguities or errors and [SUCCESS] if everything is ok and should proceed.",
        ContextParameters = new()
        {
            Architecture = new()
            {
                Value = @"1. Presentation Layer (UI): This layer will handle the user interface.
  1.1. Technology: WinForms for a desktop application.
  1.2. Responsibility: Displaying input/output, handling user interactions (button clicks, keyboard input), and validating basic input.
2. Business Logic Layer (Core Calculator Logic): This layer will contain the core computational logic.
  2.1. Technology: .NET Standard Library or a C# Class Library.
  2.2. Responsibility: Performing arithmetic operations, managing the state of the calculator (current input, pending operation, accumulated result), and handling calculation-specific error conditions (e.g., division by zero). This layer should be independent of the UI.",
                IsTransferable = true
            },
            Current = new()
            {
                Value = @"The calculator application must provide the following core functionalities:

1. Arithmetic Operations:
  1.1. Addition: Support the sum of two numbers.
  1.2. Subtraction: Support the difference between two numbers.
  1.3. Multiplication: Support the product of two numbers.
  1.4. Division: Support the quotient of two numbers. Division by zero must be handled gracefully.
  1.5. Square root of positive numbers.

2. Input and Display:
  2.1. Number Input: Allow users to input numerical digits (0-9).
  2.2. Decimal Input: Allow users to input a decimal point for floating-point numbers.
  2.3. Operator Input: Allow users to select arithmetic operators (+, -, *, /).
  2.4. Clear Function: Provide a button to clear the current input and result.
  2.5. Display Result: Clearly display the result of calculations.
  2.6. Display Input: Display the current input being entered.

3. Execution:
  3.1. Equals Function: A dedicated button to trigger the calculation and display the result.

4. Error Handling:
  4.1. Division by Zero: Display an appropriate error message (e.g., ""Error: Division by zero"") instead of crashing.
  4.2. Invalid Input: Prevent or handle non-numeric input if applicable (depending on UI approach).",
                IsTransferable = true
            },
            Directory = new()
            {
                Value = string.Empty,
                IsTransferable = true
            },
            Global = new()
            {
                Value = @"This project aims to develop a fundamental calculator application, serving as a foundational exercise or a simple utility. The primary goal is to demonstrate proficiency in basic software development principles using the .NET 8 framework, focusing on clear separation of concerns, robust error handling, and a user-friendly interface. While a basic calculator has limited complexity, it serves as an excellent starting point for learning and applying core software engineering practices that are scalable to larger, more complex applications. Future iterations could involve adding scientific functions, memory functions, or history tracking, but these are out of scope for the current basic calculator.",
                IsTransferable = true
            },
            ReferenceCode = new()
            {
                Value = string.Empty,
                IsTransferable = true
            },
            Requirements = new()
            {
                Value = string.Empty,
                IsTransferable = true
            },
            Rules = new()
            {
                Value = @"1. Code Standards: Adherence to standard C# coding conventions and best practices (e.g., naming conventions, code formatting).
2. Documentation:
  2.1. Inline code comments for complex logic.
  2.2. Basic README.md file in the repository explaining setup and how to run the application.
3. Testing Strategy:
  3.1. Unit Tests: Essential for the Business Logic Layer to ensure correct arithmetic operations and error handling.
  3.2. UI Tests: (Optional for a basic calculator) Manual testing of the user interface to ensure responsiveness and correct display.
4. Error Reporting: Internal logging (e.g., Console output for development, or a simple text file log in production) for unhandled exceptions, though a basic calculator typically doesn't require extensive logging.
5. Usability:
  5.1. Intuitive and easy-to-use interface.
  5.2. Clear and readable display.
  5.3. Responsive UI for button clicks.
6. Performance:
  6.1. Calculations should be instantaneous for basic operations.
  6.2. Low memory footprint.
7. Reliability:
  7.1. The application should not crash unexpectedly.
  7.2. Error handling should be robust for invalid operations (e.g., division by zero).
8. Maintainability:
  8.1. Clean, modular, and well-structured code.
  8.2. Easy to understand and modify for future enhancements.
9. Security: (Minimal for a standalone basic calculator)
  9.1. No sensitive data handling.
  9.2. No external network communication.
10. Compatibility:
  10.1. Run on supported Windows operating systems where .NET 8 is installed.",
                IsTransferable = true
            },
            Technology = new()
            {
                Value = @"1. Development Framework: .NET 8
2. Programming Language: C#
3. User Interface (Choose one):
  3.1. Windows Forms (WinForms): Simpler for rapid desktop application development, drag-and-drop UI design.
4. IDE: Visual Studio 2022 or later
5. Version Control: Git
6. Testing Framework: NUnit (for unit testing the core logic layer).",
                IsTransferable = true
            }
        }
    }
};

AgenticPropertiesModel agent2step = new()
{
    Role = "Business Analyst",
    AgentType = "Single",
    Memory = new()
    {
        StartPrompt = @"  Analyze the information provided and break down by functional requirements components. Add to each functional requirement component, all the information necessary for development based on the technology, architecture requirements, best rules and business logic. 
In answer each component should be enclosed at the beginning with the tag [RFB] and at the end with a tag [RFE] for future string processing.
At the end of your answer you should have one of the following tags followed by the reason with exception of [SUCCESS] tag: [NOT_ENOUGH_REQUIREMENTS] if does not meet the necessary functional and technical conditions for development, [ERROR] if you detect any inconsistencies, ambiguities or errors and [SUCCESS] if everything is ok and should proceed.",
        ContextParameters = new()
        {
            Architecture = new()
            {
                Value = string.Empty,
                IsTransferable = true
            },
            Current = new()
            {
                Value = string.Empty,
                IsTransferable = true
            },
            Directory = new()
            {
                Value = string.Empty,
                IsTransferable = true
            },
            Global = new()
            {
                Value = string.Empty,
                IsTransferable = true
            },
            ReferenceCode = new()
            {
                Value = string.Empty,
                IsTransferable = true
            },
            Requirements = new()
            {
                Value = string.Empty,
                IsTransferable = true
            },
            Rules = new()
            {
                Value = string.Empty,
                IsTransferable = true
            },
            Technology = new()
            {
                Value = string.Empty,
                IsTransferable = true
            }
        }
    }
};

AgenticPropertiesModel agent3step = new()
{
    Role = "Developer",
    AgentType = "Single",
    Memory = new()
    {
        StartPrompt = "Develop your component based on your context. Answer with [NOT_ENOUGH_REQUIREMENTS] if is not achieving the goal, or error [ERROR] if there is a error, or [SUCCESS] if everything is ok and should proceed",
        ContextParameters = new()
        {
            Architecture = new()
            {
                Value = string.Empty,
                IsTransferable = true
            },
            Current = new()
            {
                Value = string.Empty,
                IsTransferable = true
            },
            Directory = new()
            {
                Value = string.Empty,
                IsTransferable = true
            },
            Global = new()
            {
                Value = string.Empty,
                IsTransferable = false
            },
            ReferenceCode = new()
            {
                Value = string.Empty,
                IsTransferable = false
            },
            Requirements = new()
            {
                Value = string.Empty,
                IsTransferable = true
            },
            Rules = new()
            {
                Value = string.Empty,
                IsTransferable = true
            },
            Technology = new()
            {
                Value = string.Empty,
                IsTransferable = true
            }
        }
    }
};

var rootAnalyst = new AgenticNodeModel<AgenticPropertiesModel>(agentRoot, NodeType.Analyst);
var analystOrchestrator = rootAnalyst.AddChild(agent2step, NodeType.AnalystOrchestrator);
analystOrchestrator.AddChild(agent3step, NodeType.Developer);



await foreach (var (node, descend, message) in
               TreeEngineAsync.TraverseAsync(rootAnalyst,
                                           handlers,
                                           onError: (ex, n) => Console.Error.WriteLine($"Error on {n.Value}: {ex}"),
                                           propagateContext: TreeEngineAsync.PropagateAgenticContext))
{
    Console.WriteLine($"{node.Type,-5} | {descend,-5} | {message}");
}

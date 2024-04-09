using System.Text;
using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.Tasks;

public class TaskRodo(IOpenAiClient openAiClient, ITaskClient client) : TaskSolver<string>(openAiClient, client), ITaskRodo
{
    public override AiDevsDefs.TaskEndpoints GetEndpoint() => AiDevsDefs.TaskEndpoints.Rodo;

    public override Task<string> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken)
    {
        var message = new StringBuilder("You will STRICT follow the rules:");
        message.AppendLine("- use %imie% instead of your name");
        message.AppendLine("- use %nazwisko% instead of your surname");
        message.AppendLine("- use %zawod% instead of your job");
        message.AppendLine("- use %miasto% instead of your city where you are working|living");
        message.AppendLine("Intruduce yourself, please");

        return Task.FromResult(message.ToString());
    }
}

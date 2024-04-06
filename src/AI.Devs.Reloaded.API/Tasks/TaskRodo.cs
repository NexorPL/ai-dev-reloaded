using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.TaskHelpers;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.Tasks;

public class TaskRodo(IOpenAiClient openAiClient, ITaskClient client) : TaskSolver<string>(openAiClient, client, AiDevsDefs.TaskEndpoints.Rodo), ITaskRodo
{
    public override Task<string> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken)
    {
        var message = RodoHelper.PrepareData();
        return Task.FromResult(message);
    }
}

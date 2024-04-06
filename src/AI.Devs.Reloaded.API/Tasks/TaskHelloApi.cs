using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.Tasks;

public class TaskHelloApi(IOpenAiClient openAiClient, ITaskClient client) : TaskSolver<string>(openAiClient, client), ITaskHelloApi
{
    public override AiDevsDefs.TaskEndpoints GetEndpoint() => AiDevsDefs.TaskEndpoints.HelloApi;

    public override Task<string> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken)
    {
        return Task.FromResult(taskResponse!.cookie!);
    }
}

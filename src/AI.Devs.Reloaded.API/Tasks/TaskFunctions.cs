using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.TaskHelpers;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.Tasks;

public class TaskFunctions(IOpenAiClient openAiClient, ITaskClient client) : TaskSolver<object>(openAiClient, client, AiDevsDefs.TaskEndpoints.Functions), ITaskFunctions
{
    public override Task<object> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken)
    {
        var function = FunctionsHelper.PrepareFunctionAddUser();
        return Task.FromResult(function);
    }
}

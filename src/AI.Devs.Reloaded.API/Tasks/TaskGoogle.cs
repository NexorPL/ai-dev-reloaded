using AI.Devs.Reloaded.API.Configurations;
using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Utils.Consts;
using Microsoft.Extensions.Options;

namespace AI.Devs.Reloaded.API.Tasks;

public class TaskGoogle(IOpenAiClient openAiClient, ITaskClient client, IOptions<OwnApiOptions> options) : TaskSolver<string>(openAiClient, client), ITaskGoogle
{
    private readonly IOptions<OwnApiOptions> _options = options;

    public override AiDevsDefs.TaskEndpoints GetEndpoint() => AiDevsDefs.TaskEndpoints.Google;

    public override Task<string> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken)
    {
        var googleUrl = _options.Value.GoogleUrl;
        return Task.FromResult(googleUrl);
    }
}

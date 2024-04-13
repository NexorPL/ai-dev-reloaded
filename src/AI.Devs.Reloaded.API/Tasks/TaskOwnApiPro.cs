using AI.Devs.Reloaded.API.Configurations;
using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Utils.Consts;
using Microsoft.Extensions.Options;
namespace AI.Devs.Reloaded.API.Tasks;

public class TaskOwnApiPro(IOpenAiClient openAiClient, ITaskClient client, IOptions<OwnApiOptions> options) : TaskSolver<string>(openAiClient, client), ITaskOwnApiPro
{
    private readonly IOptions<OwnApiOptions> _options = options;

    public override AiDevsDefs.TaskEndpoints GetEndpoint() => AiDevsDefs.TaskEndpoints.Ownapipro;

    public override Task<string> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken)
    {
        var ownApiUrl = _options.Value.ProUrl;
        return Task.FromResult(ownApiUrl);
    }
}

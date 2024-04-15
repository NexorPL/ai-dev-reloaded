using AI.Devs.Reloaded.API.Configurations;
using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Utils.Consts;
using Microsoft.Extensions.Options;

namespace AI.Devs.Reloaded.API.Tasks;

public class TaskMeme(
    IOpenAiClient openAiClient,
    ITaskClient client, 
    IRenderFormClient renderClient, 
    IOptions<RenderFormOptions> options
    ) : TaskSolver<string>(openAiClient, client), ITaskMeme
{
    private readonly IRenderFormClient _renderClient = renderClient;
    private readonly IOptions<RenderFormOptions> _options = options;

    public override AiDevsDefs.TaskEndpoints GetEndpoint() => AiDevsDefs.TaskEndpoints.Meme;

    public override async Task<string> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken)
    {
        var data = new Dictionary<string, string>() { { "title.text", taskResponse.text! }, { "image.src", taskResponse.image! } };
        var renderRequest = new Contracts.RenderForm.Request(_options.Value.TemplateId, data);

        var renderResponse = await _renderClient.RenderFormAsync(renderRequest, cancellationToken);

        return renderResponse.href;
    }
}

using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.Tasks;

public class TaskMd2html(IOpenAiClient openAiClient, ITaskClient client) : TaskSolver<string>(openAiClient, client), ITaskMd2html
{
    public override AiDevsDefs.TaskEndpoints GetEndpoint() => AiDevsDefs.TaskEndpoints.Md2html;

    public override async Task<string> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken)
    {
        var aiResponse = await _openAiClient.CompletionsAsync(SYSTEM_PROMPT, taskResponse.InputAsString()!, MODEL_GPT_FINE_TUNNED, cancellationToken);
        return aiResponse.AssistanceFirstMessage;

    }

    private const string SYSTEM_PROMPT = "Convert markdown into html";
    private const string MODEL_GPT_FINE_TUNNED = "ft:gpt-3.5-turbo-0125:personal:mg-aidevs:9FozhOIA";
}

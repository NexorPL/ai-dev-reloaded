using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.Tasks;

public class TaskBlogger(IOpenAiClient openAiClient, ITaskClient client) : TaskSolver<List<string>>(openAiClient, client), ITaskBlogger
{
    public override AiDevsDefs.TaskEndpoints GetEndpoint() => AiDevsDefs.TaskEndpoints.Blogger;

    public override async Task<List<string>> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken)
    {
        var messages = BlogHelper.PrepareData(taskResponse);

        var response = await _openAiClient.CompletionsAsync(messages, cancellationToken);

        var answers = BlogHelper.PrepareAnswer(response);
        return answers;
    }
}

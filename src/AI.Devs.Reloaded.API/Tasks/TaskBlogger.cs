using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.Tasks.Abstractions;

namespace AI.Devs.Reloaded.API.Tasks;

public class TaskBlogger(IOpenAiClient openAiClient, ITaskClient client) : TaskSolver<List<string>>(openAiClient, client, Utils.Consts.AiDevsDefs.TaskEndpoints.Blogger), ITaskBlogger
{
    public override async Task<List<string>> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken)
    {
        var messages = BlogHelper.PrepareData(taskResponse);

        var response = await _openAiClient.CompletionsAsync(messages, cancellationToken);

        var answers = BlogHelper.PrepareAnswer(response);
        return answers;
    }
}

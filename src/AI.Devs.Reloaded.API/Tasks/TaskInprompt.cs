using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.Tasks;

public class TaskInprompt(IOpenAiClient openAiClient, ITaskClient client) : TaskSolver<string>(openAiClient, client), ITaskInprompt
{
    public override AiDevsDefs.TaskEndpoints GetEndpoint() => AiDevsDefs.TaskEndpoints.Inprompt;

    public override async Task<string> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken)
    {
        var messages = InpromptHelper.PrepareData(taskResponse);
        var response = await _openAiClient.CompletionsAsync(messages, cancellationToken);

        var messagesWithSource = InpromptHelper.PrepareDataWithSource(taskResponse, response);
        var finalResponse = await _openAiClient.CompletionsAsync(messagesWithSource, cancellationToken);

        var parsedAnswer = InpromptHelper.ParseAnswer(finalResponse);
        return parsedAnswer;
    }
}

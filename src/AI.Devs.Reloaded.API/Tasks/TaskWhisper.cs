using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.TaskHelpers;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.Tasks;

public class TaskWhisper(IOpenAiClient openAiClient, ITaskClient client) : TaskSolver<string>(openAiClient, client), ITaskWhisper
{
    public override AiDevsDefs.TaskEndpoints GetEndpoint() => AiDevsDefs.TaskEndpoints.Whisper;

    public override async Task<string> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken)
    {
        var messages = WhisperHelper.PrepareData(taskResponse);
        var completionResponse = await _openAiClient.CompletionsAsync(messages, cancellationToken);

        var url = completionResponse.AssistanceFirstMessage;
        using var stream = await _client.GetFileAsync(url, cancellationToken);

        var answerText = await _openAiClient.AudioTranscriptionsAsync(stream, cancellationToken);
        return answerText;
    }
}

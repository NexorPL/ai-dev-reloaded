using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.TaskHelpers;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.Tasks;

public class TaskEmbedding(IOpenAiClient openAiClient, ITaskClient client) : TaskSolver<List<float>>(openAiClient, client), ITaskEmbedding
{
    public override AiDevsDefs.TaskEndpoints GetEndpoint() => AiDevsDefs.TaskEndpoints.Embedding;

    public override async Task<List<float>> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken)
    {
        var messages = EmbeddingHelper.PrepareData(taskResponse);
        var completionResponse = await _openAiClient.CompletionsAsync(messages, cancellationToken);
        var embeddingResponse = EmbeddingHelper.ParseResponse(completionResponse);

        var response = await _openAiClient.EmbeddingAsync(embeddingResponse.input, embeddingResponse.embedding, cancellationToken);
        return response.data[0].embedding;
    }
}

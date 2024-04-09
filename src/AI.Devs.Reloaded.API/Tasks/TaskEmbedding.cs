using System.Text;
using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.Models.OpenAi;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.Tasks;

public class TaskEmbedding(IOpenAiClient openAiClient, ITaskClient client) : TaskSolver<List<float>>(openAiClient, client), ITaskEmbedding
{
    public override AiDevsDefs.TaskEndpoints GetEndpoint() => AiDevsDefs.TaskEndpoints.Embedding;

    public override async Task<List<float>> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken)
    {
        var messages = PrepareData(taskResponse);
        var completionResponse = await _openAiClient.CompletionsAsync(messages, cancellationToken);
        var embeddingResponse = ParseResponse(completionResponse);

        var response = await _openAiClient.EmbeddingAsync(embeddingResponse.input, embeddingResponse.embedding, cancellationToken);
        return response.data[0].embedding;
    }

    public static List<Contracts.OpenAi.Completions.Message> PrepareData(TaskResponse response)
    {
        var systemPromptBuilder = new StringBuilder("Return in JSON {\"embedding\": \"kind of embedding\", \"input\": \"string to embedding\"}");
        systemPromptBuilder.AppendLine("Rules:");
        systemPromptBuilder.AppendLine("- Return only JSON with two properties");
        systemPromptBuilder.AppendLine("- Read allowed embedding types");
        systemPromptBuilder.AppendLine("- Read only text to embedding");

        var systemMessage = Contracts.OpenAi.Completions.Message.CreateSystemMessage(systemPromptBuilder.ToString());
        var userMessage = Contracts.OpenAi.Completions.Message.CreateUserMessage(response.msg!);

        var messages = new List<Contracts.OpenAi.Completions.Message>() { systemMessage, userMessage };

        return messages;
    }

    public static EmbeddingResponse ParseResponse(Contracts.OpenAi.Completions.Response response)
    {
        var embeddingResponse = response.DeserializeToModel<EmbeddingResponse>();

        return embeddingResponse!;
    }
}

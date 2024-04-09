using System.Text;
using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.Models.OpenAi;

namespace AI.Devs.Reloaded.API.TaskHelpers;

public static class EmbeddingHelper
{
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

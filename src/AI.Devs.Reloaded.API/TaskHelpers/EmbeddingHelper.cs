using System.Text;
using System.Text.Json;
using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.Models.OpenAi;
using AI.Devs.Reloaded.API.Utils.Consts;

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

        var systemMessage = new Contracts.OpenAi.Completions.Message(Utils.Consts.OpenAiApi.Roles.System, systemPromptBuilder.ToString());
        var userMessage = new Contracts.OpenAi.Completions.Message(Utils.Consts.OpenAiApi.Roles.User, response.msg!);

        var messages = new List<Contracts.OpenAi.Completions.Message>() { systemMessage, userMessage };

        return messages;
    }

    public static EmbeddingResponse ParseResponse(Contracts.OpenAi.Completions.Response response)
    {
        var content = response.choices.Single(x => x.message.role == OpenAiApi.Roles.Assistant).message.content;
        var embeddingResponse = JsonSerializer.Deserialize<EmbeddingResponse>(content);

        return embeddingResponse!;
    }
}

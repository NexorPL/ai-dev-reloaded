using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.TaskHelpers;

public static class WhisperHelper
{
    public static List<Contracts.OpenAi.Completions.Message> PrepareData(TaskResponse response)
    {
        var systemPrompt = "Return ONLY url in provided text by user. Nothing else";
        var systemMessage = new Contracts.OpenAi.Completions.Message(OpenAiApi.Roles.System, systemPrompt);
        var userMessage = new Contracts.OpenAi.Completions.Message(OpenAiApi.Roles.User, response.msg!);

        var messages = new List<Contracts.OpenAi.Completions.Message>() { systemMessage, userMessage };

        return messages;
    }

    public static string ParseResponse(Contracts.OpenAi.Completions.Response openAiResponse)
    {
        var url = openAiResponse.choices.Single(x => x.message.role == OpenAiApi.Roles.Assistant).message.content;
        return url!;
    }
}

using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.TaskHelpers;

public static class WhisperHelper
{
    public static List<Contracts.OpenAi.Completions.Message> PrepareData(TaskResponse response)
    {
        var systemPrompt = "Return ONLY url in provided text by user. Nothing else";
        var systemMessage = Contracts.OpenAi.Completions.Message.CreateSystemMessage(systemPrompt);
        var userMessage = Contracts.OpenAi.Completions.Message.CreateUserMessage(response.msg!);
        var messages = new List<Contracts.OpenAi.Completions.Message>() { systemMessage, userMessage };

        return messages;
    }
}

using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.TaskHelpers;

public static class WhoamiHelper
{
    public static List<Contracts.OpenAi.Completions.Message> PrepareData(TaskResponse response)
    {
        var systemPrompt = new System.Text.StringBuilder("Your role is to guess the person the user is trying to describe based on facts.");
        systemPrompt.AppendLine("Say ONLY the name and surname of that person ONLY if you know the answer and you are sure for 95%.");
        systemPrompt.AppendLine($"Say \"{DontKnow}\" when you Don't know.");

        var systemMessage = new Contracts.OpenAi.Completions.Message(OpenAiApi.Roles.System, systemPrompt.ToString());
        var userMessage = new Contracts.OpenAi.Completions.Message(OpenAiApi.Roles.User, response.hint!);

        var messages = new List<Contracts.OpenAi.Completions.Message>() { systemMessage, userMessage };
        return messages;
    }

    public static string ParseAnswer(Contracts.OpenAi.Completions.Response openAiResponse)
    {
        var answer = openAiResponse.choices.Single(x => x.message.role == OpenAiApi.Roles.Assistant).message.content;
        return answer!;
    }

    public static bool IsCorrectAnswer(string aiAnswer)
    {
        return string.Compare(aiAnswer, DontKnow, StringComparison.OrdinalIgnoreCase) != 0;
    }

    public static void ExtendMessages(List<Contracts.OpenAi.Completions.Message> messages, string assistanceText, TaskResponse response)
    {
        var assistantMessage = new Contracts.OpenAi.Completions.Message(OpenAiApi.Roles.Assistant, assistanceText);
        var userPrompt = new Contracts.OpenAi.Completions.Message(OpenAiApi.Roles.User, response.hint!);

        messages.Add(assistantMessage);
        messages.Add(userPrompt);
    }

    private static readonly string DontKnow = "Don't know";
}

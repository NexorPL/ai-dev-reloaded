using System.Text;
using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.TaskHelpers;

public static class ScraperHelper
{
    public static async Task<List<Contracts.OpenAi.Completions.Message>> PrepareData(TaskResponse taskResponse, Stream data)
    {
        using var reader = new StreamReader(data);
        var source = await reader.ReadToEndAsync();

        var promptBuilder = new StringBuilder(taskResponse.msg!);
        promptBuilder.AppendLine("Sources###");
        promptBuilder.AppendLine(source);
        promptBuilder.AppendLine("###");

        var sysPrompt = new Contracts.OpenAi.Completions.Message(
            OpenAiApi.Roles.System,
            promptBuilder.ToString()
        );

        var userPrompt = new Contracts.OpenAi.Completions.Message(
            OpenAiApi.Roles.User,
            taskResponse.question!
        );

        var messages = new List<Contracts.OpenAi.Completions.Message>() { sysPrompt, userPrompt };

        return messages;
    }

    public static string ParseAnswer(Contracts.OpenAi.Completions.Response openAiResponse)
    {
        var answer = openAiResponse.choices.Single(x => x.message.role == OpenAiApi.Roles.Assistant).message.content;
        return answer!;
    }
}

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

        var sysPrompt = Contracts.OpenAi.Completions.Message.CreateSystemMessage(promptBuilder.ToString());
        var userPrompt = Contracts.OpenAi.Completions.Message.CreateUserMessage(taskResponse.question!);
        var messages = new List<Contracts.OpenAi.Completions.Message>() { sysPrompt, userPrompt };

        return messages;
    }
}

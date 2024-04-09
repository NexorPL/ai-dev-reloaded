using System.Text;
using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.Tasks;

public static class InpromptHelper
{
    public static List<Contracts.OpenAi.Completions.Message> PrepareData(TaskResponse response)
    {
        var systemPrompt = Contracts.OpenAi.Completions.Message.CreateSystemMessage("Return only NAME and nothing more.");
        var userPrompt = Contracts.OpenAi.Completions.Message.CreateUserMessage(response.question!);
        var messages = new List<Contracts.OpenAi.Completions.Message>() { systemPrompt, userPrompt };

        return messages;
    }

    public static List<Contracts.OpenAi.Completions.Message> PrepareDataWithSource(
        TaskResponse taskResponse,
        Contracts.OpenAi.Completions.Response openAiResponse
    )
    {
        var name = openAiResponse.choices.Single(x => x.message.role == OpenAiApi.Roles.Assistant).message.content;

        var filteredList = taskResponse.InputAsList().Where(x => x.StartsWith(name, StringComparison.OrdinalIgnoreCase));

        var systemPromptBuilder = new StringBuilder("Your answer is short and percise. Answering only according to sources.");
        systemPromptBuilder.AppendLine("Sources###");

        foreach (var item in filteredList)
        {
            systemPromptBuilder.AppendLine($"- {item}");
        }

        systemPromptBuilder.AppendLine("###");

        var systemPrompt = Contracts.OpenAi.Completions.Message.CreateSystemMessage(systemPromptBuilder.ToString());
        var userPrompt = Contracts.OpenAi.Completions.Message.CreateUserMessage(taskResponse.question!);
        var messages = new List<Contracts.OpenAi.Completions.Message>() { systemPrompt, userPrompt };

        return messages;
    }
}

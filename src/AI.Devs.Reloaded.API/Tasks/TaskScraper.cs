using System.Text;
using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.Tasks;

public class TaskScraper(IOpenAiClient openAiClient, ITaskClient client) : TaskSolver<string>(openAiClient, client), ITaskScraper
{
    public override AiDevsDefs.TaskEndpoints GetEndpoint() => AiDevsDefs.TaskEndpoints.Scraper;

    public override async Task<string> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken)
    {
        using var stream = await _client.GetFileAsync(taskResponse.InputAsString(), cancellationToken);
        using var reader = new StreamReader(stream);
        var source = await reader.ReadToEndAsync();

        var promptBuilder = new StringBuilder(taskResponse.msg!);
        promptBuilder.AppendLine("Sources###");
        promptBuilder.AppendLine(source);
        promptBuilder.AppendLine("###");

        var sysPrompt = Contracts.OpenAi.Completions.Message.CreateSystemMessage(promptBuilder.ToString());
        var userPrompt = Contracts.OpenAi.Completions.Message.CreateUserMessage(taskResponse.question!);
        var messages = new List<Contracts.OpenAi.Completions.Message>() { sysPrompt, userPrompt };

        var openAiResponse = await _openAiClient.CompletionsAsync(messages, cancellationToken);

        return openAiResponse.AssistanceFirstMessage;
    }
}

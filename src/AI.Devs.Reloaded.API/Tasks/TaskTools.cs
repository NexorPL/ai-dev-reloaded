using System.Text.Json;
using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Tasks.Models;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.Tasks;

public class TaskTools(IOpenAiClient openAiClient, ITaskClient client) : TaskSolver<object>(openAiClient, client), ITaskTools
{
    public override AiDevsDefs.TaskEndpoints GetEndpoint() => AiDevsDefs.TaskEndpoints.Tools;

    public override async Task<object> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken)
    {
        var messages = GetMessages(taskResponse);
        var aiResponse = await _openAiClient.CompletionsAsync(messages, cancellationToken);
        object answer = aiResponse.AssistanceFirstMessage.Contains("date")
            ? aiResponse.DeserializeToModel<CalendatDto>()
            : aiResponse.DeserializeToModel<ToolsDto>();

        return answer;
    }

    private static List<Contracts.OpenAi.Completions.Message> GetMessages(TaskResponse response)
    {
        var system = @"Decide ONLY what action should be done between ToDo and Calender.
Return always answer as json
Remember to return dates always in format YYYY-MM-DD

Facts'''
Today is Tuesday. Its 2024-04-09 today
'''

Examples ###
- Przypomnij mi, że mam kupić mleko
{""tool"":""ToDo"",""desc"":""Kup mleko""}
- Jutro mam spotkanie z Marianem
{""tool"":""Calendar"",""desc"":""Spotkanie z Marianem"",""date"":""2024-04-10""}
###";

        var systemPrompt = new Contracts.OpenAi.Completions.SystemMessage(system);
        var userPrompt = new Contracts.OpenAi.Completions.UserMessage(response.question!);
        var messages = new List<Contracts.OpenAi.Completions.Message>() { systemPrompt, userPrompt };

        return messages;
    }
}

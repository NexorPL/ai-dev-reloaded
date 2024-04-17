using System.Text.Json;
using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.Tasks;

public class TaskOptimaldb(IOpenAiClient openAiClient, ITaskClient client) : TaskSolver<string>(openAiClient, client), ITaskOptimaldb
{
    public override AiDevsDefs.TaskEndpoints GetEndpoint() => AiDevsDefs.TaskEndpoints.Optimaldb;

    public override async Task<string> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken)
    {
        using var stream = await _client.GetFileAsync(taskResponse.database!, cancellationToken);
        using var reader = new StreamReader(stream);
        var jsonData = await reader.ReadToEndAsync(cancellationToken);
        var data = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(jsonData)!;
        var optimalData = new Dictionary<string, string>();

        foreach (var user in data)
        {
            var mergedInformation = string.Join(" ", user.Value);
            var aiPartResponse = await _openAiClient.CompletionsAsync(SYSTEM_PROMPT, mergedInformation, cancellationToken);
            optimalData.Add(user.Key, aiPartResponse.AssistanceFirstMessage);
        }

        var answerJson = JsonSerializer.Serialize(optimalData);
        return answerJson;
    }

    private const string SYSTEM_PROMPT = "Jesteś asystentem do skracania treści. Każde wprowadzone zdanie uznaj jako osobny fakt o danej osobie. Skróć ten fakt możliwie jak się da, a następnie zwróć wynik beż podawania imienia. Zastosuj to dla każdego kolejnego zdania. Nie pomijaj żadnego zdania. Każde nowe zdania zacznij od myślnika\n\nPrzykład###\n1. Gra na okulele\n2. ulubiony film to Matrix\n3. Je pizze z ananasem\n###\n";
}

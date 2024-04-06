using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.Tasks;

public class TaskLiar(IOpenAiClient openAiClient, ITaskClient client) : TaskSolver<string>(openAiClient, client), ITaskLiar
{
    private const string Question = "Which river flow through Szczecin? Short Answer";

    public override AiDevsDefs.TaskEndpoints GetEndpoint() => AiDevsDefs.TaskEndpoints.Liar;

    public override async Task<IResult> SolveProblem(CancellationToken ct)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, cts.Token);

        var taskParamters = new List<KeyValuePair<string, string>>()
        {
            new("question", Question)
        };

        var token = await GetToken(linkedCts.Token);
        var taskResponse = await _client.GetTaskPostAsync(token, taskParamters, linkedCts.Token);
        var answerAi = await PrepareAnswer(taskResponse, linkedCts.Token);
        var answer = await _client.SendAnswerAsync(token, answerAi, linkedCts.Token);

        return Results.Ok(answer);
    }

    public override async Task<string> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken)
    {
        var fullInput = $"{Question} {taskResponse.answer!}";
        var response = await _openAiClient.GuardrailsAsync(fullInput, cancellationToken);
        var content = response.choices.Single(x => x.message.role == OpenAiApi.Roles.Assistant).message.content;

        return content;
    }
}

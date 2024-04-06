using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.Tasks;

public abstract class TaskSolver<TAnswerModel>(IOpenAiClient openAiClient, ITaskClient client) : ITaskSolver<TAnswerModel>
{
    protected readonly IOpenAiClient _openAiClient = openAiClient;
    protected readonly ITaskClient _client = client;

    public virtual async Task<IResult> SolveProblem(CancellationToken ct)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, cts.Token);

        var token = await GetToken(linkedCts.Token);
        var taskResponse = await _client.GetTaskAsync(token, linkedCts.Token);
        var answerAi = await PrepareAnswer(taskResponse, linkedCts.Token);
        var answer = await _client.SendAnswerAsync(token, answerAi, linkedCts.Token);

        return Results.Ok(answer);
    }

    protected Task<string> GetToken(CancellationToken cancellationToken)
    {
        return _client.GetTokenAsync(GetEndpoint().Name, cancellationToken);
    }

    public abstract Task<TAnswerModel> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken);

    public abstract AiDevsDefs.TaskEndpoints GetEndpoint();
}

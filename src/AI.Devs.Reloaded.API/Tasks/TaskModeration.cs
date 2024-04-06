using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.Tasks;

public class TaskModeration(IOpenAiClient openAiClient, ITaskClient client) : TaskSolver<List<int>>(openAiClient, client), ITaskModeration
{
    public override AiDevsDefs.TaskEndpoints GetEndpoint() => AiDevsDefs.TaskEndpoints.Moderation;

    public override async Task<List<int>> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken)
    {
        var answers = new List<int>();

        foreach (var input in taskResponse.InputAsList())
        {
            var response = await _openAiClient.ModerationAsync(input, cancellationToken);
            var anyFlagged = response.results.Any(r => r.Flagged) ? 1 : 0;

            answers.Add(anyFlagged);
        }

        return answers!;
    }
}

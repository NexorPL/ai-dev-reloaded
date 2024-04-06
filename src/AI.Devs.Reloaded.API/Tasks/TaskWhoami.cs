using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.TaskHelpers;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.Tasks;

public class TaskWhoamI(IOpenAiClient openAiClient, ITaskClient client) : TaskSolver<string>(openAiClient, client), ITaskWhoami
{
    public override AiDevsDefs.TaskEndpoints GetEndpoint() => AiDevsDefs.TaskEndpoints.Whoami;

    public override async Task<string> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken)
    {
        var messages = WhoamiHelper.PrepareData(taskResponse);

        const int tries = 10;
        int counter = 1;
        var answerAi = string.Empty;

        do
        {
            var responseAi = await _openAiClient.CompletionsAsync(messages, cancellationToken);
            var localAnswerAi = WhoamiHelper.ParseAnswer(responseAi);
            bool knownAnswer = WhoamiHelper.IsCorrectAnswer(localAnswerAi);

            if (knownAnswer)
            {
                answerAi = localAnswerAi;
                break;
            }

            var token = await GetToken(cancellationToken);
            taskResponse = await _client.GetTaskAsync(token, cancellationToken);
            WhoamiHelper.ExtendMessages(messages, localAnswerAi, taskResponse);

        } while (counter++ < tries);

        ArgumentException.ThrowIfNullOrEmpty(answerAi, nameof(answerAi));

        return answerAi;
    }
}

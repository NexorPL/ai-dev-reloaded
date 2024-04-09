using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.Tasks;

public class TaskWhoamI(IOpenAiClient openAiClient, ITaskClient client) : TaskSolver<string>(openAiClient, client), ITaskWhoami
{
    public override AiDevsDefs.TaskEndpoints GetEndpoint() => AiDevsDefs.TaskEndpoints.Whoami;

    public override async Task<string> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken)
    {
        var messages = PrepareData(taskResponse);

        const int tries = 10;
        int counter = 1;
        var answerAi = string.Empty;

        do
        {
            var responseAi = await _openAiClient.CompletionsAsync(messages, cancellationToken);
            var localAnswerAi = responseAi.AssistanceFirstMessage;
            bool knownAnswer = IsCorrectAnswer(localAnswerAi);

            if (knownAnswer)
            {
                answerAi = localAnswerAi;
                break;
            }

            var token = await GetToken(cancellationToken);
            taskResponse = await _client.GetTaskAsync(token, cancellationToken);
            ExtendMessages(messages, localAnswerAi, taskResponse);

        } while (counter++ < tries);

        ArgumentException.ThrowIfNullOrEmpty(answerAi, nameof(answerAi));

        return answerAi;
    }

    public static List<Contracts.OpenAi.Completions.Message> PrepareData(TaskResponse response)
    {
        var systemPrompt = new System.Text.StringBuilder("Your role is to guess the person the user is trying to describe based on facts.");
        systemPrompt.AppendLine("Say ONLY the name and surname of that person ONLY if you know the answer and you are sure for 95%.");
        systemPrompt.AppendLine($"Say \"{DontKnow}\" when you Don't know.");

        var systemMessage = Contracts.OpenAi.Completions.Message.CreateUserMessage(systemPrompt.ToString());
        var userMessage = Contracts.OpenAi.Completions.Message.CreateUserMessage(response.hint!);

        var messages = new List<Contracts.OpenAi.Completions.Message>() { systemMessage, userMessage };
        return messages;
    }

    public static bool IsCorrectAnswer(string aiAnswer)
    {
        return string.Compare(aiAnswer, DontKnow, StringComparison.OrdinalIgnoreCase) != 0;
    }

    public static void ExtendMessages(List<Contracts.OpenAi.Completions.Message> messages, string assistanceText, TaskResponse response)
    {
        var assistantMessage = Contracts.OpenAi.Completions.Message.CreateAssistantMessage(assistanceText);
        var userPrompt = Contracts.OpenAi.Completions.Message.CreateUserMessage(response.hint!);

        messages.Add(assistantMessage);
        messages.Add(userPrompt);
    }

    private static readonly string DontKnow = "Don't know";
}

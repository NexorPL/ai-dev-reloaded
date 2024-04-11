namespace AI.Devs.OwnApi.HttpClients.Abstractions;

public interface IOpenAiClient
{
    Task<Contracts.OpenAi.Completions.Response> CompletionsAsync(List<Contracts.OpenAi.Completions.Message> messages, CancellationToken cancellationToken);
}

namespace AI.Devs.Reloaded.API.HttpClients.Abstractions;

public interface IOpenAiClient
{
    Task<Contracts.OpenAi.Moderation.Response> ModerationAsync(string input, CancellationToken cancellationToken);
    Task<Contracts.OpenAi.Completions.Response> CompletionsAsync(string input, CancellationToken cancellationToken);
    Task<Contracts.OpenAi.Completions.Response> GuardrailsAsync(string input, CancellationToken cancellationToken);
}

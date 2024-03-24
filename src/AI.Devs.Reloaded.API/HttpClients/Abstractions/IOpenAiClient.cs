namespace AI.Devs.Reloaded.API.HttpClients.Abstractions;

public interface IOpenAiClient
{
    Task<Contracts.OpenAi.Moderation.Response> Moderation(string input, CancellationToken cancellationToken);
    Task<Contracts.OpenAi.Completions.Response> Completions(string input, CancellationToken cancellationToken);
}

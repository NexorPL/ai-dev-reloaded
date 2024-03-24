using AI.Devs.Reloaded.API.Contracts.OpenAi.Moderation;

namespace AI.Devs.Reloaded.API.HttpClients.Abstractions;

public interface IOpenAiClient
{
    Task<Response> Moderation(string input, CancellationToken cancellationToken);
}

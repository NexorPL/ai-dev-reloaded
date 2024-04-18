using AI.Devs.OwnApi.Contracts.OpenAi.Completions;

namespace AI.Devs.OwnApi.HttpClients.Abstractions;

public interface IOpenAiClient
{
    Task<Response> CompletionsAsync(List<Message> messages, CancellationToken ct);
    Task<Response> CompletionsAsync(string systemPrompt, string userPrompt, List<Tool> tools, CancellationToken ct);
}

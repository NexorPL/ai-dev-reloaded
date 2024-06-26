﻿namespace AI.Devs.Reloaded.API.HttpClients.Abstractions;

public interface IOpenAiClient
{
    Task<Contracts.OpenAi.Moderation.Response> ModerationAsync(string input, CancellationToken cancellationToken);
    Task<Contracts.OpenAi.Completions.Response> CompletionsAsync(string systemPrompt, string userPrompt, CancellationToken cancellationToken);
    Task<Contracts.OpenAi.Completions.Response> CompletionsAsync(string systemPrompt, string userPrompt, string modelGpt, CancellationToken cancellationToken);
    Task<Contracts.OpenAi.Completions.Response> CompletionsAsync(List<Contracts.OpenAi.Completions.Message> messages, CancellationToken cancellationToken);
    Task<Contracts.OpenAi.Completions.Response> CompletionsVisionAsync(List<Contracts.OpenAi.Completions.MessageVision> messages, CancellationToken cancellationToken);
    Task<Contracts.OpenAi.Completions.Response> GuardrailsAsync(string input, CancellationToken cancellationToken);
    Task<Contracts.OpenAi.Embedding.Response> EmbeddingAsync(string input, string model, CancellationToken cancellationToken);
    Task<string> AudioTranscriptionsAsync(Stream stream, CancellationToken cancellationToken);

}

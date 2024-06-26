﻿using System.Text.Json;
using AI.Devs.OwnApi.HttpClients.Abstractions;
using AI.Devs.OwnApi.Utils;
using static AI.Devs.OwnApi.Utils.Consts.OpenAiApi;

namespace AI.Devs.OwnApi.HttpClients;

public class OpenAiClient(HttpClient httpClient, ILogger<OpenAiClient> logger) : IOpenAiClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<OpenAiClient> _logger = logger;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    public async Task<Contracts.OpenAi.Completions.Response> CompletionsAsync(List<Contracts.OpenAi.Completions.Message> messages, CancellationToken ct)
    {
        if (messages is null || !messages.Any())
        {
            throw new ArgumentNullException(nameof(messages));
        }

        var request = new Contracts.OpenAi.Completions.Request(ModelsGpt.Gpt35Turbo, messages);

        return await CallOpenAiApi<Contracts.OpenAi.Completions.Response, Contracts.OpenAi.Completions.Request>(
            "v1/chat/completions", 
            request,
            ct
        );
    }

    public async Task<Contracts.OpenAi.Completions.Response> CompletionsAsync(string systemPrompt, string userPrompt, List<Contracts.OpenAi.Completions.Tool> tools, CancellationToken ct)
    {
        ArgumentException.ThrowIfNullOrEmpty(systemPrompt, nameof(systemPrompt));
        ArgumentException.ThrowIfNullOrEmpty(userPrompt, nameof(userPrompt));

        if (tools is null || !tools.Any())
        {
            throw new ArgumentException(nameof(tools));
        }

        var messages = new List<Contracts.OpenAi.Completions.Message>()
        {
            Contracts.OpenAi.Completions.Message.CreateSystemMessage(systemPrompt),
            Contracts.OpenAi.Completions.Message.CreateUserMessage(userPrompt),
        };

        var request = new Contracts.OpenAi.Completions.Request(ModelsGpt.Gpt35Turbo, messages, tools);
        return await CallOpenAiApi<Contracts.OpenAi.Completions.Response, Contracts.OpenAi.Completions.Request>(
            "v1/chat/completions",
            request,
            ct
        );
    }

    private async Task<TResponse> CallOpenAiApi<TResponse, TRequest>(string endpoint, TRequest request, CancellationToken ct)
    {
        var uri = UriHelper.CreateRelativeUri(endpoint);

        try
        {
            var result = await _httpClient.PostAsJsonAsync(uri, request, _jsonSerializerOptions, ct);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync(ct);

                _logger.LogInformation($"{endpoint} - Response received from {uri}: {content}");

                var response = JsonSerializer.Deserialize<TResponse>(content, _jsonSerializerOptions);
                return response!;
            }
            else
            {
                _logger.LogInformation($"{endpoint}- Failed");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{endpoint} - error occured");
            throw;
        }

        throw new Exception();
    }
}

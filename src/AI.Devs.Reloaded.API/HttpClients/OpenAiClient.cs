using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.Utils;
using static AI.Devs.Reloaded.API.Utils.Consts.OpenAiApi;

namespace AI.Devs.Reloaded.API.HttpClients;

public class OpenAiClient(HttpClient httpClient, ILogger<OpenAiClient> logger) : IOpenAiClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<OpenAiClient> _logger = logger;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    public async Task<Contracts.OpenAi.Moderation.Response> ModerationAsync(string input, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(input, nameof(input));

        var request = new Contracts.OpenAi.Moderation.Request(input);

        return await CallOpenAiApi<Contracts.OpenAi.Moderation.Response, Contracts.OpenAi.Moderation.Request>(
            "v1/moderations",
            request,
            cancellationToken
            );
    }

    public Task<Contracts.OpenAi.Completions.Response> CompletionsAsync(string systemPrompt, string userPrompt, CancellationToken cancellationToken)
    {
        var messages = new List<Contracts.OpenAi.Completions.Message>()
        {
            Contracts.OpenAi.Completions.Message.CreateSystemMessage(systemPrompt),
            Contracts.OpenAi.Completions.Message.CreateUserMessage(userPrompt),
        };

        return CompletionsAsync(messages, cancellationToken);
    }

    public async Task<Contracts.OpenAi.Completions.Response> CompletionsAsync(List<Contracts.OpenAi.Completions.Message> messages, CancellationToken cancellationToken)
    {
        if (messages is null || !messages.Any())
        {
            throw new ArgumentNullException(nameof(messages));
        }

        var request = new Contracts.OpenAi.Completions.Request(ModelsGpt.Gpt35Turbo, messages);

        return await CallOpenAiApi<Contracts.OpenAi.Completions.Response, Contracts.OpenAi.Completions.Request>(
            "v1/chat/completions", 
            request, 
            cancellationToken
        );
    }

    public async Task<Contracts.OpenAi.Completions.Response> CompletionsVisionAsync(List<Contracts.OpenAi.Completions.MessageVision> messages, CancellationToken cancellationToken)
    {
        if (messages is null || !messages.Any())
        {
            throw new ArgumentNullException(nameof(messages));
        }

        var request = new Contracts.OpenAi.Completions.RequestVision(ModelsGpt.Gpt4Turbo, messages, 300);
        return await CallOpenAiApi<Contracts.OpenAi.Completions.Response, Contracts.OpenAi.Completions.RequestVision>(
            "v1/chat/completions",
            request, 
            cancellationToken
        );
    }

    public async Task<Contracts.OpenAi.Completions.Response> GuardrailsAsync(string input, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(input, nameof(input));

        var systemPromptBuilder = new StringBuilder("Your task is to determine true of false sentences according to your knowledge");
        systemPromptBuilder.AppendLine("Your answer is short in one word only YES or NO");

        var systemMessage = Contracts.OpenAi.Completions.Message.CreateSystemMessage(systemPromptBuilder.ToString());
        var userMessage = Contracts.OpenAi.Completions.Message.CreateUserMessage(input);
        var messages = new List<Contracts.OpenAi.Completions.Message>() { systemMessage, userMessage };
        var request = new Contracts.OpenAi.Completions.Request(ModelsGpt.Gpt35Turbo, messages);

        return await CallOpenAiApi<Contracts.OpenAi.Completions.Response, Contracts.OpenAi.Completions.Request>(
            "v1/chat/completions",
            request,
            cancellationToken
            );
    }

    public async Task<Contracts.OpenAi.Embedding.Response> EmbeddingAsync(string input, string model, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(input, nameof(input));
        ArgumentException.ThrowIfNullOrEmpty(model, nameof(model));

        var request = new Contracts.OpenAi.Embedding.Request(input, model);

        return await CallOpenAiApi<Contracts.OpenAi.Embedding.Response, Contracts.OpenAi.Embedding.Request>(
            "v1/embeddings",
            request,
            cancellationToken
            );
    }

    private async Task<TResponse> CallOpenAiApi<TResponse, TRequest>(string endpoint, TRequest request, CancellationToken cancellationToken)
    {
        var uri = UriHelper.CreateRelativeUri(endpoint);

        try
        {
            var result = await _httpClient.PostAsJsonAsync(uri, request, _jsonSerializerOptions, cancellationToken);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync(cancellationToken);

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

    public async Task<string> AudioTranscriptionsAsync(Stream stream, CancellationToken cancellationToken)
    {
        var uri = UriHelper.CreateRelativeUri("v1/audio/transcriptions");

        try
        {
            using var form = new MultipartFormDataContent();
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

            form.Add(fileContent, "file", "test.mp3");
            form.Add(new StringContent(TranscriptionModels.Whisper1), "model");

            var result = await _httpClient.PostAsync(uri, form, cancellationToken);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync(cancellationToken);

                var response = JsonSerializer.Deserialize<Contracts.OpenAi.Transcriptions.Response>(content, _jsonSerializerOptions);
                return response!.text!;
            }
            else
            {
                _logger.LogInformation("AudioTranscriptions - Failed");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AudioTranscriptions - error occured");
            throw;
        }

        throw new Exception();
    }
}

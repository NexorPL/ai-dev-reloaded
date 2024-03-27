using System.Text;
using System.Text.Json;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.Utils;

namespace AI.Devs.Reloaded.API.HttpClients;

public class OpenAiClient(HttpClient httpClient, ILogger<OpenAiClient> logger) : IOpenAiClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<OpenAiClient> _logger = logger;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
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

    public async Task<Contracts.OpenAi.Completions.Response> CompletionsAsync(List<Contracts.OpenAi.Completions.Message> messages, CancellationToken cancellationToken)
    {
        if (messages is null || !messages.Any())
        {
            throw new ArgumentNullException(nameof(messages));
        }

        var request = new Contracts.OpenAi.Completions.Request(Utils.Consts.OpenAiApi.ModelsGpt.Gpt4, messages);

        return await CallOpenAiApi<Contracts.OpenAi.Completions.Response, Contracts.OpenAi.Completions.Request>(
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

        var systemMessage = new Contracts.OpenAi.Completions.Message(Utils.Consts.OpenAiApi.Roles.System, systemPromptBuilder.ToString());

        var userMessage = new Contracts.OpenAi.Completions.Message(Utils.Consts.OpenAiApi.Roles.User, input);

        var messages = new List<Contracts.OpenAi.Completions.Message>() { systemMessage, userMessage };
        var request = new Contracts.OpenAi.Completions.Request(Utils.Consts.OpenAiApi.ModelsGpt.Gpt35Turbo, messages);

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

                _logger.LogInformation($"{endpoint} - Response received from {0}: {1}", uri, content);

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

using System.Text.Json;
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
}

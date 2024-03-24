using System.Text.Json;
using AI.Devs.Reloaded.API.Contracts.OpenAi.Moderation;
using AI.Devs.Reloaded.API.Exceptions;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;

namespace AI.Devs.Reloaded.API.HttpClients;

public class OpenAiClient(HttpClient httpClient, ILogger<OpenAiClient> logger) : IOpenAiClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<OpenAiClient> _logger = logger;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<Response> Moderation(string input, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(input, nameof(input));

        var uri = new Uri("v1/moderations", UriKind.Relative);
        var request = new Request(input);

        try
        {
            var result = await _httpClient.PostAsJsonAsync(uri, request, _jsonSerializerOptions, cancellationToken);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();

                _logger.LogInformation("Moderation - Response received from {0}: {1}", uri, content);

                var response = JsonSerializer.Deserialize<Response>(content, _jsonSerializerOptions);
                return response!;
            }
            else
            {
                _logger.LogInformation($"Moderation - Failed");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Moderation - Rrror occured");
            throw;
        }

        throw new MissingModerationException();
    }
}

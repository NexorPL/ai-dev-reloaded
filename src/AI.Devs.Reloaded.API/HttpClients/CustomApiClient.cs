using System.Text.Json;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;

namespace AI.Devs.Reloaded.API.HttpClients;

public class CustomApiClient(HttpClient httpClient, ILogger<CustomApiClient> logger) : ICustomApiClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<CustomApiClient> _logger = logger;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<TResponse> GetAsync<TResponse>(string url, CancellationToken cancellationToken)
        where TResponse : class
    {
        try
        {
            var result = await _httpClient.GetAsync(url, cancellationToken);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync(cancellationToken);

                _logger.LogInformation($"{url} - Response received from {0}: {1}", url, content);

                var response = JsonSerializer.Deserialize<TResponse>(content, _jsonSerializerOptions);
                return response!;
            }
            else
            {
                _logger.LogInformation($"{url}- Failed");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{url} - error occured");
            throw;
        }

        throw new Exception();
    }
}

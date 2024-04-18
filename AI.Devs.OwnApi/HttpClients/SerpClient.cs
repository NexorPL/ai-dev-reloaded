using System.Text.Json;
using System.Web;
using AI.Devs.OwnApi.Configurations;
using AI.Devs.OwnApi.HttpClients.Abstractions;
using AI.Devs.OwnApi.Models;
using Microsoft.Extensions.Options;

namespace AI.Devs.OwnApi.HttpClients;

public class SerpClient(HttpClient httpClient, ILogger<SerpClient> logger, IOptions<SerpApiOptions> _options) : ISerpClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<SerpClient> _logger = logger;
    private readonly IOptions<SerpApiOptions> _options = _options;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    public async Task<SerpResponse> SearchAsync(string question, CancellationToken ct)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["engine"] = "google";
        query["q"] = question;
        query["api_key"] = _options.Value.ApiKey;
        var queryString = query!.ToString();

        try
        {
            var result = await _httpClient.GetAsync("search.json?" + queryString, ct);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync(ct);

                _logger.LogInformation($"Response received: {content}");

                var response = JsonSerializer.Deserialize<SerpResponse>(content, _jsonSerializerOptions);
                return response!;
            }
            else
            {
                _logger.LogInformation($"Failed");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"error occured");
            throw;
        }

        throw new Exception();
    }
}

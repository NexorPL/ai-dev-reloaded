using System.Text.Json;
using AI.Devs.Reloaded.API.Contracts.RenderForm;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.Utils;

namespace AI.Devs.Reloaded.API.HttpClients;

public class RenderFormClient(HttpClient client, ILogger<RenderFormClient> logger) : IRenderFormClient
{
    private readonly HttpClient _httpClient = client;
    private readonly ILogger<RenderFormClient> _logger = logger;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    public async Task<Response> RenderFormAsync(Request request, CancellationToken ct = default)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        return await CallOpenAiApi<Response, Request>("api/v2/render", request, ct);
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

using System.Text.Json;
using AI.Devs.Reloaded.API.Configurations;
using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.Contracts.AiDevs.Answer;
using AI.Devs.Reloaded.API.Contracts.AiDevs.Token;
using AI.Devs.Reloaded.API.Exceptions;
using AI.Devs.Reloaded.API.Extensions;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.Utils;
using Microsoft.Extensions.Options;

namespace AI.Devs.Reloaded.API.HttpClients;

public class TaskClient(HttpClient httpClient, ILogger<TaskClient> logger, IOptions<AiDevsApiOptions> options, IOptions<BrowserAgentOptions> browserAgent) : ITaskClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<TaskClient> _logger = logger;
    private readonly IOptions<BrowserAgentOptions> _browserAgent = browserAgent;
    private readonly AiDevsApiOptions _options = options.Value;

    public async Task<string> GetTokenAsync(string taskName, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(_options.ApiKey, nameof(_options.ApiKey));

        var uri = UriHelper.CreateRelativeUri($"token/{taskName}");
        var request = new TokenRequest(_options.ApiKey);

        try
        {
            var result = await _httpClient.PostAsJsonAsync(uri, request, cancellationToken);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync(cancellationToken);

                _logger.LogInformation("GetToken - Response received from {0}: {1}", uri, content);

                var response = JsonSerializer.Deserialize<TokenResponse>(content);
                return response!.token;
            }
            else
            {
                _logger.LogInformation("GetToken - Failed to get token: {0}", taskName);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetToken - Error occured");
            throw;
        }

        throw new MissingTokenException();
    }

    public async Task<TaskResponse> GetTaskAsync(string token, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));

        var uri = UriHelper.CreateRelativeUri($"task/{token}");
        
        try
        {
            var result = await _httpClient.GetAsync(uri, cancellationToken);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync(cancellationToken);

                _logger.LogInformation("GetTask - Response received from {0}: {1}", uri, content);

                var response = JsonSerializer.Deserialize<TaskResponse>(content);
                return response!;
            }
            else
            {
                _logger.LogInformation("GetTask - Failed to get task");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetTask - Error occured");
            throw;
        }

        throw new MissingTaskException();
    }

    public async Task<TaskResponse> GetTaskPostAsync(string token, IEnumerable<KeyValuePair<string, string>> parameters, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));
        CustomArgumentExceptionExtensions.ThrowIfNull(parameters, nameof(parameters));

        var uri = UriHelper.CreateRelativeUri($"/task/{token}");
        var urlEncoded = new FormUrlEncodedContent(parameters);

        try
        {
            var result = await _httpClient.PostAsync(uri, urlEncoded, cancellationToken);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync(cancellationToken);

                _logger.LogInformation("GetTaskPost - Response received from {0}: {1}", uri, content);

                var response = JsonSerializer.Deserialize<TaskResponse>(content);
                return response!;
            }
            else
            {
                _logger.LogInformation($"GetTaskPost - Failed to get task");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetTaskPost - Error occured");
            throw;
        }

        throw new MissingTaskException();
    }

    public async Task<AnswerResponse> SendAnswerAsync<TModel>(string token, TModel answer, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token, nameof(token));
        CustomArgumentExceptionExtensions.ThrowIfNull(answer, nameof(answer));

        var uri = UriHelper.CreateRelativeUri($"answer/{token}");
        var request = new AnswerRequest<TModel>(answer);

        try
        {
            var result = await _httpClient.PostAsJsonAsync(uri, request, cancellationToken);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync(cancellationToken);

                _logger.LogInformation("SendAnswer - Response received from {0}: {1}", uri, content);

                var response = JsonSerializer.Deserialize<AnswerResponse>(content);
                return response!;
            }
            else
            {
                _logger.LogInformation($"SendAnswer - Failed to get task");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SendAnswer - Error occured");
            throw;
        }

        throw new MissingAnswerException();
    }

    public async Task<Stream> GetFileAsync(string url, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add(_browserAgent.Value.Name, _browserAgent.Value.Value);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStreamAsync(cancellationToken);
    }
}

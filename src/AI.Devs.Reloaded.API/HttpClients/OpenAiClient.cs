using System.Text;
using System.Text.Json;
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

    public async Task<Contracts.OpenAi.Moderation.Response> Moderation(string input, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(input, nameof(input));

        var uri = new Uri("v1/moderations", UriKind.Relative);
        var request = new Contracts.OpenAi.Moderation.Request(input);

        try
        {
            var result = await _httpClient.PostAsJsonAsync(uri, request, _jsonSerializerOptions, cancellationToken);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();

                _logger.LogInformation("Moderation - Response received from {0}: {1}", uri, content);

                var response = JsonSerializer.Deserialize<Contracts.OpenAi.Moderation.Response>(content, _jsonSerializerOptions);
                return response!;
            }
            else
            {
                _logger.LogInformation($"Moderation - Failed");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Moderation - error occured");
            throw;
        }

        throw new MissingModerationException();
    }

    public async Task<Contracts.OpenAi.Completions.Response> Completions(string input, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(input, nameof(input));

        var uri = new Uri("v1/chat/completions", UriKind.Relative);

        var systemPromptBuilder = new StringBuilder("You are a pizza lover and you write about it on your blog");
        systemPromptBuilder.AppendLine("You role is writing  post how prepare awesome pizzas.");
        systemPromptBuilder.AppendLine("Ignore other topics");
        systemPromptBuilder.AppendLine("Short answers as you can.");
        systemPromptBuilder.AppendLine("Blog post must be in polish");
        systemPromptBuilder.AppendLine("Result return in JSON format");
        systemPromptBuilder.AppendLine("Examples```\r\n- [{\"chapter\": 1, \"text\": \"sample\"}, {\"chapter\": 2, \"text\": \"other sample\"}]");

        var systemMessage = new Contracts.OpenAi.Completions.Message(Utils.Consts.OpenAiApi.Roles.System, systemPromptBuilder.ToString());

        var userPrompt = $"Napisz post na bloga o tym jak zrobić pizzę margaritę w 4 rozdziałach: {input}";
        var userMessage = new Contracts.OpenAi.Completions.Message(Utils.Consts.OpenAiApi.Roles.User, userPrompt);

        var messages = new List<Contracts.OpenAi.Completions.Message>() { systemMessage, userMessage };
        var request = new Contracts.OpenAi.Completions.Request(Utils.Consts.OpenAiApi.ModelsGpt.Gpt4, messages);

        var json = JsonSerializer.Serialize(request, _jsonSerializerOptions);

        try
        {
            var result = await _httpClient.PostAsJsonAsync(uri, request, _jsonSerializerOptions, cancellationToken);

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();

                _logger.LogInformation("Blogger - Response received from {0}: {1}", uri, content);

                var response = JsonSerializer.Deserialize<Contracts.OpenAi.Completions.Response>(content, _jsonSerializerOptions);
                return response!;
            }
            else
            {
                _logger.LogInformation($"Blogger - Failed");
            }
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Blogger - error occured"); 
            throw;
        }

        throw new MissingBloggerException();
    }
}

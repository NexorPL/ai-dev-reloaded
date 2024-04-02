using AI.Devs.Reloaded.API.Configurations;
using AI.Devs.Reloaded.API.HttpClients;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.HttpClients.Policies;
using Microsoft.Extensions.Options;

namespace AI.Devs.Reloaded.API;

public static class DependencyInjection
{
    public static IServiceCollection AddCustomHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AiDevsApiOptions>(configuration.GetSection(AiDevsApiOptions.AiDevsApi));
        services.Configure<OpenAiApiOptions>(configuration.GetSection(OpenAiApiOptions.OpenAiApi));
        services.Configure<BrowserAgentOptions>(configuration.GetSection(BrowserAgentOptions.BrowserAgent));

        services.AddHttpClient<ITaskClient, TaskClient>((services, httpClient) =>
        {
            var options = services.GetRequiredService<IOptions<AiDevsApiOptions>>().Value;
            httpClient.BaseAddress = new Uri(options.BaseUrl);
        }).AddPolicyHandler(RetryPolicy.GetRetryPolicy());

        services.AddHttpClient<IOpenAiClient, OpenAiClient>((services, httpClient) =>
        {
            var options = services.GetRequiredService<IOptions<OpenAiApiOptions>>().Value;
            httpClient.BaseAddress = new Uri(options.BaseUrl);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", options.ApiKey);
        });

        return services;
    }
}

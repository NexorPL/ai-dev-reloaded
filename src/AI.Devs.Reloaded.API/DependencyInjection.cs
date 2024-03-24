using AI.Devs.Reloaded.API.Configurations;
using AI.Devs.Reloaded.API.HttpClients;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using Microsoft.Extensions.Options;

namespace AI.Devs.Reloaded.API;

public static class DependencyInjection
{
    public static IServiceCollection AddTaskClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AiDevsApiOptions>(configuration.GetSection(AiDevsApiOptions.AiDevsApi));

        services.AddHttpClient<ITaskClient, TaskClient>((services, httpClient) =>
         {
             var options = services.GetRequiredService<IOptions<AiDevsApiOptions>>().Value;
             httpClient.BaseAddress = new Uri(options.BaseUrl);
         });

        return services;
    }
}

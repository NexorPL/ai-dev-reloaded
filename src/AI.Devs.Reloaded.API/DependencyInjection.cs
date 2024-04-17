using AI.Devs.Reloaded.API.Configurations;
using AI.Devs.Reloaded.API.HttpClients;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.HttpClients.Policies;
using AI.Devs.Reloaded.API.Services;
using AI.Devs.Reloaded.API.Services.Abstractions;
using AI.Devs.Reloaded.API.Tasks;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using Microsoft.Extensions.Options;

namespace AI.Devs.Reloaded.API;

public static class DependencyInjection
{
    public static IServiceCollection AddCustomHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AiDevsApiOptions>(configuration.GetSection(AiDevsApiOptions.AiDevsApi));
        services.Configure<OpenAiApiOptions>(configuration.GetSection(OpenAiApiOptions.OpenAiApi));
        services.Configure<BrowserAgentOptions>(configuration.GetSection(BrowserAgentOptions.BrowserAgent));
        services.Configure<KnowledgeApiOptions>(configuration.GetSection(KnowledgeApiOptions.KnowledgeApi));
        services.Configure<OwnApiOptions>(configuration.GetSection(OwnApiOptions.OwnApi));
        services.Configure<RenderFormOptions>(configuration.GetSection(RenderFormOptions.RenderFormApi));

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

        services.AddHttpClient<ICustomApiClient, CustomApiClient>()
            .SetHandlerLifetime(TimeSpan.FromSeconds(20));

        services.AddHttpClient<IRenderFormClient, RenderFormClient>((services, httpCliet) =>
        {
            var options = services.GetRequiredService<IOptions<RenderFormOptions>>().Value;
            httpCliet.BaseAddress = new Uri(options.BaseUrl);
            httpCliet.DefaultRequestHeaders.Add("x-api-key", options.ApiKey);
        });

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<QdrantOptions>(configuration.GetSection(QdrantOptions.Qdrant));
        services.AddScoped<IQdrantService, QdrantService>();

        services.AddScoped<ITaskHelloApi, TaskHelloApi>();
        services.AddScoped<ITaskModeration, TaskModeration>();
        services.AddScoped<ITaskBlogger, TaskBlogger>();
        services.AddScoped<ITaskLiar, TaskLiar>();
        services.AddScoped<ITaskInprompt, TaskInprompt>();
        services.AddScoped<ITaskEmbedding, TaskEmbedding>();
        services.AddScoped<ITaskWhisper, TaskWhisper>();
        services.AddScoped<ITaskFunctions, TaskFunctions>();
        services.AddScoped<ITaskRodo, TaskRodo>();
        services.AddScoped<ITaskScraper, TaskScraper>();
        services.AddScoped<ITaskWhoami, TaskWhoamI>();
        services.AddScoped<ITaskSearch, TaskSearch>();
        services.AddScoped<ITaskPeople, TaskPeople>();
        services.AddScoped<ITaskKnowledge, TaskKnowledge>();
        services.AddScoped<ITaskTools, TaskTools>();
        services.AddScoped<ITaskGnome, TaskGnome>();
        services.AddScoped<ITaskOwnApi, TaskOwnApi>();
        services.AddScoped<ITaskOwnApiPro, TaskOwnApiPro>();
        services.AddScoped<ITaskMeme, TaskMeme>();
        services.AddScoped<ITaskOptimaldb, TaskOptimaldb>();

        return services;
    }
}

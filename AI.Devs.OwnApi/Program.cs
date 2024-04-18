using AI.Devs.OwnApi.Configurations;
using AI.Devs.OwnApi.HttpClients;
using AI.Devs.OwnApi.HttpClients.Abstractions;
using AI.Devs.OwnApi.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<OpenAiApiOptions>(builder.Configuration.GetSection(OpenAiApiOptions.OpenAiApi));
builder.Services.Configure<SerpApiOptions>(builder.Configuration.GetSection(SerpApiOptions.SerpApi));
builder.Services.AddHttpClient<IOpenAiClient, OpenAiClient>((services, httpClient) =>
{
    var options = services.GetRequiredService<IOptions<OpenAiApiOptions>>().Value;
    httpClient.BaseAddress = new Uri(options.BaseUrl);
    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", options.ApiKey);
});

builder.Services.AddHttpClient<ISerpClient, SerpClient>((services, httpClient) =>
{
    var options = services.GetRequiredService<IOptions<SerpApiOptions>>().Value;
    httpClient.BaseAddress = new Uri(options.BaseUrl);
});

builder.Services.AddScoped<IProAnswerService, ProAnswerService>();
builder.Services.AddScoped<IGoogleAnswerService, GoogleAnswerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapPost("/answer", async (AI.Devs.OwnApi.Models.AiDevs.Request request, IOpenAiClient client, CancellationToken ct) => await Answer(request, client, ct));
app.MapPost("/pro/answer", async (AI.Devs.OwnApi.Models.AiDevs.Request request, IProAnswerService service, CancellationToken ct) => await service.ProAnswer(request, ct));
app.MapPost("/google/answer", async (AI.Devs.OwnApi.Models.AiDevs.Request request, IGoogleAnswerService service, CancellationToken ct) => await service.Answer(request, ct));

app.Run();

static async Task<IResult> Answer(AI.Devs.OwnApi.Models.AiDevs.Request request, IOpenAiClient client, CancellationToken ct)
{
    var systemPrompt = "Return ONLY TRUE answer based on facts to the question without extra text and story";
    var messages = new List<AI.Devs.OwnApi.Contracts.OpenAi.Completions.Message>()
    {
        AI.Devs.OwnApi.Contracts.OpenAi.Completions.Message.CreateSystemMessage(systemPrompt),
        AI.Devs.OwnApi.Contracts.OpenAi.Completions.Message.CreateUserMessage(request.question!)
    };
    var aiResponse = await client.CompletionsAsync(messages, ct);
    var response = new AI.Devs.OwnApi.Models.AiDevs.Response(aiResponse.AssistanceFirstMessage);

    return Results.Ok(response);
}

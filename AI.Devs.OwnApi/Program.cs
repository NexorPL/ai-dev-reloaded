using AI.Devs.OwnApi.Configurations;
using AI.Devs.OwnApi.HttpClients;
using AI.Devs.OwnApi.HttpClients.Abstractions;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<OpenAiApiOptions>(builder.Configuration.GetSection(OpenAiApiOptions.OpenAiApi));
builder.Services.AddHttpClient<IOpenAiClient, OpenAiClient>((services, httpClient) =>
{
    var options = services.GetRequiredService<IOptions<OpenAiApiOptions>>().Value;
    httpClient.BaseAddress = new Uri(options.BaseUrl);
    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", options.ApiKey);
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapPost("/answer", async (AiDevsRequest request, IOpenAiClient client, CancellationToken ct) => await Test(request, client, ct));

app.Run();

static async Task<IResult> Test(AiDevsRequest request, IOpenAiClient client, CancellationToken ct)
{
    var systemPrompt = "Return ONLY TRUE answer based on facts to the question without extra text and story";
    var messages = new List<AI.Devs.OwnApi.Contracts.OpenAi.Completions.Message>()
    {
        AI.Devs.OwnApi.Contracts.OpenAi.Completions.Message.CreateSystemMessage(systemPrompt),
        AI.Devs.OwnApi.Contracts.OpenAi.Completions.Message.CreateUserMessage(request.question!)
    };
    var aiResponse = await client.CompletionsAsync(messages, ct);
    var response = new AiDevsResponse() { reply = aiResponse.AssistanceFirstMessage };

    return Results.Ok(response);
}

public class AiDevsRequest
{
    public required string question { get; set; }
}

public class AiDevsResponse
{
    public required string reply { get; set; }
}

using System.Text.Json;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.Models.OpenAi;
using AI.Devs.Reloaded.API.Models.OpenAi.Extensions;
using AI.Devs.Reloaded.API.Utils;

namespace AI.Devs.Reloaded.API.Tasks;

public static class TasksModules
{
    public static void AddTasks(this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/helloapi",
            async (ITaskClient client, CancellationToken ct) => await HelloApi(client, ct)
        )
        .WithName("helloapi")
        .WithOpenApi();

        app.MapGet(
            "/moderation",
            async (IOpenAiClient openAiClient, ITaskClient client, CancellationToken ct) => await Moderation(openAiClient, client, ct)
        )
        .WithName("moderation")
        .WithOpenApi();

        app.MapGet(
            "/blogger",
            async (IOpenAiClient openAiClient, ITaskClient client, CancellationToken ct) => await Blogger(openAiClient, client, ct)
        )
        .WithName("blogger")
        .WithOpenApi();

        app.MapGet(
            "/liar",
            async (IOpenAiClient openAiClient, ITaskClient client, CancellationToken ct) => await Liar(openAiClient, client, ct)
        )
        .WithName("liar")
        .WithOpenApi();
    }

    private static async Task<IResult> HelloApi(ITaskClient client, CancellationToken ct = default)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, cts.Token);

        var token = await client.GetTokenAsync("helloapi", linkedCts.Token);
        var task = await client.GetTaskAsync(token, linkedCts.Token);
        var answer = await client.SendAnswerAsync(token, task!.cookie!, linkedCts.Token);

        return Results.Ok(answer);
    }

    private static async Task<IResult> Moderation(IOpenAiClient openAiClient, ITaskClient client, CancellationToken ct = default)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, cts.Token);

        var token = await client.GetTokenAsync("moderation", linkedCts.Token);
        var task = await client.GetTaskAsync(token, linkedCts.Token);

        var answers = new List<int>();

        foreach (var input in task.input!)
        {
            var response = await openAiClient.ModerationAsync(input, linkedCts.Token);
            var anyFlagged = response.results.Any(r => r.Flagged) ? 1 : 0;

            answers.Add(anyFlagged);
        }

        var answer = await client.SendAnswerAsync(token, answers, linkedCts.Token);

        return Results.Ok(answer);
    }

    private static async Task<IResult> Blogger(IOpenAiClient openAiClient, ITaskClient client, CancellationToken ct = default)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, cts.Token);

        var token = await client.GetTokenAsync("blogger", linkedCts.Token);
        var task = await client.GetTaskAsync(token, linkedCts.Token);
        var input = string.Join(". ", task.blog!);
        var response = await openAiClient.CompletionsAsync(input, linkedCts.Token);
        var contentJson = response.choices.Single(x => x.message.role == "assistant").message.content;
        var content = JsonSerializer.Deserialize<List<BloggerRsponse>>(contentJson);
        var answers = content!.AsTextList();
        var answer = await client.SendAnswerAsync(token, answers, linkedCts.Token);

        return Results.Ok(answer);
    }

    private static async Task<IResult> Liar(IOpenAiClient openAiClient, ITaskClient client, CancellationToken ct = default)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, cts.Token);

        var question = "Which river flow through Szczecin? Short Answer";
        var taskParamters = new List<KeyValuePair<string, string>>() 
        { 
            new("question", question) 
        };

        var token = await client.GetTokenAsync("liar", linkedCts.Token);
        var task = await client.GetTaskPostAsync(token, taskParamters, linkedCts.Token);
        var fullInput = $"{question} {task.answer!}";
        var response = await openAiClient.GuardrailsAsync(fullInput, linkedCts.Token);
        var content = response.choices.Single(x => x.message.role == "assistant").message.content;
        var answer = await client.SendAnswerAsync(token, content, linkedCts.Token);

        return Results.Ok(answer);
    }
}

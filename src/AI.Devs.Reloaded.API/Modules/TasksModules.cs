using AI.Devs.Reloaded.API.HttpClients.Abstractions;

namespace AI.Devs.Reloaded.API.Tasks;

public static class TasksModules
{
    public static void AddTaskEndpoints(this IEndpointRouteBuilder app)
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

        app.MapGet(
            "/inprompt",
            async (IOpenAiClient openAiClient, ITaskClient client, CancellationToken ct) => await Inprompt(openAiClient, client, ct)
        )
        .WithName("inprompt")
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
        var messages = BlogHelper.PrepareData(task);

        var response = await openAiClient.CompletionsAsync(messages, linkedCts.Token);

        var answers = BlogHelper.PrepareAnswer(response);
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

    private static async Task<IResult> Inprompt(IOpenAiClient openAiClient, ITaskClient client, CancellationToken ct = default)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, cts.Token);

        var token = await client.GetTokenAsync("inprompt", linkedCts.Token);
        var task = await client.GetTaskAsync(token, linkedCts.Token);
        var messages = InpromptHelper.PrepareData(task);

        var response = await openAiClient.CompletionsAsync(messages, linkedCts.Token);

        var messagesWithSource = InpromptHelper.PrepareDataWithSource(task, response);

        var finalResponse = await openAiClient.CompletionsAsync(messagesWithSource, linkedCts.Token);

        var parsedAnswer = InpromptHelper.ParseAnswer(finalResponse);

        var answer = await client.SendAnswerAsync(token, parsedAnswer, linkedCts.Token);
        
        return Results.Ok(answer);
    }
}

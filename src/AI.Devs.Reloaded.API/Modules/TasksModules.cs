using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.Models;
using AI.Devs.Reloaded.API.TaskHelpers;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.Tasks;

public static class TasksModules
{
    public static void AddTaskEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(
            AiDevsDefs.TaskEndpoints.HelloApi.Endpoint,
            async (ITaskClient client, CancellationToken ct) => await HelloApi(client, ct)
        )
        .WithName(AiDevsDefs.TaskEndpoints.HelloApi.Name)
        .WithOpenApi();

        app.MapGet(
            AiDevsDefs.TaskEndpoints.Moderation.Endpoint,
            async (IOpenAiClient openAiClient, ITaskClient client, CancellationToken ct) => await Moderation(openAiClient, client, ct)
        )
        .WithName(AiDevsDefs.TaskEndpoints.Moderation.Name)
        .WithOpenApi();

        app.MapGet(
            AiDevsDefs.TaskEndpoints.Blogger.Endpoint,
            async (IOpenAiClient openAiClient, ITaskClient client, CancellationToken ct) => await Blogger(openAiClient, client, ct)
        )
        .WithName(AiDevsDefs.TaskEndpoints.Blogger.Name)
        .WithOpenApi();

        app.MapGet(
            AiDevsDefs.TaskEndpoints.Liar.Endpoint,
            async (IOpenAiClient openAiClient, ITaskClient client, CancellationToken ct) => await Liar(openAiClient, client, ct)
        )
        .WithName(AiDevsDefs.TaskEndpoints.Liar.Name)
        .WithOpenApi();

        app.MapGet(
            AiDevsDefs.TaskEndpoints.Inprompt.Endpoint,
            async (IOpenAiClient openAiClient, ITaskClient client, CancellationToken ct) => await Inprompt(openAiClient, client, ct)
        )
        .WithName(AiDevsDefs.TaskEndpoints.Inprompt.Name)
        .WithOpenApi();

        app.MapGet(
            AiDevsDefs.TaskEndpoints.Embedding.Endpoint,
            async (IOpenAiClient openAiClient, ITaskClient client, CancellationToken ct) => await Embedding(openAiClient, client, ct)
        )
        .WithName(AiDevsDefs.TaskEndpoints.Embedding.Name)
        .WithOpenApi();

        app.MapGet(
            AiDevsDefs.TaskEndpoints.Whisper.Endpoint,
            async (IOpenAiClient openAiClient, ITaskClient client, CancellationToken ct) => await Whisper(openAiClient, client, ct)
        )
        .WithName(AiDevsDefs.TaskEndpoints.Whisper.Name)
        .WithOpenApi();

        app.MapGet(
            AiDevsDefs.TaskEndpoints.Functions.Endpoint,
            async (ITaskClient client, CancellationToken ct) => await Functions(client, ct)
        )
        .WithName(AiDevsDefs.TaskEndpoints.Functions.Name)
        .WithOpenApi();

        app.MapGet(
            AiDevsDefs.TaskEndpoints.Rodo.Endpoint,
            async (ITaskClient client, CancellationToken ct) => await Rodo(client, ct)
        )
        .WithName(AiDevsDefs.TaskEndpoints.Rodo.Name)
        .WithOpenApi();

        app.MapGet(
            AiDevsDefs.TaskEndpoints.Scraper.Endpoint,
            async (IOpenAiClient openAiClient, ITaskClient client, CancellationToken ct) => await Scraper(openAiClient, client, ct)
        )
        .WithName(AiDevsDefs.TaskEndpoints.Scraper.Name)
        .WithOpenApi();

        app.MapGet(
            AiDevsDefs.TaskEndpoints.Whoami.Endpoint,
            async (IOpenAiClient openAiClient, ITaskClient client, CancellationToken ct) => await Whoami(openAiClient, client, ct)
        )
        .WithName(AiDevsDefs.TaskEndpoints.Whoami.Name)
        .WithOpenApi();
    }

    private static async Task<IResult> HelloApi(ITaskClient client, CancellationToken ct = default)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, cts.Token);

        var token = await client.GetTokenAsync(AiDevsDefs.TaskEndpoints.HelloApi.Name, linkedCts.Token);
        var taskResponse = await client.GetTaskAsync(token, linkedCts.Token);
        var answer = await client.SendAnswerAsync(token, taskResponse!.cookie!, linkedCts.Token);

        return Results.Ok(answer);
    }

    private static async Task<IResult> Moderation(IOpenAiClient openAiClient, ITaskClient client, CancellationToken ct = default)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, cts.Token);

        var token = await client.GetTokenAsync(AiDevsDefs.TaskEndpoints.Moderation.Name, linkedCts.Token);
        var taskResponse = await client.GetTaskAsync(token, linkedCts.Token);

        var answers = new List<int>();

        foreach (var input in taskResponse.InputAsList())
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

        var token = await client.GetTokenAsync(AiDevsDefs.TaskEndpoints.Blogger.Name, linkedCts.Token);
        var taskResponse = await client.GetTaskAsync(token, linkedCts.Token);
        var messages = BlogHelper.PrepareData(taskResponse);

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

        var token = await client.GetTokenAsync(AiDevsDefs.TaskEndpoints.Liar.Name, linkedCts.Token);
        var taskResponse = await client.GetTaskPostAsync(token, taskParamters, linkedCts.Token);
        var fullInput = $"{question} {taskResponse.answer!}";
        var response = await openAiClient.GuardrailsAsync(fullInput, linkedCts.Token);
        var content = response.choices.Single(x => x.message.role == OpenAiApi.Roles.Assistant).message.content;
        var answer = await client.SendAnswerAsync(token, content, linkedCts.Token);

        return Results.Ok(answer);
    }

    private static async Task<IResult> Inprompt(IOpenAiClient openAiClient, ITaskClient client, CancellationToken ct = default)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, cts.Token);

        var token = await client.GetTokenAsync(AiDevsDefs.TaskEndpoints.Inprompt.Name, linkedCts.Token);
        var taskResponse = await client.GetTaskAsync(token, linkedCts.Token);
        var messages = InpromptHelper.PrepareData(taskResponse);

        var response = await openAiClient.CompletionsAsync(messages, linkedCts.Token);

        var messagesWithSource = InpromptHelper.PrepareDataWithSource(taskResponse, response);

        var finalResponse = await openAiClient.CompletionsAsync(messagesWithSource, linkedCts.Token);

        var parsedAnswer = InpromptHelper.ParseAnswer(finalResponse);

        var answer = await client.SendAnswerAsync(token, parsedAnswer, linkedCts.Token);

        return Results.Ok(answer);
    }

    private static async Task<IResult> Embedding(IOpenAiClient openAiClient, ITaskClient client, CancellationToken ct = default)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, cts.Token);

        var token = await client.GetTokenAsync(AiDevsDefs.TaskEndpoints.Embedding.Name, linkedCts.Token);
        var taskResponse = await client.GetTaskAsync(token, linkedCts.Token);

        var messages = EmbeddingHelper.PrepareData(taskResponse);
        var completionResponse = await openAiClient.CompletionsAsync(messages, linkedCts.Token);
        var embeddingResponse = EmbeddingHelper.ParseResponse(completionResponse);

        var response = await openAiClient.EmbeddingAsync(embeddingResponse.input, embeddingResponse.embedding, linkedCts.Token);

        var answer = await client.SendAnswerAsync(token, response.data[0].embedding, linkedCts.Token);

        return Results.Ok(answer);
    }

    private static async Task<IResult> Whisper(IOpenAiClient openAiClient, ITaskClient client, CancellationToken ct = default)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, cts.Token);

        var token = await client.GetTokenAsync(AiDevsDefs.TaskEndpoints.Whisper.Name, linkedCts.Token);
        var taskResponse = await client.GetTaskAsync(token, linkedCts.Token);

        var messages = WhisperHelper.PrepareData(taskResponse);
        var completionResponse = await openAiClient.CompletionsAsync(messages, linkedCts.Token);

        var url = WhisperHelper.ParseResponse(completionResponse);

        using var stream = await client.GetFileAsync(url, linkedCts.Token);

        var answerText = await openAiClient.AudioTranscriptionsAsync(stream, linkedCts.Token);

        var answer = await client.SendAnswerAsync(token, answerText, linkedCts.Token);

        return Results.Ok(answer);
    }

    private static async Task<IResult> Functions(ITaskClient client, CancellationToken ct = default)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, cts.Token);

        var token = await client.GetTokenAsync(AiDevsDefs.TaskEndpoints.Functions.Name, linkedCts.Token);
        var taskResponse = await client.GetTaskAsync(token, linkedCts.Token);

        var function = FunctionsHelper.PrepareFunctionAddUser();

        var answer = await client.SendAnswerAsync(token, function, linkedCts.Token);

        return Results.Ok(answer);
    }

    private static async Task<IResult> Rodo(ITaskClient client, CancellationToken ct = default)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, cts.Token);

        var token = await client.GetTokenAsync(AiDevsDefs.TaskEndpoints.Rodo.Name, linkedCts.Token);
        var taskResponse = await client.GetTaskAsync(token, linkedCts.Token);

        var message = RodoHelper.PrepareData();

        var answer = await client.SendAnswerAsync(token, message, linkedCts.Token);

        return Results.Ok(answer);
    }

    private static async Task<IResult> Scraper(IOpenAiClient openAiClient, ITaskClient client, CancellationToken ct = default)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, cts.Token);

        var token = await client.GetTokenAsync(AiDevsDefs.TaskEndpoints.Scraper.Name, linkedCts.Token);
        var taskResponse = await client.GetTaskAsync(token, linkedCts.Token);

        using var stream = await client.GetFileAsync(taskResponse.InputAsString(), linkedCts.Token);
        var messages = await ScraperHelper.PrepareData(taskResponse, stream);

        var openAiResponse = await openAiClient.CompletionsAsync(messages, linkedCts.Token);
        var answerAi = ScraperHelper.ParseAnswer(openAiResponse);

        var answer = await client.SendAnswerAsync(token, answerAi, linkedCts.Token);

        return Results.Ok(answer);
    }

    private static async Task<IResult> Whoami(IOpenAiClient openAiClient, ITaskClient client, CancellationToken ct = default)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, cts.Token);

        var token = await client.GetTokenAsync(AiDevsDefs.TaskEndpoints.Whoami.Name, linkedCts.Token);
        var taskResponse = await client.GetTaskAsync(token, linkedCts.Token);

        var messages = WhoamiHelper.PrepareData(taskResponse);

        const int tries = 10;
        int counter = 1;
        bool knownAnswer = false;
        var answerAi = string.Empty;

        do
        {
            var responseAi = await openAiClient.CompletionsAsync(messages, linkedCts.Token);
            var localAnswerAi = WhoamiHelper.ParseAnswer(responseAi);
            knownAnswer = WhoamiHelper.IsCorrectAnswer(localAnswerAi);

            if (knownAnswer)
            {
                answerAi = localAnswerAi;
                break;
            }

            taskResponse = await client.GetTaskAsync(token, linkedCts.Token);
            WhoamiHelper.ExtendMessages(messages, localAnswerAi, taskResponse);

        } while (counter++ < tries);

        ArgumentException.ThrowIfNullOrEmpty(answerAi, nameof(answerAi));

        var answer = await client.SendAnswerAsync(token, answerAi, linkedCts.Token);

        var finalResponse = CustomResponseWhoami.CreateFromAnswerResponse(answer, counter, answerAi);

        return Results.Ok(finalResponse);
    }
}

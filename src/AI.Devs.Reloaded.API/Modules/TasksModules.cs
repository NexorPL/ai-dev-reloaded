using AI.Devs.Reloaded.API.HttpClients.Abstractions;

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
}

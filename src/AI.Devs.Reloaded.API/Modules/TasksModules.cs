using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.Tasks;

public static class TasksModules
{
    public static void AddTaskEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(
            AiDevsDefs.TaskEndpoints.HelloApi.Endpoint,
            async (ITaskHelloApi task, CancellationToken ct) => await SolveTask(task, ct)
        )
        .WithName(AiDevsDefs.TaskEndpoints.HelloApi.Name)
        .WithOpenApi();

        app.MapGet(
            AiDevsDefs.TaskEndpoints.Moderation.Endpoint,
            async (ITaskModeration task, CancellationToken ct) => await SolveTask(task, ct)
        )
        .WithName(AiDevsDefs.TaskEndpoints.Moderation.Name)
        .WithOpenApi();

        app.MapGet(
            AiDevsDefs.TaskEndpoints.Blogger.Endpoint,
            async (ITaskBlogger task, CancellationToken ct) => await SolveTask(task, ct)
        )
        .WithName(AiDevsDefs.TaskEndpoints.Blogger.Name)
        .WithOpenApi();

        app.MapGet(
            AiDevsDefs.TaskEndpoints.Liar.Endpoint,
            async (ITaskLiar task, CancellationToken ct) => await SolveTask(task, ct)
        )
        .WithName(AiDevsDefs.TaskEndpoints.Liar.Name)
        .WithOpenApi();

        app.MapGet(
            AiDevsDefs.TaskEndpoints.Inprompt.Endpoint,
            async (ITaskInprompt task, CancellationToken ct) => await SolveTask(task, ct)
        )
        .WithName(AiDevsDefs.TaskEndpoints.Inprompt.Name)
        .WithOpenApi();

        app.MapGet(
            AiDevsDefs.TaskEndpoints.Embedding.Endpoint,
            async (ITaskEmbedding task, CancellationToken ct) => await SolveTask(task, ct)
        )
        .WithName(AiDevsDefs.TaskEndpoints.Embedding.Name)
        .WithOpenApi();

        app.MapGet(
            AiDevsDefs.TaskEndpoints.Whisper.Endpoint,
            async (ITaskWhisper task, CancellationToken ct) => await SolveTask(task, ct)
        )
        .WithName(AiDevsDefs.TaskEndpoints.Whisper.Name)
        .WithOpenApi();

        app.MapGet(
            AiDevsDefs.TaskEndpoints.Functions.Endpoint,
            async (ITaskFunctions task, CancellationToken ct) => await SolveTask(task, ct)
        )
        .WithName(AiDevsDefs.TaskEndpoints.Functions.Name)
        .WithOpenApi();

        app.MapGet(
            AiDevsDefs.TaskEndpoints.Rodo.Endpoint,
            async (ITaskRodo task, CancellationToken ct) => await SolveTask(task, ct)
        )
        .WithName(AiDevsDefs.TaskEndpoints.Rodo.Name)
        .WithOpenApi();

        app.MapGet(
            AiDevsDefs.TaskEndpoints.Scraper.Endpoint,
            async (ITaskScraper task, CancellationToken ct) => await SolveTask(task, ct)
        )
        .WithName(AiDevsDefs.TaskEndpoints.Scraper.Name)
        .WithOpenApi();

        app.MapGet(
            AiDevsDefs.TaskEndpoints.Whoami.Endpoint,
            async (ITaskWhoami task, CancellationToken ct) => await SolveTask(task, ct)
        )
        .WithName(AiDevsDefs.TaskEndpoints.Whoami.Name)
        .WithOpenApi();

        app.MapGet(
            AiDevsDefs.TaskEndpoints.Search.Endpoint,
            async (ITaskSearch task, CancellationToken ct) => await SolveTask(task, ct)
        )
        .WithName(AiDevsDefs.TaskEndpoints.Search.Name)
        .WithOpenApi();

        app.MapGet(
            AiDevsDefs.TaskEndpoints.People.Endpoint,
            async (ITaskPeople task, CancellationToken ct) => await SolveTask(task, ct)
        )
        .WithName(AiDevsDefs.TaskEndpoints.People.Name)
        .WithOpenApi();
    }

    private static Task<IResult> SolveTask<TAnswer>(ITaskSolver<TAnswer> task, CancellationToken ct)
    {
        return task.SolveProblem(ct);
    }
}

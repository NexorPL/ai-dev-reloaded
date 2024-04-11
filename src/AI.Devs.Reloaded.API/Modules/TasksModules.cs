using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.Tasks;

public static class TasksModules
{
    public static void AddTaskEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetWithOpenApi<ITaskHelloApi>(AiDevsDefs.TaskEndpoints.HelloApi);
        app.MapGetWithOpenApi<ITaskModeration>(AiDevsDefs.TaskEndpoints.Moderation);
        app.MapGetWithOpenApi<ITaskBlogger>(AiDevsDefs.TaskEndpoints.Blogger);
        app.MapGetWithOpenApi<ITaskLiar>(AiDevsDefs.TaskEndpoints.Liar);
        app.MapGetWithOpenApi<ITaskInprompt>(AiDevsDefs.TaskEndpoints.Inprompt);
        app.MapGetWithOpenApi<ITaskEmbedding>(AiDevsDefs.TaskEndpoints.Embedding);
        app.MapGetWithOpenApi<ITaskWhisper>(AiDevsDefs.TaskEndpoints.Whisper);
        app.MapGetWithOpenApi<ITaskFunctions>(AiDevsDefs.TaskEndpoints.Functions);
        app.MapGetWithOpenApi<ITaskRodo>(AiDevsDefs.TaskEndpoints.Rodo);
        app.MapGetWithOpenApi<ITaskScraper>(AiDevsDefs.TaskEndpoints.Scraper);
        app.MapGetWithOpenApi<ITaskWhoami>(AiDevsDefs.TaskEndpoints.Whoami);
        app.MapGetWithOpenApi<ITaskSearch>(AiDevsDefs.TaskEndpoints.Search);
        app.MapGetWithOpenApi<ITaskPeople>(AiDevsDefs.TaskEndpoints.People);
        app.MapGetWithOpenApi<ITaskKnowledge>(AiDevsDefs.TaskEndpoints.Knowledge);
        app.MapGetWithOpenApi<ITaskTools>(AiDevsDefs.TaskEndpoints.Tools);
        app.MapGetWithOpenApi<ITaskGnome>(AiDevsDefs.TaskEndpoints.Gnome);
        app.MapGetWithOpenApi<ITaskOwnApi>(AiDevsDefs.TaskEndpoints.Ownapi);
    }

    public static void MapGetWithOpenApi<TInterface>(this IEndpointRouteBuilder app, AiDevsDefs.TaskEndpoints endpoint)
        where TInterface : ITaskBase
    {
        app.MapGet(
            endpoint.Endpoint,
            async (TInterface task, CancellationToken ct) => await SolveTask(task, ct)
        )
        .WithName(endpoint.Name)
        .WithOpenApi()
        ;
    }

    private static Task<IResult> SolveTask(ITaskBase task, CancellationToken ct)
    {
        return task.SolveProblem(ct);
    }
}

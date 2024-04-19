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
        app.MapGetWithOpenApi<ITaskOwnApiPro>(AiDevsDefs.TaskEndpoints.Ownapipro);
        app.MapGetWithOpenApi<ITaskMeme>(AiDevsDefs.TaskEndpoints.Meme);
        app.MapGetWithOpenApi<ITaskOptimaldb>(AiDevsDefs.TaskEndpoints.Optimaldb);
        app.MapGetWithOpenApi<ITaskGoogle>(AiDevsDefs.TaskEndpoints.Google);
        app.MapGetWithOpenApi<ITaskMd2html>(AiDevsDefs.TaskEndpoints.Md2html, 
            "To complete this task you need to upload your training file into OpenAi manually. " +
            "In Service TaskMk2Html set your model GPT name to accomplish this task");
    }

    public static void MapGetWithOpenApi<TInterface>(this IEndpointRouteBuilder app, AiDevsDefs.TaskEndpoints endpoint, string? description = "")
        where TInterface : ITaskBase
    {
        app.MapGet(
            endpoint.Endpoint,
            async (TInterface task, CancellationToken ct) => await SolveTask(task, ct)
        )
        .WithName(endpoint.Name)
        .WithOpenApi()
        .WithDescription(description ?? "")
        ;
    }

    private static Task<IResult> SolveTask(ITaskBase task, CancellationToken ct)
    {
        return task.SolveProblem(ct);
    }
}

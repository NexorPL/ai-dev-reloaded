using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.Tasks.Abstractions;

public interface ITaskBase 
{
    abstract AiDevsDefs.TaskEndpoints GetEndpoint();
    Task<IResult> SolveProblem(CancellationToken ct);
}

public interface ITaskSolver<TModel> : ITaskBase
{
    abstract Task<TModel> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken);
}

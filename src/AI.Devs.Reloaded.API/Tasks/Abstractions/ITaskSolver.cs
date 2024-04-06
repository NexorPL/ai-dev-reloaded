using AI.Devs.Reloaded.API.Contracts.AiDevs;

namespace AI.Devs.Reloaded.API.Tasks.Abstractions;

public interface ITaskSolver<TModel>
{
    Task<IResult> SolveProblem(CancellationToken ct);
    abstract Task<TModel> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken);
}

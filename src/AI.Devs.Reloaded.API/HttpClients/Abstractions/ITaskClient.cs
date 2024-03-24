using AI.Devs.Reloaded.API.Contracts;
using AI.Devs.Reloaded.API.Contracts.Answer;

namespace AI.Devs.Reloaded.API.HttpClients.Abstractions;

public interface ITaskClient
{
    Task<string> GetTokenAsync(string taskName, CancellationToken cancellationToken = default);
    Task<TaskResponse> GetTaskAsync(string token, CancellationToken cancellationToken = default);
    Task<AnswerResponse> SendAnswerAsync(string token, string answer, CancellationToken cancellationToken = default);
}

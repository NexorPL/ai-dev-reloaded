﻿using AI.Devs.Reloaded.API.Contracts;
using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.Contracts.Answer.AiDevs;

namespace AI.Devs.Reloaded.API.HttpClients.Abstractions;

public interface ITaskClient
{
    Task<string> GetTokenAsync(string taskName, CancellationToken cancellationToken = default);
    Task<TaskResponse> GetTaskAsync(string token, CancellationToken cancellationToken = default);
    Task<AnswerResponse> SendAnswerAsync<TModel>(string token, TModel answer, CancellationToken cancellationToken = default);
}

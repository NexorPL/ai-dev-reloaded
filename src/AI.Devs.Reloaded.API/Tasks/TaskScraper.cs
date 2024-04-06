﻿using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.TaskHelpers;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.Tasks;

public class TaskScraper(IOpenAiClient openAiClient, ITaskClient client) : TaskSolver<string>(openAiClient, client, AiDevsDefs.TaskEndpoints.Scraper), ITaskScraper
{
    public override async Task<string> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken)
    {
        using var stream = await _client.GetFileAsync(taskResponse.InputAsString(), cancellationToken);
        var messages = await ScraperHelper.PrepareData(taskResponse, stream);

        var openAiResponse = await _openAiClient.CompletionsAsync(messages, cancellationToken);
        var answerAi = ScraperHelper.ParseAnswer(openAiResponse);

        return answerAi;
    }
}

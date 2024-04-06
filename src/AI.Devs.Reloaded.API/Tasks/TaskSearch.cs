using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.Contracts.OpenAi.Embedding.Extensions;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.Services.Abstractions;
using AI.Devs.Reloaded.API.TaskHelpers;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Utils.Consts;
using Qdrant.Client.Grpc;

namespace AI.Devs.Reloaded.API.Tasks;

public class TaskSearch(IOpenAiClient openAiClient, ITaskClient client, IQdrantService qdrantService) : TaskSolver<string>(openAiClient, client, AiDevsDefs.TaskEndpoints.Search), ITaskSearch
{
    private readonly IQdrantService _qdrantService = qdrantService;

    public override async Task<string> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken)
    {
        var questionEmbedding = await _openAiClient.EmbeddingAsync(taskResponse.question!, OpenAiApi.EmbeddingTechniques.TextEmbeddingAda002, cancellationToken);
        var findUrl = taskResponse.UrlFromMsg();

        using var stream = await _client.GetFileAsync(findUrl, cancellationToken);
        var archiveAiDevs = await SearchHelper.ConvertToListArchiveAiDevs(stream, cancellationToken);

        await _qdrantService.TryCreateCollection(cancellationToken);

        var pointList = new List<PointStruct>();

        foreach (var row in archiveAiDevs!)
        {
            var embeddingResponse = await _openAiClient.EmbeddingAsync(row.info, OpenAiApi.EmbeddingTechniques.TextEmbeddingAda002, cancellationToken);
            var point = embeddingResponse.AsPointStruct(row);

            pointList.Add(point);
        }

        await _qdrantService.InsertVectors(pointList, cancellationToken);

        var searchUrl = await _qdrantService.GetFirstUrlFromSearchResult(
            [.. questionEmbedding.data[0].embedding],
            limit: 1,
            cancellationToken: cancellationToken
        );

        return searchUrl;
    }
}

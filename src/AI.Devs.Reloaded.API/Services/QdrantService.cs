using AI.Devs.Reloaded.API.Configurations;
using AI.Devs.Reloaded.API.Services.Abstractions;
using Microsoft.Extensions.Options;
using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace AI.Devs.Reloaded.API.Services;

public class QdrantService : IQdrantService
{
    private readonly QdrantClient _client;
    private readonly string _collectionName;

    public QdrantService(IOptions<QdrantOptions> options)
    {
        var channel = QdrantChannel.ForAddress(options.Value.GRpcUrl,
            new ClientConfiguration()
            {
                CertificateThumbprint = options.Value.CertificateThumbprint
            });

        _client = new QdrantClient(new QdrantGrpcClient(channel));
        _collectionName = "AiDevs2Reloaded-" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
    }

    public async Task TryCreateCollection(CancellationToken cancellationToken = default)
    {
        var collectionExists = await _client.CollectionExistsAsync(_collectionName, cancellationToken);

        if (!collectionExists)
        {
            var vectorParams = new VectorParams()
            {
                Size = 1536,
                Distance = Distance.Cosine,
                OnDisk = true
            };

            await _client.CreateCollectionAsync(_collectionName, vectorParams, cancellationToken: cancellationToken);
        }
    }

    public async Task InsertVectors(List<PointStruct> pointList, CancellationToken cancellationToken = default)
    {
        var result = await _client.UpsertAsync(_collectionName, pointList, cancellationToken: cancellationToken);

        if (result.Status != UpdateStatus.Completed)
            throw new Exception("Something went wrong in upsertAsync");
    }

    public async Task<string> GetFirstUrlFromSearchResult(float[] vectorToSearch, int limit = 1, CancellationToken cancellationToken = default)
    {
        var searchResult = await _client.SearchAsync(
           collectionName: _collectionName,
           vector: vectorToSearch,
           limit: 1
           );

        var searchUrl = searchResult[0].Payload["url"].StringValue;

        return searchUrl;
    }
}

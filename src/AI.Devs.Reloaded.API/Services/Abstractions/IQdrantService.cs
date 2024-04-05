using Qdrant.Client.Grpc;

namespace AI.Devs.Reloaded.API.Services.Abstractions;

public interface IQdrantService
{
    Task TryCreateCollection(CancellationToken cancellationToken = default);
    Task InsertVectors(List<PointStruct> pointList, CancellationToken cancellationToken = default);
    Task<string> GetFirstUrlFromSearchResult(float[] vectorToSearch, int limit = 1, CancellationToken cancellationToken = default);
}

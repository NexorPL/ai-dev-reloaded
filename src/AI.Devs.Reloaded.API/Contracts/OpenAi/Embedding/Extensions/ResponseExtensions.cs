using AI.Devs.Reloaded.API.Models;
using Qdrant.Client.Grpc;

namespace AI.Devs.Reloaded.API.Contracts.OpenAi.Embedding.Extensions;

public static class ResponseExtensions
{
    public static PointStruct AsPointStruct(this Response response, ArchiveAiDevs row)
    {
        var vector = new Vector();
        vector.Data.AddRange(response.data[0].embedding);

        var point = new PointStruct()
        {
            Vectors = new() { Vector = vector },
            Id = Guid.NewGuid(),
            Payload = {
                    ["url"] = row.url,
                    ["date"] = row.date.ToString("yyyy-MM-dd"),
                    ["title"] = row.title,
                }
        };

        return point;
    }
}

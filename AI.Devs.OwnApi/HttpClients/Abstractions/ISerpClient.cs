using AI.Devs.OwnApi.Models;

namespace AI.Devs.OwnApi.HttpClients.Abstractions;

public interface ISerpClient
{
    Task<SerpResponse> SearchAsync(string query, CancellationToken ct);
}

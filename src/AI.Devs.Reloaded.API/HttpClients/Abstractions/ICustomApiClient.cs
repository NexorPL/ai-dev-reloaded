namespace AI.Devs.Reloaded.API.HttpClients.Abstractions;

public interface ICustomApiClient 
{
    Task<TResponse> GetAsync<TResponse>(string url, CancellationToken cancellationToken) where TResponse : class;
}

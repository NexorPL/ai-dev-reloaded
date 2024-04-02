using System.Net;
using Polly;
using Polly.Extensions.Http;

namespace AI.Devs.Reloaded.API.HttpClients.Policies;

public static class RetryPolicy
{
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == HttpStatusCode.Forbidden)
            .WaitAndRetryAsync(RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    private static readonly int RetryCount = 3;
}

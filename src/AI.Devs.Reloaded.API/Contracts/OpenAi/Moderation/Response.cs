namespace AI.Devs.Reloaded.API.Contracts.OpenAi.Moderation;

public sealed record Response (string id, string model, List<Result> results)
{
}

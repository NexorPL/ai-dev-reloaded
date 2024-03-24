namespace AI.Devs.Reloaded.API.Contracts.OpenAi.Completions;

public sealed record Response(
    string id,
    string Object,
    long created,
    string model,
    string systemFingerprint,
    List<Choice> choices,
    Usage usage
)
{ };

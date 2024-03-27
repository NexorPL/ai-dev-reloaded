namespace AI.Devs.Reloaded.API.Contracts.OpenAi.Embedding;

public sealed record Response(string @object, List<Data> data, string model, Usage usage) { }

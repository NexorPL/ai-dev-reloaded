namespace AI.Devs.Reloaded.API.Contracts.OpenAi.Embedding;

public sealed record Data(string @object, int index, List<double> embedding) { }

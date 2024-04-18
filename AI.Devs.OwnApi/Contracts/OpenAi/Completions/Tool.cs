namespace AI.Devs.OwnApi.Contracts.OpenAi.Completions;

public sealed record Tool(string type, Function function) { }

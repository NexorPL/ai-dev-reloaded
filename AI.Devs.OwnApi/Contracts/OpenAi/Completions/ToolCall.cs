namespace AI.Devs.OwnApi.Contracts.OpenAi.Completions;

public sealed record ToolCall (string id, string type, FunctionResponse function) { }

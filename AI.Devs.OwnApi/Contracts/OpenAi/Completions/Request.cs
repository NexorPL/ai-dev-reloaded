namespace AI.Devs.OwnApi.Contracts.OpenAi.Completions;

public sealed record Request(string model, List<Message> messages) { }

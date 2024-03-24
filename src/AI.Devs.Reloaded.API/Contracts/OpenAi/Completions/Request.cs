namespace AI.Devs.Reloaded.API.Contracts.OpenAi.Completions;

public sealed record Request(string model, List<Message> messages) { }

namespace AI.Devs.Reloaded.API.Contracts.OpenAi.Completions;

public sealed record Request(string model, List<Message> messages) { }
public sealed record RequestVision(string model, List<MessageVision> messages, int max_tokens) { }

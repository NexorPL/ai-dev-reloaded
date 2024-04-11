namespace AI.Devs.OwnApi.Contracts.OpenAi.Completions;

public sealed record Choice(int index, Message message, string finishReason, Logprobs Logprobs) { };

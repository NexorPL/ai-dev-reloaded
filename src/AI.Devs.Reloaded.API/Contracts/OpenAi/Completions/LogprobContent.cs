namespace AI.Devs.Reloaded.API.Contracts.OpenAi.Completions;

public sealed record LogprobContent (string token, int logprob, List<int> bytes) { }

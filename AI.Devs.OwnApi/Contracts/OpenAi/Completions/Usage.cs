namespace AI.Devs.OwnApi.Contracts.OpenAi.Completions;

public sealed record Usage(
    int PromptTokens,
    int CompletionTokens,
    int TotalTokens
);

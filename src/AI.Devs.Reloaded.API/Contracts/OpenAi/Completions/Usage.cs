namespace AI.Devs.Reloaded.API.Contracts.OpenAi.Completions;

public sealed record Usage(
    int PromptTokens,
    int CompletionTokens,
    int TotalTokens
);

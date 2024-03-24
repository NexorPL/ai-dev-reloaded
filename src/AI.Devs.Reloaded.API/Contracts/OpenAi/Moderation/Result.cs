namespace AI.Devs.Reloaded.API.Contracts.OpenAi.Moderation;

public sealed record Result(bool Flagged, Category Categories, CategoryScores CategoryScores) { }

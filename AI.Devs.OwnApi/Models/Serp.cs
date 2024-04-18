namespace AI.Devs.OwnApi.Models;

public sealed record SerpResponse
{
    public required List<Article> organic_results { get; set; }
}

public sealed record Article
{
    public int Position { get; init; }
    public string? Title { get; init; }
    public string? Link { get; init; }
    public string? RedirectLink { get; init; }
    public string? DisplayedLink { get; init; }
    public string? Favicon { get; init; }
    public string? Snippet { get; init; }
    public List<string>? SnippetHighlightedWords { get; init; }
    public List<string>? Missing { get; init; }
    public string? Source { get; init; }
}

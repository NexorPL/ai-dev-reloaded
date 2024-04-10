namespace AI.Devs.Reloaded.API.Contracts.OpenAi.Completions;

public sealed record VisionContent(string type, string? text = null, VisionUrl? image_url = null) 
{
    public static VisionContent CreateTextContent(string text) => new("text", text);
    public static VisionContent CreateImageUrlContent(string url, string detail) => new("image_url", image_url: new(url, detail));
}
public sealed record VisionUrl(string url, string detail) { }

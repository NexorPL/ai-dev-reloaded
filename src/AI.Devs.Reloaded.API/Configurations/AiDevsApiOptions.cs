using AI.Devs.Reloaded.API.Configurations.Abstractions;

namespace AI.Devs.Reloaded.API.Configurations;

public class AiDevsApiOptions : ICustomApiOptions
{
    public const string AiDevsApi = "AiDevsApi";

    public string BaseUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
}

using AI.Devs.Reloaded.API.Configurations.Abstractions;

namespace AI.Devs.Reloaded.API.Configurations;

public class OpenAiApiOptions : ICustomApiOptions
{
    public const string OpenAiApi = "OpenAiApi";

    public string BaseUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
}

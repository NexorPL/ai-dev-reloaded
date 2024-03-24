using AI.Devs.Reloaded.API.Configurations.Abstractions;

namespace AI.Devs.Reloaded.API.Configurations;

public class OpenAiApiOptions : ICustomApiOptions
{
    public const string OpenAiApi = "OpenAiApi";

    public required string BaseUrl { get; set; }
    public required string ApiKey { get; set; }
}

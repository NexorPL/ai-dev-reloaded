namespace AI.Devs.OwnApi.Configurations;

public class OpenAiApiOptions
{
    public const string OpenAiApi = "OpenAiApi";

    public string BaseUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
}

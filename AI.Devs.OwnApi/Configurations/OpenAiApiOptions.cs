namespace AI.Devs.OwnApi.Configurations;

public class OpenAiApiOptions
{
    public const string OpenAiApi = "OpenAiApi";

    public required string BaseUrl { get; set; }
    public required string ApiKey { get; set; } 
}

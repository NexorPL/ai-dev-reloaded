namespace AI.Devs.OwnApi.Configurations;

public class SerpApiOptions
{
    public const string SerpApi = "SerpApi";

    public required string BaseUrl { get; set; }
    public required string ApiKey { get; set; }
}

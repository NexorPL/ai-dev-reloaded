namespace AI.Devs.Reloaded.API.Configurations;

public class KnowledgeApiOptions
{
    public const string KnowledgeApi = "KnowledgeApi";

    public required string NbpUrl { get; set; }
    public required string RestCountriesUrl { get; set; }
}

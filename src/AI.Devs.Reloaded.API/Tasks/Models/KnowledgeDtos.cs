namespace AI.Devs.Reloaded.API.Tasks.Models;

public class KnowledgeAiResponse
{
    public required string ApiKind { get; set; }
    public required string Answer { get; set; }
}

public class AllowedKnowledgeAiApiKind
{
    public const string
        Currency = "currency",
        Population = "population",
        GeneralKnwoledge = "general_knwoledge"
        ;
}

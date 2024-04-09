using System.Text.Json.Serialization;

namespace AI.Devs.Reloaded.API.Tasks.Models;

public abstract class ToolsBaseDto
{
    [JsonPropertyName("tool")]
    public required string tool { get; set; }
    [JsonPropertyName("desc")]
    public required string desc { get; set; }
}

public class ToolsDto : ToolsBaseDto { }

public class CalendatDto : ToolsBaseDto
{
    [JsonPropertyName("date")]
    public required string date { get; set; }
}

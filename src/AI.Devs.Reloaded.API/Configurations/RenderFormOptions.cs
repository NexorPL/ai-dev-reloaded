using AI.Devs.Reloaded.API.Configurations.Abstractions;

namespace AI.Devs.Reloaded.API.Configurations;

public class RenderFormOptions : ICustomApiOptions
{
    public const string RenderFormApi = "RenderFormApi";

    public required string BaseUrl { get; set; }
    public required string ApiKey { get; set; }
    public required string TemplateId { get; set; }
}

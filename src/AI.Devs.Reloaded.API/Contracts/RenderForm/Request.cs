namespace AI.Devs.Reloaded.API.Contracts.RenderForm;

public sealed record Request(string template, Dictionary<string, string> data) { }

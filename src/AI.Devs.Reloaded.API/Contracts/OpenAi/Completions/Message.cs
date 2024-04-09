namespace AI.Devs.Reloaded.API.Contracts.OpenAi.Completions;

public record Message(string role, string content) { };
public sealed record SystemMessage(string content) : Message(Utils.Consts.OpenAiApi.Roles.System, content) { };
public sealed record UserMessage(string content) : Message(Utils.Consts.OpenAiApi.Roles.User, content) { };

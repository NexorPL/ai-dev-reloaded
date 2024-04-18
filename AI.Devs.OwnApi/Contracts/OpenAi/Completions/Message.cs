namespace AI.Devs.OwnApi.Contracts.OpenAi.Completions;

public sealed record Message(string role, string content, List<ToolCall>? tool_calls = null) 
{
    public static Message CreateSystemMessage(string content) => new(Utils.Consts.OpenAiApi.Roles.System, content);
    public static Message CreateUserMessage(string content) => new(Utils.Consts.OpenAiApi.Roles.User, content);
    public static Message CreateAssistantMessage(string content) => new(Utils.Consts.OpenAiApi.Roles.Assistant, content);
};

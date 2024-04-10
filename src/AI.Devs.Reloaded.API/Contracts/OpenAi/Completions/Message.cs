namespace AI.Devs.Reloaded.API.Contracts.OpenAi.Completions;

public sealed record Message(string role, string content) 
{
    public static Message CreateSystemMessage(string content) => new(Utils.Consts.OpenAiApi.Roles.System, content);
    public static Message CreateUserMessage(string content) => new(Utils.Consts.OpenAiApi.Roles.User, content);
    public static Message CreateAssistantMessage(string content) => new(Utils.Consts.OpenAiApi.Roles.Assistant, content);
};

public sealed record MessageVision(string role, List<VisionContent> content) 
{
    public static MessageVision CreateSystemMessage(VisionContent content) => new(Utils.Consts.OpenAiApi.Roles.System, new List<VisionContent>() { content });
    public static MessageVision CreateUserMessage(List<VisionContent> content) => new(Utils.Consts.OpenAiApi.Roles.User, content);
}

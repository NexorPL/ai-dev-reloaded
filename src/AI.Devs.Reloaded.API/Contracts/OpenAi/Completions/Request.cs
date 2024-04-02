namespace AI.Devs.Reloaded.API.Contracts.OpenAi.Completions;

public sealed record Request(string model, List<Message> messages) { }
public sealed record ToolChoice(string type, KeyValuePair<string, string> function) { }
public sealed record Tool(string type, Function function) { }
public sealed record Function(string description, string name, object parameters) { }
public interface IFunctionCallingParameters { }

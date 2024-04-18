namespace AI.Devs.OwnApi.Contracts.OpenAi.Completions;

public sealed record Function(string description, string name, IParam parameters) { }

public interface IParam { }
public interface ISearchParamFunction : IParam 
{
    string Description { get; set; }
    string Type { get; set; }
}

public class SearchParamFuncton : ISearchParamFunction
{
    public required string Description { get; set; }
    public required string Type { get; set; }
}

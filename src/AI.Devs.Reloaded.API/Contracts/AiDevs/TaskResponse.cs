using System.Text.Json;

namespace AI.Devs.Reloaded.API.Contracts.AiDevs;

public class TaskResponse
{
    public int code { get; set; }
    public string? msg { get; set; }
    public string? data { get; set; }
    public string? cookie { get; set; }
    public dynamic? input { get; set; }
    public List<string>? blog { get; set; }
    public string? answer { get; set; }
    public string? question { get; set; }
    public string? hint { get; set; }
    public string? url { get; set; }
    public string? image { get; set; }
    public string? text { get; set; }

    internal List<string> InputAsList()
    {
        if (input is null)
        {
            throw new ArgumentException("Input cannot be null", nameof(input));
        }

        if (input.ValueKind == JsonValueKind.Array)
        {
            return JsonSerializer.Deserialize<List<string>>(input.GetRawText());
        }

        throw new ArgumentException("Input must be of type List<string>", nameof(input));
    }

    internal string InputAsString()
    {
        if (input is null)
        {
            throw new ArgumentException("Input cannot be null", nameof(input));
        }

        return Convert.ToString(input);
    }

    internal string UrlFromMsg()
    {
        return msg![msg!.IndexOf("https://")..];
    }
}

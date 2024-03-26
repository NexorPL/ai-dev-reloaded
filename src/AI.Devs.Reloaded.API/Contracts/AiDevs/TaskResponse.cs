namespace AI.Devs.Reloaded.API.Contracts.AiDevs;

public class TaskResponse
{
    public int code { get; set; }
    public string? msg { get; set; }
    public string? cookie { get; set; }
    public List<string>? input { get; set; }
    public List<string>? blog { get; set; }
    public string? answer { get; set; }
    public string? question { get; set; }
}

namespace AI.Devs.Reloaded.API.Configurations.Abstractions;

public interface ICustomApiOptions
{
    public string BaseUrl { get; set; }
    public string ApiKey { get; set; }
}

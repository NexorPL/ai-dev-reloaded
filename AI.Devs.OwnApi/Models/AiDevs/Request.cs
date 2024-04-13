namespace AI.Devs.OwnApi.Models.AiDevs;

public sealed record Request(string question) 
{
    //in real life this should be a part of the request.
    internal Guid UUID { get; } = Guid.Parse("053276dc-d839-49e5-91d6-8e8907d77f4f");
}

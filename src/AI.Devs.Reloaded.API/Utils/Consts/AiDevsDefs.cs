namespace AI.Devs.Reloaded.API.Utils.Consts;

public class AiDevsDefs
{
    public sealed class TaskEndpoints
    {
        internal string Name { get; }
        internal string Endpoint { get; }

        internal static readonly TaskEndpoints HelloApi = new("helloapi");
        internal static readonly TaskEndpoints Moderation = new("moderation");
        internal static readonly TaskEndpoints Blogger = new("blogger");
        internal static readonly TaskEndpoints Liar = new("liar");
        internal static readonly TaskEndpoints Inprompt = new("inprompt");
        internal static readonly TaskEndpoints Embedding = new("embedding");

        private TaskEndpoints(string name)
        {
            Name = name;
            Endpoint = "/" + name;
        }
    }
}

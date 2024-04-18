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
        internal static readonly TaskEndpoints Whisper = new("whisper");
        internal static readonly TaskEndpoints Functions = new("functions");
        internal static readonly TaskEndpoints Rodo = new("rodo");
        internal static readonly TaskEndpoints Scraper = new("scraper");
        internal static readonly TaskEndpoints Whoami = new("whoami");
        internal static readonly TaskEndpoints Search = new("search");
        internal static readonly TaskEndpoints People = new("people");
        internal static readonly TaskEndpoints Knowledge = new("knowledge");
        internal static readonly TaskEndpoints Tools = new("tools");
        internal static readonly TaskEndpoints Gnome = new("gnome");
        internal static readonly TaskEndpoints Ownapi = new("ownapi");
        internal static readonly TaskEndpoints Ownapipro = new("ownapipro");
        internal static readonly TaskEndpoints Meme = new("meme");
        internal static readonly TaskEndpoints Optimaldb = new("optimaldb");
        internal static readonly TaskEndpoints Google = new("google");

        private TaskEndpoints(string name)
        {
            Name = name;
            Endpoint = "/" + name;
        }
    }
}

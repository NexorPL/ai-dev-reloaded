namespace AI.Devs.Reloaded.API.Models.OpenAi.Extensions;

public static class BloggerResponseExtensions
{
    public static List<string> AsTextList(this List<BloggerRsponse> response)
    {
        return response.Select(x => x.text).ToList();
    }
}

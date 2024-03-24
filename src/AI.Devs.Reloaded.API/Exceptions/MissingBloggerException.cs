namespace AI.Devs.Reloaded.API.Exceptions;

public class MissingBloggerException : Exception
{
    public MissingBloggerException() : base("Missing bloger results from OpenAI API") { }
}

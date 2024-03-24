namespace AI.Devs.Reloaded.API.Exceptions;

public class MissingModerationException : Exception
{
    public MissingModerationException() : base("Missing moderation response from OpenAI API") { }
}

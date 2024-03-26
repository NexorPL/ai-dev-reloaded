namespace AI.Devs.Reloaded.API.Exceptions;

public class MissingCompletionsException : Exception
{
    public MissingCompletionsException() : base("Missing completions results from OpenAI API") { }
}

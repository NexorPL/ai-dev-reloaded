namespace AI.Devs.Reloaded.API.Exceptions;

public class MissingEmbeddingException : Exception
{
    public MissingEmbeddingException() : base ("Missing embedding result") { }
}

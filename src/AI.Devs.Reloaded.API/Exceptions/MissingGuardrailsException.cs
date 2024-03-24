namespace AI.Devs.Reloaded.API.Exceptions;

public class MissingGuardrailsException : Exception
{
    public MissingGuardrailsException() : base("Missing Guardrails response") { }
}

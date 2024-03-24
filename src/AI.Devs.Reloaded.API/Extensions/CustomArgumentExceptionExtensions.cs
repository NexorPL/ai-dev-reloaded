namespace AI.Devs.Reloaded.API.Extensions;

public static class CustomArgumentExceptionExtensions
{
    public static void ThrowIfNull<T>(T param, string name)
    {
        if (param == null) throw new ArgumentNullException(name);
    }
}

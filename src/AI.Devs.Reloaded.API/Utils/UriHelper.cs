namespace AI.Devs.Reloaded.API.Utils;

internal class UriHelper
{
    internal static Uri CreateRelativeUri(string relativeUri)
    {
        return new Uri(relativeUri, UriKind.Relative);
    }
}

namespace AI.Devs.OwnApi.Utils;

internal class UriHelper
{
    internal static Uri CreateRelativeUri(string relativeUri)
    {
        return new Uri(relativeUri, UriKind.Relative);
    }
}

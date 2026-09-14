using Codespirals.Base.Extensions;

namespace Codespirals.Blazor.WebsiteTemplate;

public static class UrlHelper
{
    /// <summary>
    /// Turn a string lowercase and change all characters that aren't "url friendly" into '_'
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    internal static string NormalizeForUrl(this string text)
        => text.MakeUrlSafe('_').Trim('_').ToLowerInvariant();
}

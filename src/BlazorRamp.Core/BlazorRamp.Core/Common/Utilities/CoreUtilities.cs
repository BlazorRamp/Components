using BlazorRamp.Core.Common.Constants;
using System.Security.Claims;

namespace BlazorRamp.Core.Common.Utilities;

internal static class CoreUtilities
{
    public static string? GetStyleAsValue(StyleAs styleAs)

    => styleAs switch
    {
        StyleAs.OnLight => CoreGlobalValues.Style_As_Light,
        StyleAs.OnDark  => CoreGlobalValues.Style_As_Dark,
        _ => null
    };


    public static string? CreateClassList(params string[] classes)
    {
        if (classes.Length == 0) return null;

        var classList = String.Join(" ", classes.Where(c => !String.IsNullOrWhiteSpace(c)).Select(c => c.Trim()));

        return String.IsNullOrWhiteSpace(classList) ? null : classList;
    }
}

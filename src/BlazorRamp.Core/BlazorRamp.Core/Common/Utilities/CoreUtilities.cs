using BlazorRamp.Core.Common.Constants;

namespace BlazorRamp.Core.Common.Utilities;

/// <summary>
/// Provides utility methods for common core operations within the BlazorRamp library.
/// </summary>
internal static class CoreUtilities
{

    /// <summary>
    /// Converts a <see cref="StyleAs"/> enum value to its corresponding string data attribute value.
    /// </summary>
    /// <param name="styleAs">The style context to convert.</param>
    /// <returns>
    /// A string value of "on-light" for <see cref="StyleAs.OnLight"/>, 
    /// "dark" for <see cref="StyleAs.OnDark"/>, 
    /// or <c>null</c> for <see cref="StyleAs.Dynamic"/>. 
    /// that excludes the data attribute from being added
    /// </returns>
    public static string? GetStyleAsValue(StyleAs styleAs)

    => styleAs switch
    {
        StyleAs.OnLight => CoreGlobalValues.Style_As_Light,
        StyleAs.OnDark  => CoreGlobalValues.Style_As_Dark,
        _ => null
    };

    /// <summary>
    /// Creates a space-separated CSS class list from the provided class names, filtering out null or whitespace entries.
    /// </summary>
    /// <param name="classes">An array of CSS class names to combine.</param>
    /// <returns>
    /// A space-separated string of trimmed class names, or <c>null</c> if no valid classes are provided.
    /// </returns>
    public static string? CreateClassList(params string[] classes)
    {
        if (classes.Length == 0) return null;

        var classList = String.Join(" ", classes.Where(c => !String.IsNullOrWhiteSpace(c)).Select(c => c.Trim()));

        return String.IsNullOrWhiteSpace(classList) ? null : classList;
    }
}

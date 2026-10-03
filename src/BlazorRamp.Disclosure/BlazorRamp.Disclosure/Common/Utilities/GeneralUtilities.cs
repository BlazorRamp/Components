using System.Text.RegularExpressions;

namespace BlazorRamp.Disclosure.Common.Utilities;

internal static class GeneralUtilities
{
    private static readonly Regex _cssLengthRegex = new(@"^(?:[0-9]+(?:\.[0-9]+)?|\.[0-9]+)(?:rem|em|lh|ch|px|vh|dvh|svh|lvh|vw)\z", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

    /// <summary>
    /// Determines whether the value is a non-negative number followed by one of the supported CSS length units.
    /// </summary>
    /// <param name="value">The value to check, for example <c>20rem</c>.</param>
    /// <returns><see langword="true"/> if the value is a supported CSS length; otherwise <see langword="false"/>.</returns>
    internal static bool IsValidCssLength(string? value)

        => !String.IsNullOrWhiteSpace(value) && _cssLengthRegex.IsMatch(value.Trim());
}

using BlazorRamp.CssClasses.Common.Constants;

namespace BlazorRamp.CssClasses.Common.Utilities;

/// <summary>
/// Provides the BEM CSS classes for the <c>br-kbd</c> block: an inline element
/// used to present a keyboard key or key combination within a line of text.
/// </summary>
public static class Kbd
{
    /// <summary>
    /// The base class, <c>br-kbd</c>. Always required.
    /// </summary>
    public const string Base = "br-kbd";

    /// <summary>
    /// Gets the class that sets the key's colour scheme.
    /// </summary>
    /// <param name="scheme">The colour scheme to apply. See <see cref="KbdScheme"/>.</param>
    /// <returns>The <c>br-kbd</c> modifier class for the given <paramref name="scheme"/>.</returns>
    public static string Scheme(KbdScheme scheme)

        => scheme switch
        {
            KbdScheme.Neutral   => $"{Base}--neutral-scheme",
            KbdScheme.Primary   => $"{Base}--primary-scheme",
            KbdScheme.Secondary => $"{Base}--secondary-scheme",
            KbdScheme.Accent    => $"{Base}--accent-scheme",
            _                   => $"{Base}--inverted-scheme",
        };

    /// <summary>
    /// Gets the class that sets the key's font size.
    /// </summary>
    /// <param name="fontSize">The font size to apply. See <see cref="KbdFontSize"/>.</param>
    /// <returns>The <c>br-kbd</c> modifier class for the given <paramref name="fontSize"/>.</returns>
    public static string FontSize(KbdFontSize fontSize)
    {
        return fontSize switch
        {
            KbdFontSize.Label   => $"{Base}--font-size-label",
            KbdFontSize.Regular => $"{Base}--font-size-one",
            KbdFontSize.Two     => $"{Base}--font-size-two",
            KbdFontSize.Three   => $"{Base}--font-size-three",
            KbdFontSize.Four    => $"{Base}--font-size-four",
            KbdFontSize.Five    => $"{Base}--font-size-five",
            KbdFontSize.Six     => $"{Base}--font-size-six",
            _                   => $"{Base}--font-size-one",
        };
    }
}

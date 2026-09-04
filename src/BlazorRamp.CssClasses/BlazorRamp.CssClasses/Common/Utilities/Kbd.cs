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
    /// <param name="unitFontSize">The font size to apply. See <see cref="UnitFontSize"/>.</param>
    /// <returns>The <c>br-kbd</c> modifier class for the given <paramref name="unitFontSize"/>.</returns>
    public static string FontSize(UnitFontSize unitFontSize)
    {
        return unitFontSize switch
        {
            UnitFontSize.Label   => $"{Base}--font-size-label",
            UnitFontSize.Regular => $"{Base}--font-size-one",
            UnitFontSize.Two     => $"{Base}--font-size-two",
            UnitFontSize.Three   => $"{Base}--font-size-three",
            UnitFontSize.Four    => $"{Base}--font-size-four",
            UnitFontSize.Five    => $"{Base}--font-size-five",
            UnitFontSize.Six     => $"{Base}--font-size-six",
            _                    => $"{Base}--font-size-one",
        };
    }

    /// <summary>
    /// Gets the class that sets the key's border radius to a fixed <c>--br-unit-radius-*</c>
    /// value, bypassing the key's default fixed radius.
    /// </summary>
    /// <param name="unitRadius">The fixed radius to apply. See <see cref="UnitRadius"/>.</param>
    /// <returns>The <c>br-kbd</c> modifier class for the given <paramref name="unitRadius"/>.</returns>
    public static string FixedRadius(UnitRadius unitRadius)

        => unitRadius switch
        {
            UnitRadius.None  => $"{Base}--radius-none",
            UnitRadius.One   => $"{Base}--radius-one",
            UnitRadius.Two   => $"{Base}--radius-two",
            UnitRadius.Three => $"{Base}--radius-three",
            UnitRadius.Four  => $"{Base}--radius-four",
            UnitRadius.Five  => $"{Base}--radius-five",
            UnitRadius.Six   => $"{Base}--radius-six",
            UnitRadius.Seven => $"{Base}--radius-seven",
            UnitRadius.Eight => $"{Base}--radius-eight",
            UnitRadius.Nine  => $"{Base}--radius-nine",
            UnitRadius.Full  => $"{Base}--radius-full",
            _ => $"{Base}--radius-none",
        };
}

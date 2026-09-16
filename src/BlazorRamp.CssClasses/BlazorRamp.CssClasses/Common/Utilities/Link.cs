using BlazorRamp.CssClasses.Common.Constants;

namespace BlazorRamp.CssClasses.Common.Utilities;

/// <summary>
/// Provides the BEM CSS classes for the <c>br-link</c> block: a theme-reactive
/// link element that shares its underlying styling with every Blazor Ramp component.
/// </summary>
public static class Link
{

    /// <summary>
    /// The base class, <c>br-link</c>. Always required.
    /// </summary>
    public const string Base = "br-link";


    /// <summary>
    /// Sets the links flex layout axis to use the direction of column instead of the default row.
    /// </summary>
    public const string UseColumns = $"{Base}--columns";

    /// <summary>
    /// Gets the class that sets the elements border radius to a fixed <c>--br-unit-radius-*</c>
    /// value, bypassing the themeable link border, part of the button border grouping that is used by default..
    /// </summary>
    /// <param name="unitRadius">The fixed radius to apply. See <see cref="UnitRadius"/>.</param>
    /// <returns>The <c>br-button</c> modifier class for the given <paramref name="unitRadius"/>.</returns>
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


using BlazorRamp.CssClasses.Common.Constants;

namespace BlazorRamp.CssClasses.Common.Utilities;

/// <summary>
/// Provides the <c>br-font-size-*</c> and <c>br-font-weight-*</c> utility classes used to set
/// an element's font size and weight, each from its own <c>--br-unit-*</c> primitive scale.
/// These are flat, single-purpose classes intended to be combined freely - including with
/// each other, e.g. a larger, bolder heading.
/// </summary>
public static class Font
{
    private const string  _sizeBase  = "br-font-size";
    private const string _weightBase = "br-font-weight";

    /// <summary>
    /// Gets the class that sets an element's font size.
    /// </summary>
    /// <param name="unitFontSize">The font size to apply. See <see cref="UnitFontSize"/>.</param>
    /// <returns>The <c>br-font-size-*</c> utility class for the given <paramref name="unitFontSize"/>.</returns>
    public static string Size(UnitFontSize unitFontSize)

        => unitFontSize switch
        {
            UnitFontSize.Label   => $"{_sizeBase}-label",
            UnitFontSize.Regular => $"{_sizeBase}-one",
            UnitFontSize.Two     => $"{_sizeBase}-two",
            UnitFontSize.Three   => $"{_sizeBase}-three",
            UnitFontSize.Four    => $"{_sizeBase}-four",
            UnitFontSize.Five    => $"{_sizeBase}-five",
            UnitFontSize.Six     => $"{_sizeBase}-six",
            _ => $"{_sizeBase}-one",
        };

    /// <summary>
    /// Gets the class that sets an element's font weight.
    /// </summary>
    /// <param name="unitFontWeight">The font weight to apply. See <see cref="UnitFontWeight"/>.</param>
    /// <returns>The <c>br-font-weight-*</c> utility class for the given <paramref name="unitFontWeight"/>.</returns>
    public static string Weight(UnitFontWeight unitFontWeight)

        => unitFontWeight switch
        {
            UnitFontWeight.Thin       => $"{_weightBase}-100",
            UnitFontWeight.ExtraLight => $"{_weightBase}-200",
            UnitFontWeight.Light      => $"{_weightBase}-300",
            UnitFontWeight.Normal     => $"{_weightBase}-400",
            UnitFontWeight.Medium     => $"{_weightBase}-500",
            UnitFontWeight.SemiBold   => $"{_weightBase}-600",
            UnitFontWeight.Bold       => $"{_weightBase}-700",
            _                         => $"{_weightBase}-400",
        };
}
using BlazorRamp.CssClasses.Common.Constants;

namespace BlazorRamp.CssClasses.Common.Utilities;

/// <summary>
/// Provides utility classes that set an element's border radius from the
/// <c>--br-unit-radius-*</c> primitive scale. Unlike the BEM block classes elsewhere
/// in this library, these are flat, single-purpose utility classes intended to be
/// combined freely with other classes on any element.
/// </summary>
/// <remarks>
/// The four corner methods use CSS logical border radius properties
/// (<c>border-start-start-radius</c> and similar), which adapt automatically to the
/// element's writing direction. These names assume a horizontal writing mode; in a
/// vertical writing mode, the block and inline axes are swapped.
/// </remarks>
public static class Radius
{
    private const string _radiusBase = "br-radius";

    /// <summary>
    /// Gets the class that sets a uniform border radius on all four corners.
    /// </summary>
    /// <param name="unitRadius">The radius value to apply. See <see cref="UnitRadius"/>.</param>
    /// <returns>The <c>br-radius-all-*</c> utility class for the given <paramref name="unitRadius"/>.</returns>
    public static string All(UnitRadius unitRadius)

        => GenerateClass(unitRadius, $"{_radiusBase}-all");

    /// <summary>
    /// Gets the class that sets the corner between the block-start and inline-start sides
    /// of the element (the top-left corner in a horizontal, left-to-right writing mode).
    /// </summary>
    /// <param name="unitRadius">The radius value to apply. See <see cref="UnitRadius"/>.</param>
    /// <returns>The <c>br-radius-start-start-*</c> utility class for the given <paramref name="unitRadius"/>.</returns>
    public static string BlockStartInlineStart(UnitRadius unitRadius)

        => GenerateClass(unitRadius, $"{_radiusBase}-start-start");

    /// <summary>
    /// Gets the class that sets the corner between the block-start and inline-end sides
    /// of the element (the top-right corner in a horizontal, left-to-right writing mode).
    /// </summary>
    /// <param name="unitRadius">The radius value to apply. See <see cref="UnitRadius"/>.</param>
    /// <returns>The <c>br-radius-start-end-*</c> utility class for the given <paramref name="unitRadius"/>.</returns>
    public static string BlockStartInlineEnd(UnitRadius unitRadius)

        => GenerateClass(unitRadius, $"{_radiusBase}-start-end");

    /// <summary>
    /// Gets the class that sets the corner between the block-end and inline-start sides
    /// of the element (the bottom-left corner in a horizontal, left-to-right writing mode).
    /// </summary>
    /// <param name="unitRadius">The radius value to apply. See <see cref="UnitRadius"/>.</param>
    /// <returns>The <c>br-radius-end-start-*</c> utility class for the given <paramref name="unitRadius"/>.</returns>
    public static string BlockEndInlineStart(UnitRadius unitRadius)

        => GenerateClass(unitRadius, $"{_radiusBase}-end-start");

    /// <summary>
    /// Gets the class that sets the corner between the block-end and inline-end sides
    /// of the element (the bottom-right corner in a horizontal, left-to-right writing mode).
    /// </summary>
    /// <param name="unitRadius">The radius value to apply. See <see cref="UnitRadius"/>.</param>
    /// <returns>The <c>br-radius-end-end-*</c> utility class for the given <paramref name="unitRadius"/>.</returns>
    public static string BlockEndInlineEnd(UnitRadius unitRadius)

        => GenerateClass(unitRadius, $"{_radiusBase}-end-end");

    private static string GenerateClass(UnitRadius unitRadius, string className)

        => unitRadius switch
        {
            UnitRadius.None  => $"{className}-0",
            UnitRadius.One   => $"{className}-1",
            UnitRadius.Two   => $"{className}-2",
            UnitRadius.Three => $"{className}-3",
            UnitRadius.Four  => $"{className}-4",
            UnitRadius.Five  => $"{className}-5",
            UnitRadius.Six   => $"{className}-6",
            UnitRadius.Seven => $"{className}-7",
            UnitRadius.Eight => $"{className}-8",
            UnitRadius.Nine  => $"{className}-9",
            UnitRadius.Full  => $"{className}-full",
            _                => $"{className}-0"
        };
}
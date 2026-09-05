using BlazorRamp.CssClasses.Common.Constants;

namespace BlazorRamp.CssClasses.Common.Utilities;

/// <summary>
/// Provides utility classes that set padding from the <c>--br-unit-space-*</c>
/// primitive scale. Unlike the BEM block classes elsewhere in this library, these
/// are flat, single-purpose utility classes intended to be combined freely with
/// other classes on any element.
/// </summary>
public static class Padding
{
    /// <summary>
    /// Gets the class that sets padding on all sides.
    /// </summary>
    /// <param name="unitSpace">The spacing value to apply. See <see cref="UnitSpace"/>.</param>
    /// <returns>The <c>br-padding-*</c> utility class for the given <paramref name="unitSpace"/>.</returns>
    public static string All(UnitSpace unitSpace)
 
        => GenerateClass(unitSpace, "br-padding");
 
    /// <summary>
    /// Gets the class that sets inline-direction (left/right in LTR) padding.
    /// </summary>
    /// <param name="unitSpace">The spacing value to apply. See <see cref="UnitSpace"/>.</param>
    /// <returns>The <c>br-padding-inline-*</c> utility class for the given <paramref name="unitSpace"/>.</returns>
    public static string Inline(UnitSpace unitSpace)
 
        => GenerateClass(unitSpace, "br-padding-inline");
 
    /// <summary>
    /// Gets the class that sets inline-start (left in LTR) padding.
    /// </summary>
    /// <param name="unitSpace">The spacing value to apply. See <see cref="UnitSpace"/>.</param>
    /// <returns>The <c>br-padding-inline-start-*</c> utility class for the given <paramref name="unitSpace"/>.</returns>
    public static string InlineStart(UnitSpace unitSpace)
 
        => GenerateClass(unitSpace, "br-padding-inline-start");
 
    /// <summary>
    /// Gets the class that sets inline-end (right in LTR) padding.
    /// </summary>
    /// <param name="unitSpace">The spacing value to apply. See <see cref="UnitSpace"/>.</param>
    /// <returns>The <c>br-padding-inline-end-*</c> utility class for the given <paramref name="unitSpace"/>.</returns>
    public static string InlineEnd(UnitSpace unitSpace)
 
        => GenerateClass(unitSpace, "br-padding-inline-end");
 
    /// <summary>
    /// Gets the class that sets block-direction (top/bottom) padding.
    /// </summary>
    /// <param name="unitSpace">The spacing value to apply. See <see cref="UnitSpace"/>.</param>
    /// <returns>The <c>br-padding-block-*</c> utility class for the given <paramref name="unitSpace"/>.</returns>
    public static string Block(UnitSpace unitSpace)
 
        => GenerateClass(unitSpace, "br-padding-block");
 
    /// <summary>
    /// Gets the class that sets block-start (top) padding.
    /// </summary>
    /// <param name="unitSpace">The spacing value to apply. See <see cref="UnitSpace"/>.</param>
    /// <returns>The <c>br-padding-block-start-*</c> utility class for the given <paramref name="unitSpace"/>.</returns>
    public static string BlockStart(UnitSpace unitSpace)
 
        => GenerateClass(unitSpace, "br-padding-block-start");
 
    /// <summary>
    /// Gets the class that sets block-end (bottom) padding.
    /// </summary>
    /// <param name="unitSpace">The spacing value to apply. See <see cref="UnitSpace"/>.</param>
    /// <returns>The <c>br-padding-block-end-*</c> utility class for the given <paramref name="unitSpace"/>.</returns>
    public static string BlockEnd(UnitSpace unitSpace)
 
        => GenerateClass(unitSpace, "br-padding-block-end");
 
    private static string GenerateClass(UnitSpace spaceUnit, string className)
 
        => spaceUnit switch
        {
            UnitSpace.None     => $"{className}-0",
            UnitSpace.One      => $"{className}-1",
            UnitSpace.Two      => $"{className}-2",
            UnitSpace.Three    => $"{className}-3",
            UnitSpace.Four     => $"{className}-4",
            UnitSpace.Five     => $"{className}-5",
            UnitSpace.Six      => $"{className}-6",
            UnitSpace.Seven    => $"{className}-7",
            UnitSpace.Eight    => $"{className}-8",
            UnitSpace.Nine     => $"{className}-9",
            UnitSpace.Ten      => $"{className}-10",
            UnitSpace.Eleven   => $"{className}-11",
            UnitSpace.Twelve   => $"{className}-12",
            UnitSpace.Thirteen => $"{className}-13",
            UnitSpace.Fourteen => $"{className}-14",
            UnitSpace.Fifteen  => $"{className}-15",
            _ => $"{className}-0",
        };
}

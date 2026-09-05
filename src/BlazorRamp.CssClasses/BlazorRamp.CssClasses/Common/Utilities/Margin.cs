using BlazorRamp.CssClasses.Common.Constants;

namespace BlazorRamp.CssClasses.Common.Utilities;


/// <summary>
/// Provides utility classes that set margin from the <c>--br-unit-space-*</c>
/// primitive scale, plus fixed <c>auto</c> margin classes. Unlike the BEM block
/// classes elsewhere in this library, these are flat, single-purpose utility
/// classes intended to be combined freely with other classes on any element.
/// </summary>
public static class Margin
{
    /// <summary>
    /// Centres the element horizontally by setting inline-direction margin to <c>auto</c>.
    /// </summary>
    public const string AutoInline = "br-margin-inline-auto";
 
    /// <summary>
    /// Sets block-direction margin to <c>auto</c>.
    /// </summary>
    public const string AutoBlock = "br-margin-block-auto";
 
    /// <summary>
    /// Sets margin to <c>auto</c> on all sides.
    /// </summary>
    public const string Auto = "br-margin-auto";
 
    /// <summary>
    /// Gets the class that sets margin on all sides.
    /// </summary>
    /// <param name="unitSpace">The spacing value to apply. See <see cref="UnitSpace"/>.</param>
    /// <returns>The <c>br-margin-*</c> utility class for the given <paramref name="unitSpace"/>.</returns>
    public static string All(UnitSpace unitSpace)
 
        => GenerateClass(unitSpace, "br-margin");
 
    /// <summary>
    /// Gets the class that sets inline-direction (left/right in LTR) margin.
    /// </summary>
    /// <param name="unitSpace">The spacing value to apply. See <see cref="UnitSpace"/>.</param>
    /// <returns>The <c>br-margin-inline-*</c> utility class for the given <paramref name="unitSpace"/>.</returns>
    public static string Inline(UnitSpace unitSpace)
 
        => GenerateClass(unitSpace, "br-margin-inline");
 
    /// <summary>
    /// Gets the class that sets inline-start (left in LTR) margin.
    /// </summary>
    /// <param name="unitSpace">The spacing value to apply. See <see cref="UnitSpace"/>.</param>
    /// <returns>The <c>br-margin-inline-start-*</c> utility class for the given <paramref name="unitSpace"/>.</returns>
    public static string InlineStart(UnitSpace unitSpace)
 
        => GenerateClass(unitSpace, "br-margin-inline-start");
 
    /// <summary>
    /// Gets the class that sets inline-end (right in LTR) margin.
    /// </summary>
    /// <param name="unitSpace">The spacing value to apply. See <see cref="UnitSpace"/>.</param>
    /// <returns>The <c>br-margin-inline-end-*</c> utility class for the given <paramref name="unitSpace"/>.</returns>
    public static string InlineEnd(UnitSpace unitSpace)
 
        => GenerateClass(unitSpace, "br-margin-inline-end");
 
    /// <summary>
    /// Gets the class that sets block-direction (top/bottom) margin.
    /// </summary>
    /// <param name="unitSpace">The spacing value to apply. See <see cref="UnitSpace"/>.</param>
    /// <returns>The <c>br-margin-block-*</c> utility class for the given <paramref name="unitSpace"/>.</returns>
    public static string Block(UnitSpace unitSpace)
 
        => GenerateClass(unitSpace, "br-margin-block");
 
    /// <summary>
    /// Gets the class that sets block-start (top) margin.
    /// </summary>
    /// <param name="unitSpace">The spacing value to apply. See <see cref="UnitSpace"/>.</param>
    /// <returns>The <c>br-margin-block-start-*</c> utility class for the given <paramref name="unitSpace"/>.</returns>
    public static string BlockStart(UnitSpace unitSpace)
 
        => GenerateClass(unitSpace, "br-margin-block-start");
 
    /// <summary>
    /// Gets the class that sets block-end (bottom) margin.
    /// </summary>
    /// <param name="unitSpace">The spacing value to apply. See <see cref="UnitSpace"/>.</param>
    /// <returns>The <c>br-margin-block-end-*</c> utility class for the given <paramref name="unitSpace"/>.</returns>
    public static string BlockEnd(UnitSpace unitSpace)
 
        => GenerateClass(unitSpace, "br-margin-block-end");
 
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
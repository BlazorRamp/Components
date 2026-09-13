using BlazorRamp.CssClasses.Common.Constants;

namespace BlazorRamp.CssClasses.Common.Utilities;

/// <summary>
/// Provides the <c>br-gaps-*</c>, <c>br-row-gap-*</c>, and <c>br-col-gap-*</c> utility
/// classes used to set the gap between children of a flex or grid container. These are
/// flat, single-purpose classes intended to be combined freely - e.g. <see cref="SetGaps"/>
/// to set both axes, then <see cref="SetRowGap"/> or <see cref="SetColGap"/> to override
/// just one axis.
/// </summary>
public static class Gap
{
    private const string _gapBase    = "br-gaps";
    private const string _rowGapBase = "br-row-gap";
    private const string _colGapBase = "br-col-gap";

    /// <summary>
    /// Gets the class that sets the row gap only, between a container's children.
    /// </summary>
    /// <param name="unitGapSize">The gap size to apply. See <see cref="UnitGapSize"/>.</param>
    /// <returns>The <c>br-row-gap-*</c> utility class for the given <paramref name="unitGapSize"/>.</returns>
    public static string SetRowGap(UnitGapSize unitGapSize)

        => GenerateClass(unitGapSize, _rowGapBase);

    /// <summary>
    /// Gets the class that sets the column gap only, between a container's children.
    /// </summary>
    /// <param name="unitGapSize">The gap size to apply. See <see cref="UnitGapSize"/>.</param>
    /// <returns>The <c>br-col-gap-*</c> utility class for the given <paramref name="unitGapSize"/>.</returns>
    public static string SetColGap(UnitGapSize unitGapSize)

        => GenerateClass(unitGapSize, _colGapBase);

    /// <summary>
    /// Gets the class that sets both the row and column gap, between a container's children.
    /// </summary>
    /// <param name="unitGapSize">The gap size to apply. See <see cref="UnitGapSize"/>.</param>
    /// <returns>The <c>br-gaps-*</c> utility class for the given <paramref name="unitGapSize"/>.</returns>
    public static string SetGaps(UnitGapSize unitGapSize)

        => GenerateClass(unitGapSize, _gapBase);

    private static string GenerateClass(UnitGapSize unitGapSize, string className)

        => unitGapSize switch
        {
            UnitGapSize.None     => $"{className}-0",
            UnitGapSize.One      => $"{className}-1",
            UnitGapSize.Two      => $"{className}-2",
            UnitGapSize.Three    => $"{className}-3",
            UnitGapSize.Four     => $"{className}-4",
            UnitGapSize.Five     => $"{className}-5",
            UnitGapSize.Six      => $"{className}-6",
            UnitGapSize.Seven    => $"{className}-7",
            UnitGapSize.Eight    => $"{className}-8",
            UnitGapSize.Nine     => $"{className}-9",
            UnitGapSize.Ten      => $"{className}-10",
            UnitGapSize.Eleven   => $"{className}-11",
            UnitGapSize.Twelve   => $"{className}-12",
            UnitGapSize.Thirteen => $"{className}-13",
            UnitGapSize.Fourteen => $"{className}-14",
            UnitGapSize.Fifteen  => $"{className}-15",
            _                    => $"{className}-0",
        };
}
using BlazorRamp.CssClasses.Common.Constants;

namespace BlazorRamp.CssClasses.Common.Utilities;

/// <summary>
/// Provides the classes for the <c>br-grid-row</c> block: a 12-column CSS Grid
/// container, and the <c>br-grid-col-*</c> utility classes used to size its
/// children across the grid at a given breakpoint and up.
/// </summary>
public static class GridRow
{
    /// <summary>
    /// The base class, <c>br-grid-row</c>. Always required on the container element.
    /// </summary>
    public const string Base = "br-grid-row";

    /// <summary>
    /// Removes the row's default block-axis margin.
    /// </summary>
    public const string NoMarginBlock = $"{Base}--no-margin-block";

    /// <summary>
    /// Gets the class that sets a child element's column span at and above the given
    /// <paramref name="unitBreakpoint"/>.
    /// </summary>
    /// <param name="unitBreakpoint">The breakpoint from which this span applies. See <see cref="UnitBreakpoint"/>.</param>
    /// <param name="unitColSpan">The number of columns to span, 1 to 12. See <see cref="UnitColSpan"/>.</param>
    /// <returns>The <c>br-grid-col-*-*</c> utility class for the given <paramref name="unitBreakpoint"/> and <paramref name="unitColSpan"/>.</returns>
    public static string ColSpan(UnitBreakpoint unitBreakpoint, UnitColSpan unitColSpan)
    {
        string infix = unitBreakpoint switch
        {
            UnitBreakpoint.Xs  => "xs",
            UnitBreakpoint.Sm  => "sm",
            UnitBreakpoint.Md  => "md",
            UnitBreakpoint.Lg  => "lg",
            UnitBreakpoint.Xl  => "xl",
            UnitBreakpoint.Xxl => "xxl",
            _                  => "xs",
        };

        return $"br-grid-col-{infix}-{(int)unitColSpan}";
    }

}
using BlazorRamp.CssClasses.Common.Constants;

namespace BlazorRamp.CssClasses.Common.Utilities;

/// <summary>
/// Provides the BEM CSS classes for the <c>br-table</c> block: a theme-reactive,
/// static (non-interactive) table for presenting tabular content.
/// </summary>
public static class Table
{
    /// <summary>
    /// The base class, <c>br-table</c>. Always required. Includes its own border and
    /// border radius, suitable for a table placed directly on the page.
    /// </summary>
    public const string Base = "br-table";

    /// <summary>
    /// Applies an alternating background colour to every other body row.
    /// </summary>
    public const string StripedRows = $"{Base}--striped-rows";

    /// <summary>
    /// Removes the table's own border and border radius, and makes the header row
    /// sticky. Use when wrapping the table in its own bordered, rounded scroll
    /// container, so the table doesn't draw a duplicate frame inside it.
    /// </summary>
    public const string NoBorder = $"{Base}--no-border";

    /// <summary>
    /// Highlights a row's background colour on hover.
    /// </summary>
    public const string HoverRows = $"{Base}--hover-rows";

    /// <summary>
    /// Gets the class that aligns a whole column - its header and every body cell -
    /// by targeting the given column position.
    /// </summary>
    /// <param name="unitColumn">The 1-based column position to target. See <see cref="UnitColumn"/>.</param>
    /// <param name="unitTextAlign">The alignment to apply. See <see cref="UnitTextAlign"/>.</param>
    /// <returns>The <c>br-table--align-col-*</c> class for the given <paramref name="unitColumn"/> and <paramref name="unitTextAlign"/>.</returns>
    public static string AlignColumn(UnitColumn unitColumn, UnitTextAlign unitTextAlign)
    {
        string alignment = unitTextAlign switch
        {
            UnitTextAlign.Start => "start",
            UnitTextAlign.Centre => "center",
            UnitTextAlign.End => "end",
            UnitTextAlign.Justify => "justify",
            _ => "start",
        };

        return $"{Base}--align-col-{(int)unitColumn}-{alignment}";
    }
}
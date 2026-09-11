using BlazorRamp.CssClasses.Common.Utilities;

namespace BlazorRamp.CssClasses.Common.Constants;

/// <summary>
/// Sets a column span from 1 to 12, used to size an element across the
/// 12-column grid via <see cref="GridRow.ColSpan"/>.
/// </summary>
public enum UnitColSpan : int
{
    /// <summary>
    /// Spans 1 column.
    /// </summary>
    One = 1,

    /// <summary>
    /// Spans 2 columns.
    /// </summary>
    Two = 2,

    /// <summary>
    /// Spans 3 columns.
    /// </summary>
    Three = 3,

    /// <summary>
    /// Spans 4 columns.
    /// </summary>
    Four = 4,

    /// <summary>
    /// Spans 5 columns.
    /// </summary>
    Five = 5,

    /// <summary>
    /// Spans 6 columns.
    /// </summary>
    Six = 6,

    /// <summary>Spans 7 columns.
    /// </summary>
    Seven = 7,

    /// <summary>
    /// Spans 8 columns.
    /// </summary>
    Eight = 8,

    /// <summary>
    /// Spans 9 columns.
    /// </summary>
    Nine = 9,

    /// <summary>
    /// Spans 10 columns.
    /// </summary>
    Ten = 10,

    /// <summary>
    /// Spans 11 columns.
    /// </summary>
    Eleven = 11,

    /// <summary>
    /// Spans all 12 columns (full width).
    /// </summary>
    Twelve = 12
}

/// <summary>
/// Identifies a responsive breakpoint, matching the <c>$breakpoints</c> Sass map
/// used to generate <see cref="GridCol"/>'s media-query classes.
/// </summary>
public enum UnitBreakpoint : int
{
    /// <summary>The base tier, applying at all widths. Maps to 0px.</summary>
    Xs = 0,
    /// <summary>
    /// Maps to 576px and up.
    /// </summary>
    Sm = 1,
    /// <summary>
    /// Maps to 768px and up.
    /// </summary>
    Md = 2,
    /// <summary>
    /// Maps to 992px and up.
    /// </summary>
    Lg = 3,
    /// <summary>
    /// Maps to 1280px and up.
    /// </summary>
    Xl = 4,
    /// <summary>
    /// Maps to 1536px and up.
    /// </summary>
    Xxl = 5
}
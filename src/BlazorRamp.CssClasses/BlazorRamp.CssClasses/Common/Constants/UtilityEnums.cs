using BlazorRamp.CssClasses.Common.Utilities;

namespace BlazorRamp.CssClasses.Common.Constants;


/// <summary>
/// Sets a spacing value from the <c>--br-unit-space-*</c> primitive scale.
/// Used by the <see cref="Padding"/> and <see cref="Margin"/> utility classes.
/// </summary>
public enum UnitSpace : int
{
    /// <summary>
    /// No spacing. Maps to <c>--br-unit-space-0</c>.
    /// </summary>
    None = 0,
    /// <summary>
    /// Maps to <c>--br-unit-space-1</c>.
    /// </summary>
    One = 1,
    /// <summary>
    /// Maps to <c>--br-unit-space-2</c>.
    /// </summary>
    Two = 2,
    /// <summary>
    /// Maps to <c>--br-unit-space-3</c>.
    /// </summary>
    Three = 3,
    /// <summary>
    /// Maps to <c>--br-unit-space-4</c>.
    /// </summary>
    Four = 4,
    /// <summary>
    /// Maps to <c>--br-unit-space-5</c>.
    /// </summary>
    Five = 5,
    /// <summary>
    /// Maps to <c>--br-unit-space-6</c>.
    /// </summary>
    Six = 6,
    /// <summary>
    /// Maps to <c>--br-unit-space-7</c>.
    /// </summary>
    Seven = 7,
    /// <summary>
    /// Maps to <c>--br-unit-space-8</c>.
    /// </summary>
    Eight = 8,
    /// <summary>
    /// Maps to <c>--br-unit-space-9</c>.
    /// </summary>
    Nine = 9,
    /// <summary>
    /// Maps to <c>--br-unit-space-10</c>.
    /// </summary>
    Ten = 10,
    /// <summary>
    /// Maps to <c>--br-unit-space-11</c>.
    /// </summary>
    Eleven = 11,
    /// <summary>
    /// Maps to <c>--br-unit-space-12</c>.
    /// </summary>
    Twelve = 12,
    /// <summary>
    /// Maps to <c>--br-unit-space-13</c>.
    /// </summary>
    Thirteen = 13,
    /// <summary>
    /// Maps to <c>--br-unit-space-14</c>.
    /// </summary>
    Fourteen = 14,
    /// <summary>
    /// Maps to <c>--br-unit-space-15</c>.
    /// </summary>
    Fifteen = 15,
}

/// <summary>
/// Sets a fixed border radius from the <c>--br-unit-radius-*</c> primitive scale,
/// bypassing any themeable border radius grouping.
/// </summary>
public enum UnitRadius : int
{
    /// <summary>
    /// No border radius. Maps to <c>--br-unit-radius-0</c>.
    /// </summary>
    None = 0,
    /// <summary>
    /// Maps to <c>--br-unit-radius-1</c>.
    /// </summary>
    One = 1,
    /// <summary>
    /// Maps to <c>--br-unit-radius-2</c>.
    /// </summary>
    Two = 2,
    /// <summary>
    /// Maps to <c>--br-unit-radius-3</c>.
    /// </summary>
    Three = 3,
    /// <summary>
    /// Maps to <c>--br-unit-radius-4</c>.
    /// </summary>
    Four = 4,
    /// <summary>
    /// Maps to <c>--br-unit-radius-5</c>.
    /// </summary>
    Five = 5,
    /// <summary>
    /// Maps to <c>--br-unit-radius-6</c>.
    /// </summary>
    Six = 6,
    /// <summary>
    /// Maps to <c>--br-unit-radius-7</c>.
    /// </summary>
    Seven = 7,
    /// <summary>
    /// Maps to <c>--br-unit-radius-8</c>.
    /// </summary>
    Eight = 8,
    /// <summary>
    /// Maps to <c>--br-unit-radius-9</c>.
    /// </summary>
    Nine = 9,
    /// <summary>
    /// Fully rounded. Maps to <c>--br-unit-radius-full</c>.
    /// </summary>
    Full = 10
}


/// <summary>
/// Sets the font size of an element.
/// </summary>
public enum UnitFontSize : int
{
    /// <summary>
    /// Label-sized text, matching the size used for input/form labels.
    /// </summary>
    Label = 0,
    /// <summary>
    /// The regular body text size.
    /// </summary>
    Regular = 1,
    /// <summary>
    /// A larger text size, one step above regular.
    /// </summary>
    Two = 2,
    /// <summary>
    /// A larger text size, two steps above regular.
    /// </summary>
    Three = 3,
    /// <summary>
    /// A larger text size, three steps above regular.
    /// </summary>
    Four = 4,
    /// <summary>
    /// A larger text size, four steps above regular.
    /// </summary>
    Five = 5,
    /// <summary>
    /// The largest text size available on this scale.
    /// </summary>
    Six = 6
}

/// <summary>
/// Sets a CSS <c>overflow</c> value. Used by <see cref="Scroll"/> and by classes exposing
/// their own overflow modifiers, such as <see cref="Section"/>.
/// </summary>
public enum UnitScroll : int
{
    /// <summary>
    /// Content is not clipped and may overflow the element's box. The CSS initial value.
    /// </summary>
    Visible = 0,

    /// <summary>
    /// Content is clipped at the padding edge, with no scrollbars offered.
    /// </summary>
    Hidden = 1,

    /// <summary>
    /// Content is clipped, and scrollbars are always shown, whether or not content overflows.
    /// </summary>
    Scroll = 2,

    /// <summary>
    /// The browser shows scrollbars only if content actually overflows. The most common choice for scrollable regions.
    /// </summary>
    Auto = 3,

    /// <summary>
    /// Content is clipped at the padding edge, like <see cref="Hidden"/>, but does not
    /// establish a new formatting context or scroll container. Not supported in all browsers.
    /// </summary>
    Clip = 4
}


/// <summary>
/// Sets a CSS <c>position</c> value. Used by <see cref="Section.Position"/> and by the
/// <see cref="Position"/> utility class.
/// </summary>
public enum UnitPosition : int
{
    /// <summary>
    /// The element is positioned according to normal document flow. Disables anchoring
    /// for components such as the Busy Indicator that expect a positioned container.
    /// </summary>
    Static = 0,

    /// <summary>
    /// The element is positioned according to normal flow, then offset relative to
    /// itself if offset properties are set. The section's default.
    /// </summary>
    Relative = 1,

    /// <summary>
    /// The element is removed from normal flow and positioned relative to its nearest
    /// positioned ancestor.
    /// </summary>
    Absolute = 2,

    /// <summary>
    /// The element is removed from normal flow and positioned relative to the viewport.
    /// </summary>
    Fixed = 3,

    /// <summary>
    /// The element is positioned according to normal flow, then treated as fixed once
    /// its scrolling ancestor reaches a given scroll position.
    /// </summary>
    Sticky = 4
}

/// <summary>
/// Sets a CSS <c>text-align</c> value.
/// </summary>
public enum UnitTextAlign : int
{
    /// <summary>
    /// Aligns text to the start edge of the line box (left in a horizontal,
    /// left-to-right writing mode). Adapts automatically for right-to-left content.
    /// </summary>
    Start = 0,

    /// <summary>
    /// Centres text within the line box.
    /// </summary>
    Centre = 1,

    /// <summary>
    /// Aligns text to the end edge of the line box (right in a horizontal,
    /// left-to-right writing mode). Adapts automatically for right-to-left content.
    /// </summary>
    End = 2,

    /// <summary>
    /// Stretches lines so each line (other than the last) has equal width, with text
    /// spaced to fill the full width of the line box.
    /// </summary>
    Justify = 3
}

/// <summary>
/// Sets a CSS <c>vertical-align</c> value.
/// </summary>
/// <remarks>
/// Only <see cref="Top"/>, <see cref="Middle"/>, and <see cref="Bottom"/> are meaningful
/// when applied to a table cell (<c>td</c>/<c>th</c>), where they align the cell's
/// content within the cell's own height. The remaining values are intended for aligning
/// inline-level content against surrounding line content, and generally have no visible
/// effect on a table cell.
/// </remarks>
public enum UnitVerticalAlign : int
{
    /// <summary>
    /// Aligns the element's baseline with the baseline of its parent. The CSS initial value.
    /// </summary>
    Baseline = 0,

    /// <summary>
    /// Lowers the element to the parent's subscript baseline.
    /// </summary>
    Sub = 1,

    /// <summary>
    /// Raises the element to the parent's superscript baseline.
    /// </summary>
    Super = 2,

    /// <summary>
    /// Aligns the top of the element with the top of the parent element's font.
    /// </summary>
    TextTop = 3,

    /// <summary>
    /// Aligns the bottom of the element with the bottom of the parent element's font.
    /// </summary>
    TextBottom = 4,

    /// <summary>
    /// Aligns the middle of the element with the baseline plus half the x-height of the parent.
    /// </summary>
    Middle = 5,

    /// <summary>
    /// Aligns the top of the element with the top of the tallest element on the line.
    /// On a table cell, aligns the cell's content to the top of the cell.
    /// </summary>
    Top = 6,

    /// <summary>
    /// Aligns the bottom of the element with the bottom of the lowest element on the line.
    /// On a table cell, aligns the cell's content to the bottom of the cell.
    /// </summary>
    Bottom = 7
}
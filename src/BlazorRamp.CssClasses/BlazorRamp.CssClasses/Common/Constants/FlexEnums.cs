using BlazorRamp.CssClasses.Common.Utilities;

namespace BlazorRamp.CssClasses.Common.Constants;


/// <summary>
/// Sets a flex factor of 0 or 1. Used by <see cref="FlexContent.Grow"/> and <see cref="FlexContent.Shrink"/>.
/// </summary>
public enum UnitFlexFactor : int
{
    /// <summary>A factor of 0.</summary>
    None = 0,

    /// <summary>A factor of 1.</summary>
    One = 1
}
/// <summary>
/// Sets whether a <see cref="FlexContent"/> container's children may wrap onto multiple lines.
/// Used by <see cref="FlexContent.Wrap"/>.
/// </summary>
public enum UnitFlexWrap : int
{
    /// <summary>
    /// Items stay on a single line, even if that overflows the container.
    /// </summary>
    NoWrap = 0,

    /// <summary>
    /// Items wrap onto multiple lines as needed.
    /// </summary>
    Wrap = 1,

    /// <summary>
    /// Items wrap onto multiple lines as needed, with lines laid out in reverse order.
    /// </summary>
    WrapReverse = 2
}
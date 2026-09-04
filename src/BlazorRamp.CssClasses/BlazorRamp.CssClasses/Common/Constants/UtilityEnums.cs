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
    Nine = 9
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
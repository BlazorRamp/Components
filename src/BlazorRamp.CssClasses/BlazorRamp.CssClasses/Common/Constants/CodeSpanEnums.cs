namespace BlazorRamp.CssClasses.Common.Constants;


/// <summary>
/// Sets the font size of a <c>br-code-span</c> element.
/// </summary>
public enum CodeSpanFontSize : int
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
/// Sets the colour scheme (text and background colour pairing) of a <c>br-code-span</c> element.
/// </summary>
public enum CodeSpanScheme : int
{
    /// <summary>
    /// An inverted colour scheme, using the canvas colour and its inverted counterpart.
    /// </summary>
    Inverted = 0,
    /// <summary>
    /// A neutral colour scheme.
    /// </summary>
    Neutral = 1,
    /// <summary>
    /// A primary-hue colour scheme.
    /// </summary>
    Primary = 2,
    /// <summary>
    /// A secondary-hue colour scheme.
    /// </summary>
    Secondary = 3,
    /// <summary>
    /// An accent-hue colour scheme.
    /// </summary>
    Accent = 4
}
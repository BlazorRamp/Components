namespace BlazorRamp.CssClasses.Common.Constants;

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
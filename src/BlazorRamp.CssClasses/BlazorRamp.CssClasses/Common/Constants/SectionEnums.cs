namespace BlazorRamp.CssClasses.Common.Constants;

/// <summary>
/// Sets the border radius grouping applied to a <c>br-section</c> element.
/// </summary>
public enum SectionRadius : int
{
    /// <summary>
    /// Border radius for general content areas. 
    /// Uses the content border radius grouping.
    /// </summary>
    Content = 0,

    /// <summary>
    /// Border radius matching cards, tables, and similar boxed UI. 
    /// Uses the container border radius grouping.
    /// </summary>
    Container = 1,

    /// <summary>
    /// Border radius matching a dialog surface, for custom dialog-like content built outside the Modal Dialog framework.
    /// Uses the dialog border radius grouping.
    /// </summary>
    Dialog = 2,
}

/// <summary>
/// Sets the background colour of a <c>br-section</c> element.
/// </summary>
public enum SectionBackground : int
{
    /// <summary>
    /// A light neutral background colour.
    /// </summary>
    NeutralLighter = 0,

    /// <summary>
    /// A light secondary-hue background colour.
    /// </summary>
    SecondaryLighter = 1,

    /// <summary>
    /// A light accent-hue background colour.
    /// </summary>
    AccentLighter = 2,

    /// <summary>
    /// A light primary-hue background colour.
    /// </summary>
    PrimaryLighter = 3
}

/// <summary>
/// Sets the text colour of a <c>br-section</c> element.
/// </summary>
public enum SectionText : int
{
    /// <summary>
    /// Neutral text colour.
    /// </summary>
    Neutral = 0,

    /// <summary>
    /// Secondary-hue text colour.
    /// </summary>
    Secondary = 1,

    /// <summary>
    /// Accent-hue text colour.
    /// </summary>
    Accent = 2,

    /// <summary>
    /// Primary-hue text colour.
    /// </summary>
    Primary = 3
}

/// <summary>
/// Sets the base font size of a <c>br-section</c> element.
/// </summary>
public enum SectionFontSize : int
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
    /// The largest text size available on this scale.
    /// </summary>
    Three = 3,
}

/// <summary>
/// Sets the text colour of a <c>br-section__heading</c> element.
/// </summary>
public enum SectionHeadingText : int
{
    /// <summary>
    /// Neutral heading text colour.
    /// </summary>
    Neutral = 0,

    /// <summary>
    /// Secondary-hue heading text colour.
    /// </summary>
    Secondary = 1,

    /// <summary>
    /// Accent-hue heading text colour.
    /// </summary>
    Accent = 2,

    /// <summary>
    /// Primary-hue heading text colour.
    /// </summary>
    Primary = 3
}

/// <summary>
/// Sets the font size of a <c>br-section__heading</c> element.
/// </summary>
public enum SectionHeadingFontSize : int
{
    /// <summary>
    /// The regular font-size used in the body content.
    /// </summary>
    Regular = 0,

    /// <summary>
    /// A larger heading size, one step above regular and the default for section headings.
    /// </summary>
    Two = 1,

    /// <summary>
    /// A larger heading size, two steps above regular.
    /// </summary>
    Three = 2,

    /// <summary>
    /// A larger heading size, three steps above regular.
    /// </summary>
    Four = 3,

    /// <summary>
    /// A larger heading size, four steps above regular.
    /// </summary>
    Five = 4,

    /// <summary>
    /// The largest heading size available on this scale.
    /// </summary>
    Six = 5
}
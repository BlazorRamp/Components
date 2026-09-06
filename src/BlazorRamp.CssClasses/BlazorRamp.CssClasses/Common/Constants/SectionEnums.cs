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
    PrimaryLighter = 3,
   /// <summary>
   /// See-through. shows what its sitting on. Check text contrast.
   /// </summary>
    Transparent = 4,
}

/// <summary>
/// Sets the text colour of a <c>br-section</c> element.
/// </summary>
public enum SectionText : int
{
    /// <summary>
    /// Neutral text colour.
    /// </summary>
    NeutralDark = 0,

    /// <summary>
    /// Secondary-hue text colour.
    /// </summary>
    SecondaryDark = 1,

    /// <summary>
    /// Accent-hue text colour.
    /// </summary>
    AccentDark = 2,

    /// <summary>
    /// Primary-hue text colour.
    /// </summary>
    PrimaryDark = 3
}


/// <summary>
/// Sets the text colour of a <c>br-section__heading</c> element.
/// </summary>
public enum SectionHeadingText : int
{
    /// <summary>
    /// Neutral heading text colour.
    /// </summary>
    NeutralDark = 0,

    /// <summary>
    /// Secondary-hue heading text colour.
    /// </summary>
    SecondaryDark = 1,

    /// <summary>
    /// Accent-hue heading text colour.
    /// </summary>
    AccentDark = 2,

    /// <summary>
    /// Primary-hue heading text colour.
    /// </summary>
    PrimaryDark = 3
}

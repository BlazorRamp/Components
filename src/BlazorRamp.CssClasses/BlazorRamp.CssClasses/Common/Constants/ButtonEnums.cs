namespace BlazorRamp.CssClasses.Common.Constants;

/// <summary>
/// Sets the size of a <c>br-button</c> element.
/// </summary>
public enum ButtonSize : int
{
    /// <summary>
    /// A smaller button. Still meets the WCAG minimum target size.
    /// </summary>
    Small = 0,
    /// <summary>
    /// The regular, default button size.
    /// </summary>
    Regular = 1,
    /// <summary>
    /// A larger button, one step above regular.
    /// </summary>
    Large = 2,
    /// <summary>
    /// The largest button size available on this scale.
    /// </summary>
    ExtraLarge = 3
}

/// <summary>
/// Sets the solid colour scheme (background, border, and contrasting text colour) of a <c>br-button</c> element.
/// </summary>
public enum ButtonSolidScheme : int
{
    /// <summary>
    /// An inverted solid scheme, using the canvas colour and its inverted counterpart.
    /// </summary>
    Inverted = 0,
    /// <summary>
    /// A neutral solid scheme.
    /// </summary>
    Neutral = 1,
    /// <summary>
    /// A primary-hue solid scheme.
    /// </summary>
    Primary = 2,
    /// <summary>
    /// A secondary-hue solid scheme.
    /// </summary>
    Secondary = 3,

    /// <summary>
    /// An accent-hue solid scheme.
    /// </summary>
    Accent = 4,
    /// <summary>
    /// A danger (error/destructive) solid scheme.
    /// </summary>
    Danger = 5,
    /// <summary>
    /// A warning solid scheme.
    /// </summary>
    Warning = 6,
    /// <summary>
    /// A success solid scheme.
    /// </summary>
    Success = 7,
    /// <summary>
    /// An informational solid scheme.
    /// </summary>
    Info = 8,
    /// <summary>
    /// A light neutral solid scheme.
    /// </summary>
    NeutralLighter = 9,
    /// <summary>
    /// A light secondary-hue solid scheme.
    /// </summary>
    SecondaryLighter = 10,
    /// <summary>
    /// A light accent-hue solid scheme.
    /// </summary>
    AccentLighter = 11,
    /// <summary>
    /// A light primary-hue solid scheme.
    /// </summary>
    PrimaryLighter = 12,
    /// <summary>
    /// A light danger solid scheme.
    /// </summary>
    DangerLighter = 13,
    /// <summary>
    /// A light warning solid scheme.
    /// </summary>
    WarningLighter = 14,
    /// <summary>
    /// A light success solid scheme.
    /// </summary>
    SuccessLighter = 15,
    /// <summary>
    /// A light informational solid scheme.
    /// </summary>
    InfoLighter = 16,
    /// <summary>
    /// A dark neutral solid scheme.
    /// </summary>
    NeutralDarker = 17,
    /// <summary>
    /// A dark secondary-hue solid scheme.
    /// </summary>
    SecondaryDarker = 18,
    /// <summary>
    /// A dark accent-hue solid scheme.
    /// </summary>
    AccentDarker = 19,
    /// <summary>
    /// A dark primary-hue solid scheme.
    /// </summary>
    PrimaryDarker = 20,
    /// <summary>
    /// A dark danger solid scheme.
    /// </summary>
    DangerDarker = 21,
    /// <summary>
    /// A dark warning solid scheme.
    /// </summary>
    WarningDarker = 22,
    /// <summary>
    /// A dark success solid scheme.
    /// </summary>
    SuccessDarker = 23,
    /// <summary>
    /// A dark informational solid scheme.
    /// </summary>
    InfoDarker = 24
}
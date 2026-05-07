using BlazorRamp.Inputs.Components;
using BlazorRamp.Inputs.Components.Summaries;

namespace BlazorRamp.Inputs.Common.Constants;

/// <summary>
/// Specifies the HTML input type for a <see cref="TextInput"/> component.
/// </summary>
public enum TextInputType : int
{
    /// <summary>
    /// Plain text input.
    /// </summary>
    Text = 0,
    /// <summary>
    /// Email address input. Provides email keyboard on mobile devices.
    /// </summary>
    Email = 1,
    /// <summary>
    /// URL input. Provides URL keyboard on mobile devices.
    /// </summary>
    Url = 2,
    /// <summary>
    /// Telephone number input. Provides numeric keyboard on mobile devices.
    /// </summary>
    Tel = 3
}
/// <summary>
/// Specifies the <c>inputmode</c> attribute value for a <see cref="NumericInput{TValue}"/> component,
/// controlling the virtual keyboard displayed on mobile devices.
/// </summary>
public enum NumericInputModeType : int
{
    /// <summary>
    /// Numeric keypad without a decimal point. Used for integer types.
    /// </summary>
    Numeric = 0,
    /// <summary
    /// >Numeric keypad with a decimal point. Used for decimal, double, and float types.
    /// </summary>
    Decimal = 1
}

/// <summary>
/// Controls how validation error messages are associated to the input and whether
/// the error region is keyboard navigable.
/// </summary>
public enum ValidationDisplayMode : int
{
    /// <summary>
    /// Error message ID and hint text ID are both included in <c>aria-describedby</c>.
    /// Error region is not tabbable. This is the default mode.
    /// </summary>
    DescribedByWithHint = 0,

    /// <summary>
    /// Only the error message ID is included in <c>aria-describedby</c> when errors
    /// are present, suppressing the hint text association. Error region is not tabbable.
    /// </summary>
    DescribedByHintSuppressed = 1,

    /// <summary>
    /// The error region is rendered as a tabbable <c>role="region"</c> element with a
    /// <c>tabindex="0"</c>, allowing keyboard users to navigate directly to the errors.
    /// Only the hint text ID is included in <c>aria-describedby</c>.
    /// </summary>
    TabbableWithHint = 2
}
/// <summary>
/// Specifies the <c>autocomplete</c> attribute value for a <see cref="PasswordInput"/> component.
/// </summary>
public enum PasswordAutoComplete : int
{
    /// <summary>
    /// Maps to <c>autocomplete="current-password"</c>. Use on login forms to allow
    /// password managers to fill existing credentials.
    /// </summary>
    CurrentPassword = 0,

    /// <summary>
    /// Maps to <c>autocomplete="new-password"</c>. Use on registration or change
    /// password forms to prevent password managers from filling existing credentials
    /// and to prompt generation of a new password.
    /// </summary>
    NewPassword = 1
}
/// <summary>
/// Sets the alignment of the text/data in the input.
/// </summary>
public enum DataPosition : int{

    /// <summary>
    /// left aligned in LTR, right aligned in RTL
    /// </summary>
    Start= 0,
    /// <summary>
    /// The text/data is centred.
    /// </summary>
    Centre = 1,
    /// <summary>
    /// Right aligned in LTR, left aligned in RTL
    /// </summary>
    End = 2
}


/// <summary>
/// Specifies the semantic title heading level rendered for the <see cref="InputErrorsSummary"/>.
/// </summary>
public enum TitleHeadingLevel : int
{
    /// <summary>
    /// Renders the title text inside an <c>&lt;h2&gt;</c> element.
    /// </summary>
    H2 = 2,
    /// <summary>
    /// Renders the title text inside an <c>&lt;h3&gt;</c> element.
    /// </summary>
    H3 = 3,
    /// <summary>
    /// Renders the title text inside an <c>&lt;h4&gt;</c> element.
    /// </summary>
    H4 = 4,
    /// <summary>
    /// Renders the title text inside an <c>&lt;h5&gt;</c> element.
    /// </summary>
    H5 = 5,
    /// <summary>
    /// Renders the title text inside an <c>&lt;h6&gt;</c> element.
    /// </summary>
    H6 = 6,
}

/// <summary>
/// Specifies when the <see cref="InputErrorsSummary"/> should be displayed.
/// </summary>
public enum SummaryDisplay: int
{
    /// <summary>
    /// The summary should only be displayed after a model validation has occurred.
    /// </summary>
    OnModelValidated = 0,

    /// <summary>
    /// The summary should always be visible when there are validation errors.
    /// </summary>
    Always = 1
}

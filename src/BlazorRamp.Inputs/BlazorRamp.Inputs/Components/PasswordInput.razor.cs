using BlazorRamp.Inputs.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics.CodeAnalysis;

namespace BlazorRamp.Inputs.Components;

/// <summary>
/// Renders an accessible password input with a show/hide password toggle button.
/// The toggle uses <c>aria-pressed</c> to communicate its state to screen readers.
/// Inherits validation state management, hint text, aria-disabled support, and SVG
/// icon support from <see cref="InputTypeBase{TValue}"/>.
/// </summary>
public class PasswordTypeInput : InputTypeBase<string>
{
    /// <summary>
    /// Gets or sets whether the input value is updated on every keystroke via the
    /// <c>oninput</c> event. When <c>false</c> the value updates on <c>onchange</c>
    /// (i.e. when the field loses focus). Defaults to <c>false</c>.
    /// </summary>
    [Parameter] public bool                 UpdateOnInput       { get; set; } = false;

    /// <summary>
    /// Gets or sets the visible label for the show password toggle button.
    /// When null, empty, or whitespace defaults to <c>"Show Password"</c>.
    /// </summary>
    [Parameter] public string               ShowPasswordText    { get; set; } = default!;

    /// <summary>
    /// Gets or sets the autocomplete hint applied to the password field. Maps to the
    /// HTML <c>autocomplete</c> attribute. Use <see cref="PasswordAutoComplete.CurrentPassword"/>
    /// for login forms and <see cref="PasswordAutoComplete.NewPassword"/> for registration
    /// or change password forms. Defaults to <see cref="PasswordAutoComplete.CurrentPassword"/>.
    /// </summary>
    [Parameter] public PasswordAutoComplete PasswordAutoComplete { get; set; } = PasswordAutoComplete.CurrentPassword;

    /// <summary>
    /// Gets or sets the text/data alignment in the input. Defaults to <see cref="DataPosition.Start"/>.
    /// </summary>
    [Parameter] public DataPosition DataPosition { get; set; } = DataPosition.Start;


    /// <summary>
    /// Gets the resolved CSS class string applied to the root element of the password input,
    /// including any additional classes passed via <see cref="InputBase{TValue}.AdditionalAttributes"/>.
    /// </summary>
    protected string PasswordInputClasses { get; private set; } = String.Empty;

    /// <summary>
    /// Gets the current HTML input type, either <c>"password"</c> or <c>"text"</c>,
    /// toggled by the show password button.
    /// </summary>
    protected string InputType            { get; private set; } = "password";

    /// <summary>
    /// Gets the resolved show password button label, derived from <see cref="ShowPasswordText"/>
    /// or the default value when not set.
    /// </summary>
    protected string ShowText             { get; private set; } = GlobalValues.Password_Input_Show_Password_Text;

    /// <summary>
    /// Gets the resolved <c>autocomplete</c> attribute value, either <c>"current-password"</c>
    /// or <c>"new-password"</c>, derived from <see cref="PasswordAutoComplete"/>.
    /// </summary>
    protected string AutoComplete { get; private set; } = "current-password";


    /// <summary>
    /// Updates the password input CSS classes and resolves the <c>autocomplete</c>
    /// attribute value from <see cref="PasswordAutoComplete"/>.
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        PasswordInputClasses = GetInputClasses(base.AdditionalAttributes);
        AutoComplete = PasswordAutoComplete == PasswordAutoComplete.CurrentPassword ? "current-password" : "new-password";
    }

    /// <summary>
    /// Resolves the show password button label from <see cref="ShowPasswordText"/>.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        ShowText = String.IsNullOrWhiteSpace(ShowPasswordText) ? GlobalValues.Password_Input_Show_Password_Text : ShowPasswordText.Trim();

    }


    /// <summary>
    /// Always returns <c>true</c> passing the raw string value through unchanged.
    /// Validation is handled entirely by the consuming application's validator.
    /// </summary>
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out string result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        validationErrorMessage = String.Empty;

        result = value!;

        return true;
    }

    /// <summary>
    /// Toggles the input type between <c>"password"</c> and <c>"text"</c> to show
    /// or hide the password value.
    /// </summary>
    protected void ToggleShowPassword()

        => InputType = InputType == "text" ? "password" : "text";

    /// <summary>
    /// Handles the binding set event, updating <see cref="InputBase{TValue}.CurrentValueAsString"/>
    /// with the raw input value. Does nothing when <see cref="InputTypeBase{TValue}.IsDisabled"/>
    /// is <c>true</c>.
    /// </summary>
    protected void HandlePropertySet(string? value)
    {
        if (base.IsDisabled) return;

        CurrentValueAsString = value;
    }

    /// <summary>
    /// Builds the CSS class string for the root element by combining the base password input
    /// class with any additional class passed via <see cref="InputBase{TValue}.AdditionalAttributes"/>.
    /// </summary>
    private static string GetInputClasses(IReadOnlyDictionary<string, object>? additionalAttributes)
    {
        var classData = additionalAttributes?.TryGetValue("class", out var extraClass) == true ? extraClass.ToString() : "";

        if (false == String.IsNullOrWhiteSpace(classData))
        {
            return $"{@GlobalValues.Password_Input_Class} {classData}";
        }

        return @GlobalValues.Password_Input_Class;
    }

    /// <summary>
    /// Returns a filtered copy of <see cref="InputBase{TValue}.AdditionalAttributes"/> with
    /// the <c>class</c> key removed, so additional attributes can be applied to the input
    /// element without duplicating the class handling.
    /// </summary>
    protected static IReadOnlyDictionary<string, object>? GetAttributes(IReadOnlyDictionary<string, object>? additionalAttributes)

        => additionalAttributes?.Where(kv => kv.Key != "class").ToDictionary();

}
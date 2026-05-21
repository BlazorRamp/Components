using BlazorRamp.Inputs.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Primitives;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace BlazorRamp.Inputs.Components;

/// <summary>
/// Renders an accessible text input supporting <c>text</c>, <c>email</c>, <c>url</c>,
/// and <c>tel</c> input types. Inherits validation state management, hint text,
/// aria-disabled support, and SVG icon support from <see cref="InputTypeBase{TValue}"/>.
/// </summary>
public class TextTypeInput : InputTypeBase<string>
{
    /// <summary>
    /// Gets or sets whether the input value is updated on every keystroke via the
    /// <c>oninput</c> event. When <c>false</c> the value updates on <c>onchange</c>
    /// (i.e. when the field loses focus). Defaults to <c>false</c>.
    /// </summary>
    [Parameter] public bool          UpdateOnInput  { get; set; } = false;

    /// <summary>
    /// Gets or sets the HTML input type. Supports <see cref="TextInputType.Text"/>,
    /// <see cref="TextInputType.Email"/>, <see cref="TextInputType.Url"/>, and
    /// <see cref="TextInputType.Tel"/>. Defaults to <see cref="TextInputType.Text"/>.
    /// </summary>
    [Parameter] public TextInputType TextInputType  { get; set; } = TextInputType.Text;


    /// <summary>
    /// Gets or sets the text/data alignment in the input. Defaults to <see cref="DataPosition.Start"/>.
    /// </summary>
    [Parameter] public DataPosition DataPosition { get; set; } = DataPosition.Start;

    /// <summary>
    /// Gets or sets whether the value should be trimmed during the on blur event. Defauts to <c>false</c>.
    /// </summary>
    [Parameter] public bool TrimOnBlur { get; set; } = false;

    /// <summary>
    /// Gets the resolved CSS class string applied to the root element of the text input,
    /// including any additional classes passed via <see cref="InputBase{TValue}.AdditionalAttributes"/>.
    /// </summary>
    protected string TextInputClasses { get; private set; } = String.Empty;

    /// <summary>
    /// Gets the current HTML input type string, resolved from <see cref="TextInputType"/>
    /// on initialisation. For example <c>"text"</c>, <c>"email"</c>, <c>"url"</c>, or <c>"tel"</c>.
    /// </summary>
    protected string InputType    { get; private set; } = "text";

    /// <summary>
    /// Updates the text input CSS classes on each parameter change.
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        TextInputClasses = GetInputClasses(base.AdditionalAttributes);
        
    }

    /// <summary>
    /// Resolves the HTML input type string from <see cref="TextInputType"/>.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        InputType = Enum.GetName<TextInputType>(TextInputType)?.ToLower() ?? "text";
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
    /// Handles the binding set event, updating <see cref="InputBase{TValue}.CurrentValueAsString"/>
    /// with the raw input value. Does nothing when <see cref="InputTypeBase{TValue}.IsDisabled"/>
    /// is <c>true</c>.
    /// </summary>
    protected void HandlePropertySet(string? value)
    {
        if (base.IsDisabled)  return;
         
        CurrentValueAsString = value;
    }


    /// <summary>
    /// Handles the input blur event and trims the whitespace from the <see cref="InputBase{TValue}.CurrentValue"/> 
    /// if the option has been set for this <see cref="TrimOnBlur" />
    /// </summary>
    protected async Task HandleOnBlur()
    {
        if (CurrentValueAsString is not null && true == TrimOnBlur) CurrentValueAsString = CurrentValueAsString.Trim();
    }

    /// <summary>
    /// Builds the CSS class string for the root element by combining the base text input
    /// class with any additional class passed via <see cref="InputBase{TValue}.AdditionalAttributes"/>.
    /// </summary>
    protected static string GetInputClasses(IReadOnlyDictionary<string, object>? additionalAttributes)
    {
        var classData = additionalAttributes?.TryGetValue("class", out var extraClass) == true ? extraClass.ToString() : "";

        if (false == String.IsNullOrWhiteSpace(classData))
        {
            return $"{@GlobalValues.Text_Input_Class} {classData}";
        }

        return @GlobalValues.Text_Input_Class;
    }

    /// <summary>
    /// Returns a filtered copy of <see cref="InputBase{TValue}.AdditionalAttributes"/> with
    /// the <c>class</c> key removed, so additional attributes can be applied to the input
    /// element without duplicating the class handling.
    /// </summary>
    protected static IReadOnlyDictionary<string, object>? GetAttributes(IReadOnlyDictionary<string, object>? additionalAttributes)

        => additionalAttributes?.Where(kv => kv.Key != "class").ToDictionary();


}

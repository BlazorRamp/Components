using BlazorRamp.Inputs.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics.CodeAnalysis;

namespace BlazorRamp.Inputs.Components.RadioGroup;

public class RadioTypeInputGroup<TValue> : InputTypeBase<TValue>
{

    /// <summary>
    /// Gets or sets the child content, options for the select element
    /// </summary>
    [Parameter] public RenderFragment? OptionValues { get; set; } = default;

    /// <summary>
    /// Gets the resolved CSS class string applied to the root element of the radio group input,
    /// including any additional classes passed via <see cref="InputBase{TValue}.AdditionalAttributes"/>.
    /// </summary>
    protected string RadioInputGroupClasses { get; private set; } = String.Empty;

    /// <summary>
    /// Updates the checkbox input CSS classes on each parameter change.
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        RadioInputGroupClasses = GetInputClasses(base.AdditionalAttributes);

    }



    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        validationErrorMessage = String.Empty;
        result = default;
        return true;
    }

    /// <summary>
    /// Builds the CSS class string for the root element by combining the base input
    /// class with any additional class passed via <see cref="InputBase{TValue}.AdditionalAttributes"/>.
    /// </summary>
    protected static string GetInputClasses(IReadOnlyDictionary<string, object>? additionalAttributes)
    {
        var classData = additionalAttributes?.TryGetValue("class", out var extraClass) == true ? extraClass.ToString() : "";

        if (false == String.IsNullOrWhiteSpace(classData))
        {
            return $"{@GlobalValues.Radio_Input_Group_Class} {classData}";
        }

        return @GlobalValues.Radio_Input_Group_Class;
    }

    /// <summary>
    /// Returns a filtered copy of <see cref="InputBase{TValue}.AdditionalAttributes"/> with
    /// the <c>class</c> key removed, so additional attributes can be applied to the input
    /// element without duplicating the class handling.
    /// </summary>
    protected static IReadOnlyDictionary<string, object>? GetAttributes(IReadOnlyDictionary<string, object>? additionalAttributes)

        => additionalAttributes?.Where(kv => kv.Key != "class").ToDictionary();

}

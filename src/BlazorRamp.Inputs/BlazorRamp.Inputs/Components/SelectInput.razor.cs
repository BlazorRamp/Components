using BlazorRamp.Inputs.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Primitives;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.Inputs.Components;

public class SelectTypeInput<TValue> : InputTypeBase<TValue>
{

    /// <summary>
    /// Gets or sets the child content, options for the select element
    /// </summary>
    [Parameter] public RenderFragment? OptionValues { get; set; } = default;

    /// <summary>
    /// Gets or sets the text/data alignment in the input. Defaults to <see cref="DataPosition.Start"/>.
    /// </summary>
    [Parameter] public DataPosition DataPosition { get; set; } = DataPosition.Start;

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private IJSObjectReference? _jSModule = null;

    /// <summary>
    /// Gets the resolved CSS class string applied to the root element of the numeric input,
    /// including any additional classes passed via <see cref="InputBase{TValue}.AdditionalAttributes"/>.
    /// </summary>
    protected string SelectInputClasses { get; private set; } = String.Empty;



    /// <summary>
    /// Updates the select input CSS classes on each parameter change.
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        SelectInputClasses = GetInputClasses(base.AdditionalAttributes);

    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        validationErrorMessage = string.Empty;

        switch (base.DataType)
        {
            case Type type when type == typeof(byte)  && byte.TryParse(value, NumberStyles.Any, null, out byte byteValue):    result = (TValue)(object)byteValue; return true;
            case Type type when type == typeof(short) && short.TryParse(value, NumberStyles.Any, null, out short shortValue): result = (TValue)(object)shortValue; return true;
            case Type type when type == typeof(int)   && int.TryParse(value, NumberStyles.Any, null, out int intValue):       result = (TValue)(object)intValue; return true;
            case Type type when type == typeof(long)  && long.TryParse(value, NumberStyles.Any, null, out long longValue):    result = (TValue)(object)longValue; return true;
            case Type type when type == typeof(Guid)  && Guid.TryParse(value, null, out Guid guidValue):                      result = (TValue)(object)guidValue; return true;
            case Type type when type == typeof(string):                                                                       result = (TValue)(object)value!; return true;
        }
        
        //should never get here with a select input in normal use
        result = default;
        validationErrorMessage = String.Concat(base.LabelNameText.TrimEnd(':').Trim(), " - ", GlobalValues.Input_Parse_General_Error_Message);
        return false;
    }


    protected void HandlePropertySet(string? value)
    {
        if (IsDisabled) return;

        if (string.IsNullOrWhiteSpace(value) && base.IsNullableType)
        {
            CurrentValue = default;
            return;
        }
        CurrentValueAsString = value;
    }

    /// <summary>
    /// Builds the CSS class string for the root element by combining the base select input
    /// class with any additional class passed via <see cref="InputBase{TValue}.AdditionalAttributes"/>.
    /// </summary>
    protected string GetInputClasses(IReadOnlyDictionary<string, object>? additionalAttributes)
    {
        var classData = additionalAttributes?.TryGetValue("class", out var extraClass) == true ? extraClass.ToString() : "";

        if (false == String.IsNullOrWhiteSpace(classData))
        {
            return $"{@GlobalValues.Select_Input_Class} {classData}";
        }

        return @GlobalValues.Select_Input_Class;
    }

    /// <summary>
    /// Returns a filtered copy of <see cref="InputBase{TValue}.AdditionalAttributes"/> with
    /// the <c>class</c> key removed, so additional attributes can be applied to the input
    /// element without duplicating the class handling.
    /// </summary>
    protected IReadOnlyDictionary<string, object>? GetAttributes(IReadOnlyDictionary<string, object>? additionalAttributes)

        => additionalAttributes?.Where(kv => kv.Key != "class").ToDictionary();
}

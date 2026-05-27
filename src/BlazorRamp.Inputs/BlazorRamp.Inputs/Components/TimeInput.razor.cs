using BlazorRamp.Inputs.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.Inputs.Components;

public class TimeTypeInput<TValue> : InputTypeBase<TValue>
{


    /// <summary>
    /// Gets or sets the text/data alignment in the input. Defaults to <see cref="DataPosition.Start"/>.
    /// </summary>
    [Parameter] public DataPosition DataPosition { get; set; } = DataPosition.End;
    /// <summary>
    /// Gets or sets the error message displayed when the input value cannot be parsed
    /// to TimeOnly. The message is prefixed with the field's
    /// display name (label text). When null, empty, or whitespace defaults to <c>"Invalid time."</c>.
    /// </summary>
    [Parameter] public string ParseErrorMessage { get; set; } = GlobalValues.Input_Parse_time_Error_Message;


    /// <summary>
    /// Gets the resolved CSS class string applied to the root element of the time input,
    /// including any additional classes passed via <see cref="InputBase{TValue}.AdditionalAttributes"/>.
    /// </summary>
    protected string TimeInputClasses { get; private set; } = String.Empty;

    /// <summary>
    /// Tracks the raw string value currently displayed in the input element, independent
    /// of the parsed <see cref="InputBase{TValue}.CurrentValue"/>. Used to preserve
    /// mid-entry display state such as trailing decimal points and formatted values.
    /// </summary>
    protected string? _stringValue = null;
    private TValue? _lastParsedValue = default;
    /// <summary>
    /// Updates the text input CSS classes on each parameter change.
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        TimeInputClasses = GetInputClasses(base.AdditionalAttributes);

        if (!Equals(CurrentValue, _lastParsedValue))
        {
            _lastParsedValue = CurrentValue;
            _stringValue = CurrentValue is TimeOnly t ? t.ToString("HH:mm") : null;
        }

    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        validationErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(value) && base.IsNullableType)
        {
            result = default!;
            return true;
        }

        if (TimeOnly.TryParseExact(value, "HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out TimeOnly parsed))
        {
            result = (TValue)(object)parsed;
            return true;
        }

        result = default!;
        validationErrorMessage = string.Concat(base.LabelNameText.TrimEnd(':').Trim(), " - ", ParseErrorMessage);
        return false;
    }
    protected void HandleOnChange(ChangeEventArgs e)
    {
        if (IsDisabled) return;
        var value = e.Value?.ToString();
        CurrentValueAsString = _stringValue = value;
        _lastParsedValue = CurrentValue;
    }

    /// <summary>
    /// Builds the CSS class string for the root element by combining the base time input
    /// class with any additional class passed via <see cref="InputBase{TValue}.AdditionalAttributes"/>.
    /// </summary>
    private static string GetInputClasses(IReadOnlyDictionary<string, object>? additionalAttributes)
    {
        var classData = additionalAttributes?.TryGetValue("class", out var extraClass) == true ? extraClass.ToString() : "";

        if (false == String.IsNullOrWhiteSpace(classData))
        {
            return $"{@GlobalValues.Time_Input_Class} {classData}";
        }

        return @GlobalValues.Time_Input_Class;
    }

    /// <summary>
    /// Returns a filtered copy of <see cref="InputBase{TValue}.AdditionalAttributes"/> with
    /// the <c>class</c> key removed, so additional attributes can be applied to the input
    /// element without duplicating the class handling.
    /// </summary>
    protected static IReadOnlyDictionary<string, object>? GetAttributes(IReadOnlyDictionary<string, object>? additionalAttributes)

        => additionalAttributes?.Where(kv => kv.Key != "class").ToDictionary();


}

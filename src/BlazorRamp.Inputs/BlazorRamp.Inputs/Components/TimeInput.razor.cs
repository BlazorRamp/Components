using BlazorRamp.Inputs.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
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

public class TimeTypeInput<TValue> : InputTypeBase<TValue>, IAsyncDisposable
{


    [Parameter] public string HoursLabelText   { get; set; } = GlobalValues.Time_Input_Hours_Text;
    [Parameter] public string MinutesLabelText { get; set; } = GlobalValues.Time_Input_Minutes_Text;
    [Parameter] public string SecondsLabelText { get; set; } = GlobalValues.Time_Input_Seconds_Text;
    [Parameter] public bool   EnableSeconds    { get; set; } = false;


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


    protected string ControlLabelledbyID { get; private set; } = Guid.NewGuid().ToString();
    protected string HoursInputID        { get; private set; } = Guid.NewGuid().ToString();
    protected string MinutesInputID      { get; private set; } = Guid.NewGuid().ToString();
    protected string SecondsInputID      { get; private set; } = Guid.NewGuid().ToString();
    protected string HoursText           { get; private set; } = GlobalValues.Time_Input_Hours_Text;
    protected string MinutesText         { get; private set; } = GlobalValues.Time_Input_Minutes_Text;
    protected string SecondsText         { get; private set; } = GlobalValues.Time_Input_Seconds_Text;


    protected string? HoursValue   { get; set; }
    protected string? MinutesValue { get; set; }
    protected string? SecondsValue { get; set; }

    protected ElementReference? HoursReference   { get; set; }
    protected ElementReference? MinutesReference { get; set; }
    protected ElementReference? SecondsReference { get; set; }

    /// <summary>
    /// Tracks the raw string value currently displayed in the input element, independent
    /// of the parsed <see cref="InputBase{TValue}.CurrentValue"/>. Used to preserve
    /// mid-entry display state such as trailing decimal points and formatted values.
    /// </summary>
    protected string? _stringValue = null;

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;


    private DotNetObjectReference<TimeTypeInput<TValue>>? _dotNetObjectRef;

    private IJSObjectReference? _jSModule = null;

    private TValue? _lastParsedValue = default;
    /// <summary>
    /// Updates the text input CSS classes on each parameter change.
    /// </summary>
    /// 


    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        TimeInputClasses = GetInputClasses(base.AdditionalAttributes);

        if (!Equals(CurrentValue, _lastParsedValue))
        {
            _lastParsedValue = CurrentValue;

            if (CurrentValue is TimeOnly timeOnly)
            {
                HoursValue   = timeOnly.Hour.ToString("D2");
                MinutesValue = timeOnly.Minute.ToString("D2");
                SecondsValue = EnableSeconds ? timeOnly.Second.ToString("D2") : string.Empty;
            }
            else
            {
                HoursValue = string.Empty;
                MinutesValue = string.Empty;
                SecondsValue = string.Empty;
            }
        }

    }
    protected override void OnInitialized()
    {
        base.OnInitialized();
        ParseErrorMessage = string.IsNullOrWhiteSpace(ParseErrorMessage) ? GlobalValues.Input_Parse_time_Error_Message : ParseErrorMessage.Trim();

        HoursText   = String.IsNullOrWhiteSpace(HoursLabelText)   ? GlobalValues.Time_Input_Hours_Text   : HoursLabelText.Trim();
        MinutesText = String.IsNullOrWhiteSpace(MinutesLabelText) ? GlobalValues.Time_Input_Minutes_Text : MinutesLabelText.Trim();
        SecondsText = String.IsNullOrWhiteSpace(SecondsLabelText) ? GlobalValues.Time_Input_Seconds_Text : SecondsLabelText.Trim();


        _stringValue = CurrentValue is TimeOnly t ? t.ToString(EnableSeconds ? "HH:mm:ss" : "HH:mm") : null;

        _lastParsedValue = CurrentValue;
    }

    /// <summary>
    /// Loads the inputs JavaScript module on first render and registers the time
    /// input handlers for character stripping.
    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender)
        {
            _jSModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_Inputs_File_Path);

            if (_jSModule is not null)
            {
                _dotNetObjectRef = DotNetObjectReference.Create(this);
                await _jSModule.InvokeVoidAsync(GlobalValues.JS_Inputs_Register_Time_Segment_Handlers, HoursReference, MinutesReference, SecondsReference);
                await _jSModule.InvokeVoidAsync(GlobalValues.JS_Inputs_Register_Focus_Out_Callback,ControlReference, _dotNetObjectRef, nameof(this.HandleComponentFocusOut));
            }
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


    protected void HandleHoursSet(string? value)
    {
        if (IsDisabled) return;
        HoursValue = value ?? string.Empty;
    }

    protected void HandleMinutesSet(string? value)
    {
        if (IsDisabled) return;
        MinutesValue = value ?? string.Empty;
    }

    protected void HandleSecondsSet(string? value)
    {
        if (IsDisabled) return;
        SecondsValue = value ?? string.Empty;
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


    [JSInvokable]
    public async Task HandleComponentFocusOut()
    {
        if (IsDisabled) return;

        // if all empty, set to default
        if (string.IsNullOrWhiteSpace(HoursValue) &&
            string.IsNullOrWhiteSpace(MinutesValue) &&
            string.IsNullOrWhiteSpace(SecondsValue))
        {
            CurrentValue = default;
            _lastParsedValue = default;
            await InvokeAsync(StateHasChanged);
            return;
        }

        var hours = int.TryParse(HoursValue, out int h) ? h : 0;
        var minutes = int.TryParse(MinutesValue, out int m) ? m : 0;
        var seconds = EnableSeconds && int.TryParse(SecondsValue, out int s) ? s : 0;

        // pad segments
        HoursValue = hours.ToString("D2");
        MinutesValue = minutes.ToString("D2");
        if (EnableSeconds) SecondsValue = seconds.ToString("D2");

        CurrentValueAsString = EnableSeconds
            ? $"{HoursValue}:{MinutesValue}:{SecondsValue}"
            : $"{HoursValue}:{MinutesValue}";

        _lastParsedValue = CurrentValue;

        await InvokeAsync(StateHasChanged);
    }


    /// <summary>
    /// Unregisters the time input JavaScript handlers, disposes the JavaScript module reference,
    /// and calls <see cref="InputTypeBase{TValue}.DisposeAsync"/> to release base resources.
    /// </summary>
    public override async ValueTask DisposeAsync()
    {
        if (_jSModule is not null)
        {
            try
            {
                await _jSModule.InvokeVoidAsync(GlobalValues.JS_Inputs_Unregister_Time_Segment_Handlers, HoursReference, MinutesReference, SecondsReference);
                await _jSModule.InvokeVoidAsync(GlobalValues.JS_Inputs_Unregister_Focus_Out_Callback, _dotNetObjectRef);
                await _jSModule.DisposeAsync();
            }
            catch { }
        }
        await base.DisposeAsync();
    }
}

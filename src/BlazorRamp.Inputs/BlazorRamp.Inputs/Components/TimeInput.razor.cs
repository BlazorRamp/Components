using BlazorRamp.Inputs.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace BlazorRamp.Inputs.Components;
/// <summary>
/// Renders an accessible time input composed of separate hours, minutes, and optional
/// seconds segment inputs. Supports <see cref="TimeOnly"/> and nullable <see cref="TimeOnly"/>.
/// The composite value is committed and parsed when focus leaves the entire group via
/// a JavaScript focusout callback. Inherits validation state management, hint text,
/// aria-disabled support, and SVG icon support from <see cref="InputTypeBase{TValue}"/>.
/// For the best screen reader experience, <see cref="InputTypeBase{TValue}.ValidationDisplayMode"/>
/// should be set to <see cref="ValidationDisplayMode.TabbableWithHint"/> with
/// <see cref="UpdateOnInput"/> set to <c>true</c>.
/// </summary>

public class TimeTypeInput<TValue> : InputTypeBase<TValue>, IAsyncDisposable
{

    /// <summary>
    /// Gets or sets the visible label text for the hours segment input.
    /// When null, empty, or whitespace defaults to <c>"Hours"</c>.
    /// </summary>
    [Parameter] public string HoursLabelText   { get; set; } = GlobalValues.Time_Input_Hours_Text;

    /// <summary>
    /// Gets or sets the visible label text for the minutes segment input.
    /// When null, empty, or whitespace defaults to <c>"Minutes"</c>.
    /// </summary>
    [Parameter] public string MinutesLabelText { get; set; } = GlobalValues.Time_Input_Minutes_Text;

    /// <summary>
    /// Gets or sets the visible label text for the seconds segment input.
    /// When null, empty, or whitespace defaults to <c>"Seconds"</c>.
    /// </summary>
    [Parameter] public string SecondsLabelText { get; set; } = GlobalValues.Time_Input_Seconds_Text;

    /// <summary>
    /// Gets or sets whether the seconds segment is rendered and included in the time value.
    /// When <c>false</c> only hours and minutes are shown and seconds default to <c>00</c>
    /// in the parsed value. Defaults to <c>false</c>.
    /// </summary>
    [Parameter] public bool   EnableSeconds    { get; set; } = false;


    /// <summary>
    /// Gets or sets whether the autocomplete attribute is added with the value of off.
    /// </summary>
    [Parameter] public bool AutoCompleteOff    { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the input value is updated on every keystroke via the
    /// <c>oninput</c> event. When <c>false</c> the value updates on focusout from
    /// the entire group. Defaults to <c>false</c>.
    /// </summary>
    [Parameter] public bool UpdateOnInput { get; set; } = false;

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
    /// Gets the unique <c>id</c> of the element used as the accessible label for the
    /// group via <c>aria-labelledby</c>.
    /// </summary>
    protected string ControlLabelledbyID { get; private set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets the unique <c>id</c> attribute applied to the hours segment input element.
    /// </summary>
    protected string HoursInputID        { get; private set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets the unique <c>id</c> attribute applied to the minutes segment input element.
    /// </summary>
    protected string MinutesInputID      { get; private set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets the unique <c>id</c> attribute applied to the seconds segment input element.
    /// </summary>
    protected string SecondsInputID      { get; private set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets the resolved label text for the hours segment, derived from
    /// <see cref="HoursLabelText"/> or the default value when not set.
    /// </summary>
    protected string HoursText           { get; private set; } = GlobalValues.Time_Input_Hours_Text;

    /// <summary>
    /// Gets the resolved label text for the minutes segment, derived from
    /// <see cref="MinutesLabelText"/> or the default value when not set.
    /// </summary>
    protected string MinutesText         { get; private set; } = GlobalValues.Time_Input_Minutes_Text;

    /// <summary>
    /// Gets the resolved label text for the seconds segment, derived from
    /// <see cref="SecondsLabelText"/> or the default value when not set.
    /// </summary>
    protected string SecondsText         { get; private set; } = GlobalValues.Time_Input_Seconds_Text;

    /// <summary>
    /// Gets or sets the raw string value currently displayed in the hours segment input.
    /// Managed independently of <see cref="InputBase{TValue}.CurrentValue"/> to preserve
    /// mid-entry display state and leading zero removal on focus.
    /// </summary>
    protected string? HoursValue   { get; set; }

    /// <summary>
    /// Gets or sets the raw string value currently displayed in the minutes segment input.
    /// Managed independently of <see cref="InputBase{TValue}.CurrentValue"/> to preserve
    /// mid-entry display state and leading zero removal on focus.
    /// </summary>
    protected string? MinutesValue { get; set; }

    /// <summary>
    /// Gets or sets the raw string value currently displayed in the seconds segment input.
    /// Only relevant when <see cref="EnableSeconds"/> is <c>true</c>.
    /// Managed independently of <see cref="InputBase{TValue}.CurrentValue"/> to preserve
    /// mid-entry display state and leading zero removal on focus.
    /// </summary>
    protected string? SecondsValue { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="ElementReference"/> for the hours segment input element,
    /// available after the component has rendered. Used for JavaScript interop to register
    /// input character-stripping handlers.
    /// </summary>
    protected ElementReference? HoursReference   { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="ElementReference"/> for the minutes segment input element,
    /// available after the component has rendered. Used for JavaScript interop to register
    /// input character-stripping handlers.
    /// </summary>
    protected ElementReference? MinutesReference { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="ElementReference"/> for the seconds segment input element,
    /// available after the component has rendered. Only populated when <see cref="EnableSeconds"/>
    /// is <c>true</c>. Used for JavaScript interop to register input character-stripping handlers.
    /// </summary>
    protected ElementReference? SecondsReference { get; set; }


    private TValue? _lastParsedValue = default;

     [Inject] private IJSRuntime JSRuntime { get; set; } = default!;


    private DotNetObjectReference<TimeTypeInput<TValue>>? _dotNetObjectRef;

    private IJSObjectReference? _jSModule = null;


    /// <summary>
    /// Updates the time input CSS classes and synchronises the segment display values
    /// when <see cref="InputBase{TValue}.CurrentValue"/> changes externally.
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        TimeInputClasses = GetInputClasses(base.AdditionalAttributes);

        if (!Equals(CurrentValue, _lastParsedValue))
        {
            _lastParsedValue = CurrentValue;

            SetDisplayValues();
        }

    }

    /// <summary>
    /// Validates that <typeparamref name="TValue"/> is <see cref="TimeOnly"/> or nullable
    /// <see cref="TimeOnly"/>, resolves segment label texts and the parse error message
    /// from parameters, and initialises the segment display values from
    /// <see cref="InputBase{TValue}.CurrentValue"/>.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        ParseErrorMessage = string.IsNullOrWhiteSpace(ParseErrorMessage) ? GlobalValues.Input_Parse_time_Error_Message : ParseErrorMessage.Trim();

        HoursText   = String.IsNullOrWhiteSpace(HoursLabelText)   ? GlobalValues.Time_Input_Hours_Text   : HoursLabelText.Trim();
        MinutesText = String.IsNullOrWhiteSpace(MinutesLabelText) ? GlobalValues.Time_Input_Minutes_Text : MinutesLabelText.Trim();
        SecondsText = String.IsNullOrWhiteSpace(SecondsLabelText) ? GlobalValues.Time_Input_Seconds_Text : SecondsLabelText.Trim();

        if (base.DataType != typeof(TimeOnly)) throw new ArgumentException(GlobalValues.Input_Time_DataType_Error_Message);

        _lastParsedValue = CurrentValue;

        SetDisplayValues();
    }

    /// <summary>
    /// Loads the JavaScript module on first render and registers the time segment
    /// character-stripping handlers and the focusout callback used to commit and
    /// parse the composite time value when focus leaves the group.
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

    /// <summary>
    /// Attempts to parse <paramref name="value"/> to <typeparamref name="TValue"/> using
    /// exact format <c>"H:m:s"</c> with <see cref="CultureInfo.InvariantCulture"/>.
    /// Returns <c>true</c> and sets <paramref name="result"/> when the value is null or
    /// whitespace and the type is nullable, or when the value parses successfully.
    /// On failure sets <paramref name="validationErrorMessage"/> to the resolved parse
    /// error message prefixed with the field display name.
    /// </summary>
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        validationErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(value) && base.IsNullableType)
        {
            result = default!;
            return true;
        }
        
        if (TimeOnly.TryParseExact(value, "H:m:s", CultureInfo.InvariantCulture, DateTimeStyles.None, out TimeOnly parsed))
        {
            result = (TValue)(object)parsed;
            return true;
        }

        result = default!;
        validationErrorMessage = string.Concat(base.LabelNameText.TrimEnd(':').Trim(), " - ", ParseErrorMessage);
        return false;
    }

    private void SetDisplayValues()
    {

        if (CurrentValue is TimeOnly timeOnly)
        {
            HoursValue = timeOnly.Hour.ToString("D2");
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

    /// <summary>
    /// Handles the binding set event for the hours segment, updating <see cref="HoursValue"/>
    /// with the raw input value. When <see cref="UpdateOnInput"/> is <c>true</c> the
    /// composite <see cref="InputBase{TValue}.CurrentValueAsString"/> is also updated
    /// immediately. Does nothing when <see cref="InputTypeBase{TValue}.IsDisabled"/> is <c>true</c>.
    /// </summary>
    protected void HandleHoursSet(string? value)
    {
        if (IsDisabled) return;
        HoursValue = value ?? string.Empty;

        if (UpdateOnInput) CurrentValueAsString = EnableSeconds ? $"{HoursValue}:{MinutesValue}:{SecondsValue}" : $"{HoursValue}:{MinutesValue}:00";
    }

    /// <summary>
    /// Handles the binding set event for the minutes segment, updating <see cref="MinutesValue"/>
    /// with the raw input value. When <see cref="UpdateOnInput"/> is <c>true</c> the
    /// composite <see cref="InputBase{TValue}.CurrentValueAsString"/> is also updated
    /// immediately. Does nothing when <see cref="InputTypeBase{TValue}.IsDisabled"/> is <c>true</c>.
    /// </summary>
    protected void HandleMinutesSet(string? value)
    {
        if (IsDisabled) return;
        MinutesValue = value ?? string.Empty;

        if (UpdateOnInput) CurrentValueAsString = EnableSeconds ? $"{HoursValue}:{MinutesValue}:{SecondsValue}" : $"{HoursValue}:{MinutesValue}:00";
    }

    /// <summary>
    /// Handles the binding set event for the seconds segment, updating <see cref="SecondsValue"/>
    /// with the raw input value. When <see cref="UpdateOnInput"/> is <c>true</c> the
    /// composite <see cref="InputBase{TValue}.CurrentValueAsString"/> is also updated
    /// immediately. Does nothing when <see cref="InputTypeBase{TValue}.IsDisabled"/> is <c>true</c>.
    /// Only relevant when <see cref="EnableSeconds"/> is <c>true</c>.
    /// </summary>
    protected void HandleSecondsSet(string? value)
    {
        if (IsDisabled) return;
        SecondsValue = value ?? string.Empty;

        if (UpdateOnInput) CurrentValueAsString = EnableSeconds ? $"{HoursValue}:{MinutesValue}:{SecondsValue}" : $"{HoursValue}:{MinutesValue}:00";
    }

    /// <summary>
    /// Removes the leading zero from a segment value when the input receives focus,
    /// allowing free editing without the leading zero interfering with entry.
    /// Returns the value unchanged when the component is readonly, aria-disabled,
    /// null, empty, or whitespace, or when the value cannot be parsed as an integer.
    /// </summary>
    internal string? RemoveLeadingZero(string? value)
    {
        if (base.ReadOnly || base.IsDisabled || string.IsNullOrWhiteSpace(value)) return value;

        return int.TryParse(value, out int parsed) ? parsed.ToString() : value;
    }

    /// <summary>
    /// Left pads the value with zeros so its always 2 digits.
    /// Returns the value unchanged when the component is readonly, aria-disabled,
    /// null, empty, or whitespace.
    /// </summary>
    internal string? PadMissingWithZero(string? value)
    {
        if (base.ReadOnly || base.IsDisabled || string.IsNullOrWhiteSpace(value)) return value;
        return value.PadLeft(2, '0');
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

    /// <summary>
    /// Invoked via JavaScript interop when focus leaves the entire time input group.
    /// Commits the current segment values to <see cref="InputBase{TValue}.CurrentValueAsString"/>,
    /// padding each segment with a leading zero where needed. When all segments are
    /// empty and the type is nullable, sets <see cref="InputBase{TValue}.CurrentValue"/>
    /// to <c>default</c>. When all segments are empty and the type is non-nullable,
    /// defaults to midnight (<c>00:00:00</c>). Does nothing when
    /// <see cref="InputTypeBase{TValue}.IsDisabled"/> is <c>true</c>.
    /// </summary>
    [JSInvokable]
    public async Task HandleComponentFocusOut()
    {
        if (IsDisabled) return;

        if (string.IsNullOrWhiteSpace(HoursValue) &&
            string.IsNullOrWhiteSpace(MinutesValue) &&
            (string.IsNullOrWhiteSpace(SecondsValue) || !EnableSeconds))
        {
            if (base.IsNullableType)
            {
                CurrentValueAsString = null;
                CurrentValue = default;
                _lastParsedValue = default;
                await InvokeAsync(StateHasChanged);
                return;
            }

            // non-nullable - treat as midnight
            HoursValue = "00";
            MinutesValue = "00";
            if (EnableSeconds) SecondsValue = "00";
            CurrentValueAsString = "00:00:00";
            _lastParsedValue = CurrentValue;
            await InvokeAsync(StateHasChanged);
            return;
        }

        HoursValue = string.IsNullOrWhiteSpace(HoursValue) ? string.Empty : HoursValue.PadLeft(2, '0');
        MinutesValue = string.IsNullOrWhiteSpace(MinutesValue) ? string.Empty : MinutesValue.PadLeft(2, '0');

        if (EnableSeconds)
            SecondsValue = string.IsNullOrWhiteSpace(SecondsValue) ? string.Empty : SecondsValue.PadLeft(2, '0');

        CurrentValueAsString = EnableSeconds
            ? $"{HoursValue}:{MinutesValue}:{SecondsValue}"
            : $"{HoursValue}:{MinutesValue}:00";

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
                await _jSModule.InvokeVoidAsync(GlobalValues.JS_Inputs_Unregister_Focus_Out_Callback, ControlReference);
                await _jSModule.DisposeAsync();
            }
            catch { }
        }
        await base.DisposeAsync();
    }
}

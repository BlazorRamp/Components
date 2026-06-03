using BlazorRamp.Inputs.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace BlazorRamp.Inputs.Components;

public class DateTypeInput<TValue> : InputTypeBase<TValue>, IAsyncDisposable
{


    /// <summary>
    /// Gets or sets the visible label text for the years segment input.
    /// When null, empty, or whitespace defaults to <c>"Years"</c>.
    /// </summary>
    [Parameter] public string YearsLabelText { get; set; } = GlobalValues.Date_Input_Years_Text;

    /// <summary>
    /// Gets or sets the visible label text for the months segment input.
    /// When null, empty, or whitespace defaults to <c>"Months"</c>.
    /// </summary>
    [Parameter] public string MonthsLabelText { get; set; } = GlobalValues.Date_Input_Months_Text;

    /// <summary>
    /// Gets or sets the visible label text for the days segment input.
    /// When null, empty, or whitespace defaults to <c>"Days"</c>.
    /// </summary>
    [Parameter] public string DaysLabelText { get; set; } = GlobalValues.Date_Input_Days_Text;

    /// <summary>
    /// Gets or sets whether the autocomplete attribute is added with the value of off.
    /// </summary>
    [Parameter] public bool AutoCompleteOff { get; set; } = true;

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
    /// to DateOnly. The message is prefixed with the field's
    /// display name (label text). When null, empty, or whitespace defaults to <c>"Invalid date."</c>.
    /// </summary>
    [Parameter] public string ParseErrorMessage { get; set; } = GlobalValues.Input_Parse_Date_Error_Message;


    /// <summary>
    /// Gets the resolved CSS class string applied to the root element of the date input,
    /// including any additional classes passed via <see cref="InputBase{TValue}.AdditionalAttributes"/>.
    /// </summary>
    protected string DateInputClasses { get; private set; } = String.Empty;

    /// <summary>
    /// Gets the unique <c>id</c> of the element used as the accessible label for the
    /// group via <c>aria-labelledby</c>.
    /// </summary>
    protected string ControlLabelledbyID { get; private set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets the unique <c>id</c> attribute applied to the years segment input element.
    /// </summary>
    protected string YearsInputID { get; private set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets the unique <c>id</c> attribute applied to the months segment input element.
    /// </summary>
    protected string MonthsInputID { get; private set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets the unique <c>id</c> attribute applied to the days segment input element.
    /// </summary>
    protected string DaysInputID { get; private set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets the resolved label text for the years segment, derived from
    /// <see cref="YearsLabelText"/> or the default value when not set.
    /// </summary>
    protected string YearsText { get; private set; } = GlobalValues.Date_Input_Years_Text;

    /// <summary>
    /// Gets the resolved label text for the months segment, derived from
    /// <see cref="MonthsLabelText"/> or the default value when not set.
    /// </summary>
    protected string MonthsText { get; private set; } = GlobalValues.Date_Input_Months_Text;

    /// <summary>
    /// Gets the resolved label text for the days segment, derived from
    /// <see cref="DaysLabelText"/> or the default value when not set.
    /// </summary>
    protected string DaysText { get; private set; } = GlobalValues.Date_Input_Days_Text;

    /// <summary>
    /// Gets or sets the raw string value currently displayed in the years segment input.
    /// Managed independently of <see cref="InputBase{TValue}.CurrentValue"/> to preserve
    /// mid-entry display state and leading zero removal on focus.
    /// </summary>
    protected string? YearsValue { get; set; }

    /// <summary>
    /// Gets or sets the raw string value currently displayed in the months segment input.
    /// Managed independently of <see cref="InputBase{TValue}.CurrentValue"/> to preserve
    /// mid-entry display state and leading zero removal on focus.
    /// </summary>
    protected string? MonthsValue { get; set; }

    /// <summary>
    /// Gets or sets the raw string value currently displayed in the days segment input.
    /// Managed independently of <see cref="InputBase{TValue}.CurrentValue"/> to preserve
    /// mid-entry display state and leading zero removal on focus.
    /// </summary>
    protected string? DaysValue { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="ElementReference"/> for the years segment input element,
    /// available after the component has rendered. Used for JavaScript interop to register
    /// input character-stripping handlers.
    /// </summary>
    protected ElementReference? YearsReference { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="ElementReference"/> for the months segment input element,
    /// available after the component has rendered. Used for JavaScript interop to register
    /// input character-stripping handlers.
    /// </summary>
    protected ElementReference? MonthsReference { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="ElementReference"/> for the days segment input element,
    /// is <c>true</c>. Used for JavaScript interop to register input character-stripping handlers.
    /// </summary>
    protected ElementReference? DaysReference { get; set; }


    private TValue? _lastParsedValue = default;

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;


    private DotNetObjectReference<DateTypeInput<TValue>>? _dotNetObjectRef;

    private IJSObjectReference? _jSModule = null;



    /// <summary>
    /// Updates the date input CSS classes and synchronises the segment display values
    /// when <see cref="InputBase{TValue}.CurrentValue"/> changes externally.
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        DateInputClasses = GetInputClasses(base.AdditionalAttributes);

        if (!Equals(CurrentValue, _lastParsedValue))
        {
            _lastParsedValue = CurrentValue;

            SetDisplayValues();
        }

    }

    /// <summary>
    /// Validates that <typeparamref name="TValue"/> is <see cref="DateOnly"/> or nullable
    /// <see cref="DateOnly"/>, resolves segment label texts and the parse error message
    /// from parameters, and initialises the segment display values from
    /// <see cref="InputBase{TValue}.CurrentValue"/>.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        ParseErrorMessage = string.IsNullOrWhiteSpace(ParseErrorMessage) ? GlobalValues.Input_Parse_Date_Error_Message : ParseErrorMessage.Trim();

        YearsText  = String.IsNullOrWhiteSpace(YearsLabelText)  ? GlobalValues.Date_Input_Years_Text  : YearsLabelText.Trim();
        MonthsText = String.IsNullOrWhiteSpace(MonthsLabelText) ? GlobalValues.Date_Input_Months_Text : MonthsLabelText.Trim();
        DaysText   = String.IsNullOrWhiteSpace(DaysLabelText)   ? GlobalValues.Date_Input_Days_Text   : DaysLabelText.Trim();

        if (base.DataType != typeof(DateOnly)) throw new ArgumentException(GlobalValues.Input_Date_DataType_Error_Message);

        _lastParsedValue = CurrentValue;

        SetDisplayValues();
    }



    /// <summary>
    /// Loads the JavaScript module on first render and registers the date segment
    /// character-stripping handlers and the focusout callback used to commit and
    /// parse the composite date value when focus leaves the group.
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
                await _jSModule.InvokeVoidAsync(GlobalValues.JS_Inputs_Register_Date_Segment_Handlers, YearsReference, MonthsReference, DaysReference);
                await _jSModule.InvokeVoidAsync(GlobalValues.JS_Inputs_Register_Focus_Out_Callback, ControlReference, _dotNetObjectRef, nameof(this.HandleComponentFocusOut));
            }


        }
    }

    /// <summary>
    /// Attempts to parse <paramref name="value"/> to <typeparamref name="TValue"/> using
    /// exact format <c>"yyyy-MM-dd"</c> with <see cref="CultureInfo.InvariantCulture"/>.
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

        var parts = value?.Split('-') ?? Array.Empty<string>();

        if (parts.Length == 3)
        {
            var paddedYear  = parts[0].PadLeft(4, '0');
            var paddedMonth = parts[1].PadLeft(2, '0');
            var paddedDay   = parts[2].PadLeft(2, '0');

            value = $"{paddedYear}-{paddedMonth}-{paddedDay}";
        }

        if (DateOnly.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly parsed))
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
        if (CurrentValue is DateOnly dateOnly)
        {
            if (!int.TryParse(YearsValue, out var y) || y != dateOnly.Year) YearsValue = dateOnly.Year.ToString("D4");

            if (!int.TryParse(MonthsValue, out var m) || m != dateOnly.Month) MonthsValue = dateOnly.Month.ToString("D2");

            if (!int.TryParse(DaysValue, out var d) || d != dateOnly.Day) DaysValue = dateOnly.Day.ToString("D2");
        }
        else
        {
            YearsValue = string.Empty;
            MonthsValue = string.Empty;
            DaysValue = string.Empty;
        }
    }

    /// <summary>
    /// Handles the binding set event for the years segment, updating <see cref="YearsValue"/>
    /// with the raw input value. When <see cref="UpdateOnInput"/> is <c>true</c> the
    /// composite <see cref="InputBase{TValue}.CurrentValueAsString"/> is also updated
    /// immediately. Does nothing when <see cref="InputTypeBase{TValue}.IsDisabled"/> is <c>true</c>.
    /// </summary>
    protected void HandleYearsSet(string? value)
    {
        if (IsDisabled) return;
        YearsValue = value ?? string.Empty;

        if (UpdateOnInput) CurrentValueAsString = $"{YearsValue}-{MonthsValue}-{DaysValue}";
    }

    /// <summary>
    /// Handles the binding set event for the months segment, updating <see cref="MonthsValue"/>
    /// with the raw input value. When <see cref="UpdateOnInput"/> is <c>true</c> the
    /// composite <see cref="InputBase{TValue}.CurrentValueAsString"/> is also updated
    /// immediately. Does nothing when <see cref="InputTypeBase{TValue}.IsDisabled"/> is <c>true</c>.
    /// </summary>
    protected void HandleMonthsSet(string? value)
    {
        if (IsDisabled) return;
        MonthsValue = value ?? string.Empty;

        if (UpdateOnInput) CurrentValueAsString = $"{YearsValue}-{MonthsValue}-{DaysValue}";
    }

    /// <summary>
    /// Handles the binding set event for the days segment, updating <see cref="DaysValue"/>
    /// with the raw input value. When <see cref="UpdateOnInput"/> is <c>true</c> the
    /// composite <see cref="InputBase{TValue}.CurrentValueAsString"/> is also updated
    /// immediately. Does nothing when <see cref="InputTypeBase{TValue}.IsDisabled"/> is <c>true</c>.
    /// </summary>
    protected void HandleDaysSet(string? value)
    {
        if (IsDisabled) return;
        DaysValue = value ?? string.Empty;

        if (UpdateOnInput) CurrentValueAsString = $"{YearsValue}-{MonthsValue}-{DaysValue}";
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
    /// Left pads the value with zeros so its always 4 digits for years, 2 digits for month or days.
    /// Returns the value unchanged when the component is readonly, aria-disabled,
    /// null, empty, or whitespace.
    /// </summary>
    internal string? PadMissingWithZero(string? value, int numberOfDigits = 2)
    {
        if (base.ReadOnly || base.IsDisabled || string.IsNullOrWhiteSpace(value)) return value;
        return value.PadLeft(numberOfDigits, '0');
    }

    /// <summary>
    /// Builds the CSS class string for the root element by combining the base date input
    /// class with any additional class passed via <see cref="InputBase{TValue}.AdditionalAttributes"/>.
    /// </summary>
    private static string GetInputClasses(IReadOnlyDictionary<string, object>? additionalAttributes)
    {
        var classData = additionalAttributes?.TryGetValue("class", out var extraClass) == true ? extraClass.ToString() : "";

        if (false == String.IsNullOrWhiteSpace(classData))
        {
            return $"{@GlobalValues.Date_Input_Class} {classData}";
        }

        return @GlobalValues.Date_Input_Class;
    }

    /// <summary>
    /// Returns a filtered copy of <see cref="InputBase{TValue}.AdditionalAttributes"/> with
    /// the <c>class</c> key removed, so additional attributes can be applied to the input
    /// element without duplicating the class handling.
    /// </summary>
    protected static IReadOnlyDictionary<string, object>? GetAttributes(IReadOnlyDictionary<string, object>? additionalAttributes)

        => additionalAttributes?.Where(kv => kv.Key != "class").ToDictionary();


    /// <summary>
    /// Invoked via JavaScript interop when focus leaves the entire date input group.
    /// Commits the current segment values to <see cref="InputBase{TValue}.CurrentValueAsString"/>,
    /// padding years to four digits and months and days to two digits where needed. 
    /// When all segments are empty and the type is nullable, sets 
    /// <see cref="InputBase{TValue}.CurrentValue"/> to <c>default</c>. When all segments 
    /// are empty and the type is non-nullable, defaults to <see cref="DateOnly.MinValue"/> 
    /// (<c>0001-01-01</c>). Does nothing when
    /// <see cref="InputTypeBase{TValue}.IsDisabled"/> is <c>true</c>.
    /// </summary>
    [JSInvokable]
    public async Task HandleComponentFocusOut()
    {
        if (IsDisabled) return;

        if (string.IsNullOrWhiteSpace(YearsValue) &&
            string.IsNullOrWhiteSpace(MonthsValue) &&
            string.IsNullOrWhiteSpace(DaysValue))
        {
            if (base.IsNullableType)
            {
                CurrentValueAsString = null;
                CurrentValue = default;
                _lastParsedValue = default;
                await InvokeAsync(StateHasChanged);
                return;
            }

            // non-nullable - treat as default value
            YearsValue  = "0001";
            MonthsValue = "01";
            DaysValue   = "01";
            CurrentValueAsString = "0001-01-01";
            _lastParsedValue = CurrentValue;
            await InvokeAsync(StateHasChanged);
            return;
        }

        YearsValue  = string.IsNullOrWhiteSpace(YearsValue)  ? string.Empty : YearsValue.PadLeft(4, '0');
        MonthsValue = string.IsNullOrWhiteSpace(MonthsValue) ? string.Empty : MonthsValue.PadLeft(2, '0');
        DaysValue  = string.IsNullOrWhiteSpace(DaysValue)   ? string.Empty : DaysValue.PadLeft(2, '0');

        CurrentValueAsString = $"{YearsValue}-{MonthsValue}-{DaysValue}";
     
        _lastParsedValue = CurrentValue;
        await InvokeAsync(StateHasChanged);
    }


    /// <summary>
    /// Unregisters the date input JavaScript handlers, disposes the JavaScript module reference,
    /// and calls <see cref="InputTypeBase{TValue}.DisposeAsync"/> to release base resources.
    /// </summary>
    public override async ValueTask DisposeAsync()
    {
        if (_jSModule is not null)
        {
            try
            {
                await _jSModule.InvokeVoidAsync(GlobalValues.JS_Inputs_Unregister_Date_Segment_Handlers, YearsReference, MonthsReference, DaysReference);
                await _jSModule.InvokeVoidAsync(GlobalValues.JS_Inputs_Unregister_Focus_Out_Callback, _dotNetObjectRef);
                await _jSModule.DisposeAsync();
            }
            catch { }
        }
        await base.DisposeAsync();
    }
}
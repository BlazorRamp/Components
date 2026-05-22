using BlazorRamp.Inputs.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BlazorRamp.Inputs.Components
{

    /// <summary>
    /// Renders an accessible numeric input supporting <c>int</c>, <c>long</c>, <c>decimal</c>,
    /// <c>double</c>, and <c>float</c> value types, as well as their nullable equivalents.
    /// Uses <c>type="text"</c> with an appropriate <c>inputmode</c> attribute to avoid browser
    /// spinner controls whilst providing the correct mobile keyboard. Parsing uses
    /// <see cref="System.Globalization.NumberStyles.Any"/> with <c>CultureInfo.CurrentCulture</c>
    /// to correctly handle locale-specific decimal and thousands separators.
    /// Inherits validation state management, hint text, aria-disabled support, and SVG
    /// icon support from <see cref="InputTypeBase{TValue}"/>.
    /// </summary>
    public class NumericTypeInput<TValue> : InputTypeBase<TValue> , IAsyncDisposable
    {

        /// <summary>
        /// Gets or sets whether the input value is updated on every keystroke via the
        /// <c>oninput</c> event. When <c>false</c> the value updates on <c>onchange</c>
        /// (i.e. when the field loses focus). Defaults to <c>false</c>.
        /// </summary>
        [Parameter] public bool     UpdateOnInput   { get; set; } = false;

        /// <summary>
        /// Gets or sets a .NET numeric format string applied to the display value when
        /// the input loses focus. For example <c>"F2"</c> displays two decimal places.
        /// Only applies to non-integer types. When the field is focused the raw unformatted
        /// value is shown to allow free editing. When null or empty no formatting is applied.
        /// </summary>
        [Parameter] public string?  Format          { get; set; } = null;

        /// <summary>
        /// Gets or sets the error message displayed when the input value cannot be parsed
        /// to <typeparamref name="TValue"/>. The message is prefixed with the field's
        /// display name (label text). When null, empty, or whitespace defaults to <c>"Invalid number."</c>.
        /// </summary>
        [Parameter] public string ParseErrorMessage { get; set; } = GlobalValues.Input_Parse_Number_Error_Message;

        /// <summary>
        /// Gets or sets the text/data alignment in the input. Defaults to <see cref="DataPosition.End"/>.
        /// </summary>
        [Parameter] public DataPosition DataPosition { get; set; } = DataPosition.End;

        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

        private IJSObjectReference? _jSModule = null;

        /// <summary>
        /// Gets the resolved CSS class string applied to the root element of the numeric input,
        /// including any additional classes passed via <see cref="InputBase{TValue}.AdditionalAttributes"/>.
        /// </summary>
        protected string NumericInputClasses { get; private set; } = String.Empty;

        /// <summary>
        /// Gets the <c>inputmode</c> attribute value applied to the input element.
        /// <c>"numeric"</c> for integer types, <c>"decimal"</c> for decimal, double, and float types.
        /// </summary>
        protected string InputMode           { get; private set; } = NumericInputModeType.Numeric.ToString().ToLower();

        /// <summary>
        /// Gets the resolved parse error message, derived from <see cref="ParseErrorMessage"/>
        /// or the default value when not set.
        /// </summary>
        protected string ParseErrorsText     { get; private set; } = GlobalValues.Input_Parse_Number_Error_Message;

        /// <summary>
        /// Tracks the raw string value currently displayed in the input element, independent
        /// of the parsed <see cref="InputBase{TValue}.CurrentValue"/>. Used to preserve
        /// mid-entry display state such as trailing decimal points and formatted values.
        /// </summary>
        protected string?     _stringValue = null;
        private readonly bool _isWholeNumber = true;
        private TValue? _lastParsedValue = default;

        /// <summary>
        /// Initialises <c>_isWholeNumber</c> by inspecting <see cref="InputTypeBase{TValue}.DataType"/>
        /// to determine whether the numeric type supports decimal places.
        /// </summary>
        public NumericTypeInput()
        {
            _isWholeNumber = !(base.DataType == typeof(decimal) || base.DataType == typeof(double) || base.DataType == typeof(float));
        }

        /// <summary>
        /// Updates the numeric input CSS classes and synchronises <see cref="_stringValue"/>
        /// with the formatted or raw current value when <see cref="InputBase{TValue}.CurrentValue"/>
        /// changes externally.
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            NumericInputClasses = GetInputClasses(AdditionalAttributes);

            if (!Equals(CurrentValue, _lastParsedValue))
            {
                _lastParsedValue = CurrentValue;

                _stringValue = !string.IsNullOrWhiteSpace(Format) && CurrentValue is not null && !_isWholeNumber && !HasErrors
                           ? string.Format(CultureInfo.CurrentCulture, $"{{0:{Format}}}", CurrentValue)
                           : CurrentValueAsString;
            }
        }

        /// <summary>
        /// Resolves the <c>inputmode</c> attribute and parse error text from parameters.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            InputMode = (base.DataType == typeof(int) || base.DataType == typeof(long)) 
                        ? NumericInputModeType.Numeric.ToString().ToLower() 
                        : NumericInputModeType.Decimal.ToString().ToLower();

            ParseErrorsText = String.IsNullOrWhiteSpace(ParseErrorMessage) ? GlobalValues.Input_Parse_Number_Error_Message : ParseErrorMessage.Trim();

            _stringValue = !string.IsNullOrWhiteSpace(Format) && CurrentValue is not null && !_isWholeNumber
                   ? string.Format(CultureInfo.CurrentCulture, $"{{0:{Format}}}", CurrentValue)
                   : CurrentValueAsString;

            _lastParsedValue = CurrentValue;
        }

        /// <summary>
        /// Loads the numeric JavaScript module on first render and registers the numeric
        /// input handlers for character stripping.
        /// </summary>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                 _jSModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_Inputs_File_Path);
                
                if (_jSModule is not null ) await _jSModule.InvokeVoidAsync(GlobalValues.JS_Inputs_Register_Numeric_Handlers, ControlReference, _isWholeNumber);
            }
        }


        /// <summary>
        /// Attempts to parse <paramref name="value"/> to <typeparamref name="TValue"/> using
        /// <see cref="System.Globalization.NumberStyles.Any"/> with <c>CultureInfo.CurrentCulture</c>.
        /// Supports <c>int</c>, <c>long</c>, <c>decimal</c>, <c>double</c>, and <c>float</c>
        /// and their nullable equivalents. Returns <c>true</c> on success. On failure sets
        /// <paramref name="validationErrorMessage"/> to the resolved parse error message
        /// prefixed with the field display name.
        /// </summary>
        protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
        {
            validationErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(value) && true == base.IsNullableType)
            {
                result = default(TValue)!;// default!;//stop the green squiggle
                return true;
            }


            switch (base.DataType)
            {
                case Type t when t == typeof(int)     && int.TryParse(value, NumberStyles.Any, null, out int intValue):             result = (TValue)(object)intValue;     return true; 
                case Type t when t == typeof(long)    && long.TryParse(value, NumberStyles.Any, null, out long longValue):          result = (TValue)(object)longValue;    return true; 
                case Type t when t == typeof(decimal) && decimal.TryParse(value, NumberStyles.Any, null, out decimal decimalValue): result = (TValue)(object)decimalValue; return true; 
                case Type t when t == typeof(double)  && double.TryParse(value, NumberStyles.Any, null, out double doubleValue):    result = (TValue)(object)doubleValue; return true; 
                case Type t when t == typeof(float)   && float.TryParse(value, NumberStyles.Any, null, out float floatValue):       result = (TValue)(object)floatValue; return true; 

            }

            result = default;
            validationErrorMessage = String.Concat(base.LabelNameText.TrimEnd(':').Trim(), " - ", ParseErrorsText);
            return false;
        }


        /// <summary>
        /// Handles the binding set event, updating <see cref="InputBase{TValue}.CurrentValueAsString"/>
        /// and <see cref="_stringValue"/> with the raw input value. Does nothing when
        /// <see cref="InputTypeBase{TValue}.IsDisabled"/> is <c>true</c>.
        /// </summary>
        protected void HandlePropertySet(string? value)
        {
            if (IsDisabled) return;

            CurrentValueAsString = _stringValue = value;
            
            _lastParsedValue = CurrentValue;

        }

        /// <summary>
        /// Handles the input blur event. Sets <see cref="InputBase{TValue}.CurrentValue"/> to
        /// <c>default</c> when the field is cleared, and applies the <see cref="Format"/> string
        /// to the display value via JavaScript interop when a format is set and the type is
        /// non-integer.
        /// </summary>
        protected async Task HandleOnBlur()
        {

            if (string.IsNullOrWhiteSpace(_stringValue)) CurrentValue = default;

            if (!_isWholeNumber && !string.IsNullOrWhiteSpace(Format) && CurrentValue is not null && _jSModule is not null && !HasErrors)
            {
                _stringValue = string.Format(CultureInfo.CurrentCulture, $"{{0:{Format}}}", CurrentValue);
                await _jSModule.InvokeVoidAsync(GlobalValues.JS_Inputs_Set_Value, ControlReference, _stringValue);
                return;
            }

            _stringValue = CurrentValueAsString;
        }


        /// <summary>
        /// Builds the CSS class string for the root element by combining the base numeric input
        /// class with any additional class passed via <see cref="InputBase{TValue}.AdditionalAttributes"/>.
        /// </summary>
        private string GetInputClasses(IReadOnlyDictionary<string, object>? additionalAttributes)
        {
            var classData = additionalAttributes?.TryGetValue("class", out var extraClass) == true ? extraClass.ToString() : "";

            if (false == String.IsNullOrWhiteSpace(classData))
            {
                return $"{@GlobalValues.Numeric_Input_Class} {classData}";
            }

            return @GlobalValues.Numeric_Input_Class;
        }

        /// <summary>
        /// Returns a filtered copy of <see cref="InputBase{TValue}.AdditionalAttributes"/> with
        /// the <c>class</c> key removed, so additional attributes can be applied to the input
        /// element without duplicating the class handling.
        /// </summary>
        protected IReadOnlyDictionary<string, object>? GetAttributes(IReadOnlyDictionary<string, object>? additionalAttributes)

            => additionalAttributes?.Where(kv => kv.Key != "class").ToDictionary();

        /// <summary>
        /// Unregisters numeric JavaScript handlers, disposes the JavaScript module reference,
        /// and calls <see cref="InputTypeBase{TValue}.DisposeAsync"/> to release base resources.
        /// </summary>
        public override async ValueTask DisposeAsync()
        {
            if (_jSModule is not null)
            {
                try
                {
                    await _jSModule.InvokeVoidAsync(GlobalValues.JS_Inputs_Unregister_Numeric_Handlers, ControlReference, _isWholeNumber);
                    await _jSModule.DisposeAsync();
                }
                catch { }
            }
            await base.DisposeAsync();
        }

    }
}

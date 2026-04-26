using BlazorRamp.Inputs.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BlazorRamp.Inputs.Components
{
    public class NumericTypeInput<TValue> : InputTypeBase<TValue> , IAsyncDisposable
    {

        [Parameter] public bool UpdateOnInput { get; set; } = false;
        [Parameter] public string? Format { get; set; } = null;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

        private IJSObjectReference? _jSModule = null;

        protected string NumericInputClasses { get; private set; } = String.Empty;
        protected string InputMode { get; private set; } = NumericInputModeType.Numeric.ToString().ToLower();

        private readonly bool _isWholeNumber = true;
        private readonly Type _dataType;
        private readonly bool _typeIsNullable;

        protected string? _stringValue = null;
        protected bool _required       = false;
        
        private TValue? _lastParsedValue = default;

        public NumericTypeInput()
        {
            _dataType       = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
            _typeIsNullable = Nullable.GetUnderlyingType(typeof(TValue)) != null;

            _isWholeNumber = !(_dataType == typeof(decimal) || _dataType == typeof(double) || _dataType == typeof(float));
        }
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            NumericInputClasses = GetInputClasses(AdditionalAttributes);

            _required = _typeIsNullable ? false : base.Required;

            if (!Equals(CurrentValue, _lastParsedValue))
            {
                _lastParsedValue = CurrentValue;

                _stringValue = !string.IsNullOrWhiteSpace(Format) && CurrentValue is not null && false == _isWholeNumber
                           ? string.Format(CultureInfo.CurrentCulture, $"{{0:{Format}}}", CurrentValue)
                           : CurrentValueAsString;

            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            InputMode = (_dataType == typeof(int) || _dataType == typeof(long)) 
                        ? NumericInputModeType.Numeric.ToString().ToLower() 
                        : NumericInputModeType.Decimal.ToString().ToLower();

        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                 _jSModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_Inputs_File_Path);
                
                if (_jSModule is not null ) await _jSModule.InvokeVoidAsync(GlobalValues.JS_Inputs_Register_Numeric_Handlers, ControlReference, _isWholeNumber);
            }
        }



        protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
        {
            validationErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(value) && _typeIsNullable)
            {
                result = default!;//stop the green squiggle
                return true;
            }


            switch (_dataType)
            {
                case Type t when t == typeof(int)     && int.TryParse(value, NumberStyles.Any, null, out int intValue):             { result = (TValue)(object)intValue;     return true; }
                case Type t when t == typeof(long)    && long.TryParse(value, NumberStyles.Any, null, out long longValue):          { result = (TValue)(object)longValue;    return true; }
                case Type t when t == typeof(decimal) && decimal.TryParse(value, NumberStyles.Any, null, out decimal decimalValue): { result = (TValue)(object)decimalValue; return true; }
                case Type t when t == typeof(double)  && double.TryParse(value, NumberStyles.Any, null, out double doubleValue):    { result = (TValue)(object)doubleValue; return true; }
                case Type t when t == typeof(float)   && float.TryParse(value, NumberStyles.Any, null, out float floatValue):       { result = (TValue)(object)floatValue; return true; }

            }

            result = default;
            return false;
        }

        protected void HandlePropertySet(string? value)
        {
            if (IsDisabled) return;

            CurrentValueAsString = _stringValue = value;
            
            _lastParsedValue = CurrentValue;

        }

        protected async Task HandleOnBlur()
        {

            if (string.IsNullOrWhiteSpace(_stringValue))  CurrentValue = default;

            if (!_isWholeNumber && !string.IsNullOrWhiteSpace(Format) && CurrentValue is not null && _jSModule is not null)
            {
                await _jSModule.InvokeVoidAsync(GlobalValues.JS_Inputs_Set_Value, ControlReference, string.Format(CultureInfo.CurrentCulture, $"{{0:{Format}}}", CurrentValue));
                return;
            }
            
            _stringValue = CurrentValueAsString;
        }



        protected string GetInputClasses(IReadOnlyDictionary<string, object>? additionalAttributes)
        {
            var classData = additionalAttributes?.TryGetValue("class", out var extraClass) == true ? extraClass.ToString() : "";

            if (false == String.IsNullOrWhiteSpace(classData))
            {
                return $"{@GlobalValues.Numeric_Input_Class} {classData}";
            }

            return @GlobalValues.Numeric_Input_Class;
        }


        protected IReadOnlyDictionary<string, object>? GetAttributes(IReadOnlyDictionary<string, object>? additionalAttributes)

            => additionalAttributes?.Where(kv => kv.Key != "class").ToDictionary();


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

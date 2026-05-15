using BlazorRamp.Inputs.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace BlazorRamp.Inputs.Components;

/// <summary>
/// Renders an accessible select input supporting <c>byte</c>, <c>short</c>, <c>int</c>, <c>long</c>,
/// <c>Guid</c>, and <c>string</c> value types, as well as their nullable equivalents.
/// Inherits validation state management, hint text, aria-disabled support, and SVG
/// icon support from <see cref="InputTypeBase{TValue}"/>.
/// Interaction blocking for readonly and aria-disabled states is handled via JavaScript
/// interop, preventing mouse and keyboard activation of the native dropdown.
/// </summary>
public class SelectTypeInput<TValue> : InputTypeBase<TValue>, IAsyncDisposable
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

    private bool _inactiveHandlerRegistered = false;


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

    /// <summary>
    /// Loads the JavaScript module on first render and registers or unregisters
    /// readonly click handlers as the <see cref="InputTypeBase{TValue}.ReadOnly"/> state changes.
    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (true == firstRender) _jSModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_Inputs_File_Path);


        if (_jSModule is null) return;

        if ((true == ReadOnly || true == IsDisabled) && false == _inactiveHandlerRegistered)
        {
            await _jSModule.InvokeVoidAsync(GlobalValues.JS_Inputs_Register_Select_Readonly_Disabled_Handlers, ControlReference);
            _inactiveHandlerRegistered = true;
        }
        if ((false == ReadOnly && false== IsDisabled) && true == _inactiveHandlerRegistered)
        {
            await _jSModule.InvokeVoidAsync(GlobalValues.JS_Inputs_Unregister_Select_Readonly_Disabled_Handlers, ControlReference);
            _inactiveHandlerRegistered = false;
        }
    }
    /// <summary>
    /// Attempts to parse <paramref name="value"/> to <typeparamref name="TValue"/>.
    /// Supports <c>byte</c>, <c>short</c>, <c>int</c>, <c>long</c>, <c>Guid</c>,<c>bool</c> and <c>string</c>
    /// and their nullable equivalents. Returns <c>true</c> on success. On failure sets
    /// <paramref name="validationErrorMessage"/> to the resolved parse error message
    /// prefixed with the field display name. In normal use this path should never be reached
    /// as the select options constrain the value to valid entries.
    /// </summary>
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        validationErrorMessage = string.Empty;

        switch (base.DataType)
        {
            case Type type when type == typeof(bool) && bool.TryParse(value, out bool boolValue): result = (TValue)(object)boolValue; return true;
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

    /// <summary>
    /// Handles the binding set event, updating <see cref="InputBase{TValue}.CurrentValueAsString"/>
    /// with the selected option value. Sets <see cref="InputBase{TValue}.CurrentValue"/> to
    /// <c>default</c> when the value is null or whitespace and the type is nullable.
    /// Does nothing when <see cref="InputTypeBase{TValue}.IsDisabled"/> is <c>true</c>.
    /// </summary>
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


    /// <summary>
    /// Disposes the JavaScript module reference,
    /// and calls <see cref="InputTypeBase{TValue}.DisposeAsync"/> to release base resources.
    /// </summary>
    public override async ValueTask DisposeAsync()
    {
        if (_jSModule is not null)
        {
            try
            {
                await _jSModule.DisposeAsync();
            }
            catch { }
        }
        await base.DisposeAsync();
    }
}

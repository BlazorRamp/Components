using BlazorRamp.Inputs.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.Inputs.Components;

/// <summary>
/// Renders an accessible checkbox input. Inherits validation state management,
/// hint text, aria-disabled support, and SVG icon support from <see cref="InputTypeBase{TValue}"/>.
/// Unlike text-based inputs, value changes are notified to the <see cref="EditContext"/>
/// immediately on click rather than on blur.
/// </summary>
public class CheckboxTypeInput : InputTypeBase<bool>, IAsyncDisposable
{

    /// <summary>
    /// Gets or sets the text/data alignment in the input. Defaults to <see cref="DataPosition.Start"/>.
    /// </summary>
    [Parameter] public DataPosition DataPosition { get; set; } = DataPosition.Start;


    /// <summary>
    /// Gets the resolved CSS class string applied to the root element of the checkbox input,
    /// including any additional classes passed via <see cref="InputBase{TValue}.AdditionalAttributes"/>.
    /// </summary>
    protected string CheckboxInputClasses { get; private set; } = String.Empty;


    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private IJSObjectReference? _jSModule = null;

    private bool _readonlyHandlerRegistered = false;

    /// <summary>
    /// Updates the checkbox input CSS classes on each parameter change.
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        CheckboxInputClasses = GetInputClasses(base.AdditionalAttributes);

    }
    /// <summary>
    /// Not supported for checkbox inputs. Always throws <see cref="NotSupportedException"/>.
    /// Bind to <see cref="InputBase{TValue}.CurrentValue"/> directly, not
    /// <see cref="InputBase{TValue}.CurrentValueAsString"/>.
    /// </summary>
    protected override bool TryParseValueFromString(string? value, out bool result, [NotNullWhen(false)] out string? validationErrorMessage)
            => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");




    /// <summary>
    /// Loads the JavaScript module on first render and registers or unregisters
    /// readonly click handlers as the <see cref="InputTypeBase{TValue}.ReadOnly"/> state changes.
    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if( true == firstRender) _jSModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_Inputs_File_Path);

        
        if (_jSModule is null) return;

        if (true == ReadOnly && false == _readonlyHandlerRegistered)
        {
            await _jSModule.InvokeVoidAsync(GlobalValues.JS_Inputs_Register_Readonly_Handlers, ControlReference);
            _readonlyHandlerRegistered = true;
        }
        if (false == ReadOnly && true == _readonlyHandlerRegistered)
        {
            await _jSModule.InvokeVoidAsync(GlobalValues.JS_Inputs_Unregister_Readonly_Handlers, ControlReference);
            _readonlyHandlerRegistered = false;
        }
    }

    /// <summary>
    /// Handles the checkbox change event, updating <see cref="InputBase{TValue}.CurrentValue"/>
    /// and immediately notifying the <see cref="EditContext"/> that the field has changed,
    /// so validation state updates on click rather than waiting for blur.
    /// Does nothing when <see cref="InputTypeBase{TValue}.IsDisabled"/> is <c>true</c>.
    /// </summary>
    protected void HandleOnChange(ChangeEventArgs e)
    {
        if (IsDisabled) return;
        CurrentValue = (bool)(e.Value ?? false);
      
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
        return $"{@GlobalValues.Checkbox_Input_Class} {classData}";
    }

    return @GlobalValues.Checkbox_Input_Class;
}

/// <summary>
/// Returns a filtered copy of <see cref="InputBase{TValue}.AdditionalAttributes"/> with
/// the <c>class</c> key removed, so additional attributes can be applied to the input
/// element without duplicating the class handling.
/// </summary>
protected static IReadOnlyDictionary<string, object>? GetAttributes(IReadOnlyDictionary<string, object>? additionalAttributes)

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
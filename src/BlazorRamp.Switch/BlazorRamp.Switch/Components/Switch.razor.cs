using BlazorRamp.Core.Common.Utilities;
using BlazorRamp.Switch.Common.Constants;
using Microsoft.AspNetCore.Components;

namespace BlazorRamp.Switch.Components;

/// <summary>
/// An accessible toggle switch component that provides a visual representation of an on/off state.
/// </summary>
public partial class Switch
{
    /// <summary>
    /// Gets or sets the label text displayed next to the switch.
    /// This property is required and cannot be null, empty or whitespace.
    /// </summary>
    [Parameter, EditorRequired] public string   Label { get; set; } = default!;

    /// <summary>
    /// Gets or sets the current state of the switch.
    /// Defaults to false (off).
    /// </summary>
    [Parameter] public bool     SwitchState { get; set; } = false;

    /// <summary>
    /// Gets or sets whether the switch is aria-disabled.
    /// Defaults to false.
    /// </summary>
    [Parameter] public bool     AriaDisabled     { get; set; } = false;

    /// <summary>
    /// Gets or sets whether to add space between the label and switch control.
    /// Defaults to false.
    /// </summary>
    [Parameter] public bool     SpaceBetween { get; set; } = false;

    /// <summary>
    /// Gets or sets the callback invoked when the switch state changes.
    /// </summary>
    [Parameter] public EventCallback<bool> SwitchStateChanged { get; set; }

    /// <summary>
    /// Gets or sets additional attributes that will be applied to the component.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private bool _switchState = false;

    private async Task RaiseOnSwitchStateChanged(bool switchState)
    {
        if (switchState == _switchState || true == AriaDisabled) return;

        _switchState = switchState;

        if (true == SwitchStateChanged.HasDelegate) await SwitchStateChanged.InvokeAsync(switchState);
    }

    /// <summary>
    /// Updates the internal state of the component when parameters are assigned or changed.
    /// Validates that the Label parameter is not null, empty or whitespace.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when Label is null, empty or whitespace.</exception>

    protected override async Task OnParametersSetAsync()
    {
        if (true == String.IsNullOrWhiteSpace(Label)) throw new ArgumentNullException(nameof(Label), "Label cannot be null, empty or whitespace");

        if (_switchState != SwitchState && false == AriaDisabled) await RaiseOnSwitchStateChanged(SwitchState);
    }
    /// <summary>
    /// Captures the initial state of the switch
    /// </summary>
    protected override void OnInitialized()
    
        => _switchState = SwitchState;
    

    private static string? BuildClassList(bool spaceBetween)

        =>  spaceBetween ? CoreUtilities.CreateClassList(GlobalValues.Switch_Class, GlobalValues.Switch_Space_Modifier_Class) : GlobalValues.Switch_Class;
}

using BlazorRamp.Core.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorRamp.Switch.Components;

public partial class Switch
{
    [Parameter] public string   Label        { get; set; } = default!;
    [Parameter] public bool     SwitchState  { get; set; } = false;
    [Parameter] public bool     Disabled     { get; set; } = false;
    [Parameter] public bool     SpaceBetween { get; set; } = false;
    [Parameter] public EventCallback<bool> SwitchStateChanged { get; set; }

    private bool    _switchState   = false;
    private string? _switchClasses = null;

    private async Task RaiseOnSwitchStateChanged(bool switchState)
    {
        if (switchState == _switchState || true == Disabled) return;

        _switchState = switchState;

        if (true == SwitchStateChanged.HasDelegate) await SwitchStateChanged.InvokeAsync(switchState);
    }

    protected override async Task OnParametersSetAsync()
    {
        if (true == String.IsNullOrWhiteSpace(Label)) throw new ArgumentNullException(nameof(Label), "Label cannot be null, empty or whitespace");

        if (_switchState != SwitchState && false == Disabled) await RaiseOnSwitchStateChanged(SwitchState);
    }
}

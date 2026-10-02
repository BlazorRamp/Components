using BlazorRamp.Disclosure.Common.Constants;
using Microsoft.AspNetCore.Components;

namespace BlazorRamp.Disclosure.Components;

public partial class Disclosure
{
    [Parameter] public RenderFragment? ChildContent { get; set; } = null;
    [Parameter] public string TriggerText            { get; set; } = default!;


    private string _triggerText = String.Empty;
    private bool    _isExpanded = false;

    private string _contentID = Guid.NewGuid().ToString();  

    protected override void OnParametersSet()
    {
        if (true == String.IsNullOrWhiteSpace(TriggerText)) throw new ArgumentNullException(nameof(TriggerText), GlobalValues.Disclosure_Trigger_Text_Exception_Message);

        _triggerText = TriggerText.Trim();
    }


    private async Task ToggleState()

        => _isExpanded = !_isExpanded;
}

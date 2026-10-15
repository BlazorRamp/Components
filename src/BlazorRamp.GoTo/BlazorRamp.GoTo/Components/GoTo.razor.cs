
using BlazorRamp.GoTo.Common.Constants;
using Microsoft.AspNetCore.Components;

namespace BlazorRamp.GoTo.Components;

public partial class GoTo
{
    [Parameter] public string LabelText { get; set; } = default!;
    [Parameter] public bool IsActive    { get; set; } = true;

    private string _markerID   = Guid.NewGuid().ToString();
    private string _popoverID = Guid.NewGuid().ToString();

    private string _labelID = Guid.NewGuid().ToString();
    private string _labelText = String.Empty;

    private string _labelPosition = "top-left";
    protected override void OnParametersSet()
    {
        _labelText = String.IsNullOrWhiteSpace(LabelText) ? throw new ArgumentNullException(nameof(LabelText), GlobalValues.GoTo_Label_Text_Exception_Message) : LabelText.Trim();
    }

}

using BlazorRamp.Inputs.Common.Constants;
using Microsoft.AspNetCore.Components;

namespace BlazorRamp.Inputs.Components.RadioGroup;


public partial class RadioInput
{
    [CascadingParameter(Name = "RadioGroupName")] private string GroupName { get; set; } = default!;
    [Parameter] public string LabelText { get; set; }

    private string _inputID = Guid.NewGuid().ToString();

    protected override void OnInitialized()
    {
        if (String.IsNullOrWhiteSpace(LabelText)) throw new ArgumentNullException(nameof(LabelText), GlobalValues.Input_Missing_Label_Text_Error_Message);
        if (String.IsNullOrWhiteSpace(GroupName)) throw new ArgumentNullException(nameof(GroupName), GlobalValues.Input_Missing_Radio_Group_Parent_Error_Message);
    }
}

using BlazorRamp.Inputs.Common.Constants;
using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;

namespace BlazorRamp.Inputs.Components.RadioGroup;

[CascadingTypeParameter(nameof(TValue))]
public partial class RadioInput<TValue>
{
    [CascadingParameter] private RadioInputGroup<TValue> ParentControl { get; set; } = default!;
    [Parameter] public string LabelText { get; set; }

    [Parameter, EditorRequired] public required TValue Value { get; set; }
    /// <summary>
    /// Gets or sets additional attributes applied to the component's root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private bool IsChecked => ParentControl.GroupValue is not null && ParentControl.GroupValue.Equals(Value);

    private string _inputID = Guid.NewGuid().ToString();
    private string _groupName = String.Empty;

    protected override void OnInitialized()
    {
        if (String.IsNullOrWhiteSpace(LabelText)) throw new ArgumentNullException(nameof(LabelText), GlobalValues.Input_Missing_Label_Text_Error_Message);
        if (ParentControl is null) throw new ArgumentNullException(nameof(ParentControl), GlobalValues.Input_Missing_Radio_Group_Parent_Error_Message);

        _groupName = ParentControl.InputID;
    }

    private void HandleOnChange()

        => ParentControl.SetGroupValue(Value);
}

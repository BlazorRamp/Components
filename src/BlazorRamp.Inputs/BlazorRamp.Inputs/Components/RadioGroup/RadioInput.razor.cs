using BlazorRamp.Inputs.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
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
    /// <summary>
    /// Gets the resolved CSS class string applied to the root element of the radio input,
    /// including any additional classes passed via <see cref="InputBase{TValue}.AdditionalAttributes"/>.
    /// </summary>
    protected string RadioInputClasses { get; private set; } = String.Empty;
    private bool IsChecked => ParentControl.GroupValue is not null && ParentControl.GroupValue.Equals(Value);

    private string _inputID = Guid.NewGuid().ToString();
    private string _groupName = String.Empty;

    /// <summary>
    /// Updates the radio input group CSS classes on each parameter change.
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        RadioInputClasses = GetInputClasses(AdditionalAttributes);
    }

    protected override void OnInitialized()
    {
        if (String.IsNullOrWhiteSpace(LabelText)) throw new ArgumentNullException(nameof(LabelText), GlobalValues.Input_Missing_Label_Text_Error_Message);
        if (ParentControl is null) throw new ArgumentNullException(nameof(ParentControl), GlobalValues.Input_Missing_Radio_Group_Parent_Error_Message);

        _groupName = ParentControl.InputID;
    }

    private void HandleOnChange()

        => ParentControl.SetGroupValue(Value);


    /// <summary>
    /// Builds the CSS class string for the root element by combining the base input
    /// class with any additional class passed via <see cref="InputBase{TValue}.AdditionalAttributes"/>.
    /// </summary>
    protected static string GetInputClasses(IReadOnlyDictionary<string, object>? additionalAttributes)
    {
        var classData = additionalAttributes?.TryGetValue("class", out var extraClass) == true ? extraClass.ToString() : "";

        if (false == String.IsNullOrWhiteSpace(classData))
        {
            return $"{@GlobalValues.Radio_Input_Class} {classData}";
        }

        return @GlobalValues.Radio_Input_Class;
    }

    /// <summary>
    /// Returns a filtered copy of <see cref="InputBase{TValue}.AdditionalAttributes"/> with
    /// the <c>class</c> key removed, so additional attributes can be applied to the input
    /// element without duplicating the class handling.
    /// </summary>
    protected static IReadOnlyDictionary<string, object>? GetAttributes(IReadOnlyDictionary<string, object>? additionalAttributes)

        => additionalAttributes?.Where(kv => kv.Key != "class").ToDictionary();

}

using BlazorRamp.Inputs.Common.Constants;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace BlazorRamp.Inputs.Components;

public class TextTypeInput : InputTypeBase<string>
{

    [Parameter] public bool          UpdateOnInput  { get; set; } = false;
    [Parameter] public TextInputType TextInputType{ get; set; } = TextInputType.Text;

    protected string InputClasses { get; private set; } = String.Empty;
    protected string InputType    { get; private set; } = "text";
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        InputClasses = GetInputClasses(MutableAttributes);
      
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        InputType = Enum.GetName<TextInputType>(TextInputType)!.ToLower();
    }
    protected override bool TryParseValueFromString(string? value, out string? result, [NotNullWhen(false)] out string? validationErrorMessage)
    {

        validationErrorMessage = String.Empty;

        result = value;

        return true;
    }

    protected void HandlePropertySet(string? value)
    {
        if (base.IsDisabled)  return;
         
        CurrentValueAsString = value;
    }

    protected string GetInputClasses(Dictionary<string, object> additionalAttributes)
    {
        var classData = additionalAttributes?.TryGetValue("class", out var extraClass) == true ? extraClass.ToString() : "";

        if (false == String.IsNullOrWhiteSpace(classData))
        {
            additionalAttributes?.Remove("class");

            return $"{@GlobalValues.Text_Input_Class} {classData}";
        }

        return @GlobalValues.Text_Input_Class;
    }


}

using BlazorRamp.Inputs.Common.Constants;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace BlazorRamp.Inputs.Components;

public class TextTypeInput : InputTypeBase<string>
{

    [Parameter] public bool          UpdateOnInput  { get; set; } = false;
    [Parameter] public TextInputType TextInputType  { get; set; } = TextInputType.Text;
    protected string TextInputClasses { get; private set; } = String.Empty;
    protected string InputType    { get; private set; } = "text";
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        TextInputClasses = GetInputClasses(base.AdditionalAttributes);
        
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        InputType = Enum.GetName<TextInputType>(TextInputType)?.ToLower() ?? "text";
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


    protected string GetInputClasses(IReadOnlyDictionary<string, object>? additionalAttributes)
    {
        var classData = additionalAttributes?.TryGetValue("class", out var extraClass) == true ? extraClass.ToString() : "";

        if (false == String.IsNullOrWhiteSpace(classData))
        {
            return $"{@GlobalValues.Text_Input_Class} {classData}";
        }

        return @GlobalValues.Text_Input_Class;
    }

    protected IReadOnlyDictionary<string, object>? GetAttributes(IReadOnlyDictionary<string, object>? additionalAttributes)

        => additionalAttributes?.Where(kv => kv.Key != "class").ToDictionary();

}

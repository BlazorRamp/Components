using BlazorRamp.Inputs.Common.Constants;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace BlazorRamp.Inputs.Components;

public class PasswordTypeInput : InputTypeBase<string>
{

    [Parameter] public bool                 UpdateOnInput       { get; set; } = false;
    [Parameter] public string               ShowPasswordText    { get; set; } = default!;
    [Parameter] public PasswordAutoComplete PasswordAutoComplete { get; set; } = PasswordAutoComplete.CurrentPassword;

    protected string InputClasses { get; private set; } = String.Empty;
    protected string InputType    { get; private set; } = "password";
    protected string ShowText     { get; private set; } = GlobalValues.Password_Input_Show_Password_Text;

    protected string AutoComplete { get; private set; } = "current-password";
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        InputClasses = GetInputClasses(MutableAttributes);
        AutoComplete = PasswordAutoComplete == PasswordAutoComplete.CurrentPassword ? "current-password" : "new-password";
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        ShowText = String.IsNullOrWhiteSpace(ShowPasswordText) ? GlobalValues.Password_Input_Show_Password_Text : ShowPasswordText.Trim();

    }
    protected override bool TryParseValueFromString(string? value, out string? result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        validationErrorMessage = String.Empty;

        result = value;

        return true;
    }

    protected void ToggleShowPassword()

        => InputType = InputType == "text" ? "password" : "text";

    protected void HandlePropertySet(string? value)
    {
        if (base.IsDisabled) return;

        CurrentValueAsString = value;
    }

    protected string GetInputClasses(Dictionary<string, object> additionalAttributes)
    {
        var classData = additionalAttributes?.TryGetValue("class", out var extraClass) == true ? extraClass.ToString() : "";

        if (false == String.IsNullOrWhiteSpace(classData))
        {
            additionalAttributes?.Remove("class");

            return $"{@GlobalValues.Password_Input_Class} {classData}";
        }

        return @GlobalValues.Password_Input_Class;
    }


}
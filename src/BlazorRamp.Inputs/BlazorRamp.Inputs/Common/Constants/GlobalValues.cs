namespace BlazorRamp.Inputs.Common.Constants;

internal class GlobalValues
{

    public const string JS_Inputs_File_Path                        = "./_content/BlazorRamp.Inputs/assets/js/inputs.js";
    public const string JS_Inputs_Register_Aria_Disabled_Handlers   = "registerAriaDisabledHandlers";
    public const string JS_Inputs_Unregister_Aria_Disabled_Handlers = "unregisterAriaDisabledHandlers";


    public const string Text_Input_Class = "br-text-input";

    public const string Text_Input_Label_Class          = $"{Text_Input_Class}__label";
    public const string Text_Input_Hint_Class           = $"{Text_Input_Class}__hint";
    public const string Text_Input_Error_Wrapper_Class  = $"{Text_Input_Class}__error-wrapper";
    public const string Text_Input_Error_Class          = $"{Text_Input_Class}__error";
    public const string Text_Input_Asterisk_Class       = $"{Text_Input_Class}__asterisk";
    public const string Text_Input_Field_Class          = $"{Text_Input_Class}__field";
    public const string Text_Input_Icon_Class           = $"{Text_Input_Class}__icon";
    public const string Text_Input_State_Icon_Class     = $"{Text_Input_Class}__state-icon";


    public const string Text_Input_State_Icon_Valid_Modifier   = $"{Text_Input_State_Icon_Class}--valid";
    public const string Text_Input_State_Icon_Invalid_Modifier = $"{Text_Input_State_Icon_Class}--invalid";


    public const string Text_Input_Svg_Css_Variable_Name = "--_br-svg-text-input-icon";

    public const string Default_Errors_label = "Errors";

}

namespace BlazorRamp.DebounceFilter.Common.Constants;

internal class GlobalValues
{

    public const string JS_Debounce_Filter_File_Path            = "./_content/BlazorRamp.DebounceFilter/assets/js/debounce-filter.js";
    public const string JS_Register_Debounce_Filter_Handler     = "registerDebounceFilterHandler";
    public const string JS_Unregister_Debounce_Filter_Handler   = "unregisterDebounceFilterHandler";
    public const string JS_Clear_Debounce_Filter                = "clearDebounceFilter";


    public const string Debounce_Filter_Regex_Error_Message      = "System error, filtering is unavailable at this time.";
    public const string Debounce_Filter_Regex_Validation_Message = "Invalid characters, filtering paused.";
    public const string Debounce_Filter_Label_Text               = "Filter";

    public const int Debounce_DelayMs = 500;


    public const string Debounce_Filter_Svg_Css_Variable_Name = "--_br-svg-debounce-filter-icon";

    public const string Debounce_Filter_Class                = "br-debounce-filter";
    public const string Debounce_Filter_Label_Class          = $"{Debounce_Filter_Class}__label";
    public const string Debounce_Filter_Hint_Class           = $"{Debounce_Filter_Class}__hint";
    public const string Debounce_Filter_Error_Class          = $"{Debounce_Filter_Class}__error";
    public const string Debounce_Filter_Field_Class          = $"{Debounce_Filter_Class}__field";
    public const string Debounce_Filter_Icon_Class           = $"{Debounce_Filter_Class}__icon";
    public const string Debounce_Filter_State_Icon_Class     = $"{Debounce_Filter_Class}__state-icon";
}

namespace BlazorRamp.Tooltip.Common.Constants;

internal class GlobalValues
{
    public const string JS_Module_File_Path = "./_content/BlazorRamp.Tooltip/assets/js/tooltip.js";

    public const string JS_Register_Tooltip_Func = "registerTooltip";
    public const string JS_Unregister_Tooltip_Func = "unregisterTooltip";

    public const string JS_Close_Open_Tooltips_Func = "closeOpenTooltips";



    public const string Tooltip_class = "br-tooltip";

    public const string Tooltip_Content_Wrapper_class = $"{Tooltip_class}__content-wrapper";
    public const string Tooltip_Content_Area_class    = $"{Tooltip_class}__content-area";
    public const string Tooltip_Content_Area_Modifier = $"{Tooltip_class}__content-area--inverted-colours";
    public const string Tooltip_Content_class         = $"{Tooltip_class}__content";
    public const string Tooltip_Closer_Class          = $"{Tooltip_class}__closer";
    public const string Tooltip_icon_class            = $"{Tooltip_class}__icon";

}

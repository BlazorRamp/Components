namespace BlazorRamp.Accordion.Common.Constants;

internal class GlobalValues
{

    public const string Heading_Text_Exception_Message = "Heading text cannot be null, empty or just whitespace";
    
    public const string JS_Module_File_Path        = "./_content/BlazorRamp.Accordion/assets/js/accordion.js";
    public const string JS_Register_Handler_Func   = "registerKeyHandler";
    public const string JS_UnRegister_Handler_Func = "unregisterKeyHandler";


    public const string Accordion_Class = "br-accordion";

    public const string Accordion_Panel_Class           = $"{Accordion_Class}__panel";
    public const string Accordion_Heading_Class         = $"{Accordion_Class}__heading";
    public const string Accordion_Heading_Icon_Class    = $"{Accordion_Class}__heading-icon";
    public const string Accordion_Trigger_Class         = $"{Accordion_Class}__trigger";
    public const string Accordion_Trigger_Content_Class = $"{Accordion_Class}__trigger-content";
    public const string Accordion_Trigger_Icon_Class    = $"{Accordion_Class}__trigger-icon";


    public const string Accordion_Trigger_Focus_Modifier = $"{Accordion_Trigger_Class}--code-focused";


    public const string KeyBoard_Up_Arrow_Key   = "ArrowUp";
    public const string KeyBoard_Down_Arrow_Key = "ArrowDown";
    public const string KeyBoard_Home_Key       = "Home";
    public const string KeyBoard_End_Key        = "End";

    public const string Accordion_Svg_Css_Variable_Name = "--_br-svg-accordion-icon";

}


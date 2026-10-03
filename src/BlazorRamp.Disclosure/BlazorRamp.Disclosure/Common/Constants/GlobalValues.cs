namespace BlazorRamp.Disclosure.Common.Constants;

internal class GlobalValues
{

    public const string Disclosure_Trigger_Text_Exception_Message = "Trigger text cannot be null, empty or just whitespace";

    public const string Disclosure_Content_Max_Height_Exception_Message = "The value specified was invalid and cannot be used";
    public const string Disclosure_Trigger_Icon_Svg_Css_Variable_Name = "--_br-svg-disclosure-trigger-icon";

    public const string Disclosure_Content_Height_Css_Variable_Name = "--_br-comp-disclosure-content-max-height";

    public const string Disclosure_Class = "br-disclosure";

    public const string Disclosure_Trigger_Content_Class = $"{Disclosure_Class}__trigger-content";
    public const string Disclosure_Trigger_Class = $"{Disclosure_Class}__trigger";
    public const string Disclosure_Trigger_Modifier_Class = $"{Disclosure_Class}__trigger--no-border";
    public const string Disclosure_Trigger_Expander_Icon_Class = $"{Disclosure_Class}__trigger-expander-icon";
    public const string Disclosure_Icon_Class = $"{Disclosure_Class}__icon";

    public const string Disclosure_Content_Class = $"{Disclosure_Class}__content";
    public const string Disclosure_Content_Modifier_Class = $"{Disclosure_Class}__content--no-border";


}

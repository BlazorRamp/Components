namespace BlazorRamp.ActionsPopover.Common.Constants;

internal class GlobalValues
{
    public const string JS_File_Path = "./_content/BlazorRamp.ActionsPopover/assets/js/actions-popover.js";

    public const string JS_Register_Focus_Out_Handler              = "registerFocusOutHandler";
    public const string JS_Unregister_Focus_Out_Handler            = "unregisterFocusOutHandler";
    public const string JS_Register_Prevent_Click_Action_Handler   = "registerPreventClickAction";
    public const string JS_Unregister_Prevent_Click_Action_Handler = "unregisterPreventClickAction";
    public const string JS_Hide_Popover_Func                       = "hidePopover";

    public const string Actions_Popover_Button_Text_Exception_Message = "ButtonText cannot be null, empty or just whitespace";
    public const string Actions_Popover_Link_Text_Exception_Message   = "LinkText cannot be null, empty or just whitespace";

    public const string Actions_Popover_Panel_Cascading_ID_Name = "ActionsPopoverPanelId";

    public const string Actions_Popover_Trigger_Text = "Actions";

    public const string Actions_Popover_Trigger_Icon_Svg_Css_Variable_Name = "--_br-svg-popover-trigger-icon";
    public const string Actions_Popover_Action_Icon_Svg_Css_Variable_Name  = "--_br-svg-popover-action-icon";

    public const string Actions_Popover_Action_Icon_Colour_Variable_Name  ="--_br-svg-popover-action-icon-colour";

    public const string Actions_Popover_Class                        = "br-actions-popover";
    public const string Actions_Popover_Trigger_Class                = $"{Actions_Popover_Class}__trigger";
    public const string Actions_Popover_Trigger_Text_Class           = $"{Actions_Popover_Class}__trigger-text";
    public const string Actions_Popover_Trigger_Icon_Class           = $"{Actions_Popover_Class}__trigger-icon";
    public const string Actions_Popover_Trigger_Expander_Icon_Class  = $"{Actions_Popover_Class}__trigger-expander-icon";
    public const string Actions_Popover_Panel_Class                  = $"{Actions_Popover_Class}__panel";
    public const string Actions_Popover_Action_Button_Class          = $"{Actions_Popover_Class}__action-button";
    public const string Actions_Popover_Action_Link_Class            = $"{Actions_Popover_Class}__action-link";
    public const string Actions_Popover_Action_Text_Class            = $"{Actions_Popover_Class}__action-text";
    public const string Actions_Popover_Panel_Separator_Class        = $"{Actions_Popover_Class}__panel-separator";
    public const string Actions_Popover_Action_Icon_Slot_Class       = $"{Actions_Popover_Class}__icon-slot";
    public const string Actions_Popover_Action_Icon_Slot_Modifier    = $"{Actions_Popover_Action_Icon_Slot_Class}--with-icon";

}

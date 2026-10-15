namespace BlazorRamp.GoTo.Common.Constants;

internal class GlobalValues
{
    public const string JS_Module_File_Path = "./_content/BlazorRamp.GoTo/assets/js/goto.js";
    public const string JS_ShowPopover_Func = "showGoTo";
    public const string JS_Get_GoToData_Func = "getGoToData";

    public const string JS_Register_ShortCut_CallBack_Func = "registerShortCutCallBack";

    public const string JS_Show_GoTo_Viewer_Func = "showGoToViewer";

    public const string JS_Scroll_Marker_Func = "scrollMarkerIntoView";
    public const string GoTo_Label_Text_Exception_Message = "LabelText cannot be null, empty, or whitespace.";
    public const string GoTo_Block_Class = "br-goto";

    public const string GoTo_Label_Class = $"{GoTo_Block_Class}__label";

    public const string GoTo_Viewer_Block_Class = "br-goto-viewer";

    public const string GoTo_Viewer_Base_Class = $"{GoTo_Viewer_Block_Class}__base";
    public const string GoTo_Viewer_Header_Class = $"{GoTo_Viewer_Block_Class}__header";
    public const string GoTo_Viewer_Content_Class = $"{GoTo_Viewer_Block_Class}__content";

    public const string GoTo_Viewer_Footer_Class = $"{GoTo_Viewer_Block_Class}__footer";

    public const string GoTo_Viewer_Footer_Button_Class = $"{GoTo_Viewer_Block_Class}__footer-button";


    public const string GoTo_Close_Button_Text = "Close";


}

using BlazorRamp.GoTo.Common.Models;
using System.Reflection.Metadata;

namespace BlazorRamp.GoTo.Common.Constants;

internal class GlobalValues
{
    public const string JS_Module_File_Path = "./_content/BlazorRamp.GoTo/assets/js/goto.js";
    public const string JS_ShowPopover_Func = "showGoTo";
    public const string JS_Get_GoToData_Func = "getGoToData";
    public const string JS_Register_GoTo_Component_Func = "registerGoToComponent";
    public const string JS_Show_GoTo_Viewer_Func = "showGoToViewer";
    public const string JS_Close_GoTo_Viewer_Func = "closeGoToViewer";
    public const string JS_Scroll_Marker_Func = "scrollMarkerIntoView";

    public const string JS_Set_Shortcut_key_Func = "setGoToShortcut";
    public const string GoTo_Label_Text_Exception_Message = "LabelText cannot be null, empty, or whitespace.";
    
    public const string GoTo_Block_Class = "br-goto";
    public const string GoTo_Label_Class = $"{GoTo_Block_Class}__label";
    public const string GoTo_Viewer_Block_Class = "br-goto-viewer";

    public const string GoTo_Viewer_Content_Area_Class  = $"{GoTo_Viewer_Block_Class}__content-area";
    public const string GoTo_Viewer_Header_Class        = $"{GoTo_Viewer_Block_Class}__header";
    public const string GoTo_Viewer_Content_Class       = $"{GoTo_Viewer_Block_Class}__content";
    public const string GoTo_Viewer_List_Class          = $"{GoTo_Viewer_Block_Class}__list";
    public const string GoTo_Viewer_List_Item_Class     = $"{GoTo_Viewer_Block_Class}__list-item";
    public const string Goto_Viewer_List_Button         = $"{GoTo_Viewer_Block_Class}__list-button";
    public const string GoTo_Viewer_Footer_Class        = $"{GoTo_Viewer_Block_Class}__footer";
    public const string GoTo_Viewer_Footer_Button_Class = $"{GoTo_Viewer_Block_Class}__footer-button";


    public const string GoTo_Viewer_Trigger_Block_Class = "br-goto-trigger";
    public const string GoTo_Viewer_Trigger_Content_Class = $"{GoTo_Viewer_Trigger_Block_Class}__content";

    public const string GoTo_Heading_Text = "Go to Markers";
    public const string GoTo_Close_Button_Text = "Close";
    public const string GoTo_No_Markers_Text = "No go to markers defined for this page.";

    public const string GoTo_Trigger_Button_Text = "Go to viewer";


    //public static readonly GoToShortcut GoTo_Default_Shortcut_Model = new(Key: "g", Alt: true, Ctrl: false, Shift: false, Meta: false);
    public static readonly GoToShortcut GoTo_Default_Shortcut_Model = new(Key:"y", Alt: false, Ctrl:true, Shift:true, Meta:false);
}

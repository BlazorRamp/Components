namespace BlazorRamp.Tabs.Common.Constants;

internal class GlobalValues
{

    public const string JS_Module_File_Path     = "./_content/BlazorRamp.Tabs/assets/js/tabs.js";
    public const string JS_Register_Tabs_Func   = "registerTabs";
    public const string JS_UnRegister_Tabs_Func = "unregisterTabs";

    public const string Error_Message_Needs_Tabs_Componenent = "A Tab can only exist within a valid Tabs component";
    public const string Error_Message_Tab_Title              = "The tab title cannot be null, empty or whitespace as its the accessible name";

    public const string Tabs_Default_ACC_Name = "Tabs";

    public const string Tabs_Class             = "br-tabs";
    public const string Tabs_Tab_list_Class    = $"{Tabs_Class}__tab-list";
    public const string Tabs_Tab_Class         = $"{Tabs_Class}__tab";
    public const string Tabs_Tab_Title_Class   = $"{Tabs_Class}__tab-title";
    public const string Tabs_Tab_Panel_Class   = $"{Tabs_Class}__tab-panel";
    public const string Tabs_Tab_Icon_Class    = $"{Tabs_Class}__tab-icon";
    public const string Tabs_Tab_Content_Class = $"{Tabs_Class}__tab-content";

    public const string Tabs_Tab_Content_Icon_Top_Mdoifier    =  $"{Tabs_Tab_Content_Class}--icon-top";
    public const string Tabs_Tab_Content_Icon_Right_Mdoifier  = $"{Tabs_Tab_Content_Class}--icon-right";
    public const string Tabs_Tab_Content_Icon_Bottom_Mdoifier = $"{Tabs_Tab_Content_Class}--icon-bottom";
    public const string Tabs_Tab_Content_Icon_Left_Mdoifier   = $"{Tabs_Tab_Content_Class}--icon-left";


    public const string Tabs_Tab_Panel_Active_Modifier = $"{Tabs_Tab_Panel_Class}--active";

    public const string KeyBoard_Left_Arrow_Key  = "ArrowLeft";
    public const string KeyBoard_Right_Arrow_Key = "ArrowRight";

    public const string KeyBoard_Home_Key        = "Home";
    public const string KeyBoard_End_Key         = "End";
    public const string KeyBoard_Tab_Key         = "Tab";


    public const string Tab_Svg_Css_Variable_Name = "--_br-svg-tab-icon";

}

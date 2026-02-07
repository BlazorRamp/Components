namespace BlazorRamp.WebSite.common.Constants;
public static class GlobalValues
{

    public const string JS_Module_File_Path   = "./assets/js/test-component.js";
    public const string JS_Initialise_Func    = "initialise";
    public const string JS_Check_Close_Nav    = "checkCloseSideNavigation";
    public const string JS_Show_Modal_Dialog  = "showModalDialog";
    public const string JS_Close_Modal_Dialog = "closeModalDialog";




    public const string Page_Title_Overview_Introduction = "Introduction";
    public const string Page_Title_Overview_About        = "About Blazor Ramp";

    public const string Page_Title_Busy_Screen           = "Overview & Page Test";
    public const string Page_Title_Busy_Container        = "Busy Container Test";
    public const string Page_Title_Modal_Busy_Screen     = "Modal Busy Page Test";
    public const string Page_Title_Busy_Short            = "Busy With Short Delay Test";
    public const string Page_Title_Final_Words           = "Final Words";
    public const string Page_Title_SkipTo_Overview_Test  = "Overview & Tests";
    public const string Page_Title_Switch_Overview_Test  = "Overview & Tests";

    public const string Path_Overview_Introduction = "/";
    public const string Path_Overview_About = "/about-blazor-ramp";

    public const string Path_SkipTo_Overview = "/skip-to/overview-tests";
    public const string Path_Switch_Overview = "/switch/overview-tests";

    public const string Path_Busy_Screen        = "/busy-indicator/busy-page";
    public const string Path_Busy_Container     = "/busy-indicator/busy-container";
    public const string Path_Busy_Modal_Screen  = "/busy-indicator/modal-busy-page";
    public const string Path_Busy_Short         = "/busy-indicator/busy-short";
    public const string Path_Faqs_Final_Words   = "/final-words";


    public const string Main_Nav_Section_Heading_Overview      = "Overview";
    public const string Main_Nav_Section_Heading_BusyIndicator = "Busy Indicator";
    public const string Main_Nav_Section_Heading_SkipTo        = "Skip To";
    public const string Main_Nav_Section_Heading_Switch        = "Switch";
    public const string Main_Nav_Section_Heading_ModalDialog   = "Modal Dialog Framework";
    public const string Main_Nav_Section_Heading_FAQs = "FAQs";


    public const string Main_Navigation_Aria_Title = "Main Menu";
    public const string Main_Navigation_ID = "main-navigation";

    public const string Main_Navigation_Class = "main-navigation";
    public const string Main_Navigation_Link_Class = $"{Main_Navigation_Class}__link";
    public const string Main_Navigation_Heading_Class = $"{Main_Navigation_Class}__heading";



    public const string Theme_Dark_Text = "The site is now using a dark coloured theme.";
    public const string Theme_Light_Text = "The site is now using a light coloured theme.";


    public const string Info_Box_Class                      = "info-box";
    public const string Info_Box_Heading_Class              = $"{Info_Box_Class}__heading";
    public const string Info_Box_Heading_Primary_Modifier   = $"{Info_Box_Heading_Class}--primary";
    public const string Info_Box_Coloured_Modifier          = $"{Info_Box_Class}--coloured";
    public const string Info_Box_Container_Test_Modifier    = $"{Info_Box_Class}--container-test";

    public const string Button_Test_Class = $"test-button";
    public const string Link_Button_Class = $"link-button";
    public const string Link_Class        = $"link";
    public const string Link_Bot_Modifier = $"{Link_Class}--testingbot";

    public const string Link_Button_Right_Arrow_Modifier = $"{Link_Button_Class}--right-arrow";
    public const string Link_Button_Left_Arrow_Modifier = $"{Link_Button_Class}--left-arrow";

    public const string Modal_Screen_Dialog_Title_ID = "Screen-Dialog-Title-ID";

    public const int Start_Width_For_Collapsed_Menu = 576;
}

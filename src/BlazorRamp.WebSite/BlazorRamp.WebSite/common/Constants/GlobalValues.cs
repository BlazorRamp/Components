using BlazorRamp.WebSite.Pages.Components.AccordionComponent;
using System.Data.Common;
using System.Runtime.InteropServices.JavaScript;
using static System.Net.Mime.MediaTypeNames;

namespace BlazorRamp.WebSite.common.Constants;
public static class GlobalValues
{

    public const string JS_Module_File_Path   = "./assets/js/test-component.js";
    public const string JS_Initialise_Func    = "initialise";
    public const string JS_Check_Close_Nav    = "checkCloseSideNavigation";
    public const string JS_Show_Modal_Dialog  = "showModalDialog";
    public const string JS_Close_Modal_Dialog = "closeModalDialog";


    public const string Main_Navigation_Aria_Title = "Main Menu";
    public const string Main_Navigation_ID = "main-navigation";

    public const string Main_Navigation_Class = "main-navigation";
    public const string Main_Navigation_Link_Class = $"{Main_Navigation_Class}__link";
    public const string Main_Navigation_Heading_Class = $"{Main_Navigation_Class}__heading";


    public const string Main_Nav_Section_Heading_Overview = "Overview";
    public const string Main_Nav_Section_Heading_BusyIndicator = "Busy Indicator";
    public const string Main_Nav_Section_Heading_SkipTo = "Skip To";
    public const string Main_Nav_Section_Heading_Switch = "Switch";
    public const string Main_Nav_Section_Heading_Modal_Dialog = "Modal Dialog";
    public const string Main_Nav_Section_Heading_Toggle_Tip = "Toggletip";
    public const string Main_Nav_Section_Heading_Tabs = "Tabs";
    public const string Main_Nav_Section_Heading_Accordion = "Accordion";
    public const string Main_Nav_Section_Heading_Nav_Group = "Nav Group";
    public const string Main_Nav_Section_Heading_FAQs = "FAQs";

    public const string Main_Nav_Heading_Overview    = "Overview";
    public const string Main_Nav_Heading_Frameworks  = "Frameworks";
    public const string Main_Nav_Heading_Services    = "Services";
    public const string Main_Nav_Heading_Components  = "Components";
    public const string Main_Nav_Heading_Faqs        = "FAQs";

    public const string Root_Path_Overview   = "/";
    public const string Root_Path_Components = "/components";
    public const string Root_Path_Services   = "/services";
    public const string Root_Path_Frameworks = "/frameworks";
    public const string Root_Path_FAQS       = "/faqs";


  
    public const string Common_Page_Path_About           = "about";
    public const string Common_Page_Path_Overview       = "overview";
    public const string Common_Page_Path_Overview_Tests = "overview-tests";
    public const string Common_Page_Path_Quirks_Issues  = "quirks-issues";
    public const string Common_Page_Path_Final_Words    = "final-words";

    public const string Common_Page_Path_Busy_Overview_Page_Test = "overview-page-test";
    public const string Common_Page_Path_Busy_Container_Test     = "container-test";
    public const string Common_Page_Path_Busy_Modal_Page_Test    = "modal-page-test";
    public const string Common_Page_Path_Busy_Short_Delay_Test   = "short-delay-test";

    public const string Common_Page_Title_Introduction        = "Introduction";
    public const string Common_Page_Title_About               = "About Blazor Ramp";
    public const string Common_Page_Title_Overview            = "Overview";
    public const string Common_Page_Title_Overview_Tests      = "Overview & Tests";
    public const string Common_Page_Title_Overview_Page_Tests = "Overview & Page Tests";
    public const string Common_Page_Title_Quirks_Issues       = "Quirks & Issues";
    public const string Common_Page_Title_Final_Words         = "Final Words";

    public const string Common_Page_Title_Modal_Page_Test  = "Modal Page Test";
    public const string Common_Page_Title_Container_Test   = "Container Test";
    public const string Common_Page_Title_Short_Delay_Test = "Short Delay Test";


    public const string Common_Page_Title_FAQ_Questions = "Frequently Asked Questions";



    public const string Component_Name_Accordion       = "Accordion";
    public const string Component_Name_Announcement    = "Announcement History";
    public const string Component_Name_Busy            = "Busy Indicator";
    public const string Component_Name_Debounce_Filter = "Debounce Filter";

    public const string Component_Name_Inputs            = "Inputs";
    public const string Component_Name_Text_Input        = "Text Input";
    public const string Component_Name_TextArea_Input    = "TextArea Input";
    public const string Component_Name_Time_Input        = "Time Input";
    public const string Component_Name_Date_Input        = "Date Input";
    public const string Component_Name_Password_Input    = "Password Input";
    public const string Component_Name_Numeric_Input     = "Numeric Input";
    public const string Component_Name_Checkbox_Input    = "Checkbox Input";
    public const string Component_Name_Radio_Input_Group = "Radio Input Group";
    public const string Component_Name_Select_Input      = "Select Input";
    public const string Component_Name_Errors_Summary    = "Input Errors Summary";

    public const string Component_Name_NavGroup  = "Nav Group";
    public const string Component_Name_Pager     = "Pager";
    public const string Component_Name_SkipTo    = "Skip To";
    public const string Component_Name_Switch    = "Switch";
    public const string Component_Name_Tabs      = "Tabs";
    public const string Component_Name_Toggletip = "Toggletip";


    public const string Component_Path_Part_Accordion       = "accordion";
    public const string Component_Path_Part_Announcement    = "announcement-history";
    public const string Component_Path_Part_Busy            = "busy-indicator";
    public const string Component_Path_Part_DebounceFilter  = "debounce-filter";
    public const string Component_Path_Part_Inputs          = "inputs";
    public const string Component_Path_Part_TextInput       = "text-input";
    public const string Component_Path_Part_TextAreaInput   = "textarea-input";
    public const string Component_Path_Part_TimeInput       = "time-input";
    public const string Component_Path_Part_DateInput       = "date-input";
    public const string Component_Path_Part_PasswordInput   = "password-input";
    public const string Component_Path_Part_NumericInput    = "numeric-input";
    public const string Component_Path_Part_CheckboxInput   = "checkbox-input";
    public const string Component_Path_Part_RadioInputGroup = "radio-input-group";
    public const string Component_Path_Part_SelectInput     = "select-input";
    public const string Component_Path_Part_ErrorsSummary   = "errors-summary";
    public const string Component_Path_Part_Pager           = "pager";
    public const string Component_Path_Part_SkipTo          = "skip-to";
    public const string Component_Path_Part_Switch          = "switch";
    public const string Component_Path_Part_Tabs            = "tabs";
    public const string Component_Path_Part_Toggletip       = "toggletip";
    public const string Component_Path_Part_NavGroup        = "nav-group";

    public const string Framework_Name_Modal = "Modal Dialog";
    public const string Framework_Path_Part_Modal = "modal-dialog";

    public const string Faqs_Name = "FAQ";




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

    public const string Tab_Stop_Indicator_Class = "tab-stop";

    public const int Start_Width_For_Collapsed_Menu = 576;

    public const string Site_Link_Docs_Busy           = "https://docs.blazorramp.uk/components/debounce-filter/overview";
    public const string Site_Link_Docs_DebounceFilter = "https://docs.blazorramp.uk/components/busy-indicator/overview";
    public const string Site_Link_Docs_Switch         = "https://docs.blazorramp.uk/components/switch/overview";
    public const string Site_Link_Docs_SkipTo         = "https://docs.blazorramp.uk/components/skip-to/overview";
    public const string Site_Link_Docs_Pager          = "https://docs.blazorramp.uk/components/pager/overview";
    public const string Site_Link_Docs_Modal_Dialog   = "https://docs.blazorramp.uk/frameworks/modal-dialog/overview";
    public const string Site_Link_Docs_ToggleTip      = "https://docs.blazorramp.uk/components/toggletip/overview";
    public const string Site_Link_Docs_Tabs           = "https://docs.blazorramp.uk/components/tabs/overview";
    public const string Site_Link_Docs_Accordion      = "https://docs.blazorramp.uk/components/accordion/overview";
    public const string Site_Link_Docs_Nav_Group      = "https://docs.blazorramp.uk/components/nav-group/overview";

    public const string Site_Link_Docs_Inputs = "https://docs.blazorramp.uk/components/inputs/overview";
}

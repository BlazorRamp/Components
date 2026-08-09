using BlazorRamp.DocSite.Pages;
using System.Data.Common;

namespace BlazorRamp.DocSite.Common.Constants;

public class GlobalValues
{
    public const string JS_Module_File_Path = "./assets/js/doc-site.js";
    public const string JS_Initialise_Func  = "initialise";
    public const string JS_Check_Close_Nav  = "checkCloseSideNavigation";
    public const string JS_Set_Focus        = "setFocus";


    public const string Info_Box_Class = "info-box";
    public const string Info_Box_Heading_Class = $"{Info_Box_Class}__heading";
    public const string Info_Box_Coloured_Modifier = $"{Info_Box_Class}--coloured";
    public const string Info_Box_Heading_Primary_Modifier = $"{Info_Box_Heading_Class}--primary";

    public const string Main_Navigation_Aria_Title = "Main Menu";
    public const string Main_Navigation_ID = "main-navigation";
    public const string Main_Navigation_Class = "main-navigation";
    public const string Main_Navigation_Link_Class = $"{Main_Navigation_Class}__link";
    public const string Main_Navigation_Heading_Class = $"{Main_Navigation_Class}__heading";
    public const string Main_Navigation_Home_Class = $"{Main_Navigation_Class}__home";


    public const string Site_Banner_Title = "Blazor Ramp Docs";

    public const int Start_Width_For_Collapsed_Menu = 576;

    public const string Main_Nav_Heading_Getting_Started = "Getting Started";
    public const string Main_Nav_Heading_Theming    = "Theming";
    public const string Main_Nav_Heading_Frameworks = "Frameworks";
    public const string Main_Nav_Heading_Services   = "Services";
    public const string Main_Nav_Heading_Components = "Components";
    public const string Main_Nav_Heading_FAQs       = "FAQs";

    public const string Root_path_Getting_Started = "/";
    public const string Root_path_Components = "/components";
    public const string Root_path_Theming = "/theming";
    public const string Root_path_Services = "/services";
    public const string Root_path_Frameworks = "/frameworks";
    public const string Root_path_FAQS = "/faqs";

    public const string Common_Page_Path_Introduction   = "introduction";
    public const string Common_Page_Path_Roadmap        = "roadmap";
    public const string Common_Page_Path_Overview       = "overview";
    public const string Common_Page_Path_Api            = "api";
    public const string Common_Page_Path_Installation   = "installation";
    public const string Common_Page_Path_Accessibility  = "accessibility";
    public const string Common_Page_Path_Usage          = "usage";
    public const string Common_Page_Path_CSS_Variables  = "css-variables";
    public const string Common_Page_Path_Questions      = "questions";

    public const string Common_Page_Path_About          = "about";
    public const string Common_Page_Path_Basic_Usage    = "basic-usage";
    public const string Common_Page_Path_Filtering      = "filtering";
    public const string Common_Page_Path_Selecting      = "selecting";
    public const string Common_Page_Path_Sorting        = "sorting";
    public const string Common_Page_Path_Styling        = "styling";
    public const string Common_Page_Path_Paging         = "paging";
    public const string Common_Page_Path_Templating     = "templating";
    public const string Common_Page_Path_Virtualizing   = "virtualizing";
    public const string Common_Page_Path_Typical_Usage  = "typical-usage";

    public const string Common_Page_Title_Introduction   = "Introduction";
    public const string Common_Page_Title_Roadmap        = "Roadmap";
    public const string Common_Page_Title_Overview       = "Overview";
    public const string Common_Page_Title_Api            = "API Reference";
    public const string Common_Page_Title_Installation   = "Installation";
    public const string Common_Page_Title_Accessibility  = "Accessibility";
    public const string Common_Page_Title_Usage          = "Usage";
    public const string Common_Page_Title_Core_Variables = "Core CSS Variables";
    public const string Common_Page_Title_Shared_CSS     = "Shared CSS Variables";
    public const string Common_Page_Title_FAQ_Questions  = "Frequently Asked Questions";

    public const string Common_Page_Title_Usage_About       = "About The Examples";
    public const string Common_Page_Title_Usage_Basic       = "Basic Usage";
    public const string Common_Page_Title_Usage_Typical     = "Typical Usage";
    public const string Common_Page_Title_Filtering         = "Filtering";
    public const string Common_Page_Title_Selecting         = "Selecting";
    public const string Common_Page_Title_Sorting           = "Sorting";
    public const string Common_Page_Title_Content_Styling   = "Content Styling";
    public const string Common_Page_Title_Templating        = "Templating";
    public const string Common_Page_Title_Paging            = "Paging";
    public const string Common_Page_Title_Virtualizing      = "Virtualizing";


    public const string Component_Name_Accordion          = "Accordion";
    public const string Component_Name_Actions_Popover    = "Actions Popover";
    public const string Component_Name_Announcement       = "Announcement History";
    public const string Component_Name_Busy               = "Busy Indicator";
    public const string Component_Name_Data_Table         = "Data Table";
    public const string Component_Name_Debounce_Filter    = "Debounce Filter";

    public const string Component_Name_Inputs             = "Inputs";
    public const string Component_Name_Text_Input         = "Text Input";
    public const string Component_Name_TextArea_Input     = "TextArea Input";
    public const string Component_Name_Time_Input         = "Time Input";
    public const string Component_Name_Date_Input         = "Date Input";
    public const string Component_Name_Password_Input     = "Password Input";
    public const string Component_Name_Numeric_Input      = "Numeric Input";
    public const string Component_Name_Checkbox_Input     = "Checkbox Input";
    public const string Component_Name_Select_Input       = "Select Input";
    public const string Component_Name_Radio_Input_Group  = "Radio Input Group";

    public const string Component_Name_Input_Errors_Summary = "Input Errors Summary";

    public const string Component_Name_NavGroup     = "Nav Group";
    public const string Component_Name_Pager        = "Pager";
    public const string Component_Name_SkipTo       = "Skip To";
    public const string Component_Name_Switch       = "Switch";
    public const string Component_Name_Tabs         = "Tabs";
    public const string Component_Name_Toggletip    = "Toggletip";

    public const string Component_Path_Part_Accordion       = "accordion";
    public const string Component_Path_Part_ActionsPopover  = "actions-popover";
    public const string Component_Path_Part_Announcement    = "announcement-history";
    public const string Component_Path_Part_Busy            = "busy-indicator";
    public const string Component_Path_Part_DataTable       = "data-table";
    public const string Component_Path_Part_DebounceFilter  = "debounce-filter";
    public const string Component_Path_Part_Inputs          = "inputs";
    public const string Component_Path_Part_TextInput       = "text-input";
    public const string Component_Path_Part_TextAreaInput   = "textarea-input";
    public const string Component_Path_Part_TimeInput       = "time-input";
    public const string Component_Path_Part_DateInput       = "date-input";
    public const string Component_Path_Part_PasswordInput   = "password-input";
    public const string Component_Path_Part_NumericInput    = "numeric-input";
    public const string Component_Path_Part_CheckboxInput   = "checkbox-input";
    public const string Component_Path_Part_SelectInput     = "select-input";
    public const string Component_Path_Part_RadioInputGroup = "radio-input-group";
    public const string Component_Path_Part_InputErrors     = "input-errors-summary";
    public const string Component_Path_Part_SkipTo          = "skip-to";
    public const string Component_Path_Part_Pager           = "pager";
    public const string Component_Path_Part_Switch          = "switch";
    public const string Component_Path_Part_Tabs            = "tabs";
    public const string Component_Path_Part_Toggletip       = "toggletip";
    public const string Component_Path_Part_NavGroup        = "nav-group";
    public const string Component_Path_Part_Usage           = "usage";


    public const string Service_Name_Live_Region        = "Live Region";
    public const string Service_Path_Part_Live_Region   = "live-region";

    public const string Framework_Name_Modal         = "Modal Dialog";
    public const string Framework_Path_Part_Modal   = "modal-dialog";

    public const string Getting_Started_Name = "Getting Started";
    public const string Theming_Name = "Theming";
    public const string Faqs_Name    = "FAQ";

    public const string Link_Button_Class = $"link-button";
    public const string Link_Class = $"link";
    public const string Link_Bot_Modifier = $"{Link_Class}--testingbot";

    public const string Link_Button_Right_Arrow_Modifier = $"{Link_Button_Class}--right-arrow";
    public const string Link_Button_Left_Arrow_Modifier = $"{Link_Button_Class}--left-arrow";

    public const string Tab_Stop_Indicator_Class = "tab-stop";

    public const string Web_Site_Path_Component_Test = "https://blazorramp.uk";
    public const string Web_Site_Path_GitHub_Repo = "https://github.com/BlazorRamp/Components";


    public const string Validated_Repo_Site = "https://github.com/code-dispenser/Validated";
    public const string Validated_Blazor_Repo_Site = "https://github.com/code-dispenser/Validated-Blazor";

    public const string Test_Component_Path_ToggleTip = $"{Web_Site_Path_Component_Test}/toggle-tip/overview-tests";


    public const string JAWS_Issues_Link_FS     = "https://github.com/FreedomScientific/standards-support/issues/959";
    public const string JAWS_Issues_Link_Blazor = "https://github.com/dotnet/aspnetcore/issues/68157";

}

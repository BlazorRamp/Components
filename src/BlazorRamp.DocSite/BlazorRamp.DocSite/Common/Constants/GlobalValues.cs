using BlazorRamp.DocSite.Pages;
using System.Data.Common;
using System.Drawing;

namespace BlazorRamp.DocSite.Common.Constants;

public class GlobalValues
{
    public const string JS_Doc_Site_Module_File_Path = "./assets/js/doc-site.js";
    public const string JS_Doc_Theme_Module_File_Path = "./assets/js/doc-themes.js";
    public const string JS_Doc_Site_Initialise_Func  = "initialise";
    public const string JS_Doc_Site_Check_Close_Nav  = "checkCloseSideNavigation";
    public const string JS_Doc_Site_Set_Focus        = "setFocus";

    public const string JS_Theme_Get_Comp_Style_Property_Func = "getComputedStyleProperty";//variable name
    public const string JS_Theme_Get_Resolved_Colour_Func     = "getResolvedHexColourValue";//variable name
    public const string JS_Theme_Set_Style_Property_Func      = "setStyleProperty";// name, value
    public const string JS_Theme_Set_Element_Variable_Func    = "setElementVariable";// id, name, value, remove
    public const string JS_Theme_Remove_Style_Property_Func   = "removeStyleProperty";

    public const string JS_Theme_Get_Comp_Style_Properties_Func = "getComputedStyleProperties"; //takes and returns List<CssProperty>

    public const string JS_Theme_Get_Raw_Properties_From_Stylesheet = "getRawPropertiesFromStylesheet";

    public const string JS_Theme_Apply_Opacity_To_Hex_Value = "applyOpacityToHex";//(foregroundHex: string, opacityValue: number = 1, backgroundHex: string = '#ffffff'):

    public const string Stylesheet_Name = "main.min.css";
    public const string Stylesheet_Dark_Theme_Selector = ":root:has(#theme-toggler[aria-checked=\"true\"])";

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
    public const string Common_Page_Path_Sandbox        = "sandbox";
    public const string Common_Page_Path_Colours        = "colours";

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
    public const string Common_Page_Title_Colours       = "Colours";
    public const string Common_Page_Title_Sandbox        = "Sandbox";
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



    public const string Colour_Palette_Class              = $"br-colour-palette";
    public const string Colour_Palette_Heading_Class      = $"{Colour_Palette_Class}__heading";
    public const string Colour_Palette_Information_Class  = $"{Colour_Palette_Class}__information";
    public const string Colour_Palette_Swatch_Class       = $"{Colour_Palette_Class}__swatch";
    public const string Colour_Palette_Swatch_Name_Class  = $"{Colour_Palette_Class}__swatch-name";
    public const string Colour_Palette_Black_Swatch_Modifier = $"{Colour_Palette_Swatch_Class}--black";

    public const string Colour_Swatch_Text_Colour_Var_Name = "--_br-swatch-text-colour";
    public const string Colour_Swatch_Background_Var_Name  = "--_br-swatch-background";



    public const string Theme_Colour_Tool_Class          = "theme-colour-tool";
    public const string Theme_Colour_Tool_Title_Class    = $"{Theme_Colour_Tool_Class}__title";
    public const string Theme_Colour_Tool_Label_Class    = $"{Theme_Colour_Tool_Class}__label";
    public const string Theme_Colour_Tool_Content_Class  = $"{Theme_Colour_Tool_Class}__content";
    public const string Theme_Colour_Tool_Controls_Class = $"{Theme_Colour_Tool_Class}__controls";
    public const string Theme_Colour_Tool_Swatch_Class   = $"{Theme_Colour_Tool_Class}__swatch";
    public const string Theme_Colour_Tool_Output_Class   = $"{Theme_Colour_Tool_Class}__output";
    public const string Theme_Colour_Tool_Button_Class = $"{Theme_Colour_Tool_Class}__button";

    public const string Regex_Hex_Colour_Pattern = "^#?[a-fA-F0-9]{6}$";
    public const string Regex_Colour_Key_Pattern = "^[#a-fA-F0-9]$";
    public const string Regex_Hex_Replace_Pattern = "[^a-fA-F0-9]";
    public const string Incorrect_Hex_Value_Exception_Message = "Invalid hex value. The value should only contain a-f, A-F and 0-9 characters, optionally starting with the pound # symbol";




    public const string Theme_Border_Tool_Class = $"theme-border-tool";

    public const string Theme_Border_Tool_Title_Class           = $"{Theme_Border_Tool_Class}__title";
    public const string Theme_Border_Tool_Group_Info_Class      = $"{Theme_Border_Tool_Class}__group-info";
    public const string Theme_Border_Tool_Border_Group_Class    = $"{Theme_Border_Tool_Class}__border-group";
    public const string Theme_Border_Tool_Control_Group_Class   = $"{Theme_Border_Tool_Class}__control-group";
    public const string Theme_Border_Tool_Label_Class           =  $"{Theme_Border_Tool_Class}__label";
    public const string Theme_Border_Tool_Content_Class         = $"{Theme_Border_Tool_Class}__content";

    public const string Theme_Border_Tool_Buttons_Class         = $"{Theme_Border_Tool_Class}__tool-buttons";



    public const int Radius_Button_Max_Range_Value             = 10;
    public const int Radius_Content_Max_Range_Value            = 9;
    public const int Radius_Input_Max_Range_Value              = 7;
    public const int Radius_Input_Field_Max_Range_Value        = 5;
    public const int Radius_Reveal_Max_Range_Value             = 9;
    public const int Radius_Container_Max_Range_Value          = 7;
    public const int Radius_Container_Header_Max_Range_Value   = 7;
    public const int Radius_Dialog_Surface_Max_Range_Value     = 9;
    public const int Radius_Dialog_Content_Max_Range_Value     = 9;
    public const int Radius_Menu_Trigger_Panel_Max_Range_Value = 7;
    public const int Radius_Menu_Item_Max_Range_Value          = 7;



    public const string Theme_Dialog_Class                 = "theme-dialog";
    public const string Theme_Dialog_Title_Class           = $"{Theme_Dialog_Class}__title";
    public const string Theme_Dialog_Title_Tools_Class     = $"{Theme_Dialog_Class}__tools";
    public const string Theme_Dialog_Content_Heading_Class = $"{Theme_Dialog_Class}__content-heading";
    public const string Theme_Dialog_Content_Class         = $"{Theme_Dialog_Class}__content";
    public const string Theme_Dialog_Scroll_Region_Class   = $"{Theme_Dialog_Class}__scroll-region";
    public const string Theme_Dialog_Footer_Class          = $"{Theme_Dialog_Class}__footer";
    public const string Theme_Dialog_Icon_Class            = $"{Theme_Dialog_Class}__icon";
    public const string Theme_Dialog_Copied_Icon_Class     = $"{Theme_Dialog_Class}__copied-icon";


}

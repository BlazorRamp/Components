using BlazorRamp.DocSite.Pages;

namespace BlazorRamp.DocSite.Common.Constants;

public class GlobalValues
{
    public const string JS_Module_File_Path   = "./assets/js/doc-site.js";
    public const string JS_Initialise_Func    = "initialise";
    public const string JS_Check_Close_Nav    = "checkCloseSideNavigation";


    public const string Info_Box_Class                      = "info-box";
    public const string Info_Box_Heading_Class              = $"{Info_Box_Class}__heading";
    public const string Info_Box_Coloured_Modifier          = $"{Info_Box_Class}--coloured";
    public const string Info_Box_Heading_Primary_Modifier   = $"{Info_Box_Heading_Class}--primary";

    public const string Main_Navigation_Aria_Title    = "Main Menu";
    public const string Main_Navigation_ID            = "main-navigation";
    public const string Main_Navigation_Class         = "main-navigation";
    public const string Main_Navigation_Link_Class    = $"{Main_Navigation_Class}__link";
    public const string Main_Navigation_Heading_Class = $"{Main_Navigation_Class}__heading";
    public const string Main_Navigation_Home_Class = $"{Main_Navigation_Class}__home";


    public const string Site_Banner_Title = "Blazor Ramp Docs";

    public const int Start_Width_For_Collapsed_Menu = 576;

      
    public const string Path_Overview_Introduction           = "/";
    public const string Path_Overview_Getting_Started        = "/getting-started";
    public const string Path_Overview_Accessibility          = "accessibility";
    public const string Path_Overview_Roadmap                = "/roadmap";
    public const string Path_Services_Live_Region            = "/services/live-region";
    public const string Path_Frameworks_Modal_Dialog         = "/frameworks/modal-dialog";
    public const string Path_Components_Announcement_History = "/components/announcement-history";
    public const string Path_Components_Busy_Indicator       = "/components/busy-indicator";
    public const string Path_Theming_Overview                = "/theming/overview";

    public const string Page_Title_Overview_Introduction            = "Overview - Introduction";
    public const string Page_Title_Overview_Getting_Started         = "Overview - Getting Started";
    public const string Page_Title_Overview_Accessibility           = "Overview - Accessibility";
    public const string Page_Title_Overview_Roadmap                 = "Overview - Roadmap";
    public const string Page_Title_Services_Live_Region             = "Services - Live Regions";
    public const string Page_Title_Frameworks_Modal_Dialog          = "Frameworks - Modal Dialog";
    public const string Page_Title_Components_Announcement_History  = "Components - Announcement History";
    public const string Page_Title_Components_Busy_Indicator        = "Components - Busy Indicator";
    
    public const string Page_Title_Theming_Overview                 = "Theming - Overview";


    public const string Page_Heading_Overview_Introduction           = "Introduction";
    public const string Page_Heading_Overview_Getting_Started        = "Getting Started";
    public const string Page_Heading_Overview_Accessibility          = "Accessibility";
    public const string Page_Heading_Overview_Roadmap                = "Roadmap";

    public const string Page_Heading_Theming_Overview                = "Overview";

    public const string Page_Heading_Services_Live_Region            = "Live Region Service";
    public const string Page_Heading_Frameworks_Modal_Dialog         = "Modal Dialog Framework";
    public const string Page_Heading_Components_Announcement_History = "Announcement History";
    public const string Page_Heading_Components_Busy_Indicator       = "Busy Indicator";

    public const string Main_Nav_Section_Heading_About      = "Overview";
    public const string Main_Nav_Section_Heading_Theming    = "Theming";
    public const string Main_Nav_Section_Heading_Frameworks = "Frameworks";
    public const string Main_Nav_Section_Heading_Services   = "Services";
    public const string Main_Nav_Section_Heading_Components = "Components";
}

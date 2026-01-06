namespace BlazorRamp.Core.Common.Constants;

internal static class CoreGlobalValues
{
    public const string JS_Utils_File_Path          = "./_content/BlazorRamp.Core/assets/js/core-utilities.js";
    public const string JS_Utils_Screen_Width_Func  = "getScreenInnerWidth";

    public const string JS_Live_Region_File_Path                = "./_content/BlazorRamp.Core/assets/js/core-live-region.js";
    public const string JS_Live_Region_Announce_Func            = "announcement";
    public const string JS_Live_Region_Check_Close_Popover_Func = "closePopoverOnLocationChanged";


    public const string Blazor_Ramp_ID             = "blazor-ramp";
    public const string Live_Regions_ID            = $"{Blazor_Ramp_ID}-live-regions";
    public const string Live_Region_Assertive_One_ID = $"{Blazor_Ramp_ID}-live-region-assertive_one";
    public const string Live_Region_Polite_One_ID    = $"{Blazor_Ramp_ID}-live-region-polite_one";

    public const string Live_Region_Assertive_Two_ID = $"{Blazor_Ramp_ID}-live-region-assertive_two";
    public const string Live_Region_Polite_Two_ID    = $"{Blazor_Ramp_ID}-live-region-polite_two";

    public const string Component_Name = "Announcement History";

    public const int Live_Region_Delay_MS =      1200;

    public const string CSS_Visually_Hidden_Class = "br-visually-hidden";

    public const string Style_As_Dark = "dark";
    public const string Style_As_Light = "light";

    public const string Live_Regions_Class    = "br-visually-hidden";

    public const string AH_ID                 = $"{Blazor_Ramp_ID}-announcement-history";
    public const string AH_Components_ID      = $"{AH_ID}-components";
    public const string AH_Title_ID           = $"{AH_ID}-title";
    public const string AH_Dialog_ID          = $"{AH_ID}-dialog";
    public const string AH_Content_ID         = $"{AH_ID}-content";
    public const string AH_Close_Button_ID    = $"{AH_ID}-close";
    public const string AH_Clear_Button_ID    = $"{AH_ID}-clear";
    public const string AH_Refresh_Button_ID  = $"{AH_ID}-refresh";
    public const string AH_Trigger_Button_ID  = $"{AH_ID}-trigger";

    public const string AH_Class              = "br-announcement-history";
    public const string AH_Popover_Class      = $"{AH_Class}__popover";
    public const string AH_Button_Class       = $"{AH_Class}__button";
    public const string AH_Content_Area_Class = $"{AH_Class}__content-area";
    public const string AH_Content_Class      = $"{AH_Class}__content";
    public const string AH_Header_Class       = $"{AH_Class}__header";
    public const string AH_Ol_list_Class      = $"{AH_Class}__numbered-list";
    public const string AH_Footer_Class       = $"{AH_Class}__footer";
    public const string AH_Trigger_Class      = $"{AH_Class}__trigger";
    public const string AH_Trigger_Modifier   = $"{AH_Trigger_Class}--only-focus-visible";
    public const string AH_Icon_Class         = $"{AH_Class}__icon";

    public const string AH_Text_For_Refresh_Btn = "Refresh";
    public const string AH_Text_For_Close_Btn   = "Close";
    public const string AH_Text_For_Clear_Btn   = "Clear & Close";
    public const string AH_Text_For_Heading     = "Recent alerts";
    public const string AH_Text_No_Content      = "No alerts";
    public const string AH_Text_For_Trigger_Btn = "Alerts";

}

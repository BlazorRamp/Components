namespace BlazorRamp.BusyIndicator.Common.Constants;

internal static class GlobalValues
{
    public const string JS_File_Path                = "./_content/BlazorRamp.BusyIndicator/assets/js/busy-indicator.js";
    public const string JS_Start_Busy_Indicator     = "startBusyIndicator";
    public const string JS_Stop_Busy_Indicator      = "stopBusyIndicator";

    public const string Argument_Null_Empty_Exception_Message = "The argument cannot be null or empty.";


    public const string Busy_Class         = "br-busy-indicator";
    public const string Busy_Content_Class = $"{Busy_Class}__content";
    public const string Busy_Spinner_Class = $"{Busy_Class}__spinner";
    public const string Busy_Text_Class    = $"{Busy_Class}__text";

    public const string Busy_Fix_At_Container_Modifier  = $"{Busy_Class}--unfixed";
    public const string Busy_Centred_Modifier           = $"{Busy_Class}--centred";
    public const string Busy_Display_Modifier           = $"{Busy_Class}--display-flex";

    public const int Busy_Indicator_Timeout_MS = 30_000;

    public const string Busy_Indicator_Start_Text  = "Operation Started";
    public const string Busy_Indicator_End_Text    = "Operation Completed";
    public const string Busy_Indicator_Label       = "Status";

    public const string Component_Name  = "Busy Indicator";

    public const int Live_Region_Buffer_MS = 50;

}


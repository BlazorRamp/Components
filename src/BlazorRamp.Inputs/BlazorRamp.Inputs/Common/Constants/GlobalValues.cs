namespace BlazorRamp.Inputs.Common.Constants;

internal class GlobalValues
{

    public const string JS_Inputs_File_Path                        = "./_content/BlazorRamp.Inputs/assets/js/inputs.js";
    public const string JS_Inputs_Register_Aria_Disabled_Handlers   = "registerAriaDisabledHandlers";
    public const string JS_Inputs_Unregister_Aria_Disabled_Handlers = "unregisterAriaDisabledHandlers";

    public const string JS_Inputs_Register_Readonly_Handlers = "registerReadOnlyHandlers";
    public const string JS_Inputs_Unregister_Readonly_Handlers = "unregisterReadOnlyHandlers";

    public const string JS_Inputs_Register_Select_Readonly_Disabled_Handlers = "registerSelectReadOnlyDisabledHandlers";
    public const string JS_Inputs_Unregister_Select_Readonly_Disabled_Handlers = "unregisterSelectReadOnlyDisabledHandlers";

    public const string JS_Inputs_Register_Numeric_Handlers   = "registerNumericHandlers";
    public const string JS_Inputs_Unregister_Numeric_Handlers = "unregisterNumericHandlers";

    public const string JS_Inputs_Register_Time_Segment_Handlers   = "registerTimeSegmentHandlers";
    public const string JS_Inputs_Unregister_Time_Segment_Handlers = "unregisterTimeSegmentHandlers";

    public const string JS_Inputs_Register_Date_Segment_Handlers   = "registerDateSegmentHandlers";
    public const string JS_Inputs_Unregister_Date_Segment_Handlers = "unregisterDateSegmentHandlers";

    public const string JS_Inputs_Register_Focus_Out_Callback   = "registerElementFocusOutHandler";
    public const string JS_Inputs_Unregister_Focus_Out_Callback = "unregisterElementFocusOutHandler";

    public const string JS_Inputs_Format_Date_For_Announcement_Func = "formatDateForAnnouncement";

    public const string JS_Inputs_Set_Value         = "setInputValue";
    public const string JS_Inputs_Set_Input_Focus   = "setInputFocus";
    public const string JS_Inputs_Set_Summary_Focus  = "setSummaryFocus";

    public const string Input_Parse_Number_Error_Message  = "Invalid number.";
    public const string Input_Parse_General_Error_Message = "Invalid entry.";

    public const string Input_Parse_time_Error_Message = "Invalid time.";
    public const string Input_Parse_Date_Error_Message = "Invalid date.";

    public const string Input_Time_DataType_Error_Message = "Only the data type TimeOnly or nullable TimeOnly is supported";
    public const string Input_Date_DataType_Error_Message = "Only the data type DateOnly or nullable DateOnly is supported";


    public const string Input_Missing_Label_Text_Error_Message = "Label Text cannot be null, empty or whitespace";
    public const string Input_Missing_Radio_Group_Parent_Error_Message = "The radio input must be used inside a radio input group";


    public const string Input_Svg_Css_Variable_Name = "--_br-svg-input-icon";

    public const string Text_Input_Class                = "br-text-input";
    public const string Text_Input_Label_Class          = $"{Text_Input_Class}__label";
    public const string Text_Input_Hint_Class           = $"{Text_Input_Class}__hint";
    public const string Text_Input_Error_Wrapper_Class  = $"{Text_Input_Class}__error-wrapper";
    public const string Text_Input_Error_Class          = $"{Text_Input_Class}__error";
    public const string Text_Input_Asterisk_Class       = $"{Text_Input_Class}__asterisk";
    public const string Text_Input_Field_Class          = $"{Text_Input_Class}__field";
    public const string Text_Input_Icon_Class           = $"{Text_Input_Class}__icon";
    public const string Text_Input_State_Icon_Class     = $"{Text_Input_Class}__state-icon";


    public const string TextArea_Input_Class                = "br-textarea-input";
    public const string TextArea_Input_Label_Class          = $"{TextArea_Input_Class}__label";
    public const string TextArea_Input_Hint_Class           = $"{TextArea_Input_Class}__hint";
    public const string TextArea_Input_Error_Wrapper_Class  = $"{TextArea_Input_Class}__error-wrapper";
    public const string TextArea_Input_Error_Class          = $"{TextArea_Input_Class}__error";
    public const string TextArea_Input_Asterisk_Class       = $"{TextArea_Input_Class}__asterisk";
    public const string TextArea_Input_Field_Area_Class     = $"{TextArea_Input_Class}__field-area";
    public const string TextArea_Input_Field_Class          = $"{TextArea_Input_Class}__field";
    public const string TextArea_Input_Field_Counter_Class  = $"{TextArea_Input_Class}__field-counter";
    public const string TextArea_Input_Icon_Class           = $"{TextArea_Input_Class}__icon";
    public const string TextArea_Input_State_Icon_Class     = $"{TextArea_Input_Class}__state-icon";


    public const string Password_Input_Class                = "br-password-input";
    public const string Password_Input_Label_Class          = $"{Password_Input_Class}__label";
    public const string Password_Input_Hint_Class           = $"{Password_Input_Class}__hint";
    public const string Password_Input_Error_Wrapper_Class  = $"{Password_Input_Class}__error-wrapper";
    public const string Password_Input_Error_Class          = $"{Password_Input_Class}__error";
    public const string Password_Input_Asterisk_Class       = $"{Password_Input_Class}__asterisk";
    public const string Password_Input_Field_Class          = $"{Password_Input_Class}__field";
    public const string Password_Input_Show_Password_Class  = $"{Password_Input_Class}__show-password";
    public const string Password_Input_Icon_Class           = $"{Password_Input_Class}__icon";
    public const string Password_Input_State_Icon_Class     = $"{Password_Input_Class}__state-icon";

    public const string Password_Input_Show_Password_Text    = "Show Password";

    public const string Numeric_Input_Class                 = "br-numeric-input";
    public const string Numeric_Input_Label_Class           = $"{Numeric_Input_Class}__label";
    public const string Numeric_Input_Hint_Class            = $"{Numeric_Input_Class}__hint";
    public const string Numeric_Input_Error_Wrapper_Class   = $"{Numeric_Input_Class}__error-wrapper";
    public const string Numeric_Input_Error_Class           = $"{Numeric_Input_Class}__error";
    public const string Numeric_Input_Asterisk_Class        = $"{Numeric_Input_Class}__asterisk";
    public const string Numeric_Input_Field_Class           = $"{Numeric_Input_Class}__field";
    public const string Numeric_Input_Icon_Class            = $"{Numeric_Input_Class}__icon";
    public const string Numeric_Input_State_Icon_Class      = $"{Numeric_Input_Class}__state-icon";


    public const string Checkbox_Input_Class                = "br-checkbox-input";
    public const string Checkbox_Input_Label_Class          = $"{Checkbox_Input_Class}__label";
    public const string Checkbox_Input_Hint_Class           = $"{Checkbox_Input_Class}__hint";
    public const string Checkbox_Input_Error_Wrapper_Class  = $"{Checkbox_Input_Class}__error-wrapper";
    public const string Checkbox_Input_Error_Class          = $"{Checkbox_Input_Class}__error";
    public const string Checkbox_Input_Asterisk_Class       = $"{Checkbox_Input_Class}__asterisk";
    public const string Checkbox_Input_Field_Class          = $"{Checkbox_Input_Class}__field";
    public const string Checkbox_Input_Icon_Class           = $"{Checkbox_Input_Class}__icon";
    public const string Checkbox_Input_State_Icon_Class     = $"{Checkbox_Input_Class}__state-icon";


    public const string Select_Input_Class               = "br-select-input";
    public const string Select_Input_Label_Class         = $"{Select_Input_Class}__label";
    public const string Select_Input_Hint_Class          = $"{Select_Input_Class}__hint";
    public const string Select_Input_Error_Wrapper_Class = $"{Select_Input_Class}__error-wrapper";
    public const string Select_Input_Error_Class         = $"{Select_Input_Class}__error";
    public const string Select_Input_Asterisk_Class      = $"{Select_Input_Class}__asterisk";
    public const string Select_Input_Field_Class         = $"{Select_Input_Class}__field";
    public const string Select_Input_Icon_Class          = $"{Select_Input_Class}__icon";
    public const string Select_Input_State_Icon_Class    = $"{Select_Input_Class}__state-icon";



    public const string Radio_Input_Group_Class               = "br-radio-input-group";
    public const string Radio_Input_Group_Label_Class         = $"{Radio_Input_Group_Class}__label";
    public const string Radio_Input_Group_Hint_Class          = $"{Radio_Input_Group_Class}__hint";
    public const string Radio_Input_Group_Field_Area_Class    = $"{Radio_Input_Group_Class}__field-area";
    public const string Radio_Input_Group_Asterisk_Class      = $"{Radio_Input_Group_Class}__asterisk";
    public const string Radio_Input_Group_Error_Wrapper_Class = $"{Radio_Input_Group_Class}__error-wrapper";
    public const string Radio_Input_Group_Error_Class         = $"{Radio_Input_Group_Class}__error";
    public const string Radio_Input_Group_Icon_Class          = $"{Radio_Input_Group_Class}__icon";
    public const string Radio_Input_Group_State_Icon_Class    = $"{Radio_Input_Group_Class}__state-icon";

    public const string Radio_Input_Group_Field_Area_Modifier = $"{Radio_Input_Group_Field_Area_Class}--vertical";

    public const string Radio_Input_Class       = "br-radio-input";
    public const string Radio_Input_Label_Class = $"{Radio_Input_Class}__label";
    public const string Radio_Input_field_Class = $"{Radio_Input_Class}__field";

    public const string Radio_Input_Group_Cascade_Value_Name = "RadioGroupName";


    public const string Time_Input_Class                = "br-time-input";
    public const string Time_Input_Label_Class          = $"{Time_Input_Class}__label";
    public const string Time_Input_Hint_Class           = $"{Time_Input_Class}__hint";
    public const string Time_Input_Error_Wrapper_Class  = $"{Time_Input_Class}__error-wrapper";
    public const string Time_Input_Error_Class          = $"{Time_Input_Class}__error";
    public const string Time_Input_Asterisk_Class       = $"{Time_Input_Class}__asterisk";
    public const string Time_Input_Field_Area_Class     = $"{Time_Input_Class}__field-area";

    public const string Time_Input_Segment_Class         = $"{Time_Input_Class}__segment";
    public const string Time_Input_Segment_Wrapper_Class = $"{Time_Input_Class}__segment-wrapper";
    public const string Time_Input_Segment_Label_Class   = $"{Time_Input_Class}__segment-label";

    public const string Time_Input_Field_Class          = $"{Time_Input_Class}__field";
    public const string Time_Input_Icon_Class           = $"{Time_Input_Class}__icon";
    public const string Time_Input_State_Icon_Class     = $"{Time_Input_Class}__state-icon";

    public const string Time_Input_Hours_Text   = "Hours";
    public const string Time_Input_Minutes_Text = "Minutes";
    public const string Time_Input_Seconds_Text = "Seconds";




    public const string Date_Input_Class                 = "br-date-input";
    public const string Date_Input_Label_Class           = $"{Date_Input_Class}__label";
    public const string Date_Input_Hint_Class            = $"{Date_Input_Class}__hint";
    public const string Date_Input_Error_Wrapper_Class   = $"{Date_Input_Class}__error-wrapper";
    public const string Date_Input_Error_Class           = $"{Date_Input_Class}__error";
    public const string Date_Input_Asterisk_Class        = $"{Date_Input_Class}__asterisk";
    public const string Date_Input_Field_Area_Class      = $"{Date_Input_Class}__field-area";

    public const string Date_Input_Segment_Class         = $"{Date_Input_Class}__segment";
    public const string Date_Input_Segment_Wrapper_Class = $"{Date_Input_Class}__segment-wrapper";
    public const string Date_Input_Segment_Label_Class   = $"{Date_Input_Class}__segment-label";

    public const string Date_Input_Field_Class           = $"{Date_Input_Class}__field";
    public const string Date_Input_Icon_Class            = $"{Date_Input_Class}__icon";
    public const string Date_Input_State_Icon_Class      = $"{Date_Input_Class}__state-icon";

    public const string Date_Input_Years_Text            = "Years";
    public const string Date_Input_Months_Text           = "Months";
    public const string Date_Input_Days_Text             = "Days";














    public const string Input_Errors_Summary_Class = "br-input-errors-summary";

    public const string Input_Errors_Summary_No_Errors_Modifier = $"{Input_Errors_Summary_Class}--no-errors";

    public const string Input_Errors_Summary_Heading     = $"{Input_Errors_Summary_Class}__heading";
    public const string Input_Errors_Summary_Sub_Heading = $"{Input_Errors_Summary_Class}__sub-heading";
    public const string Input_Errors_Summary_List        = $"{Input_Errors_Summary_Class}__list";
    public const string Input_Errors_Summary_List_Item   = $"{Input_Errors_Summary_Class}__list-item";
    public const string Input_Errors_Summary_Link        = $"{Input_Errors_Summary_Class}__link";

    public const string Input_Errors_Summary_Title        = "There is a problem with your entries";
    public const string Input_Errors_Summary_Input_Suffix = "field";

    public const string Default_Errors_label = "Errors";

}

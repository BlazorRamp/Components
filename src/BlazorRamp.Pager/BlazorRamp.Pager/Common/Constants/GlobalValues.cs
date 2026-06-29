namespace BlazorRamp.Pager.Common.Constants;

internal class GlobalValues
{
    public const string Pager_No_Records_Text         = "No entries found.";
    public const string Pager_Query_String_Param_Name = "page";

    public const string Pager_Aria_Label = "Results Pager";

    public const string Pager_Selector_Next_Text  = "Next";
    public const string Pager_Selector_Prev_Text  = "Previous";
    public const string Pager_Selector_First_Text = "First";
    public const string Pager_Selector_Last_Text  = "Last";

    public const string Pager_Class           = "br-pager";

    public const string Pager_Align_Start_Modifier  = $"{Pager_Class}--start";
    public const string Pager_Align_End_Modifier    = $"{Pager_Class}--end";

    public const string Pager_Items_Class     = $"{Pager_Class}__items";

    public const string Pager_Button_Class    = $"{Pager_Class}__button";
    public const string Pager_Link_Class      = $"{Pager_Class}__link";
    public const string Pager_Icon_Class      = $"{Pager_Class}__icon";

    public const string Pager_Button_First_Modifier    = $"{Pager_Button_Class}--first";
    public const string Pager_Button_Previous_Modifier = $"{Pager_Button_Class}--previous";
    public const string Pager_Button_Next_Modifier     = $"{Pager_Button_Class}--next";
    public const string Pager_Button_Last_Modifier     = $"{Pager_Button_Class}--last";

    public const string Pager_Link_First_Modifier    = $"{Pager_Link_Class}--first";
    public const string Pager_Link_Previous_Modifier = $"{Pager_Link_Class}--previous";
    public const string Pager_Link_Next_Modifier     = $"{Pager_Link_Class}--next";
    public const string Pager_Link_Last_Modifier     = $"{Pager_Link_Class}--last";


    public const string Pager_Icon_Next_Modifier        = $"{Pager_Icon_Class}--next";
    public const string Pager_Icon_Prev_Modifier        = $"{Pager_Icon_Class}--prev";
    public const string Pager_Icon_First_Modifier       = $"{Pager_Icon_Class}--first";
    public const string Pager_Icon_Last_Modifier        = $"{Pager_Icon_Class}--last";

    public const string Pager_Selector_Text_Class = $"{Pager_Class}__selector-text";
    public const string Pager_Information_Class   = $"{Pager_Class}__information";


    public const string Pager_Count_Text        = "Current page {firstpage} of {lastpage}, items {startrow} to {endrow}";
    public const string Pager_Filter_Count_Text = ", {filteredrows} items filtered from {totalrows}.";
}

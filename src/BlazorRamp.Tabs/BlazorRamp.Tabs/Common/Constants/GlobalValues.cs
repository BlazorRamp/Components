using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.Tabs.Common.Constants;

internal class GlobalValues
{
    public const string Error_Message_Needs_Tabs_Componenent = "A Tab can only exist within a valid Tabs component";
    public const string Error_Message_Tab_Title              = "The tab title cannot be null, empty or whitespace as its the accessible name";


    public const string Tabs_Class           = "br-tabs";
    public const string Tabs_Tab_list_Class  = $"{Tabs_Class}__tab-list";
    public const string Tabs_Tab_Class       = $"{Tabs_Class}__tab";
    public const string Tabs_Tab_Title_Class = $"{Tabs_Class}__tab-title";
    public const string Tabs_Tab_Panel_Class = $"{Tabs_Class}__tab-panel";

    public const string Tabs_Tab_Panel_Active_Modifier = $"{Tabs_Tab_Panel_Class}--active";



}

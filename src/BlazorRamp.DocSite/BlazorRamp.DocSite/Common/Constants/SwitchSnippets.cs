namespace BlazorRamp.DocSite.Common.Constants;

public class SwitchSnippets
{
    public const string Add_Switch_Style_Sheet = """
        <head>
            <link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
            <link rel="stylesheet" href="_content/BlazorRamp.Switch/assets/css/switch.min.css" />
        </head>
        """;


    public const string Switch_Two_Way_Bind_Example = """
        <Switch @bind-SwitchState="@_switchState" Label="Airplane mode:" AriaDisabled="@_switchDisabled" SpaceBetween="false" />

        @code {

            private bool _switchState    = false;
            private bool _switchDisabled = false;
        }
        """;

    public const string Switch_One_Way_Bind_Event_Example = """
        <Switch SwitchState="@_switchState" Label="Airplane mode:" AriaDisabled="@_switchDisabled" SpaceBetween="false" 
        SwitchStateChanged="HandleSwitchChange" />

        @code {

            private bool _switchState    = false;
            private bool _switchDisabled = false;

            private void HandleSwitchChange(bool switchState)
            {
                _switchState = switchState;
            }
        }
        """;
}

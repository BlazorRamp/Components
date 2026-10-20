namespace BlazorRamp.DocSite.Common.Constants;

public class GoToSnippets
{
    public const string Add_GoTo_Style_Sheet = """
        <head>
            <link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
            <link rel="stylesheet" href="_content/BlazorRamp.GoTo/assets/css/goto.min.css" />
        </head>
        """;

    public const string Placement_On_Router_Page = """

        <AnnouncementHistory RefreshText="Refresh" ClearCloseText="Clear & Close" CloseText="Close" 
            NoDataText="No announcements" Title="Recent Announcements" TriggerVisible="true" TriggerText="Alerts" />

        <SkipTo IconVisible="true" SkipToText="Skip to content" SkipToType="SkipToType.Site" TargetID="app__main" />

        <ModalDialogContainer />

        <GoToViewer CloseButtonText="Close" ExcludeTriggerButton="false" GoToShortcut="@(new(Key:"z", Alt:true))" 
                    HeadingText="Go to Markers" NoGoToMarkersText="No go to markers defined for this page." TriggerButtonText="Go to viewer" />

         <Router AppAssembly="@typeof(App).Assembly" . . .

        """;

    public const string Simple_Usage_Example = """
        <GoTo LabelText="GoTo usage page before the tab stop button" />

        <button type="button" class="@Button.Base">Button only used as a tab stop</button>

        @* 
            Theme border tool code removed for brevity 
        *@
   
        <GoTo LabelText="GoTo usage page navigation" />
        """;
}

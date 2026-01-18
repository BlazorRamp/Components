namespace BlazorRamp.DocSite.Common.Constants;

public class CodeSnippets
{
    public const string Add_Core_Style_Sheet = """
        <head>
            <link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
        </head>
        """;

    public const string Add_Core_Package = "dotnet add package BlazorRamp.Core";

    public const string Add_Core_Script = """
        <script src="_framework/blazor.web.js"></script>
        <script type="module" src="_content/BlazorRamp.Core/assets/js/core-live-region.js"></script>
        """;

    public const string Add_Live_Region_Service = """
        @using BlazorRamp.Core.Common.Extensions;

        builder.Services.AddBlazorRampCore();
        """;

    public const string Add_Announcement_History_Component = """
        <AnnouncementHistory RefreshText="Refresh" ClearCloseText="Clear & Close" CloseText="Close" 
        NoDataText="No announcements" Title="Recent Announcements" TriggerVisible="true" TriggerText="Alerts" />

        <Router AppAssembly . . .
        """;
}

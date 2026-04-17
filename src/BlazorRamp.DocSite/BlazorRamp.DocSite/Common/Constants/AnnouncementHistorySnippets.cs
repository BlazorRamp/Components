namespace BlazorRamp.DocSite.Common.Constants;

public class AnnouncementHistorySnippets
{
    public const string Add_Announcement_History_Component = """
        <AnnouncementHistory RefreshText="Refresh" ClearCloseText="Clear & Close" CloseText="Close" 
        NoDataText="No announcements" Title="Recent Announcements" TriggerVisible="true" TriggerText="Alerts" />

        <Router AppAssembly . . .
        """;

    public const string Add_Configure_Announcement_History_Component = """
        <AnnouncementHistory Title="Recent Announcements" CloseText="Close" ClearCloseText="Clear & Close" 
         RefreshText="Refresh" TriggerText="Alerts" TriggerVisible="false" />
        /*
            * Which is the same as
        */
        <AnnouncementHistory />
        """;
}

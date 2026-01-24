namespace BlazorRamp.DocSite.Common.Constants;

public class CodeSnippets
{
    public const string Add_Core_Style_Sheet = """
        <head>
            <link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
        </head>
        """;
    public const string Add_Busy_Indicator_Style_Sheet = """
        <head>
            <link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
            <link rel="stylesheet" href="_content/BlazorRamp.BusyIndicator/assets/css/busy-indicator.min.css" />
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

    public const string Make_Announcement = """
        var announcement = new Announcement("The site is now using a dark coloured theme.", AnnouncementType.Info,  "Dark Theme Switch", LiveRegionType.Polite);

        await _liveRegionService.MakeAnnouncement(announcement);
        """;

    public const string Add_Configure_Announcement_History_Component = """
        <AnnouncementHistory Title="Recent Announcements" CloseText="Close" ClearCloseText="Clear & Close" 
         RefreshText="Refresh" TriggerText="Alerts" TriggerVisible="false" />
        /*
            * Which is the same as
        */
        <AnnouncementHistory />
        """;


    public const string Dark_Theme_Setting = """
        :root:has(#theme-toggler[aria-checked="true"]) {
         --br-comp-history-trigger-pane-surface-background:  var(--br-unit-colour-neutral-5);
         --br-comp-history-trigger-pane-surface-text:        var(--br-unit-colour-neutral-90);
         --br-comp-all-pane-base-background:                 var(--br-unit-colour-neutral-80);
         --br-comp-all-focus-indicator-colour:               var(--br-unit-colour-primary-30);
         --br-comp-all-pane-surface-background:              var(--br-unit-colour-neutral-70);
         --br-comp-all-area-header-background:               var(--br-unit-colour-neutral-70);
         --br-comp-all-area-content-text:                    var(--br-unit-colour-neutral-5);
         --br-unit-colour-canvas:                            var(--br-unit-colour-neutral-90);
         --br-unit-colour-canvas-text:                       var(--br-unit-colour-neutral-5);
         --br-comp-all-button-text:                          var(--br-unit-colour-primary-text-light);
         --br-comp-all-area-header-text:                     var(--br-unit-colour-primary-10);
         --br-comp-all-button-state-hover:                   hsl(from var(--br-unit-colour-primary) h s l / var(--br-unit-opacity-val-30));
         --br-comp-all-link-current-page-background-colour:  var(--br-unit-colour-secondary-darker);
         --br-comp-all-link-hover-background-colour:         hsl(from var(--br-unit-colour-secondary) h s l / 0.15);
         --br-comp-all-link-active-background-colour:        hsl(from var(--br-unit-colour-secondary) h s l / 0.3);
         --br-comp-all-link-focused-background-colour:       var(--br-unit-colour-secondary-darker);
         --br-info-box-alternate-heading-text:               var(--br-unit-colour-accent-light);
         --br-kbd-background-colour:                         var(--br-unit-colour-info-darker);
         --br-unit-colour-canvas-inverted:                   var(--br-unit-colour-neutral-5);
         --br-unit-colour-canvas-text-inverted:              var(--br-unit-colour-neutral-90);
         --br-comp-all-button-background:                    var(--br-unit-colour-primary-70);
         --br-comp-all-button-text:                          var(--br-unit-colour-primary-text-light);
         --br-comp-all-button-border-colour:                 var(--br-unit-colour-primary-30);
         --br-code-block-background-colour:                  var(--br-unit-colour-secondary-darker);
         --br-code-block-text:                               var(--br-unit-colour-info-lighter);
        }
        """;


    public const string Busy_Indicator_Example = """

        <BusyIndicator AriaStartText="Saving, please wait" AriaEndText=@_endMessage ShowIndicator="@_showIndicator" 
                OverlayPosition="OverlayPosition.Screen" BusyText=". . . Saving . . . ."  ContentPosition="ContentPosition.Top" 
                EndStatus="@_announcementType" DisplayTimeoutMS="15_000" OnBusyCompleted="HandleOnBusyCompleted" />

        public async Task SaveCustomer(CustomerData customerData)
        {
            _showIndicator = true;

            try
            {
                await _customerService.UpdateCustomer(customerData);
                _announcementType = AnnouncementType.OperationCompleted;
                _endMessage = "Saved Successfully";

            }
            catch(Exception ex)
            {
                /*
                    * Perhaps show some dialog or set a summary panel with the details etc
                 */

                _announcementType = AnnouncementType.SystemWarning;
                _endMessage = "You data was not saved, please review the details in the summary panel.";
            }
            finally
            {
                _showIndicator = false;
            }
        }
        """;
}

using BlazorRamp.Core.Services;
using BlazorRamp.Notifications.Common.Constants;
using BlazorRamp.Notifications.Common.Models;
using BlazorRamp.Notifications.Services;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;

namespace BlazorRamp.Notifications.Components;

public partial class NotificationCentre : IAsyncDisposable
{
    /// <summary>
    /// Gets or sets the heading of the viewer, which also provides its accessible name. If <see langword="null"/>,
    /// empty or whitespace, the default text is used.
    /// </summary>
    [Parameter] public string HeadingText   { get; set; }  = GlobalValues.Notification_Centre_Heading_Text;
    [Parameter] public string RemoveAllText { get; set; } = GlobalValues.Notification_Centre_Remove_All_Text;
    [Parameter] public string RemoveText    { get; set; } = GlobalValues.Notification_Item_Remove_Text;
    [Parameter] public string CloseText     { get; set; } = GlobalValues.Notification_Centre_Close_Text;

    [Inject] private INotificationService NotificationService { get; set; } = default!;
    [Inject] private ILiveRegionService   LiveRegionService   { get; set; } = default!;

    private List<NotificationItem> _notificationItems = [];
    private bool _disposed = false;

    private string _headingText   = GlobalValues.Notification_Centre_Heading_Text;
    private string _removeAllText = GlobalValues.Notification_Centre_Remove_All_Text;
    private string _removeText    = GlobalValues.Notification_Item_Remove_Text;
    private string _closeText    = GlobalValues.Notification_Centre_Close_Text;

    protected override void OnParametersSet()
    {
        _headingText   = String.IsNullOrWhiteSpace(HeadingText)   ? GlobalValues.Notification_Centre_Heading_Text    : HeadingText.Trim();
        _removeAllText = String.IsNullOrWhiteSpace(RemoveAllText) ? GlobalValues.Notification_Centre_Remove_All_Text : RemoveAllText.Trim();
        _removeText    = String.IsNullOrWhiteSpace(RemoveText)    ? GlobalValues.Notification_Item_Remove_Text       : RemoveText.Trim();
        _closeText     = String.IsNullOrWhiteSpace(CloseText)     ? GlobalValues.Notification_Centre_Close_Text      : CloseText.Trim();

    }
    protected override Task OnInitializedAsync()
    {
        return base.OnInitializedAsync();
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(true == firstRender)
        {
            await ((NotificationService)NotificationService).RegisterService();
            await ((NotificationService)NotificationService).RegisterNotificationCentreHandlers(@GlobalValues.Notification_Centre_Dialog_ID);
            ((NotificationService)NotificationService).OnNotificationsChanged += NotificationCentre_OnNotificationsChanged;
        }

    }

    private void NotificationCentre_OnNotificationsChanged(NotificationItem[] notificationItems)
    {
        _notificationItems.Clear();
        _notificationItems = notificationItems.ToList();
        StateHasChanged();
    }

    private async Task RemoveNotifications()
    
        => await NotificationService.RemoveNotifications();

    private async Task CloseCentre()

        => await NotificationService.CloseNotificationCentre(@GlobalValues.Notification_Centre_Dialog_ID);

    public async ValueTask DisposeAsync()
    {
        if (true == _disposed) return;

        try
        {
            ((NotificationService)NotificationService).OnNotificationsChanged -= NotificationCentre_OnNotificationsChanged;
            await((NotificationService)NotificationService).RegisterNotificationCentreHandlers(@GlobalValues.Notification_Centre_Dialog_ID);
            _disposed = true;
        }
        catch { }
    }
}

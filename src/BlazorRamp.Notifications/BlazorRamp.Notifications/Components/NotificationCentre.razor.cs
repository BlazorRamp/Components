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
    [Parameter] public string HeadingText { get; set; } = GlobalValues.Notification_Centre_Heading_Text;
    [Inject] private INotificationService NotificationService { get; set; }

    private List<NotificationItem> _notificationItems = [];
    private bool _disposed = false;

    private string _headingText = GlobalValues.Notification_Centre_Heading_Text;

    protected override void OnParametersSet()
    {
        _headingText = String.IsNullOrWhiteSpace(HeadingText) ? GlobalValues.Notification_Centre_Heading_Text : HeadingText.Trim();
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

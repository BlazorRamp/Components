using BlazorRamp.Notifications.Common.Models;
using BlazorRamp.Notifications.Services;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;

namespace BlazorRamp.Notifications.Components;

public partial class NotificationCentre : IDisposable
{

    [Inject] private INotificationService NotificationService { get; set; }

    private List<NotificationItem> _notificationItems = [];
    private bool _disposed = false;


    protected override Task OnParametersSetAsync()
    {
        return base.OnParametersSetAsync();
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

            ((NotificationService)NotificationService).OnNotificationsChanged += NotificationCentre_OnNotificationsChanged;
        }

    }

    private void NotificationCentre_OnNotificationsChanged(NotificationItem[] notificationItems)
    {
        _notificationItems.Clear();
        _notificationItems = notificationItems.ToList();
        StateHasChanged();
    }

    public void Dispose()
    {
        Debug.WriteLine("Dispose");

        if (true == _disposed) return;

        try
        {
            ((NotificationService)NotificationService).OnNotificationsChanged -= NotificationCentre_OnNotificationsChanged;
            _disposed = true;
        }
        catch { }
    }
}

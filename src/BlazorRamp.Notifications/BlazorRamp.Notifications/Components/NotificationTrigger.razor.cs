using BlazorRamp.Notifications.Common.Constants;
using BlazorRamp.Notifications.Common.Models;
using BlazorRamp.Notifications.Services;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;

namespace BlazorRamp.Notifications.Components;

public partial class NotificationTrigger: IDisposable
{

    [Parameter] public string ButtonText { get; set; } = GlobalValues.Notification_Trigger_Text;
    [Inject] private INotificationService NotificationService { get; set; } = default!;

    private bool _disposed = false;
    private int  _notificationCount = 0;

    private bool _expanded = false;

    private string _triggerText = GlobalValues.Notification_Trigger_Text;
    protected override void OnParametersSet()
    {
        _triggerText = String.IsNullOrWhiteSpace(ButtonText) ? GlobalValues.Notification_Trigger_Text : ButtonText.Trim();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if(true == firstRender)
        {
            ((NotificationService)NotificationService).OnNotificationsChanged += NotificationCentre_OnNotificationsChanged;
        }
    }
    private async void NotificationCentre_OnNotificationsChanged(NotificationItem[] notificationItems, string? focusID = null)
    {
        _notificationCount = notificationItems.Count();
        await InvokeAsync(StateHasChanged);
    }

    private async Task ShowNotificaitonCentre()

        => await ((NotificationService)NotificationService).ShowNotificationCentre(GlobalValues.Notification_Centre_Dialog_ID);



    public void Dispose()
    {
        if (true == _disposed) return;

        try
        {
            ((NotificationService)NotificationService).OnNotificationsChanged -= NotificationCentre_OnNotificationsChanged;

            _disposed = true;
        }
        catch { }
    }
}

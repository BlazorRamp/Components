using BlazorRamp.Notifications.Common.Constants;
using BlazorRamp.Notifications.Common.Models;
using BlazorRamp.Notifications.Services;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;

namespace BlazorRamp.Notifications.Components;

public partial class NotificationTrigger: IDisposable
{

    [Parameter] public string AriaLabelText { get; set; } = GlobalValues.Notification_Trigger_Text;
    [Parameter] public string HasPopupText   { get; set; }
    [Inject] private INotificationService NotificationService { get; set; } = default!;

    private bool _disposed = false;
    private int  _notificationCount = 0;

    private bool _expanded = false;

    private string _ariaLabelText      = GlobalValues.Notification_Trigger_Text;
    private string _ariaLabelCountText = GlobalValues.Notification_Trigger_Text;
    private string _ariaDescribedText = GlobalValues.Notification_Trigger_Popup_Text;

    private string _describedByID = Guid.NewGuid().ToString();

    protected override void OnParametersSet()
    {
        _ariaLabelText      = String.IsNullOrWhiteSpace(AriaLabelText) ? GlobalValues.Notification_Trigger_Text : AriaLabelText.Trim();
        _ariaLabelCountText = _ariaLabelText.Replace("{count}", _notificationCount.ToString());
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
        _ariaLabelCountText = _ariaLabelText.Replace("{count}", _notificationCount.ToString());

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

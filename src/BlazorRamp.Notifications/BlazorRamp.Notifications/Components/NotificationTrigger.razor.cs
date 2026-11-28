using BlazorRamp.Notifications.Common.Constants;
using BlazorRamp.Notifications.Common.Models;
using BlazorRamp.Notifications.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorRamp.Notifications.Components;

public partial class NotificationTrigger: IDisposable
{

    [Parameter] public string AriaLabelText { get; set; } = GlobalValues.Notification_Trigger_Text;
    [Parameter] public string HasPopupText   { get; set; } = GlobalValues.Notification_Trigger_Popup_Text;
    [Inject] private INotificationService NotificationService { get; set; } = default!;

    private bool _disposed = false;
    private int  _notificationCount = 0;

    private string _ariaLabelTemplateText = GlobalValues.Notification_Trigger_Text;
    private string _ariaLabelText        = GlobalValues.Notification_Trigger_Text;
    private string _ariaDescribedText    = GlobalValues.Notification_Trigger_Popup_Text;

    private string _describedByID = Guid.NewGuid().ToString();

    protected override void OnParametersSet()
    {
        _ariaLabelTemplateText  = String.IsNullOrWhiteSpace(AriaLabelText) ? GlobalValues.Notification_Trigger_Text : AriaLabelText.Trim();
        _ariaLabelText          = _ariaLabelTemplateText.Replace("{count}", _notificationCount.ToString());
        _ariaDescribedText      = String.IsNullOrWhiteSpace(HasPopupText) ? GlobalValues.Notification_Trigger_Popup_Text : HasPopupText.Trim();
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
        _ariaLabelText = _ariaLabelTemplateText.Replace("{count}", _notificationCount.ToString());

        await InvokeAsync(StateHasChanged);
    }

    private async Task ShowNotificaitonCentre()

        => await ((NotificationService)NotificationService).ShowNotificationCentre();



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

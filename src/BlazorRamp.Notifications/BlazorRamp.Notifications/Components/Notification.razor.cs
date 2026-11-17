using BlazorRamp.Notifications.Common.Constants;
using BlazorRamp.Notifications.Common.Models;
using BlazorRamp.Notifications.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorRamp.Notifications.Components;

public partial class Notification
{
    [Parameter] public NotificationItem NotificationItem { get; set; }

    [Parameter] public string RemoveText { get; set; }
    [Inject] private INotificationService NotificationService { get; set; } = default!;

    private bool _displayActions = false;
    private string _removeText = GlobalValues.Notification_Item_Remove_Text;


    protected override void OnParametersSet()
    {
        _removeText = String.IsNullOrWhiteSpace(RemoveText) ? GlobalValues.Notification_Item_Remove_Text : RemoveText.Trim();
        _displayActions = (NotificationItem is null || String.IsNullOrWhiteSpace(NotificationItem.ActionText) || String.IsNullOrWhiteSpace(NotificationItem.ActionUrl)) ? false : true;
    }


    private async Task HandleRemoveNotification(string notificationItemID)
    
        => await NotificationService.RemoveNotification(notificationItemID);



}

using BlazorRamp.Core.Common.Constants;
using BlazorRamp.Core.Services;
using BlazorRamp.Notifications.Common.Constants;
using BlazorRamp.Notifications.Common.Models;
using BlazorRamp.Notifications.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorRamp.Notifications.Components;

public partial class Notification
{
    [Parameter] public NotificationItem NotificationItem { get; set; }

    [Parameter] public string RemoveText { get; set; } = GlobalValues.Notification_Item_Remove_Text;

    [Parameter] public string RemovedText { get; set; } = GlobalValues.Notification_Item_Remove_Text;


    [Inject] private INotificationService NotificationService { get; set; } = default!;
    [Inject] private ILiveRegionService LiveRegionService     { get; set; } = default!;

    private bool _displayActions = false;
    private string _removeText = GlobalValues.Notification_Item_Remove_Text;
    private string _removedText = GlobalValues.Notification_Item_Removed_Text;


    protected override void OnParametersSet()
    {
        _removeText     = String.IsNullOrWhiteSpace(RemoveText) ? GlobalValues.Notification_Item_Remove_Text : RemoveText.Trim();
        _removedText    = String.IsNullOrWhiteSpace(RemoveText) ? GlobalValues.Notification_Item_Removed_Text : RemovedText.Trim();
        _displayActions = (NotificationItem is null || String.IsNullOrWhiteSpace(NotificationItem.ActionText) || String.IsNullOrWhiteSpace(NotificationItem.ActionUrl)) ? false : true;
    }


    private async Task HandleRemoveNotification(string notificationItemID)
    {
        await MakeAnnouncement(_removedText);

        await NotificationService.RemoveNotification(notificationItemID);
       
    }


    private async Task CloseNotificationCentre()

        => await NotificationService.CloseNotificationCentre(GlobalValues.Notification_Centre_Dialog_ID);


    private async Task MakeAnnouncement(string message)

        => await LiveRegionService.MakeAnnouncement(new(message, AnnouncementType.Info, "Remove button", LiveRegionType.Assertive),false);
    
}

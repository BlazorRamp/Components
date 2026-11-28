using BlazorRamp.Core.Common.Constants;
using BlazorRamp.Core.Services;
using BlazorRamp.Notifications.Common.Constants;
using BlazorRamp.Notifications.Common.Models;
using BlazorRamp.Notifications.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorRamp.Notifications.Components;

public partial class Notification
{
    [Parameter] public NotificationItem NotificationItem { get; set; }

    [Parameter] public string RemoveText { get; set; } = GlobalValues.Notification_Item_Remove_Text;

    [Parameter] public string RemovedText { get; set; } = GlobalValues.Notification_Item_Removed_Text;

    [Inject] private INotificationService NotificationService { get; set; } = default!;
    [Inject] private ILiveRegionService LiveRegionService     { get; set; } = default!;

    private bool _displayActions = false;
    private string _removeText = GlobalValues.Notification_Item_Remove_Text;
    private string _removedText = GlobalValues.Notification_Item_Removed_Text;

    private bool _removing = false;

    private string GetItemClasses()
        => _removing ? $"{GlobalValues.Notification_Item_Class} {GlobalValues.Notification_Item_Modifier_Class}" : GlobalValues.Notification_Item_Class;
    
    protected override void OnParametersSet()
    {
        _removeText     = String.IsNullOrWhiteSpace(RemoveText) ? GlobalValues.Notification_Item_Remove_Text : RemoveText.Trim();
        _removedText    = String.IsNullOrWhiteSpace(RemovedText) ? GlobalValues.Notification_Item_Removed_Text : RemovedText.Trim();

        _removeText    = _removeText.Replace("{title}", NotificationItem.Title ?? "");

        _displayActions = (NotificationItem is null || String.IsNullOrWhiteSpace(NotificationItem.ActionText) || String.IsNullOrWhiteSpace(NotificationItem.ActionUrl)) ? false : true;
    }


    private async Task HandleRemoveNotification(string notificationItemID)
    {
        if (_removing) return;

        _removing = true;

        StateHasChanged();

        await NotificationService.RemoveNotification(notificationItemID, _removedText);
       
    }
    private async Task HandleActionClick(MouseEventArgs mouseEventArgs)
    {
        if (mouseEventArgs.CtrlKey || mouseEventArgs.MetaKey || mouseEventArgs.ShiftKey) return;

        await NotificationService.CloseNotificationCentre();
        
        if (false == String.IsNullOrWhiteSpace(NotificationItem.ActionFocusID)) await ((NotificationService)NotificationService).SetFocus(NotificationItem.ActionFocusID);

    }

}

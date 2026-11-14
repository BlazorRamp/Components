using BlazorRamp.Notifications.Common.Models;
using BlazorRamp.Notifications.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorRamp.Notifications.Components;

public partial class Notification
{
    [Parameter] public NotificationItem NotificationItem { get; set; }
    [Inject] private INotificationService NotificationService { get; set; } = default!;

    private string _displayTime = DateTime.UtcNow.ToString();
    private async Task HandleRemoveNotification(string notificationItemID)
    
        => await NotificationService.RemoveNotification(notificationItemID);


    protected override async Task OnParametersSetAsync()
    {
        
    }
}

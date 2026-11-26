using BlazorRamp.Notifications.Common.Constants;
using BlazorRamp.Notifications.Common.Models;

namespace BlazorRamp.Notifications.Services;

public interface INotificationService
{

    string ServiceID { get; }

    Task AddNotificationItem(NotificationItem notificationItem);
    Task AddNotificationItems(NotificationItem[] notificationItems);

    Task<NotificationItem[]> GetNotifications(NotificationFilter filter = NotificationFilter.Active);

    Task RemoveNotification(string notificationItemID);

    Task RemoveNotifications();

    Task CloseNotificationCentre();
    Task ShowNotificationCentre();
    event Action<NotificationItem[], string?> OnNotificationsChanged;


}

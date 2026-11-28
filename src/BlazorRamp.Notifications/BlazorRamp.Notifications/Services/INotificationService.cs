using BlazorRamp.Notifications.Common.Constants;
using BlazorRamp.Notifications.Common.Models;

namespace BlazorRamp.Notifications.Services;

public interface INotificationService
{

    Task AddNotificationItem(NotificationItem notificationItem, string announcmentText = GlobalValues.Notification_Item_New_Default_Text);
    Task AddNotificationItems(NotificationItem[] notificationItems);

    Task<NotificationItem[]> GetNotifications(NotificationFilter filter = NotificationFilter.Active);

    Task RemoveNotification(string notificationItemID, string removeText);

    Task RemoveNotifications();

    Task CloseNotificationCentre();
    Task ShowNotificationCentre();


    Task InitialiseSessionStore(string userStoreKey = "", NotificationItem[]? notificationItems = null);

    event Action<NotificationItem[], string?> OnNotificationsChanged;


}

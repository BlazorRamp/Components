using BlazorRamp.Notifications.Common.Constants;
using BlazorRamp.Notifications.Common.Models;

namespace BlazorRamp.Notifications.Services;

public interface INotificationService
{

    string ServiceID { get; }

    Task AddNotificationItem(NotificationItem notificationItem);

    Task<NotificationItem[]> GetNotifications(NotificationFilter filter = NotificationFilter.Active);

    //event Action? NotificationsChanged;

    //Task InitialiseAsync(string? userStoreKey, IEnumerable<NotificationItem>? notifications = null);
    //Task SendAsync(NotificationItem notification);                    // something just happened: adds it and announces it
    //Task AddRangeAsync(IEnumerable<NotificationItem> notifications);  // catching up from the server: adds new ones, no announcement
    //Task RemoveAsync(string notificationId);
    //Task ClearAsync();                                            // the user's "Clear all"
    //Task ResetAsync();                                            // sign-out: wipes the saved data
}

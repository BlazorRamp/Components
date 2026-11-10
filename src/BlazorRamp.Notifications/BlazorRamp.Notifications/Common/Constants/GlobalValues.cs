using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.Notifications.Common.Constants;


internal class GlobalValues
{
    //export { registerNotificationService, unregisterNotificationService, initialiseNotificationStore,
    //addNotificationItem, removeNotificationItem, clearNotifications, addNotificationItems, getAllNotifications, clearNotificationStore }

    public const string JS_Module_File_Path              = "./_content/BlazorRamp.Notifications/assets/js/notifications.js";
    public const string JS_Register_Service_Func         = "registerNotificationService";// (serviceId: string, dotNetObjectReference: any, callbackName: string)
    public const string JS_Unregister_Service_Func       = "unregisterNotificationService"; // (serviceId: string)
    public const string JS_Add_Notification_Func         = "addNotificationItem"; //(notificationItem: NotificationItem)
    public const string JS_Add_Notifications_Func        = "addNotificationItems"; //(notificationItems: NotificationItem[])
    public const string JS_Remove_Notification_Func      = "removeNotificationItem";//(notificationId: string)
    public const string JS_Clear_Notifications_Func      = "clearNotifications";
    public const string JS_Get_All_Notifications_Func    = "getAllNotifications";
    public const string JS_Clear_Notification_Store      = "clearNotificationStore";
    public const string JS_Initialise_Notification_Store = "initialiseNotificationStore";
}

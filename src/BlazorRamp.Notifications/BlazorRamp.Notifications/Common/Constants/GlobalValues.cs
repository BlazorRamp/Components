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
    public const string JS_Remove_Notification_Func      = "removeNotificationItem";//(notificationItemId: string)
    public const string JS_Clear_Notifications_Func      = "clearNotifications";
    public const string JS_Get_All_Notifications_Func    = "getAllNotifications";
    public const string JS_Clear_Notification_Store      = "clearNotificationStore";
    public const string JS_Initialise_Notification_Store = "initialiseNotificationStore";
    public const string JS_Register_Centre_Handlers      = "registerNotificationCentreHandlers";
    public const string JS_Unregister_Centre_Handlers    = "unregisterNotificationCentreHandlers";
    public const string Blazor_Ramp_ID = "blazor-ramp";

    public const string Notification_Centre_Dialog_ID = $"{Blazor_Ramp_ID}-notification-centre";

    public const string Notification_Centre_Heading_Text = "Notifications";

    public const string Notification_Centre_Class              = "br-notification-centre";

    public const string Notification_Centre_Content_Area_Class = $"{Notification_Centre_Class}__content-area";
    public const String Notification_Centre_Header_Class = $"{Notification_Centre_Class}__header";
    public const string Notification_Centre_Content_Class = $"{Notification_Centre_Class}__content";

    public const string Notification_Centre_Footer_Class = $"{Notification_Centre_Class}__footer";

    public const string Notification_Item_Class            = "br-notification-item";
    public const string Notification_Item_Title_Area_Class = $"{Notification_Item_Class}__title-area";
    public const string Notification_Item_Title_Class      = $"{Notification_Item_Class}__title";
    public const string Notification_Item_Time_Class       = $"{Notification_Item_Class}__time";
    public const string Notification_Item_Content_Class    = $"{Notification_Item_Class}__content";

    public const string Notification_Item_Footer_Area_Class = $"{Notification_Item_Class}__footer-area";

    public const string Notification_Item_Button_Class       = $"{Notification_Item_Class}__button";

    public const string Notification_Item_Remove_Icon_Class = $"{Notification_Item_Class}__remove-icon";

    //public const string Notification_Dialog_ID = 
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.Notifications.Common.Constants;

/// <summary>
/// Specifies the type or category of a notificataion for semantic classification.
/// </summary>
public enum NotificationType : int
{
    /// <summary>
    /// General informational notification.
    /// </summary>
    Info = 0,
}


public enum NotificationFilter : int
{
    Active = 0,
    Deleted = 1,

    All = 3
    
}
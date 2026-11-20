using BlazorRamp.Core.Common.Constants;
using BlazorRamp.Core.Common.Models;
using BlazorRamp.Core.Services;
using BlazorRamp.Notifications.Common.Constants;
using BlazorRamp.Notifications.Common.Models;
using BlazorRamp.Notifications.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorRamp.Notifications.Components;

public partial class NotificationCentre : IAsyncDisposable
{
    /// <summary>
    /// Gets or sets the heading of the viewer, which also provides its accessible name. If <see langword="null"/>,
    /// empty or whitespace, the default text is used.
    /// </summary>
    [Parameter] public string HeadingText         { get; set; }  = GlobalValues.Notification_Centre_Heading_Text;
    [Parameter] public string RemoveAllText       { get; set; } = GlobalValues.Notification_Centre_Remove_All_Text;
    [Parameter] public string RemovedAllText      { get; set; } = GlobalValues.Notification_Centre_Removed_All_Text;
    [Parameter] public string RemoveText          { get; set; } = GlobalValues.Notification_Item_Remove_Text;
    [Parameter] public string RemovedText         { get; set; } = GlobalValues.Notification_Item_Removed_Text;
    [Parameter] public string CloseText           { get; set; } = GlobalValues.Notification_Centre_Close_Text;
    [Parameter] public string NoNotificationsText { get; set; } = GlobalValues.Notification_Centre_No_Notifications_Text;

    [Inject] private INotificationService NotificationService { get; set; } = default!;
    [Inject] private ILiveRegionService   LiveRegionService   { get; set; } = default!;

    //[Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private List<NotificationItem> _notificationItems = [];
    private bool _disposed = false;

    private string _headingText         = GlobalValues.Notification_Centre_Heading_Text;
    private string _removeAllText       = GlobalValues.Notification_Centre_Remove_All_Text;
    private string _removedAllText      = GlobalValues.Notification_Centre_Removed_All_Text;
    private string _removeText          = GlobalValues.Notification_Item_Remove_Text;
    private string _removedText         = GlobalValues.Notification_Item_Removed_Text;
    private string _closeText           = GlobalValues.Notification_Centre_Close_Text;
    private string _noNotificationsText = GlobalValues.Notification_Centre_No_Notifications_Text;
    private string _closeButtonID       = Guid.NewGuid().ToString();
    private string _headingTextID = Guid.NewGuid().ToString();
    private bool _removeAllCalled = false;

    protected override void OnParametersSet()
    {
        _headingText         = String.IsNullOrWhiteSpace(HeadingText)         ? GlobalValues.Notification_Centre_Heading_Text          : HeadingText.Trim();
        _removeAllText       = String.IsNullOrWhiteSpace(RemoveAllText)       ? GlobalValues.Notification_Centre_Remove_All_Text       : RemoveAllText.Trim();
        _removedAllText      = String.IsNullOrWhiteSpace(RemovedAllText)      ? GlobalValues.Notification_Centre_Removed_All_Text      : RemovedAllText.Trim();
        _removeText          = String.IsNullOrWhiteSpace(RemoveText)          ? GlobalValues.Notification_Item_Remove_Text             : RemoveText.Trim();
        _removedText         = String.IsNullOrWhiteSpace(RemovedText)         ? GlobalValues.Notification_Item_Removed_Text            : RemovedText.Trim();
        _closeText           = String.IsNullOrWhiteSpace(CloseText)           ? GlobalValues.Notification_Centre_Close_Text            : CloseText.Trim();
        _noNotificationsText = String.IsNullOrWhiteSpace(NoNotificationsText) ? GlobalValues.Notification_Centre_No_Notifications_Text : NoNotificationsText.Trim();

    }
    protected override Task OnInitializedAsync()
    {
        return base.OnInitializedAsync();
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(true == firstRender)
        {
            await ((NotificationService)NotificationService).RegisterService();
            await ((NotificationService)NotificationService).RegisterNotificationCentreHandlers(@GlobalValues.Notification_Centre_Dialog_ID);
            ((NotificationService)NotificationService).OnNotificationsChanged += NotificationCentre_OnNotificationsChanged;
        }

    }



    private async void NotificationCentre_OnNotificationsChanged(NotificationItem[] notificationItems, string? focusID = null)
    {
        _notificationItems.Clear();
        _notificationItems = notificationItems.ToList();

        if (focusID is not null) await ((NotificationService)NotificationService).SetFocus(focusID);

        if (_notificationItems.Count == 0)  await ((NotificationService)NotificationService).SetFocus(GlobalValues.Notification_Centre_Dialog_ID);

        await InvokeAsync(StateHasChanged);
    }


    private async Task RemoveNotifications()
    {
        if (true == _removeAllCalled) return;
        
        _removeAllCalled = true;
        
        await LiveRegionService.MakeAnnouncement(new Announcement(_removedAllText, AnnouncementType.Info, $"{_removeAllText} button", LiveRegionType.Assertive), true);
        await NotificationService.RemoveNotifications();
      
        
        _removeAllCalled = false;
    }


    
    private async Task CloseCentre()

        => await NotificationService.CloseNotificationCentre(@GlobalValues.Notification_Centre_Dialog_ID);

    public async ValueTask DisposeAsync()
    {
        if (true == _disposed) return;

        try
        {
            ((NotificationService)NotificationService).OnNotificationsChanged -= NotificationCentre_OnNotificationsChanged;
            await((NotificationService)NotificationService).UnRegisterNotificationCentreHandlers(@GlobalValues.Notification_Centre_Dialog_ID);
            _disposed = true;
        }
        catch { }
    }
}

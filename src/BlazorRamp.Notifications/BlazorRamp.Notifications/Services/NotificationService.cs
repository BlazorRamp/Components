using BlazorRamp.Notifications.Common.Constants;
using BlazorRamp.Notifications.Common.Extensions;
using BlazorRamp.Notifications.Common.Models;
using Microsoft.JSInterop;
using Microsoft.VisualBasic;

namespace BlazorRamp.Notifications.Services;

internal sealed class NotificationService : INotificationService, IAsyncDisposable
{
    private readonly DotNetObjectReference<NotificationService>? _serviceReference = null;
    private readonly IJSRuntime _jsRuntime;
    private readonly string     _serviceID;
    private readonly SemaphoreSlim _moduleLock   = new(1, 1);
    private readonly SemaphoreSlim _registerLock = new(1, 1);
    private IJSObjectReference? _module;
    private bool                _isDisposed   = false;
    private bool                _isRegistered = false;

    private Action<NotificationItem[]>? _onNotificationsChanged;

    public event Action<NotificationItem[]> OnNotificationsChanged
    {
        add    { _onNotificationsChanged += value; RegisterService().AwaitTask(_ => { }); }
        remove { _onNotificationsChanged -= value; }
    }


    public string ServiceID => _serviceID;

    public NotificationService(IJSRuntime jsRuntime)
    {
        _serviceReference = DotNetObjectReference.Create(this);
        _jsRuntime        = jsRuntime;
        _serviceID        = Guid.NewGuid().ToString();
    }

    private async Task<IJSObjectReference> GetModuleAsync()
    {
        await _moduleLock.WaitAsync();
        try { return _module ??= await _jsRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_Module_File_Path); }
        finally { _moduleLock.Release(); }
    }

    public async Task RegisterService()
    {
        var notificationModule = await GetModuleAsync();   // before the register lock, so the two locks never nest

        await _registerLock.WaitAsync();
        try
        {
            if (_isRegistered || _isDisposed) return;

            await notificationModule.InvokeVoidAsync(GlobalValues.JS_Register_Service_Func, _serviceID, _serviceReference, nameof(NotificationsChanged));

            _isRegistered = true;   // only after success, so a failure can retry
        }
        finally { _registerLock.Release(); }
    }


    public async Task<NotificationItem[]> GetNotifications(NotificationFilter filter = NotificationFilter.Active)
    {
        var module = await GetModuleAsync();
        return await module.InvokeAsync<NotificationItem[]>(GlobalValues.JS_Get_All_Notifications_Func, filter.ToString());
    }

    public async Task AddNotificationItem(NotificationItem notificationItem)
    {
        var module = await GetModuleAsync();

        await module.InvokeVoidAsync(GlobalValues.JS_Add_Notification_Func, notificationItem);
    }



    public async Task RemoveNotification(string notificationItemID)
    {
        var module = await GetModuleAsync();

        await module.InvokeVoidAsync(GlobalValues.JS_Remove_Notification_Func, notificationItemID);
    }


    public async Task RegisterNotificationCentreHandlers(string notificationCentreID)
    {
        var module = await GetModuleAsync();
        await module.InvokeVoidAsync(GlobalValues.JS_Register_Centre_Handlers, notificationCentreID);
    }
    public async Task UnRegisterNotificationCentreHandlers(string notificationCentreID)
    {
        var module = await GetModuleAsync();
        await module.InvokeVoidAsync(GlobalValues.JS_Unregister_Centre_Handlers, notificationCentreID);
    }

    [JSInvokable]
    public Task NotificationsChanged(NotificationItem[] notifications)
    {
        _onNotificationsChanged?.Invoke(notifications);

        return Task.CompletedTask;
    }
    public async ValueTask DisposeAsync()
    {
        if (_isDisposed) return;

        _isDisposed = true;

        try
        {
            if (_module is not null)
            {
                if (_isRegistered) await _module.InvokeVoidAsync(GlobalValues.JS_Unregister_Service_Func, _serviceID);

                await _module.DisposeAsync();
            }
        }
        catch (Exception exception) when (exception is JSDisconnectedException or InvalidOperationException)
        {
            // The circuit has already gone, or the module never loaded, so there is nothing to unregister.
        }
        finally
        {
            _serviceReference?.Dispose();
            _moduleLock.Dispose();
            _registerLock.Dispose();
        }
    }


}
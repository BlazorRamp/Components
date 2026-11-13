using BlazorRamp.Notifications.Common.Constants;
using BlazorRamp.Notifications.Common.Extensions;
using BlazorRamp.Notifications.Common.Models;
using Microsoft.JSInterop;

namespace BlazorRamp.Notifications.Services;

internal sealed class NotificationService : INotificationService, IAsyncDisposable
{
    private readonly DotNetObjectReference<NotificationService>? _serviceReference = null;
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;
    private readonly IJSRuntime _jsRuntime;
    private readonly string     _serviceID;
    private bool                _isDisposed   = false;
    private bool                _isRegistered = false;

    public event Action<NotificationItem[]> OnNotificationsChanged  = delegate { };

    public string ServiceID => _serviceID; 

    public NotificationService(IJSRuntime jsRuntime)
    {
        _serviceReference = DotNetObjectReference.Create(this);
        _jsRuntime        = jsRuntime;
        _serviceID        = Guid.NewGuid().ToString();

        _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_Module_File_Path).AsTask());

        if (OperatingSystem.IsBrowser()) RegisterService().AwaitTask(_ => { });

    }

    public async Task RegisterService()
    {
        if (_isRegistered) return;

        var notificationModule = await _moduleTask.Value;

        await notificationModule.InvokeVoidAsync(GlobalValues.JS_Register_Service_Func, _serviceID, _serviceReference, nameof(NotificationsChanged));

        _isRegistered = true;
    }

    [JSInvokable]
    public async Task NotificationsChanged(NotificationItem[] notifications)
    {
        if (OnNotificationsChanged.GetInvocationList().Length > 1) OnNotificationsChanged.Invoke(notifications);

        await Task.CompletedTask;
    }
    public async ValueTask DisposeAsync()
    {
        if (true == _isDisposed) return;
        
        _isDisposed = true;

        try
        {
            if (_moduleTask.IsValueCreated)
            {
                var notificationModule = await _moduleTask.Value;

                if (true == _isRegistered) await notificationModule.InvokeVoidAsync(GlobalValues.JS_Unregister_Service_Func, _serviceID);

                await notificationModule.DisposeAsync();
            }
        }
        catch (Exception exception) when (exception is JSDisconnectedException or InvalidOperationException)
        {
            // The circuit has already gone, or the module never loaded, so there is nothing to unregister.
        }
        finally
        {
            _serviceReference?.Dispose();
        }
    }
}

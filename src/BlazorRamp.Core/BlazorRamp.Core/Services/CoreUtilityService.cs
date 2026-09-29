using BlazorRamp.Core.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorRamp.Core.Services;

internal sealed class CoreUtilityService : ICoreUtilityService, IAsyncDisposable
{

    private IJSObjectReference? _jsUtilityModule;
    private readonly IJSRuntime _jsRuntime;
    private bool _isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="CoreUtilityService"/> class.
    /// </summary>
    /// <param name="jsRuntime">The JSRuntime used for module importing and JS calls.</param>
    public CoreUtilityService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    /// <summary>
    /// Lazily imports and retrieves the JavaScript module for utility service operations.
    /// </summary>
    private async Task<IJSObjectReference> GetJSUtilityModule(string modulePath)

        => _jsUtilityModule ??= await _jsRuntime.InvokeAsync<IJSObjectReference>("import", modulePath);

    /// <inheritdoc />
    public async ValueTask RegisterContainerForAutoTabindex(ElementReference container, string labelName = "Scroll region", bool addRoleAndLabel = true)
    {
        var module = await GetJSUtilityModule(CoreGlobalValues.JS_Utils_File_Path);
        await module.InvokeVoidAsync(CoreGlobalValues.JS_Utils_Register_Container_For_Tab_Func, container, labelName, addRoleAndLabel);
    }

    /// <inheritdoc />
    public async ValueTask UnregisterContainerForAutoTabindex(ElementReference container)
    {
        try
        {
            var module = await GetJSUtilityModule(CoreGlobalValues.JS_Utils_File_Path);
            await module.InvokeVoidAsync(CoreGlobalValues.JS_Utils_Unregister_Container_For_Tab_Func, container);
        }
        catch (JSDisconnectedException) { } // circuit is gone, there is nothing left to unregister
    }

    /// <summary>
    /// Performs asynchronous disposal of resources, specifically the JS module reference 
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_isDisposed) return;

        if (_jsUtilityModule is not null)
        {
            try
            {
                await _jsUtilityModule!.DisposeAsync();
            }
            catch { }// Circuit is disconnected (JSDisconnectedException), JS interop is no longer available - safe to ignore
        }

        _isDisposed = true;
    }
}

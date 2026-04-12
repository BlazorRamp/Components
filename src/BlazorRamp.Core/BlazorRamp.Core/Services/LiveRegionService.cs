using BlazorRamp.Core.Common.Constants;
using BlazorRamp.Core.Common.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace BlazorRamp.Core.Services;

/// <summary>
/// Provides an implementation of <see cref="ILiveRegionService"/> that manages 
/// JavaScript interop for ARIA live regions and popover state.
/// </summary>
public sealed class LiveRegionService : ILiveRegionService, IAsyncDisposable
{
    private IJSObjectReference?        _jsLiveRegionModule;
    private readonly IJSRuntime        _jsRuntime;        
    private readonly NavigationManager _navigationManager;
    private bool _isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="LiveRegionService"/> class.
    /// </summary>
    /// <param name="jsRuntime">The JSRuntime used for module importing and JS calls.</param>
    /// <param name="navigationManager">The NavigationManager used to track location changes for popover management.</param>

    public LiveRegionService(IJSRuntime jsRuntime, NavigationManager navigationManager)
    {
        _jsRuntime         = jsRuntime;
        _navigationManager = navigationManager;

        _navigationManager.LocationChanged+= NavigationManager_LocationChanged;
    }

    /// <inheritdoc />
    public async Task MakeAnnouncement(Announcement announcement)
    {
        
         var jsLiveRegionModule = await GetJSLiveRegionModule(CoreGlobalValues.JS_Live_Region_File_Path);

        if (announcement == null || String.IsNullOrWhiteSpace(announcement.Message)) return;

        await jsLiveRegionModule.InvokeVoidAsync(CoreGlobalValues.JS_Live_Region_Announce_Func, announcement);
    }



    /// <summary>
    /// Lazily imports and retrieves the JavaScript module for live region operations.
    /// </summary>
    private async Task<IJSObjectReference> GetJSLiveRegionModule(string modulePath)

        => _jsLiveRegionModule ??= await _jsRuntime.InvokeAsync<IJSObjectReference>("import", modulePath);

    /*
        * Best of a bad bunch of options
     */
    /// <summary>
    /// Handles the <see cref="NavigationManager.LocationChanged"/> event to automatically 
    /// close any open history popovers upon navigation.
    /// </summary>
    private async void NavigationManager_LocationChanged(object? sender, LocationChangedEventArgs e)
    {
        _jsLiveRegionModule = await GetJSLiveRegionModule(CoreGlobalValues.JS_Live_Region_File_Path);

        if (_jsLiveRegionModule is not null) await _jsLiveRegionModule.InvokeVoidAsync(CoreGlobalValues.JS_Live_Region_Check_Close_Popover_Func);  
        
    }

    /// <summary>
    /// Performs asynchronous disposal of resources, specifically the JS module reference 
    /// and unhooking navigation events.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_isDisposed) return;

        if (_jsLiveRegionModule is not null)
        {
            try
            {
                await _jsLiveRegionModule!.DisposeAsync();
            }
            catch { }// Circuit is disconnected (JSDisconnectedException), JS interop is no longer available - safe to ignore
        }

        _navigationManager.LocationChanged -= NavigationManager_LocationChanged;

        _isDisposed = true;
    }
}

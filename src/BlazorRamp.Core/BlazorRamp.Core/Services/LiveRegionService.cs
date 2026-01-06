using BlazorRamp.Core.Common.Constants;
using BlazorRamp.Core.Common.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using BlazorRamp.Core.Common.Utilities;
namespace BlazorRamp.Core.Services;

public sealed class LiveRegionService : ILiveRegionService, IAsyncDisposable
{
    private IJSObjectReference?        _jsLiveRegionModule;
    private readonly IJSRuntime        _jsRuntime;        
    private readonly NavigationManager _navigationManager;
    private bool _isDisposed;


    public LiveRegionService(IJSRuntime jsRuntime, NavigationManager navigationManager)
    {
        _jsRuntime         = jsRuntime;
        _navigationManager = navigationManager;

        _navigationManager.LocationChanged+= NavigationManager_LocationChanged;
    }

    public async Task MakeAnnouncement(Announcement announcement)
    {
        if (announcement == null || String.IsNullOrWhiteSpace(announcement.Message)) return;

        var jsLiveRegionModule = await GetJSLiveRegionModule(CoreGlobalValues.JS_Live_Region_File_Path);

        await jsLiveRegionModule.InvokeVoidAsync(CoreGlobalValues.JS_Live_Region_Announce_Func, announcement);
    }

    private async Task<IJSObjectReference> GetJSLiveRegionModule(string modulePath)

        => _jsLiveRegionModule ??= await _jsRuntime.InvokeAsync<IJSObjectReference>("import", modulePath);

    /*
        * Best of a bad bunch of options
     */
    private async void NavigationManager_LocationChanged(object? sender, LocationChangedEventArgs e)
    {
        _jsLiveRegionModule = await GetJSLiveRegionModule(CoreGlobalValues.JS_Live_Region_File_Path);

        if (_jsLiveRegionModule is not null) await _jsLiveRegionModule.InvokeVoidAsync(CoreGlobalValues.JS_Live_Region_Check_Close_Popover_Func); ; 
        
    }

    public async ValueTask DisposeAsync()
    {
        if (_isDisposed) return;

        if (_jsLiveRegionModule is not null) await _jsLiveRegionModule!.DisposeAsync();

        _navigationManager.LocationChanged -= NavigationManager_LocationChanged;

        _isDisposed = true;
    }
}

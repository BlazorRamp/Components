using BlazorRamp.Core.Common.Utilities;
using BlazorRamp.SkipTo.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace BlazorRamp.SkipTo.Components.SkipTo;

/// <summary>
/// An accessibility-first skip link component that allows keyboard users to bypass repetitive navigation content.
/// </summary>
public partial class SkipTo : IAsyncDisposable
{
    /// <summary>
    /// Gets or sets the text displayed by the skip link. 
    /// Defaults to "Skip to content".
    /// </summary>
    [Parameter] public string SkipToText     { get; set; } = GlobalValues.SkipTo_Text;

    /// <summary>
    /// Gets or sets the ID of the target element to move focus to. 
    /// Defaults to "main-content".
    /// </summary>
    [Parameter] public string TargetID       { get; set; }  = GlobalValues.SkipTo_Target_ID;

    /// <summary>
    /// Gets or sets whether the skip link is positioned for the entire page or a specific section. 
    /// Defaults to Page.
    /// <remarks>
    /// Only one Skip To at the Site level is supported. 
    /// You can use as many Section level ones on interactive pages as needed.
    /// </remarks>
    /// </summary>
    [Parameter] public SkipToType SkipToType { get; set; } = SkipToType.Site;

    /// <summary>
    /// Gets or sets a value indicating whether the skip link icon is visible. 
    /// Defaults to true.
    /// </summary>
    [Parameter] public bool IconVisible      { get; set; } = true;

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private IJSRuntime       JSRuntime          { get; set; } = default!;

    private IJSObjectReference? _skipToModule;

    private string _skipToText    = GlobalValues.SkipTo_Text;
    private string _targetID      = GlobalValues.SkipTo_Target_ID;
    private bool   _iconVisible   = true;
    private bool   _isInteractive = false;
    private bool   _disposed     = false;
    private string _relativeUrl = "";

    /// <summary>
    /// Updates the internal state of the component when parameters are assigned or changed state and formats the target identifier.
    /// </summary>
    protected override void OnParametersSet()
    {
        _skipToText  = String.IsNullOrWhiteSpace(SkipToText) ? GlobalValues.SkipTo_Text : SkipToText.Trim();
        _targetID    = String.IsNullOrWhiteSpace(TargetID) ? GlobalValues.SkipTo_Target_ID : TargetID.Trim();
        _targetID    = _targetID.StartsWith('#') ? _targetID : $"#{_targetID}";
        _iconVisible = IconVisible;
        _relativeUrl = CreateRelativeUrl(NavigationManager, _targetID);
    }

    private static string? BuildClassList(SkipToType skipToType)
    
        => skipToType == SkipToType.Section ? CoreUtilities.CreateClassList(GlobalValues.SkipTo_Class, GlobalValues.SkipTo_Container_Modifier) : GlobalValues.SkipTo_Class;


    private void NavigationManager_LocationChanged(object? sender, LocationChangedEventArgs e)
    {
        _relativeUrl = (new UriBuilder(e.Location).Path + _targetID).TrimStart('/');
        StateHasChanged();
    }

    private string CreateRelativeUrl(NavigationManager navigationManager, string targetID)
    
        => navigationManager.ToBaseRelativePath(NavigationManager.Uri) + targetID;

    private async Task HandleNavigation(string navigateTo, string targetID)
    {
        if (String.IsNullOrWhiteSpace(navigateTo)) return;

        NavigationManager.NavigateTo(navigateTo, false, false);

        if (_skipToModule is not null) await _skipToModule.InvokeVoidAsync(GlobalValues.JS_SkipTo_Scroll_Focus_Func, targetID.TrimStart('#'));
    }

    /// <summary>
    /// Initializes interactivity by importing the required JavaScript module and subscribing to location changes.
    /// </summary>
    /// <param name="firstRender">Indicates if this is the first time the component is being rendered.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        _isInteractive = true;

        if (true == firstRender)
        {
            NavigationManager.LocationChanged+=NavigationManager_LocationChanged;

            _skipToModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_SkipTo_File_Path);
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Performs asynchronous clean up by disposing of the JavaScript module, unsubscribing from events, and suppressing finalization.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> representing the completed disposal operation.</returns>
    public async ValueTask DisposeAsync()
    {
        if (_disposed == false)
        {
            if (_skipToModule is not null) await _skipToModule.DisposeAsync();

            NavigationManager.LocationChanged -= NavigationManager_LocationChanged;

            GC.SuppressFinalize(this);

            _disposed = true;
        }
    }
}

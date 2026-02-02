using BlazorRamp.BusyIndicator.Common.Constants;
using BlazorRamp.Core.Common.Constants;
using BlazorRamp.Core.Common.Extensions;
using BlazorRamp.Core.Common.Models;
using BlazorRamp.Core.Common.Utilities;
using BlazorRamp.Core.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorRamp.BusyIndicator.Components;
/// <summary>
/// A Busy indicator that provides visual and screen-reader feedback, making the content behind it (excluding the triggering button)
/// inert so it is not accessible by any user until the operation completes.
/// </summary>
public sealed partial class BusyIndicator : ComponentBase, IAsyncDisposable
{
    /// <summary>
    /// Gets or sets the optional text announced by screen readers when the busy state begins.
    /// </summary>
    [Parameter] public string?          AriaStartText    { get; set; } = String.Empty;

    /// <summary>
    /// Gets or sets the required text announced by screen readers when the busy state ends.
    /// A default value of "Operation Completed" will be used if not provided.
    /// </summary>
    [Parameter] public string           AriaEndText      { get; set; } = GlobalValues.Busy_Indicator_End_Text;
    
    /// <summary>
    /// Gets or sets the content to be wrapped by the busy indicator.
    /// </summary>
    [Parameter] public RenderFragment?  ChildContent     { get; set; } = default;
    
    /// <summary>
    /// Gets or sets the positioning of the overlay, either relative to its container or the entire screen.
    /// A default value of 0 (Container) will be used if not provided.
    /// </summary>
    [Parameter] public OverlayPosition  OverlayPosition  { get; set; } = OverlayPosition.Container;
    
    /// <summary>
    /// Gets or sets the vertical alignment of the indicator content (Top or Centre).
    /// A default value of 0 (Top) will be used if not provided.
    /// </summary>
    [Parameter] public ContentPosition  ContentPosition  { get; set; } = ContentPosition.Top;
    
    /// <summary>
    /// Gets or sets a value indicating whether the busy indicator is currently visible / started.
    /// </summary>
    [Parameter] public bool             ShowIndicator    { get; set; } = false;
    
    /// <summary>
    /// Gets or sets the optional (aria-hidden) visual text displayed alongside the 
    /// spinner or hour glass. 
    /// The default value is an empty string.
    /// </summary>
    [Parameter] public string?          BusyText         { get; set; } = String.Empty;
    
    /// <summary>
    /// Gets or sets the identifier or description for what triggered the indicator such as "Save Customer Button".
    /// This provides context for the screen reader user when reviewing the logs.
    /// To negate exceptions, an empty string is used if not provided.
    /// </summary>
    [Parameter] public string?          IndicatorTrigger { get; set; } = String.Empty;
    
    /// <summary>
    /// Gets or sets the maximum time in milliseconds the indicator remains visible before auto-dismissing.
    /// This is important as until dismissed all content beneath the indicator it will remain inert.
    /// A default value of 30 seconds is used if not provided.
    /// </summary>
    [Parameter] public int              DisplayTimeoutMS { get; set; } = GlobalValues.Busy_Indicator_Timeout_MS;
    
    /// <summary>
    /// Gets or sets the type of announcement made when the operation completes.
    /// A default value of 2 (OperationCompleted) is used if not provided. Currently this
    /// value is not exposed but may be utilised in the future to filter announcements.
    /// </summary>
    [Parameter] public AnnouncementType EndStatus        { get; set; } = AnnouncementType.OperationCompleted;
    
    /// <summary>
    /// An event that is fired  after the busy state has finished and the indicator is hidden.
    /// </summary>
    [Parameter] public EventCallback    OnBusyCompleted  { get; set; }

    [Inject] private IJSRuntime         JsRuntime         { get; set; } = default!;
    [Inject] private ILiveRegionService LiveRegionService { get; set; } = default!;

    private ElementReference BusyIndicatorRef { get; set; }

    private IJSObjectReference? _jsModule = default;

    private readonly string _ariaLiveID      = Guid.NewGuid().ToString();
    private readonly string _busyIndicatorID = Guid.NewGuid().ToString();
    
    private string  _busyClasses        = GlobalValues.Busy_Class;
    private string  _busyContentClasses = GlobalValues.Busy_Content_Class;
    private bool    _prevShowIndicator  = false;
    private string  _busyText           = String.Empty;
    private int     _timeOut            = GlobalValues.Busy_Indicator_Timeout_MS;
    private string  _ariaStartText      = GlobalValues.Busy_Indicator_Start_Text;
    private string  _ariaEndText        = GlobalValues.Busy_Indicator_End_Text;
    private string  _indicatorTrigger   = String.Empty;

    private bool _disposed = false;

    private OverlayPosition _overlayPosition = OverlayPosition.Container;

    /// <summary>
    /// Updates the internal state of the component when parameters are assigned or changed.
    /// This method normalizes text inputs, ensures the timeout is within valid bounds, 
    /// and regenerates the CSS class list based on current positioning values.
    /// </summary>
    protected override void OnParametersSet()
    {
        _busyClasses        = BuildBusyClasses(_overlayPosition, ContentPosition);
        _busyContentClasses = GlobalValues.Busy_Content_Class;
        _busyText           = String.IsNullOrWhiteSpace(BusyText) ? String.Empty : BusyText.Trim();
        _timeOut            = DisplayTimeoutMS <= 0 ? GlobalValues.Busy_Indicator_Timeout_MS : DisplayTimeoutMS;
        _indicatorTrigger   = String.IsNullOrWhiteSpace(IndicatorTrigger) ? String.Empty : IndicatorTrigger.Trim();
        _ariaStartText      = String.IsNullOrWhiteSpace(AriaStartText) ? String.Empty : AriaStartText.Trim();
        _ariaEndText        = String.IsNullOrWhiteSpace(AriaEndText)  ? GlobalValues.Busy_Indicator_End_Text : AriaEndText.Trim(); 

    }
    /// <summary>
    /// Captures the initial overlay position during component initialization 
    /// to prevent layout shifts if the parameter changes during the lifecycle.
    /// </summary>
    protected override void OnInitialized()
    { 
        /*
             * Don't allow change after initialisation. 
        */ 
        _overlayPosition  = OverlayPosition;
    }

    /// <summary>
    /// Manages the JavaScript interop and state transitions between busy and idle states.
    /// Handles the import of the JS module, triggers screen reader announcements, 
    /// and manages the delay before hiding the indicator to ensure full announcement delivery.
    /// </summary>
    /// <param name="firstRender">True if this is the first time the component is rendered.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (true == firstRender)
        {
            _jsModule          = await JsRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_File_Path);
            _prevShowIndicator = false;
            return;
        }

        if (_prevShowIndicator == ShowIndicator) return;

        _prevShowIndicator = ShowIndicator;

        var announcementText = ShowIndicator ? _ariaStartText : _ariaEndText;
      
        if (ShowIndicator)
        {
            await StartBusyIndicator(BusyIndicatorRef, GlobalValues.Busy_Display_Modifier, _timeOut, _overlayPosition);
            await MakeAnnouncement(announcementText, EndStatus, _indicatorTrigger, ShowIndicator);
        }
        else
        {

            await MakeAnnouncement(announcementText, EndStatus, _indicatorTrigger, ShowIndicator);
            
            //time needed to finish reading before ending otherwise SR may start reading content due to a focus event

            await Task.Delay(CoreGlobalValues.Live_Region_Delay_MS);

            await StopBusyIndicator(BusyIndicatorRef);

            if (OnBusyCompleted.HasDelegate) await OnBusyCompleted.InvokeAsync();
        }


    }
    /// <summary>
    /// Formats and dispatches an announcement to the Live Region Service.
    /// </summary>
    /// <param name="ariaText">The text to be read by the screen reader.</param>
    /// <param name="announcementType">The category of the announcement (e.g., OperationCompleted).</param>
    /// <param name="indicatorTrigger">The description of the element that triggered the busy state.</param>
    /// <param name="showing">Indicates if the indicator is currently active; forces 'OperationStarted' type if true.</param>
    /// <param name="liveRegionType">The politeness level of the live region (defaults to Assertive).</param>
    private async Task MakeAnnouncement(string ariaText, AnnouncementType announcementType, string indicatorTrigger,  bool showing, LiveRegionType liveRegionType = LiveRegionType.Assertive)
    {
        var announceType = showing ? AnnouncementType.OperationStarted : announcementType;
        var announcement = new Announcement(ariaText, announceType, indicatorTrigger, liveRegionType);

        await LiveRegionService.MakeAnnouncement(announcement);
    }

    private async ValueTask StartBusyIndicator(ElementReference busyIndicatorRef, string displayModifierClass, int displayTimeoutMs, OverlayPosition overlay)
    
        => await (_jsModule != null).WhenTrue(() => _jsModule!.InvokeVoidAsync(GlobalValues.JS_Start_Busy_Indicator, busyIndicatorRef, displayModifierClass, displayTimeoutMs, overlay.ToString().ToLower()));
    

    private async ValueTask StopBusyIndicator(ElementReference busyIndicatorRef)
    
        =>  await (_jsModule != null && !_disposed).WhenTrue(() => _jsModule!.InvokeVoidAsync(GlobalValues.JS_Stop_Busy_Indicator, busyIndicatorRef));

    /// <summary>
    /// Constructs the CSS class string based on the current positioning and alignment parameters.
    /// </summary>
    /// <param name="overlay">The positioning behaviour (Container or Screen).</param>
    /// <param name="content">The vertical/horizontal alignment of the spinner and text.</param>
    /// <returns>A space-separated string of CSS classes.</returns>
    private static string BuildBusyClasses(OverlayPosition overlay, ContentPosition content)
    {
        var classes = new List<string> { GlobalValues.Busy_Class };

        if (overlay == OverlayPosition.Container) classes.Add(GlobalValues.Busy_Fix_At_Container_Modifier);

        if (content == ContentPosition.Centre) classes.Add(GlobalValues.Busy_Centred_Modifier);

        return string.Join(" ", classes);
    }

    /// <summary>
    /// Performs asynchronous cleanup by disposing of the JavaScript module reference 
    /// and ensuring the indicator is marked as disposed to prevent further JS calls.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        /*
            * Needed the dispose flag for the stop method as when at container level the user could start a spinner and move to another page
            * only for the stop to be called after the module ref has been disposed.
        */ 
        if(_jsModule != null && !_disposed)
        {
            try
            {
                await _jsModule!.DisposeAsync();
            }
            catch { }// Circuit is disconnected (JSDisconnectedException), JS interop is no longer available - safe to ignore
        }

        _disposed = true;
    }

}

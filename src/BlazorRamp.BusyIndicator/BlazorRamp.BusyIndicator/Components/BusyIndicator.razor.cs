using BlazorRamp.BusyIndicator.Common.Constants;
using BlazorRamp.Core.Common.Constants;
using BlazorRamp.Core.Common.Extensions;
using BlazorRamp.Core.Common.Models;
using BlazorRamp.Core.Common.Utilities;
using BlazorRamp.Core.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorRamp.BusyIndicator.Components;

public sealed partial class BusyIndicator : ComponentBase, IAsyncDisposable
{
    [Parameter] public string           IndicatorLabel   { get; set; } = GlobalValues.Busy_Indicator_Label;
    [Parameter] public string?          AriaStartText    { get; set; } = String.Empty;
    [Parameter] public string           AriaEndText      { get; set; } = GlobalValues.Busy_Indicator_End_Text;
    [Parameter] public RenderFragment?  ChildContent     { get; set; } = default;
    [Parameter] public OverlayPosition  OverlayPosition  { get; set; } = OverlayPosition.Container;
    [Parameter] public ContentPosition  ContentPosition  { get; set; } = ContentPosition.Top;
    [Parameter] public bool             ShowIndicator    { get; set; } = false;
    [Parameter] public string?          BusyText         { get; set; } = String.Empty;
    [Parameter] public string?          IndicatorTrigger { get; set; } = String.Empty;
    [Parameter] public int              DisplayTimeoutMS { get; set; } = GlobalValues.Busy_Indicator_Timeout_MS;
    [Parameter] public StyleAs          StyleAs          { get; set; } = StyleAs.Dynamic;
    [Parameter] public AnnouncementType EndStatus        { get; set; } = AnnouncementType.OperationCompleted;

    [Parameter] public EventCallback    OnBusyCompleted  { get; set; }

    [Inject] private IJSRuntime         JsRuntime         { get; set; } = default!;
    [Inject] private ILiveRegionService LiveRegionService { get; set; } = default!;

    private ElementReference BusyIndicatorRef { get; set; }

    private IJSObjectReference? _jsModule = default;

    private readonly string _ariaLiveID      = Guid.NewGuid().ToString();
    private readonly string _busyIndicatorID = Guid.NewGuid().ToString();
    
    private string  _busyClasses        = GlobalValues.Busy_Class;
    private string  _busyContentClasses = GlobalValues.Busy_Content_Class;
    private string  _indicatorLabel     = GlobalValues.Busy_Indicator_Label;
    private bool    _prevShowIndicator  = false;
    private string  _busyText           = String.Empty;
    private int     _timeOut            = GlobalValues.Busy_Indicator_Timeout_MS;
    private string  _ariaStartText      = GlobalValues.Busy_Indicator_Start_Text;
    private string  _ariaEndText        = GlobalValues.Busy_Indicator_End_Text;
    private string? _styleAs            = null;
    private string  _indicatorTrigger   = String.Empty;

    private bool _disposed = false;

    private OverlayPosition _overlayPosition = OverlayPosition.Container;

    protected override void OnParametersSet()
    {
        _busyClasses        = BuildBusyClasses(_overlayPosition, ContentPosition);
        _busyContentClasses = GlobalValues.Busy_Content_Class;
        _busyText           = String.IsNullOrWhiteSpace(BusyText) ? String.Empty : BusyText.Trim();
        _timeOut            = DisplayTimeoutMS <= 0 ? GlobalValues.Busy_Indicator_Timeout_MS : DisplayTimeoutMS;
        _indicatorTrigger   = String.IsNullOrWhiteSpace(IndicatorTrigger) ? String.Empty : IndicatorTrigger.Trim();
        _ariaStartText      = String.IsNullOrWhiteSpace(AriaStartText) ? String.Empty : AriaStartText.Trim();
        _ariaEndText        = String.IsNullOrWhiteSpace(AriaEndText)  ? GlobalValues.Busy_Indicator_End_Text : AriaEndText.Trim(); 

        _styleAs           = CoreUtilities.GetStyleAsValue(StyleAs);
    }

    protected override void OnInitialized()
    { 
        /*
             * Don't allow change after initialisation. 
        */ 
        _overlayPosition  = OverlayPosition;
        _indicatorLabel   = String.IsNullOrWhiteSpace(IndicatorLabel) ? GlobalValues.Busy_Indicator_Label : IndicatorLabel.Trim();
    }
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
            //time needed to finish reading before ending otherwise SR may start reading content due to a focus event
            await MakeAnnouncement(announcementText, EndStatus, _indicatorTrigger, ShowIndicator);

            await Task.Delay(CoreGlobalValues.Live_Region_Delay_MS);

            await StopBusyIndicator(BusyIndicatorRef);

            if (OnBusyCompleted.HasDelegate) await OnBusyCompleted.InvokeAsync();
        }


    }

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
        

    private static string BuildBusyClasses(OverlayPosition overlay, ContentPosition content)
    {
        var classes = new List<string> { GlobalValues.Busy_Class };

        if (overlay == OverlayPosition.Container) classes.Add(GlobalValues.Busy_Fix_At_Container_Modifier);

        if (content == ContentPosition.Centre) classes.Add(GlobalValues.Busy_Centred_Modifier);

        return string.Join(" ", classes);
    }


    public async ValueTask DisposeAsync()
    {
        /*
            * Needed the dispose flag for the stop method as when at container level the user could start a spinner and move to another page
            * only for the stop to be called after the module ref has been disposed.
        */ 
        await (_jsModule != null && !_disposed).WhenTrue(() => _jsModule!.DisposeAsync());

        _disposed = true;
    }






}

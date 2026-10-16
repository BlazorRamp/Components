
using BlazorRamp.GoTo.Common.Constants;
using BlazorRamp.GoTo.Common.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorRamp.GoTo.Components;

public partial class GoToViewer :IAsyncDisposable
{

    [Parameter] public string CloseButtonText   { get; set; } = GlobalValues.GoTo_Close_Button_Text;
    [Parameter] public string NoGotoMarkersText { get; set; } = GlobalValues.GoTo_No_Markers_Text;
    [Parameter] public string HeadingText       { get; set; } = GlobalValues.GoTo_Heading_Text;

    [Inject] IJSRuntime JSRuntime { get; set; }

    private DotNetObjectReference<GoToViewer>? _dotNetObjectRef = default;
    private IJSObjectReference _jSModule = null;
    private List<GoToData> _gotoList     = [];
    private string         _gotoViewerID = Guid.NewGuid().ToString();

    private string _closeButtonText   = GlobalValues.GoTo_Close_Button_Text;
    private string _noGoToMarkersText = GlobalValues.GoTo_No_Markers_Text;
    private string _headingText       = GlobalValues.GoTo_Heading_Text;
    private string _headingTextID    = Guid.NewGuid().ToString();

    protected override void OnParametersSet()
    {
        _closeButtonText   = String.IsNullOrWhiteSpace(CloseButtonText)   ? GlobalValues.GoTo_Close_Button_Text : CloseButtonText.Trim();
        _noGoToMarkersText = string.IsNullOrWhiteSpace(NoGotoMarkersText) ? GlobalValues.GoTo_No_Markers_Text   : NoGotoMarkersText.TrimEnd();
        _headingText       = string.IsNullOrWhiteSpace(HeadingText)      ? GlobalValues.GoTo_Heading_Text   : HeadingText.TrimEnd(); ;
    }


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (true == firstRender)
        {
            _dotNetObjectRef = DotNetObjectReference.Create(this);
            _jSModule        = await JSRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_Module_File_Path);

            await _jSModule.InvokeVoidAsync(GlobalValues.JS_Register_GoTo_Component_Func, _dotNetObjectRef, nameof(HandleJSCallBack), _gotoViewerID);

        }
    }
    

    private async Task ScrollToMarker(string markerID)
    {
        if (_jSModule is not null) await _jSModule.InvokeVoidAsync(GlobalValues.JS_Scroll_Marker_Func, markerID);
    }
        

    private async Task GoToMarker(GoToData gotoItem)
    {
        await _jSModule.InvokeVoidAsync(GlobalValues.JS_ShowPopover_Func, gotoItem.MarkerID, gotoItem.PopoverID);
    }

    private async Task CloseGotoViewer(string gotoViewID)
    {
        if (_jSModule is not null) await _jSModule.InvokeVoidAsync(GlobalValues.JS_Close_GoTo_Viewer_Func, gotoViewID);
    }

    [JSInvokable]
    public async Task HandleJSCallBack(GoToData[] goToData)
    {
        _gotoList.Clear();
        _gotoList.AddRange(goToData);
        await InvokeAsync(StateHasChanged);
        await _jSModule.InvokeVoidAsync(GlobalValues.JS_Show_GoTo_Viewer_Func, _gotoViewerID);

    }
    /// <summary>
    /// Releases the JS module reference.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_jSModule is not null)
        {
            try
            {
                await _jSModule.DisposeAsync();
            }
            catch { }
        }
    }
}

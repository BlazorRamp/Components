
using BlazorRamp.GoTo.Common.Constants;
using BlazorRamp.GoTo.Common.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorRamp.GoTo.Components;

public partial class GoToViewer :IAsyncDisposable
{

    [Parameter] public string CloseButtonText { get; set; } = GlobalValues.GoTo_Close_Button_Text;

    [Inject] IJSRuntime JSRuntime { get; set; }

    private DotNetObjectReference<GoToViewer>? _dotNetObjectRef = default;
    private IJSObjectReference _jSModule = null;
    private List<GoToData> _gotoList     = [];
    private string         _gotoViewerID = Guid.NewGuid().ToString();

    private string _closeButtonText = GlobalValues.GoTo_Close_Button_Text;

    protected override void OnParametersSet()
    {
        _closeButtonText = String.IsNullOrWhiteSpace(CloseButtonText) ? GlobalValues.GoTo_Close_Button_Text : CloseButtonText.Trim();
    }


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (true == firstRender)
        {
            _dotNetObjectRef = DotNetObjectReference.Create(this);
            _jSModule        = await JSRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_Module_File_Path);

            await _jSModule.InvokeVoidAsync(GlobalValues.JS_Register_ShortCut_CallBack_Func, _dotNetObjectRef, nameof(HandleJSCallBack));

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


using BlazorRamp.GoTo.Common.Constants;
using BlazorRamp.GoTo.Common.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorRamp.GoTo.Components;

/// <summary>
/// The viewer that lists the <see cref="GoTo"/> markers on the current page and moves focus to the one
/// the user chooses. It opens with <c>Ctrl+Shift+Y</c> or through a trigger button that is only visible
/// while it has keyboard focus. Add it once, to a layout or router component, using an interactive render mode.
/// </summary>
public partial class GoToViewer :IAsyncDisposable
{
    /// <summary>
    /// Gets or sets the text of the viewer's close button. If <see langword="null"/>, empty or whitespace,
    /// the default text is used.
    /// </summary>
    [Parameter] public string CloseButtonText   { get; set; } = GlobalValues.GoTo_Close_Button_Text;

    /// <summary>
    /// Gets or sets the message shown when the page has no markers. If <see langword="null"/>, empty or
    /// whitespace, the default text is used.
    /// </summary>
    [Parameter] public string NoGoToMarkersText { get; set; } = GlobalValues.GoTo_No_Markers_Text;

    /// <summary>
    /// Gets or sets the heading of the viewer, which also provides its accessible name. If <see langword="null"/>,
    /// empty or whitespace, the default text is used.
    /// </summary>
    [Parameter] public string HeadingText       { get; set; } = GlobalValues.GoTo_Heading_Text;

    /// <summary>
    /// Gets or sets the text of the trigger button. It should mention the keyboard shortcut. If <see langword="null"/>,
    /// empty or whitespace, the default text is used.
    /// </summary>
    [Parameter] public string TriggerButtonText  { get; set; } = GlobalValues.GoTo_Trigger_Button_Text;

    /// <summary>
    /// Gets or sets a value indicating whether the trigger button is left out. When <see langword="true"/>,
    /// the viewer can only be opened with the keyboard shortcut. Defaults to <see langword="false"/>.
    /// </summary>
    [Parameter] public bool ExcludeTriggerButton { get; set; } = false;


    /// <summary>
    /// Gets or sets the keyboard shortcut that opens the viewer. It must be a single letter or digit combined with at least one of
    /// Alt, Ctrl or Meta, so that it isn't a character key shortcut under WCAG 2.1.4. If it is <see langword="null"/> or not valid,
    /// it is ignored and the previous shortcut stays in place. Defaults to Alt+G (Option+G on macOS).
    /// </summary>
    [Parameter] public GoToShortcut GoToShortcut { get; set; } = GlobalValues.GoTo_Default_Shortcut_Model;

    [Inject] IJSRuntime JSRuntime { get; set; } = default!;

    private DotNetObjectReference<GoToViewer>? _dotNetObjectRef = default;
    private IJSObjectReference? _jSModule = null;
    private List<GoToData> _gotoList     = [];
    private string         _gotoViewerID = Guid.NewGuid().ToString();

    private string _closeButtonText   = GlobalValues.GoTo_Close_Button_Text;
    private string _noGoToMarkersText = GlobalValues.GoTo_No_Markers_Text;
    private string _headingText       = GlobalValues.GoTo_Heading_Text;
    private string _headingTextID     = Guid.NewGuid().ToString();
    private string _triggerButtonText = GlobalValues.GoTo_Trigger_Button_Text;

    private GoToShortcut _goToShortcut = GlobalValues.GoTo_Default_Shortcut_Model;
    private GoToShortcut _lastShortcut = GlobalValues.GoTo_Default_Shortcut_Model;

    /// <summary>
    /// Replaces any blank text parameters with their defaults, trims the others, and takes the shortcut if it is valid.
    /// </summary>
    protected override void OnParametersSet()
    {
        _closeButtonText   = String.IsNullOrWhiteSpace(CloseButtonText)   ? GlobalValues.GoTo_Close_Button_Text : CloseButtonText.Trim();
        _noGoToMarkersText = String.IsNullOrWhiteSpace(NoGoToMarkersText) ? GlobalValues.GoTo_No_Markers_Text   : NoGoToMarkersText.Trim();
        _headingText       = String.IsNullOrWhiteSpace(HeadingText)       ? GlobalValues.GoTo_Heading_Text   : HeadingText.Trim(); 

        if(GoToShortcut is not null && GoToShortcut.IsValid())
        {
            _goToShortcut = GoToShortcut with { Key = GoToShortcut.Key, Shift = GoToShortcut.Shift, Alt = GoToShortcut.Alt, Meta = GoToShortcut.Meta };
        }

        _triggerButtonText = String.IsNullOrWhiteSpace(TriggerButtonText) ? GlobalValues.GoTo_Trigger_Button_Text : TriggerButtonText.Trim();

        _triggerButtonText = $"{_triggerButtonText} ({_goToShortcut.ToDisplayText()})";
    }

    /// <summary>
    /// On first render, loads the JavaScript module and registers the keyboard shortcut and the viewer's
    /// arrow-key handling.
    /// </summary>
    /// <param name="firstRender">
    /// <see langword="true"/> if this is the first time the component has rendered; otherwise <see langword="false"/>.
    /// </param>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (true == firstRender)
        {
            _dotNetObjectRef = DotNetObjectReference.Create(this);
            _jSModule        = await JSRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_Module_File_Path);

            await _jSModule.InvokeVoidAsync(GlobalValues.JS_Register_GoTo_Component_Func, _dotNetObjectRef, nameof(HandleJSCallBack), _gotoViewerID, _goToShortcut);
            _lastShortcut = _goToShortcut;
        }

        if (_jSModule is not null && _goToShortcut != _lastShortcut)
        {
            _lastShortcut = _goToShortcut;
            await _jSModule.InvokeVoidAsync(GlobalValues.JS_Set_Shortcut_key_Func, _goToShortcut);
        }
    }

    /// <summary>
    /// Builds the list of markers currently on the page and shows the viewer. Called by the trigger button.
    /// </summary>
    /// <param name="gotoViewID">The id of the viewer element.</param>
    private async Task HandleOpenViewer(string gotoViewID)
    {
        if (_jSModule is not null)
        {
            _gotoList.Clear();

            var data = await _jSModule.InvokeAsync<GoToData[]>(GlobalValues.JS_Get_GoToData_Func);
           _gotoList.AddRange(data);
            await InvokeAsync(StateHasChanged);
            await _jSModule.InvokeVoidAsync(GlobalValues.JS_Show_GoTo_Viewer_Func, gotoViewID);
        }
    }

    /// <summary>
    /// Scrolls the marker into view and marks it as the current one while its list button has focus.
    /// </summary>
    /// <param name="markerID">The id of the marker element.</param>
    private async Task ScrollToMarker(string markerID)
    {
        if (_jSModule is not null) await _jSModule.InvokeVoidAsync(GlobalValues.JS_Scroll_Marker_Func, markerID);
    }

    /// <summary>
    /// Moves focus to the chosen marker and shows its label, which also closes the viewer.
    /// </summary>
    /// <param name="gotoItem">The list entry that was chosen.</param>
    private async Task GoToMarker(GoToData gotoItem)
    {
        if (_jSModule is not null) await _jSModule.InvokeVoidAsync(GlobalValues.JS_ShowPopover_Func, gotoItem.MarkerID, gotoItem.PopoverID);
    }

    /// <summary>
    /// Closes the viewer.
    /// </summary>
    /// <param name="gotoViewID">The id of the viewer element.</param>
    private async Task CloseGotoViewer(string gotoViewID)
    {
        if (_jSModule is not null) await _jSModule.InvokeVoidAsync(GlobalValues.JS_Close_GoTo_Viewer_Func, gotoViewID);
    }

    /// <summary>
    /// Called from JavaScript when the keyboard shortcut is pressed. Replaces the list with the markers
    /// found on the page, renders it and then shows the viewer.
    /// </summary>
    /// <param name="goToData">The markers found on the page, in document order.</param>
    [JSInvokable]
    public async Task HandleJSCallBack(GoToData[] goToData)
    {
        _gotoList.Clear();
        _gotoList.AddRange(goToData);
        await InvokeAsync(StateHasChanged);
        if (_jSModule is not null) await _jSModule.InvokeVoidAsync(GlobalValues.JS_Show_GoTo_Viewer_Func, _gotoViewerID);

    }

    /// <summary>
    /// Releases the JavaScript module reference. Any exception, such as a disconnected circuit, is ignored.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_jSModule is not null)
        {
            try
            {
                await _jSModule.DisposeAsync();
                _dotNetObjectRef?.Dispose();
            }
            catch { }
        }
    }
}

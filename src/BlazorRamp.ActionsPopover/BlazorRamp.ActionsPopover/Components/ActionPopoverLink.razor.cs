using BlazorRamp.ActionsPopover.Common.Constants;
using BlazorRamp.ActionsPopover.Common.Models;
using BlazorRamp.ActionsPopover.Common.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorRamp.ActionsPopover.Components;

public partial class ActionPopoverLink<TData> : ComponentBase, IAsyncDisposable
{
    
    [Parameter] public string     LinkText       { get; set; }
    [Parameter] public TData?     ItemData       { get; set; }
    [Parameter] public TargetType TargetType     { get; set; } = TargetType.Self;
    [Parameter] public string     Path           { get; set; } = "/";
    [Parameter] public bool       PreventDefault { get; set; } = false;

    /// <summary>
    /// Gets or sets the name of a CSS custom property that resolves to an SVG data URI,
    /// used as a mask-image icon next to the item text. Must begin with <c>--</c>.
    /// For example: <c>--svg-my-icon</c>.
    /// </summary>
    [Parameter] public string? SvgIcon { get; set; } = default;

    /// <summary>
    /// Invoked when the action button is clicked. Unlike <see cref="EventCallback"/>, this does
    /// <b>not</b> automatically trigger a re-render of the consuming component — call
    /// <c>StateHasChanged()</c> yourself if the action needs to update the UI.
    /// </summary>
    [Parameter] public Func<LinkActionData<TData>, Task>? OnClick { get; set; }

    [CascadingParameter(Name = $"{GlobalValues.Actions_Popover_Panel_Cascading_ID_Name}")]
    private string? PopoverID { get; set; }

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
    private IJSObjectReference? _jSModule = null;

    private ElementReference AnchorElementRef { get; set; }

    private string? _svgIcon        = null;
    private bool    _preventDefault = false;
    protected override void OnParametersSet()
    {
        if (true == String.IsNullOrWhiteSpace(LinkText)) throw new ArgumentNullException(nameof(LinkText), GlobalValues.Actions_Popover_Link_Text_Exception_Message);
        _svgIcon = GeneralHelpers.CheckSetSvgVariable(SvgIcon, GlobalValues.Actions_Popover_Action_Icon_Svg_Css_Variable_Name);
        base.OnParametersSet();
    }

    protected override void OnInitialized()
    {
        _preventDefault = PreventDefault;
    }
    private async Task RaiseOnClick(string linkText, TData? itemData, TargetType targetType, string path)
    {
        var actionData = CreateActionData(linkText, itemData, targetType, path);

        if (_jSModule != null) await _jSModule.InvokeVoidAsync(GlobalValues.JS_Hide_Popover_Func, PopoverID);

        if (OnClick is not null) await OnClick.Invoke(actionData);
    }

    private static LinkActionData<TData> CreateActionData(string linkText, TData? payload, TargetType targetType, string path)

        => new(linkText, payload, targetType, path);

    /// <summary>
    /// Imports the JavaScript module and registers the prevent click action handler 
    /// to stop the default href action as we are using async not sync
    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {

        if (true == firstRender)
        {
            _jSModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_File_Path);

            
            if (_jSModule is not null && true == _preventDefault ) await _jSModule.InvokeVoidAsync(GlobalValues.JS_Register_Prevent_Click_Action_Handler, AnchorElementRef);

        }
    }

    /// <summary>
    /// Releases the JavaScript module reference and unregisters the prevent click action handler registered during
    /// <c>OnAfterRenderAsync</c>.
    /// </summary>
    public async ValueTask DisposeAsync()
    {

        if (_jSModule is not null)
        {
            try
            {
                if(true == _preventDefault )await _jSModule.InvokeVoidAsync(GlobalValues.JS_Unregister_Prevent_Click_Action_Handler);
                await _jSModule.DisposeAsync();

            }
            catch { }
        }

    }
}

using BlazorRamp.ActionsPopover.Common.Constants;
using BlazorRamp.ActionsPopover.Common.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Reflection.Emit;

namespace BlazorRamp.ActionsPopover.Components;

partial class ActionsPopover : IAsyncDisposable
{

    [Parameter] public RenderFragment? PopoverItems { get; set; }

    [Parameter] public string TriggerText { get; set; } = GlobalValues.Actions_Popover_Trigger_Text!;

    /// <summary>
    /// Gets or sets the position of the popover relative to the trigger.
    /// Defaults to <see cref="ActionsPopoverPosition.BottomLeft"/>.
    /// </summary>
    [Parameter] public ActionsPopoverPosition ActionsPopoverPosition { get; set; } = ActionsPopoverPosition.BottomLeft;


    /// <summary>
    /// Gets or sets the name of a CSS custom property that resolves to an SVG data URI,
    /// used as a mask-image icon next to the item text. Must begin with <c>--</c>.
    /// For example: <c>--svg-my-icon</c>.
    /// </summary>
    [Parameter] public string? SvgIcon { get; set; } = default;

    private ElementReference PopoverElementRef   { get; set; }
    private ElementReference ContainerElementRef { get; set; }

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
    private IJSObjectReference? _jSModule = null;

    private string _popoverPanelID  = Guid.NewGuid().ToString();
    private string _triggerTextID   = Guid.NewGuid().ToString();
    private string _popoverPosition = "bottom-left";
    private string? _svgIcon        = null;

    protected override void OnParametersSet()
    {
        _svgIcon = GeneralHelpers.CheckSetSvgVariable(SvgIcon, GlobalValues.Actions_Popover_Trigger_Icon_Svg_Css_Variable_Name);
    }
    protected override void OnInitialized()
    {

        _popoverPosition = GetActionsPopoverPositionFromEnum(ActionsPopoverPosition);
    }

    /// <summary>
    /// Imports the JavaScript module and registers the focus-out handler used close the 
    /// popover when just tabbing away from the actions list
    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {

        if (true == firstRender)
        {
            _jSModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_File_Path);

           // if (_jSModule is not null) await _jSModule.InvokeVoidAsync(GlobalValues.JS_Register_Focus_Out_Handler,ContainerElementRef, PopoverElementRef);

        }
    }

    internal string GetActionsPopoverPositionFromEnum(ActionsPopoverPosition popoverPosition)

        => popoverPosition switch
        {
            ActionsPopoverPosition.TopCentre => "top-centre",
            ActionsPopoverPosition.TopLeft => "top-left",
            ActionsPopoverPosition.TopRight => "top-right",
            ActionsPopoverPosition.CentreLeft => "centre-left",
            ActionsPopoverPosition.CentreRight => "centre-right",
            ActionsPopoverPosition.BottomCentre => "bottom-centre",
            ActionsPopoverPosition.BottomLeft => "bottom-left",
            ActionsPopoverPosition.BottomRight => "bottom-right",
            _ => "bottom-left",

        };




    /// <summary>
    /// Releases the JavaScript module reference and unregisters the focus-out handler registered during
    /// <c>OnAfterRenderAsync</c>.
    /// </summary>
    public async ValueTask DisposeAsync()
    {

        if (_jSModule is not null)
        {
            try
            {
                await _jSModule.InvokeVoidAsync(GlobalValues.JS_Unregister_Focus_Out_Handler);
                await _jSModule.DisposeAsync();

            }
            catch { }
        }

    }
}

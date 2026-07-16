using BlazorRamp.ActionsPopover.Common.Constants;
using BlazorRamp.ActionsPopover.Common.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorRamp.ActionsPopover.Components;

/// <summary>
/// A trigger button that opens a popover panel containing a list of actions — for example,
/// a row-actions button in a data table offering Edit, Delete, or View.
/// </summary>
partial class ActionsPopover : IAsyncDisposable
{
    /// <summary>
    /// Gets or sets the action items rendered inside the popover panel, typically
    /// a combination of <see cref="ActionPopoverButton{TData}"/>, <see cref="ActionPopoverLink{TData}"/>,
    /// and <see cref="ActionPopoverSeparator"/>.
    /// </summary>
    [Parameter] public RenderFragment? PopoverItems { get; set; }

    /// <summary>
    /// Gets or sets the text displayed on the trigger button. Defaults to <c>"Actions"</c>.
    /// </summary>
    [Parameter] public string TriggerText { get; set; } = GlobalValues.Actions_Popover_Trigger_Text!;

    /// <summary>
    /// Gets or sets whether the trigger should stretch to fill the width of its container,
    /// rather than shrinking to fit its own content.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>false</c>, so the trigger sizes to its content by default. Set to
    /// <c>true</c> for contexts like table rows, where every trigger in a column should
    /// share the same width — combined with the browser's own table column sizing, the
    /// widest trigger in the column determines the width, and <see cref="Stretch"/> makes
    /// every other row's trigger fill that same width rather than staying narrower.
    /// </remarks>
    [Parameter] public bool Stretch { get; set; } = false;

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

    /// <summary>
    /// Gets or sets additional attributes that will be applied to the components button element,
    /// the popover trigger.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private ElementReference PopoverElementRef   { get; set; }
    private ElementReference ContainerElementRef { get; set; }

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
    private IJSObjectReference? _jSModule = null;

    private string _popoverPanelID  = Guid.NewGuid().ToString();
    private string _triggerTextID   = Guid.NewGuid().ToString();
    private string _popoverPosition = "bottom-left";
    private string? _svgIcon        = null;
    private string _triggerText     = GlobalValues.Actions_Popover_Trigger_Text; 

    /// <summary>
    /// Resolves <see cref="SvgIcon"/> into a CSS custom property style.
    /// </summary>
    protected override void OnParametersSet()
    {
        _svgIcon          = GeneralHelpers.CheckSetSvgVariable(SvgIcon, GlobalValues.Actions_Popover_Trigger_Icon_Svg_Css_Variable_Name);
        _triggerText     = String.IsNullOrWhiteSpace(TriggerText) ?  GlobalValues.Actions_Popover_Trigger_Text : TriggerText.Trim();
        _popoverPosition = GetActionsPopoverPositionFromEnum(ActionsPopoverPosition);
        base.OnParametersSet();
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

            if (_jSModule is not null) await _jSModule.InvokeVoidAsync(GlobalValues.JS_Register_Focus_Out_Handler,ContainerElementRef, PopoverElementRef);

        }
    }

    private string GetActionsPopoverPositionFromEnum(ActionsPopoverPosition popoverPosition)

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
                await _jSModule.InvokeVoidAsync(GlobalValues.JS_Unregister_Focus_Out_Handler, ContainerElementRef, PopoverElementRef);
                await _jSModule.DisposeAsync();

            }
            catch { }
        }

    }
}

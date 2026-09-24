
using BlazorRamp.Tooltip.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorRamp.Tooltip.Components;

/// <summary>
/// A hover/focus-triggered tooltip built on the Popover API. Wraps <see cref="ChildContent"/>
/// and shows <see cref="TooltipText"/> in a positioned popup when the trigger is hovered or focused.
/// </summary>
public partial class Tooltip: IAsyncDisposable
{
    /// <summary>
    /// Gets or sets the content the tooltip is attached to (the trigger).
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; } = null;

    /// <summary>
    /// Gets or sets the text displayed inside the tooltip.
    /// </summary>
    [Parameter, EditorRequired] public string TooltipText { get; set; } = default!;

    /// <summary>
    /// Gets or sets whether the tooltip uses its inverted colour scheme.
    /// </summary>
    [Parameter] public bool InvertColours { get; set; } = false;

    /// <summary>
    /// Gets or sets the unique id used for the tooltip element, so the consumer can
    /// reference it via <c>aria-describedby</c> on the trigger element.
    /// </summary>
    [Parameter, EditorRequired] public string TooltipID { get; set; } = default!;

    /// <summary>
    /// Gets or sets the position of the popover relative to the trigger.
    /// Defaults to <see cref="TooltipPosition.TopCentre"/>.
    /// </summary>
    [Parameter] public TooltipPosition TooltipPosition { get; set; } = TooltipPosition.TopCentre;

    /// <summary>
    /// Gets or sets additional attributes that will be applied to the container element,
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private IJSObjectReference? _jSModule = null;    
    private string _containerID = Guid.NewGuid().ToString();
    private string _tooltipID = String.Empty;

    private string _tooltipPosition = "top-centre";

    private string _formattedText = String.Empty;

    /// <summary>
    /// Validates <see cref="TooltipText"/> and recomputes the formatted text and CSS position
    /// modifier whenever <see cref="TooltipText"/> or <see cref="TooltipPosition"/> changes.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if <see cref="TooltipText"/> is null, empty, or whitespace.</exception>
    protected override void OnParametersSet()
    {
        if (String.IsNullOrWhiteSpace(TooltipText)) throw new ArgumentNullException(nameof(TooltipText), "TooltipText cannot be null, empty, or whitespace.");

        _formattedText   = TooltipText.Trim().Replace("\r\n", "\n");
        _tooltipPosition = GetTooltipPopoverPositionFromEnum(TooltipPosition);
    }
    /// <summary>
    /// Validates that <see cref="TooltipID"/> has been supplied and captures the id for use in markup.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if <see cref="TooltipID"/> is null, empty, or whitespace.</exception>
    protected override void OnInitialized()
    {
        if (String.IsNullOrWhiteSpace(TooltipID)) throw new ArgumentNullException(nameof(TooltipID), "TooltipID cannot be null, empty, or whitespace.");
        _tooltipID = TooltipID;
    }

    /// <summary>
    /// Imports the tooltip JS module and registers this instance's event handlers on first render.
    /// </summary>

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
       if(true == firstRender)
        {
            _jSModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_Module_File_Path);

            if (_jSModule is not null) await _jSModule.InvokeVoidAsync(GlobalValues.JS_Register_Tooltip_Func, _containerID, _tooltipID);
            
        }
    }
    /// <summary>
    /// Closes any currently open tooltips. Invoked by the pointer-only close control.
    /// </summary>
    private async Task CloseTooltip()
    {
        if (_jSModule is not null) await _jSModule.InvokeVoidAsync(GlobalValues.JS_Close_Open_Tooltips_Func);
    }


    private string GetContentAreaClasses(bool invertedColours)

        => $"{GlobalValues.Tooltip_Content_Area_class}{(invertedColours? " " + GlobalValues.Tooltip_Content_Area_Modifier : "")}";

    private string GetTooltipPopoverPositionFromEnum(TooltipPosition tooltipPosition)

        => tooltipPosition switch
        {
            TooltipPosition.TopCentre    => "top-centre",
            TooltipPosition.TopLeft      => "top-left",
            TooltipPosition.TopRight     => "top-right",
            TooltipPosition.CentreLeft   => "centre-left",
            TooltipPosition.CentreRight  => "centre-right",
            TooltipPosition.BottomCentre => "bottom-centre",
            TooltipPosition.BottomLeft   => "bottom-left",
            TooltipPosition.BottomRight  => "bottom-right",
            _ => "top-centre",

        };

    /// <summary>
    /// Unregisters this instance's JS event handlers and releases the JS module reference.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_jSModule is not null)
        {
            try
            {
                await _jSModule.InvokeVoidAsync(GlobalValues.JS_Unregister_Tooltip_Func, _containerID);
                await _jSModule.DisposeAsync();
            }
            catch { }
        }
    }
}

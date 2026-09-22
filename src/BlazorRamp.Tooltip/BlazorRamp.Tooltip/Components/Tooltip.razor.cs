
using BlazorRamp.Tooltip.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Runtime.InteropServices;

namespace BlazorRamp.Tooltip.Components;

public partial class Tooltip: IAsyncDisposable
{
    [Parameter] public RenderFragment? ChildContent  { get; set; } = null;
    [Parameter] public string          TooltipText   { get; set; } = String.Empty;
    [Parameter] public bool            InvertColours { get; set; } =false;

    [Parameter] public string TooltipID { get; set; }

    /// <summary>
    /// Gets or sets the position of the popover relative to the trigger.
    /// Defaults to <see cref="TooltipPosition.TopCentre"/>.
    /// </summary>
    [Parameter] public TooltipPosition TooltipPosition { get; set; } = TooltipPosition.TopCentre;

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private IJSObjectReference? _jSModule = null;    
    private string _containerID = Guid.NewGuid().ToString();
    private string _tooltipID = String.Empty;

    private string _tooltipPosition = "top-centre";


    protected override void OnParametersSet()
    
        =>  _tooltipPosition = GetTooltipPopoverPositionFromEnum(TooltipPosition);
    

    protected override void OnInitialized()
    {
       if(String.IsNullOrWhiteSpace(TooltipID)) throw new ArgumentNullException(nameof(TooltipID));

        _tooltipID = TooltipID;
    }

    

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
       if(true == firstRender)
        {
            _jSModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_Module_File_Path);

            if (_jSModule is not null) await _jSModule.InvokeVoidAsync(GlobalValues.JS_Register_Tooltip_Func, _containerID, _tooltipID);
            
        }
    }

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

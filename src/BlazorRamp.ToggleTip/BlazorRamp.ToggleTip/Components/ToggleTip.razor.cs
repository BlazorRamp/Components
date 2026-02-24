using BlazorRamp.ToggleTip.Common.Constants;
using Microsoft.AspNetCore.Components;
using System.Reflection.Metadata;
namespace BlazorRamp.ToggleTip.Components;

public partial class ToggleTip
{
    [Parameter] public string Label     { get; set; } = GlobalValues.ToggleTip_Label;
    [Parameter] public string CloseText { get; set; } = GlobalValues.ToggleTip_Close_Text;
    [Parameter] public bool   ShowLabel { get; set; } = true;
    [Parameter] public bool   ShowClose { get; set; } = true;

    [Parameter] public ToggleTipLabelOrder ToggleTipLabelOrder { get; set; } = ToggleTipLabelOrder.LabelFirst;
    [Parameter] public ToggleTipPosition   ToggleTipPosition   { get; set; } = ToggleTipPosition.TopCentre;

    private string _contentID         = Guid.NewGuid().ToString();
    private string _label             = GlobalValues.ToggleTip_Label;
    private string _closeText         = GlobalValues.ToggleTip_Close_Text;
    private string _toggleTipPosition = "top-centre";


    protected override void OnInitialized()
    {
        _label             = String.IsNullOrWhiteSpace(Label) ? GlobalValues.ToggleTip_Label : Label.Trim();
        _toggleTipPosition = GetToggleTipPositionFromEnum(ToggleTipPosition); 
        _closeText         = String.IsNullOrWhiteSpace(CloseText) ? GlobalValues.ToggleTip_Close_Text : CloseText.Trim(); 
    }

    internal string GetToggleTipPositionFromEnum(ToggleTipPosition toggleTipPosition)

        => toggleTipPosition switch
        {
            ToggleTipPosition.TopCentre    => "top-centre",
            ToggleTipPosition.TopLeft      => "top-left",
            ToggleTipPosition.TopRight     => "top-right",
            ToggleTipPosition.CentreLeft   => "centre-left",
            ToggleTipPosition.CentreRight  => "centre-right",
            ToggleTipPosition.BottomCentre => "bottom-centre",
            ToggleTipPosition.BottomLeft   => "bottom-left",
            ToggleTipPosition.BottomRight  => "bottom-right",
            _                              => "top-centre",

        };

    internal string BuildTrggerClasses(ToggleTipLabelOrder labelOrder)

        => labelOrder switch
        {
            ToggleTipLabelOrder.LabelFirst => $"{GlobalValues.ToggleTip_Trigger_Class} {GlobalValues.ToggleTip_Trigger_Modifier_Class}",
            _ => GlobalValues.ToggleTip_Trigger_Class
        };
}

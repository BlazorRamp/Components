using BlazorRamp.ToggleTip.Common.Constants;
using Microsoft.AspNetCore.Components;

namespace BlazorRamp.ToggleTip.Components;

/// <summary>
/// A Blazor component that renders an accessible toggletip — a button-triggered popover
/// displaying supplemental information in a positioned, dismissible pane.
/// </summary>
public sealed partial class ToggleTip
{
    /// <summary>
    /// Gets or sets the content displayed inside the toggletip popover.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the visible label text shown alongside the trigger icon. Defaults to "Supplemental information".
    /// </summary>
    [Parameter] public string Label { get; set; } = GlobalValues.ToggleTip_Label;

    /// <summary>
    /// Gets or sets the text for the close button inside the popover. Defaults to "Close" 
    /// </summary>
    [Parameter] public string CloseText { get; set; } = GlobalValues.ToggleTip_Close_Text;

    /// <summary>
    /// Gets or sets a value indicating whether the trigger label is visible. Defaults to <see langword="true" />.
    /// </summary>
    [Parameter] public bool ShowLabel { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the close button is shown inside the popover. Defaults to <see langword="true"/>.
    /// </summary>
    [Parameter] public bool ShowClose { get; set; } = true;

    /// <summary>
    /// Gets or sets the display order of the label and icon within the trigger button.
    /// Defaults to <see cref="ToggleTipLabelOrder.LabelFirst"/>.
    /// </summary>
    [Parameter] public ToggleTipLabelOrder ToggleTipLabelOrder { get; set; } = ToggleTipLabelOrder.LabelFirst;

    /// <summary>
    /// Gets or sets the position of the toggletip popover relative to the trigger.
    /// Defaults to <see cref="ToggleTipPosition.TopCentre"/>.
    /// </summary>
    [Parameter] public ToggleTipPosition ToggleTipPosition { get; set; } = ToggleTipPosition.TopCentre;

    /// <summary>
    /// Gets or sets the size of the toggletip popover.
    /// Defaults to <see cref="ToggleTipSize.Small"/>.
    /// </summary>
    [Parameter] public ToggleTipSize ToggleTipSize { get; set; } = ToggleTipSize.Small;

    /// <summary>
    /// Gets or sets additional attributes that will be applied to the component.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private string _contentID         = Guid.NewGuid().ToString();
    private string _label             = GlobalValues.ToggleTip_Label;
    private string _closeText         = GlobalValues.ToggleTip_Close_Text;
    private string _toggleTipPosition = "top-centre";

    /// <summary>
    /// Resolves component parameters on initialisation, trimming whitespace from <see cref="Label"/> and <see cref="CloseText"/>
    /// and falling back to defaults if either is null or empty, and converting <see cref="ToggleTipPosition"/> to its CSS modifier string.
    /// </summary>
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

    internal string BuildTriggerClasses(ToggleTipLabelOrder labelOrder, bool showLabel)
    {  
        var classes = labelOrder switch
                       {
                           ToggleTipLabelOrder.IconFirst => $"{GlobalValues.ToggleTip_Trigger_Class} {GlobalValues.ToggleTip_Trigger_Order_Modifier_Class}",
                           _ => GlobalValues.ToggleTip_Trigger_Class
                       };

        return showLabel ? classes : $"{classes} {GlobalValues.ToggleTip_Trigger_No_Label_Modifier}";
        
    }

    internal string BuildToggleTipClasses(ToggleTipSize toggleTipSize)
    {    
        var classes = toggleTipSize switch
                       {
                           ToggleTipSize.Small  => GlobalValues.ToggleTip_Small_Modifier_Class,
                           ToggleTipSize.Medium => GlobalValues.ToggleTip_medium_Modifier_Class,
                           ToggleTipSize.Large  => GlobalValues.ToggleTip_large_Modifier_Class,
                           _                    => GlobalValues.ToggleTip_Small_Modifier_Class
                       };

        return $"{GlobalValues.ToggleTip_Class} {classes}";
    }

}

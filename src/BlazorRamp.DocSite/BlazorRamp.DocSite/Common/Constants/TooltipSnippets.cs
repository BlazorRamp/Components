namespace BlazorRamp.DocSite.Common.Constants;

public class TooltipSnippets
{
    public const string Add_Tooltip_Style_Sheet = """
        <head>
            <link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
            <link rel="stylesheet" href="_content/BlazorRamp.Tooltip/assets/css/tooltip.min.css" />
        </head>
        """;


    public const string Icon_Button_Example = """
        <Tooltip TooltipID="save-button-tooltip-id" TooltipText="Saves the current item" TooltipPosition="TooltipPosition.TopCentre">
            <button type="button" aria-describedby="save-button-tooltip-id" class="@Button.Base @Button.Scheme(ButtonSolidScheme.SuccessLighter) 
                                                                                   @Button.Squared @Button.Size(ButtonSize.Regular) @Button.UseColumns 
                                                                                   @Button.FixedRadius(UnitRadius.Two) @Gap.SetGaps(UnitGapSize.None)">
                <span class="@SvgIcon.Base" style="--_svg-icon-source:var(--svg-save-icon);"></span>
            </button>
        </Tooltip>
        """;
}

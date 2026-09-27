namespace BlazorRamp.DocSite.Common.Constants;

public class TooltipSnippets
{
    public const string Add_Tooltip_Style_Sheet = """
        <head>
            <link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
            <link rel="stylesheet" href="_content/BlazorRamp.Tooltip/assets/css/tooltip.min.css" />
        </head>
        """;


    public const string Icon_Button_Example = """"
        <Tooltip TooltipID="save-button-tooltip-id" TooltipText="Saves the current item to disk" TooltipPosition="TooltipPosition.TopLeft">
            <button aria-label="Save" type="button" aria-describedby="save-button-tooltip-id" class="@Button.Base @Button.Scheme(ButtonSolidScheme.SuccessLighter) 
                                                                                   @Button.Squared @Button.Size(ButtonSize.Regular) @Button.UseColumns 
                                                                                   @Button.FixedRadius(UnitRadius.Two) @Gap.SetGaps(UnitGapSize.None)">
                <span class="@SvgIcon.Base" style="--_svg-icon-source:var(--svg-save-icon);"></span>
            </button>
        </Tooltip>

        <Tooltip TooltipID="edit-button-tooltip-id" TooltipText="@_editDetails" TooltipPosition="TooltipPosition.BottomRight" InvertColours="true">
            <button aria-label="Edit" type="button" aria-describedby="edit-button-tooltip-id" class="@Button.Base @Button.Scheme(ButtonSolidScheme.InfoLighter)
                @Button.Squared @Button.Size(ButtonSize.Regular) @Button.UseColumns
                @Button.FixedRadius(UnitRadius.Two) @Gap.SetGaps(UnitGapSize.None)">
                    <span class="@SvgIcon.Base" style="--_svg-icon-source:var(--svg-pencil-icon);"></span>
            </button>
        </Tooltip>

        <Tooltip TooltipID="settings-button-tooltip-id" TooltipText="Settings" TooltipPosition="TooltipPosition.CentreRight" InvertColours="false">
            <button aria-labelledby="settings-button-tooltip-id" type="button" class="@Button.Base @Button.Scheme(ButtonSolidScheme.WarningLighter)
                @Button.Squared @Button.Size(ButtonSize.Regular) @Button.UseColumns
                @Button.FixedRadius(UnitRadius.Two) @Gap.SetGaps(UnitGapSize.None)">
                <span class="@SvgIcon.Base" style="--_svg-icon-source:var(--svg-settings-icon);"></span>
            </button>
        </Tooltip>
        
        @code {

            public string _editDetails = """
               Edit the currently selected item.

               Lorem ipsum dolor sit amet, consectetur adipiscing elit.Integer convallis, lorem non suscipit tempor, metus dolor  eleifend nulla, et commodo.
            """;
        }
        """";
}

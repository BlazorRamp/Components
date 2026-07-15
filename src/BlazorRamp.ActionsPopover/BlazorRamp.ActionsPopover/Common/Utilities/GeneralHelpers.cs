using BlazorRamp.ActionsPopover.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.ActionsPopover.Common.Utilities;

internal static class GeneralHelpers
{

    internal static string? CheckSetSvgVariable(string? svgIcon, string variableName)
    {
        var iconVariable = String.IsNullOrWhiteSpace(svgIcon)
                                ? null
                                : svgIcon.TrimStart().StartsWith("--") ? $"var({svgIcon!.Trim().TrimEnd(':')})" : null;

        return iconVariable is null ? null : $"{variableName}:{iconVariable};";
    }

    internal static string? CheckSetColourVariable(string? colourValue, string variableName)
    {
        var value = String.IsNullOrWhiteSpace(colourValue) ? null : colourValue.Trim().TrimEnd(';');
        return value is null ? null : $"{variableName}:{value};";
    }

    internal static string GetTargetType(PopoverLinkTargetType targetType)

    => targetType switch
    {
        PopoverLinkTargetType.Self   => "_self",
        PopoverLinkTargetType.Blank  => "_blank",
        PopoverLinkTargetType.Parent => "_parent",
        PopoverLinkTargetType.Top => "_top",
        _ => "_self"
    };
}

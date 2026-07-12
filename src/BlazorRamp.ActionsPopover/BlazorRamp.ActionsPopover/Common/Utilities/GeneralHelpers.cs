using BlazorRamp.ActionsPopover.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.ActionsPopover.Common.Utilities;

internal class GeneralHelpers
{
    /// <summary>
    /// Validates that a SvgIcon parameter and returns a CSS inline style string
    /// setting the internal SVG custom property, or <see langword="null"/> if the value is
    /// absent or does not begin with <c>--</c>.
    /// </summary>
    internal static string? CheckSetSvgVariable(string? svgIcon, string variableName)
    {
        var iconVariable = String.IsNullOrWhiteSpace(svgIcon)
                                ? null
                                : svgIcon.TrimStart().StartsWith("--") ? $"var({svgIcon!.Trim().TrimEnd(':')})" : null;

        return iconVariable is null ? null : $"{variableName}:{iconVariable};";
    }
}

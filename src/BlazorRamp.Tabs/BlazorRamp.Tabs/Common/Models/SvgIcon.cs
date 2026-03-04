using BlazorRamp.Tabs.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.Tabs.Common.Models;

internal class SvgIcon (string svgData)
{
    public string? Value { get; set; } = CheckSetSvgIcon(svgData);

    public static string? CheckSetSvgIcon(string? svgIcon)
    {
        var icon = String.IsNullOrWhiteSpace(svgIcon) ? null : (svgIcon.StartsWith("<svg ", StringComparison.InvariantCultureIgnoreCase) ? svgIcon.Trim() : null);

        return icon is null ? null : $"url(\"data:image/svg+xml,{Uri.EscapeDataString(icon)})";
    }

}

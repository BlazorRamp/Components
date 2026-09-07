
using BlazorRamp.CssClasses.Common.Constants;
 
namespace BlazorRamp.CssClasses.Common.Utilities;
 
/// <summary>
/// Provides utility classes that set an element's CSS <c>vertical-align</c>. Unlike the
/// BEM block classes elsewhere in this library, these are flat, single-purpose utility
/// classes intended to be combined freely with other classes on any element.
/// </summary>
/// <remarks>
/// Only <see cref="UnitVerticalAlign.Top"/>, <see cref="UnitVerticalAlign.Middle"/>, and
/// <see cref="UnitVerticalAlign.Bottom"/> are meaningful when applied to a table cell
/// (<c>td</c>/<c>th</c>).
/// </remarks>
public static class VerticalAlign
{
    private const string _verticalAlignBase = "br-vertical-align";
 
    /// <summary>
    /// Gets the class that sets an element's CSS <c>vertical-align</c>.
    /// </summary>
    /// <param name="unitVerticalAlign">The vertical alignment to apply. See <see cref="UnitVerticalAlign"/>.</param>
    /// <returns>The <c>br-vertical-align-*</c> utility class for the given <paramref name="unitVerticalAlign"/>.</returns>
    public static string SetAs(UnitVerticalAlign unitVerticalAlign)
 
        => unitVerticalAlign switch
        {
            UnitVerticalAlign.Baseline   => $"{_verticalAlignBase}-baseline",
            UnitVerticalAlign.Bottom     => $"{_verticalAlignBase}-bottom",
            UnitVerticalAlign.Middle     => $"{_verticalAlignBase}-middle",
            UnitVerticalAlign.Sub        => $"{_verticalAlignBase}-sub",
            UnitVerticalAlign.Super      => $"{_verticalAlignBase}-super",
            UnitVerticalAlign.TextTop    => $"{_verticalAlignBase}-text-top",
            UnitVerticalAlign.TextBottom => $"{_verticalAlignBase}-text-bottom",
            UnitVerticalAlign.Top        => $"{_verticalAlignBase}-top",
            _                            => $"{_verticalAlignBase}-baseline",
        };
}
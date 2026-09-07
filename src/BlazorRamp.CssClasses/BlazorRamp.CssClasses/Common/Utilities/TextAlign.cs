using BlazorRamp.CssClasses.Common.Constants;
 
namespace BlazorRamp.CssClasses.Common.Utilities;
 
/// <summary>
/// Provides utility classes that set an element's CSS <c>text-align</c>. Unlike the BEM
/// block classes elsewhere in this library, these are flat, single-purpose utility
/// classes intended to be combined freely with other classes on any element.
/// </summary>
public static class TextAlign
{
    private const string _textAlignBase = "br-text-align";
 
    /// <summary>
    /// Gets the class that sets an element's CSS <c>text-align</c>.
    /// </summary>
    /// <param name="unitTextAlign">The text alignment to apply. See <see cref="UnitTextAlign"/>.</param>
    /// <returns>The <c>br-text-align-*</c> utility class for the given <paramref name="unitTextAlign"/>.</returns>
    public static string SetAs(UnitTextAlign unitTextAlign)
 
        => unitTextAlign switch
        {
            UnitTextAlign.Start   => $"{_textAlignBase}-start",
            UnitTextAlign.Centre  => $"{_textAlignBase}-center",
            UnitTextAlign.End     => $"{_textAlignBase}-end",
            UnitTextAlign.Justify => $"{_textAlignBase}-justify",
            _                     => $"{_textAlignBase}-start",
        };
}
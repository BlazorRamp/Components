using BlazorRamp.CssClasses.Common.Constants;

namespace BlazorRamp.CssClasses.Common.Utilities;

/// <summary>
/// Provides the <c>br-box-shadow-*</c> utility classes used to set an element's box shadow
/// from the <c>--br-unit-box-shadow-*</c> primitive scale.
/// </summary>
public static class BoxShadow
{
    /// <summary>
    /// Gets the class that sets an element's box shadow.
    /// </summary>
    /// <param name="unitBoxShadow">The box shadow to apply. See <see cref="UnitBoxShadow"/>.</param>
    /// <returns>The <c>br-box-shadow-*</c> utility class for the given <paramref name="unitBoxShadow"/>.</returns>
    public static string SetShadow(UnitBoxShadow unitBoxShadow)

        => unitBoxShadow switch
        {
            UnitBoxShadow.One => "br-box-shadow-1",
            UnitBoxShadow.Two => "br-box-shadow-2",
            _ => "br-box-shadow-0",
        };
}
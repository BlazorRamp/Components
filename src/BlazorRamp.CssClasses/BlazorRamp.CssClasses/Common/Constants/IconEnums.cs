using BlazorRamp.CssClasses.Common.Utilities;

namespace BlazorRamp.CssClasses.Common.Constants;

/// <summary>
/// Sets an icon's size as a multiple of the current font-size. Used by <see cref="SvgIcon"/>.
/// <c>Small</c> and <c>Regular</c> fit inside any <see cref="ButtonSize"/> without adding height;
/// <c>Large</c> and <c>ExtraLarge</c> exceed a button's default line-height and will make the
/// button taller regardless of its own size tier.
/// </summary>
public enum UnitIconSize : int
{
    /// <summary>
    /// 1.25em. Fits inside any button size without adding height.
    /// </summary>
    Small = 0,

    /// <summary>
    /// 1.5em, one step above small. Still fits inside any button size without adding height.
    /// </summary>
    Regular = 1,

    /// <summary>
    /// 1.75em, one step above regular. Exceeds a button's default line-height and will add height.
    /// </summary>
    Large = 2,

    /// <summary>
    /// 2em, the largest size on this scale. Exceeds a button's default line-height and will add height.
    /// </summary>
    ExtraLarge = 3
}

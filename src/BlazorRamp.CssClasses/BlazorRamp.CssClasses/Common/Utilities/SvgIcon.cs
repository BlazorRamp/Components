using BlazorRamp.CssClasses.Common.Constants;

namespace BlazorRamp.CssClasses.Common.Utilities;

/// <summary>
/// Provides the <c>br-svg-icon</c> class: a masked, <c>currentColor</c>-tinted icon that
/// can be dropped into a <see cref="Button"/>, a <see cref="Link"/>, or anywhere else text
/// colour is meaningful. Supply the icon by setting the <c>--_svg-icon-source</c> custom
/// property inline (a <c>mask-image</c> URL or data URI) on the element carrying this class -
/// without it, the icon renders as a plain <c>currentColor</c> square, so a missing icon is
/// obvious rather than silently invisible. Always add <c>aria-hidden="true"</c> to the icon
/// element itself, since it carries no meaning of its own.
/// </summary>
public static class SvgIcon
{
    /// <summary>
    /// The base class, <c>br-svg-icon</c>. Always required. Renders at the
    /// <see cref="UnitIconSize.Regular"/> size unless a <see cref="Size"/> modifier is applied.
    /// </summary>
    public const string Base = "br-svg-icon";

    /// <summary>
    /// Gets the class that sets the icon's size, as a multiple of the current font-size.
    /// </summary>
    /// <param name="unitIconSize">The size to apply. See <see cref="UnitIconSize"/>.</param>
    /// <returns>The <c>br-svg-icon--*</c> modifier class for the given <paramref name="unitIconSize"/>.</returns>
    public static string Size(UnitIconSize unitIconSize)

        => unitIconSize switch
        {
            UnitIconSize.Small      => $"{Base}--small",
            UnitIconSize.Regular    => $"{Base}--regular",
            UnitIconSize.Large      => $"{Base}--large",
            UnitIconSize.ExtraLarge => $"{Base}--extra-large",
            _                       => $"{Base}--regular",
        };
}
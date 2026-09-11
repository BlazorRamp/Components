using BlazorRamp.CssClasses.Common.Constants;

namespace BlazorRamp.CssClasses.Common.Utilities;

/// <summary>
/// Provides the BEM CSS classes for the <c>br-flex-content</c> block: a flex container,
/// plus the <c>br-flex-wrap-*</c> utility classes used to control its wrapping behaviour.
/// Combine with <see cref="LayoutAlignment"/>'s classes to align its children.
/// </summary>
public static class FlexContent
{
    /// <summary>
    /// The base class, <c>br-flex-content</c>. Always required.
    /// </summary>
    public const string Base = "br-flex-content";

    /// <summary>
    /// Sets the flex container's main axis to the block (column) direction instead of the default row.
    /// </summary>
    public const string UseColumns = $"{Base}--columns";

    private const string _wrapBase = "br-flex-wrap";

    /// <summary>
    /// Gets the class that sets whether the container's children may wrap onto multiple lines.
    /// </summary>
    /// <param name="unitFlexWrap">The wrapping behaviour to apply. See <see cref="UnitFlexWrap"/>.</param>
    /// <returns>The <c>br-flex-wrap-*</c> utility class for the given <paramref name="unitFlexWrap"/>.</returns>
    public static string Wrap(UnitFlexWrap unitFlexWrap)

        => unitFlexWrap switch
        {
            UnitFlexWrap.NoWrap      => $"{_wrapBase}-none",
            UnitFlexWrap.Wrap        => $"{_wrapBase}-wrap",
            UnitFlexWrap.WrapReverse => $"{_wrapBase}-wrap-reverse",
            _ => $"{_wrapBase}-none",
        };
}
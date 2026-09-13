using BlazorRamp.CssClasses.Common.Constants;

namespace BlazorRamp.CssClasses.Common.Utilities;

/// <summary>
/// Provides the BEM CSS classes for the <c>br-flex-content</c> block: a flex container,
/// plus the <c>br-flex-content--*</c> modifier classes used to control its wrapping behaviour.
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


    private const string _flexGrowBase = "br-flex-grow";
    private const string _flexShrinkBase = "br-flex-shrink";


    /// <summary>
    /// Gets the class that sets whether the container's children may wrap onto multiple lines.
    /// </summary>
    /// <param name="unitFlexWrap">The wrapping behaviour to apply. See <see cref="UnitFlexWrap"/>.</param>
    /// <returns>The <c>br-flex-content--*</c> modifier class for the given <paramref name="unitFlexWrap"/>.</returns>
    public static string Wrap(UnitFlexWrap unitFlexWrap)

        => unitFlexWrap switch
        {
            UnitFlexWrap.NoWrap      => $"{Base}--no-wrap",
            UnitFlexWrap.Wrap        => $"{Base}--wrap",
            UnitFlexWrap.WrapReverse => $"{Base}--wrap-reverse",
            _                        => $"{Base}--no-wrap",
        };

    /// <summary>
    /// Gets the class that sets whether a flex item may grow to fill available space along the main axis.
    /// </summary>
    /// <param name="unitFlexFactor">The grow factor to apply. See <see cref="UnitFlexFactor"/>.</param>
    /// <returns>The <c>br-flex-grow-*</c> utility class for the given <paramref name="unitFlexFactor"/>.</returns>
    public static string Grow(UnitFlexFactor unitFlexFactor)

        => unitFlexFactor switch
        {
            UnitFlexFactor.None => $"{_flexGrowBase}-0",
            UnitFlexFactor.One  => $"{_flexGrowBase}-1",
            _ => $"{_flexGrowBase}-0",
        };

    /// <summary>
    /// Gets the class that sets whether a flex item may shrink below its base size when space is limited.
    /// </summary>
    /// <param name="unitFlexFactor">The shrink factor to apply. See <see cref="UnitFlexFactor"/>.</param>
    /// <returns>The <c>br-flex-shrink-*</c> utility class for the given <paramref name="unitFlexFactor"/>.</returns>
    public static string Shrink(UnitFlexFactor unitFlexFactor)

        => unitFlexFactor switch
        {
            UnitFlexFactor.None => $"{_flexShrinkBase}-0",
            UnitFlexFactor.One  => $"{_flexShrinkBase}-1",
            _ => $"{_flexShrinkBase}-1",
        };
}
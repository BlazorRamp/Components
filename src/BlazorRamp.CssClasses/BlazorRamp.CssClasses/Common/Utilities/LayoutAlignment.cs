using BlazorRamp.CssClasses.Common.Constants;

namespace BlazorRamp.CssClasses.Common.Utilities;

/// <summary>
/// Provides the CSS utility classes for aligning content, items, and individual elements
/// within a flex or grid container. Shared by <see cref="FlexContent"/> and
/// <see cref="GridRow"/> consumers alike, since the underlying alignment properties and
/// values are identical across both layout modes.
/// </summary>
public static class LayoutAlignment
{
    private const string _justifyContentBase = "br-justify-content";
    private const string _alignContentBase   = "br-align-content";
    private const string _justifyItemsBase   = "br-justify-items";
    private const string _alignItemsBase     = "br-align-items";
    private const string _justifySelfBase    = "br-justify-self";
    private const string _alignSelfBase      = "br-align-self";

    /// <summary>
    /// Gets the class that sets how a container distributes extra space along its main axis
    /// between and around content lines.
    /// </summary>
    /// <param name="unitJustifyContent">The distribution to apply. See <see cref="UnitJustifyContent"/>.</param>
    /// <returns>The <c>br-justify-content-*</c> utility class for the given <paramref name="unitJustifyContent"/>.</returns>
    public static string JustifyContent(UnitJustifyContent unitJustifyContent)

        => unitJustifyContent switch
        {
            UnitJustifyContent.Start        => $"{_justifyContentBase}-start",
            UnitJustifyContent.Centre       => $"{_justifyContentBase}-center",
            UnitJustifyContent.End          => $"{_justifyContentBase}-end",
            UnitJustifyContent.Stretch      => $"{_justifyContentBase}-stretch",
            UnitJustifyContent.SpaceBetween => $"{_justifyContentBase}-between",
            UnitJustifyContent.SpaceAround  => $"{_justifyContentBase}-around",
            UnitJustifyContent.SpaceEvenly  => $"{_justifyContentBase}-evenly",
            _                                => $"{_justifyContentBase}-start",
        };

    /// <summary>
    /// Gets the class that sets how a container distributes extra space along its cross axis
    /// between and around content lines.
    /// </summary>
    /// <param name="unitAlignContent">The distribution to apply. See <see cref="UnitAlignContent"/>.</param>
    /// <returns>The <c>br-align-content-*</c> utility class for the given <paramref name="unitAlignContent"/>.</returns>
    public static string AlignContent(UnitAlignContent unitAlignContent)

        => unitAlignContent switch
        {
            UnitAlignContent.Start        => $"{_alignContentBase}-start",
            UnitAlignContent.Centre       => $"{_alignContentBase}-center",
            UnitAlignContent.End          => $"{_alignContentBase}-end",
            UnitAlignContent.Stretch      => $"{_alignContentBase}-stretch",
            UnitAlignContent.SpaceBetween => $"{_alignContentBase}-between",
            UnitAlignContent.SpaceAround  => $"{_alignContentBase}-around",
            UnitAlignContent.SpaceEvenly  => $"{_alignContentBase}-evenly",
            _                              => $"{_alignContentBase}-start",
        };

    /// <summary>
    /// Gets the class that sets how a container aligns its items within their cells along
    /// the inline axis, as a group.
    /// </summary>
    /// <param name="unitJustifyItems">The alignment to apply. See <see cref="UnitJustifyItems"/>.</param>
    /// <returns>The <c>br-justify-items-*</c> utility class for the given <paramref name="unitJustifyItems"/>.</returns>
    public static string JustifyItems(UnitJustifyItems unitJustifyItems)

        => unitJustifyItems switch
        {
            UnitJustifyItems.Start   => $"{_justifyItemsBase}-start",
            UnitJustifyItems.Centre  => $"{_justifyItemsBase}-center",
            UnitJustifyItems.End     => $"{_justifyItemsBase}-end",
            UnitJustifyItems.Stretch => $"{_justifyItemsBase}-stretch",
            _                         => $"{_justifyItemsBase}-start",
        };

    /// <summary>
    /// Gets the class that sets how a container aligns its items along the cross axis, as a group.
    /// </summary>
    /// <param name="unitAlignItems">The alignment to apply. See <see cref="UnitAlignItems"/>.</param>
    /// <returns>The <c>br-align-items-*</c> utility class for the given <paramref name="unitAlignItems"/>.</returns>
    public static string AlignItems(UnitAlignItems unitAlignItems)

        => unitAlignItems switch
        {
            UnitAlignItems.Start   => $"{_alignItemsBase}-start",
            UnitAlignItems.Centre  => $"{_alignItemsBase}-center",
            UnitAlignItems.End     => $"{_alignItemsBase}-end",
            UnitAlignItems.Stretch => $"{_alignItemsBase}-stretch",
            _                       => $"{_alignItemsBase}-start",
        };

    /// <summary>
    /// Gets the class that sets how a single grid item aligns itself within its cell along
    /// the inline axis, overriding the container's <see cref="UnitJustifyItems"/> value.
    /// </summary>
    /// <param name="unitJustifySelf">The alignment to apply. See <see cref="UnitJustifySelf"/>.</param>
    /// <returns>The <c>br-justify-self-*</c> utility class for the given <paramref name="unitJustifySelf"/>.</returns>
    public static string JustifySelf(UnitJustifySelf unitJustifySelf)

        => unitJustifySelf switch
        {
            UnitJustifySelf.Start   => $"{_justifySelfBase}-start",
            UnitJustifySelf.Centre  => $"{_justifySelfBase}-center",
            UnitJustifySelf.End     => $"{_justifySelfBase}-end",
            UnitJustifySelf.Stretch => $"{_justifySelfBase}-stretch",
            _                        => $"{_justifySelfBase}-start",
        };

    /// <summary>
    /// Gets the class that sets how a single flex or grid item aligns itself along the cross
    /// axis, overriding the container's <see cref="UnitAlignItems"/> value.
    /// </summary>
    /// <param name="unitAlignSelf">The alignment to apply. See <see cref="UnitAlignSelf"/>.</param>
    /// <returns>The <c>br-align-self-*</c> utility class for the given <paramref name="unitAlignSelf"/>.</returns>
    public static string AlignSelf(UnitAlignSelf unitAlignSelf)

        => unitAlignSelf switch
        {
            UnitAlignSelf.Start   => $"{_alignSelfBase}-start",
            UnitAlignSelf.Centre  => $"{_alignSelfBase}-center",
            UnitAlignSelf.End     => $"{_alignSelfBase}-end",
            UnitAlignSelf.Stretch => $"{_alignSelfBase}-stretch",
            _                      => $"{_alignSelfBase}-start",
        };
}
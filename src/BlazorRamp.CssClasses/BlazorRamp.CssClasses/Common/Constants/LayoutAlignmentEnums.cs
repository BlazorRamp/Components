using BlazorRamp.CssClasses.Common.Utilities;

namespace BlazorRamp.CssClasses.Common.Constants;

/// <summary>
/// Sets how a flex or grid container distributes extra space along its main axis
/// between and around content lines. Used by <see cref="LayoutAlignment.JustifyContent"/>.
/// </summary>
public enum UnitJustifyContent : int
{
    /// <summary>
    /// Packs lines toward the start of the container.
    /// </summary>
    Start = 0,

    /// <summary>
    /// Packs lines toward the centre of the container.
    /// </summary>
    Centre = 1,

    /// <summary>
    /// Packs lines toward the end of the container.
    /// </summary>
    End = 2,

    /// <summary>
    /// Stretches lines to fill the container's main axis.
    /// </summary>
    Stretch = 3,

    /// <summary>
    /// Distributes lines evenly, with the first line flush to the start and the last flush to the end.
    /// </summary>
    SpaceBetween = 4,

    /// <summary>
    /// Distributes lines evenly, with equal space around each line.
    /// </summary>
    SpaceAround = 5,

    /// <summary>
    /// Distributes lines evenly, with equal space between and around each line.
    /// </summary>
    SpaceEvenly = 6
}

/// <summary>
/// Sets how a flex or grid container distributes extra space along its cross axis
/// between and around content lines. Used by <see cref="LayoutAlignment.AlignContent"/>.
/// </summary>
public enum UnitAlignContent : int
{
    /// <summary>
    /// Packs lines toward the start of the container.
    /// </summary>
    Start = 0,

    /// <summary>
    /// Packs lines toward the centre of the container.
    /// </summary>
    Centre = 1,

    /// <summary>
    /// Packs lines toward the end of the container.
    /// </summary>
    End = 2,

    /// <summary>
    /// Stretches lines to fill the container's cross axis.
    /// </summary>
    Stretch = 3,

    /// <summary>
    /// Distributes lines evenly, with the first line flush to the start and the last flush to the end.
    /// </summary>
    SpaceBetween = 4,

    /// <summary>
    /// Distributes lines evenly, with equal space around each line.
    /// </summary>
    SpaceAround = 5,

    /// <summary>
    /// Distributes lines evenly, with equal space between and around each line.
    /// </summary>
    SpaceEvenly = 6
}

/// <summary>
/// Sets how a flex or grid container aligns its items along the cross axis, as a group.
/// Used by <see cref="LayoutAlignment.AlignItems"/>.
/// </summary>
public enum UnitAlignItems : int
{
    /// <summary>
    /// Aligns items to the start of the cross axis.
    /// </summary>
    Start = 0,

    /// <summary>
    /// Aligns items to the centre of the cross axis.
    /// </summary>
    Centre = 1,

    /// <summary>
    /// Aligns items to the end of the cross axis.
    /// </summary>
    End = 2,

    /// <summary>
    /// Stretches items to fill the cross axis.
    /// </summary>
    Stretch = 3
}

/// <summary>
/// Sets how a grid container aligns its items within their cells along the inline axis, as a group.
/// Used by <see cref="LayoutAlignment.JustifyItems"/>.
/// </summary>
public enum UnitJustifyItems : int
{
    /// <summary>
    /// Aligns items to the start of their cell.
    /// </summary>
    Start = 0,

    /// <summary>
    /// Aligns items to the centre of their cell.
    /// </summary>
    Centre = 1,

    /// <summary>
    /// Aligns items to the end of their cell.
    /// </summary>
    End = 2,

    /// <summary>
    /// Stretches items to fill their cell.
    /// </summary>
    Stretch = 3
}

/// <summary>
/// Sets how a single grid item aligns itself within its cell along the inline axis,
/// overriding the container's <see cref="UnitJustifyItems"/> value. Used by <see cref="LayoutAlignment.JustifySelf"/>.
/// </summary>
public enum UnitJustifySelf : int
{
    /// <summary>
    /// Aligns the item to the start of its cell.
    /// </summary>
    Start = 0,

    /// <summary>
    /// Aligns the item to the centre of its cell.
    /// </summary>
    Centre = 1,

    /// <summary>
    /// Aligns the item to the end of its cell.
    /// </summary>
    End = 2,

    /// <summary>
    /// Stretches the item to fill its cell.
    /// </summary>
    Stretch = 3
}

/// <summary>
/// Sets how a single flex or grid item aligns itself along the cross axis,
/// overriding the container's <see cref="UnitAlignItems"/> value. Used by <see cref="LayoutAlignment.AlignSelf"/>.
/// </summary>
public enum UnitAlignSelf : int
{
    /// <summary>
    /// Aligns the item to the start of the cross axis.
    /// </summary>
    Start = 0,

    /// <summary>
    /// Aligns the item to the centre of the cross axis.
    /// </summary>
    Centre = 1,

    /// <summary>
    /// Aligns the item to the end of the cross axis.
    /// </summary>
    End = 2,

    /// <summary>
    /// Stretches the item to fill the cross axis.
    /// </summary>
    Stretch = 3
}


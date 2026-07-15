using BlazorRamp.ActionsPopover.Components;

namespace BlazorRamp.ActionsPopover.Common.Constants;


/// <summary>
/// Defines the position of the toggletip popover relative to its trigger element.
/// </summary>
public enum ActionsPopoverPosition : int
{
    /// <summary>
    /// Positions the popover above and centred on the trigger.
    /// </summary>
    TopCentre = 0,
    /// <summary>
    /// Positions the popover above and aligned to the left of the trigger.
    /// </summary>
    TopLeft = 1,
    /// <summary>
    /// Positions the popover above and aligned to the right of the trigger.
    /// </summary>
    TopRight = 2,
    /// <summary>
    /// Positions the popover to the left and vertically centred on the trigger.
    /// </summary>
    CentreLeft = 3,
    /// <summary>
    /// Positions the popover to the right and vertically centred on the trigger.
    /// </summary>
    CentreRight = 4,
    /// <summary>
    /// Positions the popover below and centred on the trigger.
    /// </summary>
    BottomCentre = 5,
    /// <summary>
    /// Positions the popover below and aligned to the left of the trigger.
    /// </summary>
    BottomLeft = 6,
    /// <summary>
    /// Positions the popover below and aligned to the right of the trigger.
    /// </summary>
    BottomRight = 7,
}

/// <summary>
/// Defines the <c>target</c> attribute behaviour of an <see cref="ActionPopoverLink{TData}"/>.
/// </summary>
public enum PopoverLinkTargetType : int
{
    /// <summary>
    /// Opens the link in the same browsing context (<c>target="_self"</c>).
    /// </summary>
    Self =0,
    /// <summary>
    /// Opens the link in a new browsing context (<c>target="_blank"</c>).
    /// </summary>
    Blank = 1,
    /// <summary>
    /// Opens the link in the parent browsing context. Equivalent to <c>target="_parent"</c>.
    /// Falls back to <c>_self</c> if there is no parent.
    /// </summary>
    Parent = 2,

    /// <summary>
    /// Opens the link in the topmost browsing context. Equivalent to <c>target="_top"</c>.
    /// Falls back to <c>_self</c> if there is no parent.
    /// </summary>
    Top = 3
}
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
    /// <summary
    /// >Positions the popover to the left and vertically centred on the trigger.
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

public enum MenuItemType : int { Action, Url }

public enum TargetType : int { Self, Blank }
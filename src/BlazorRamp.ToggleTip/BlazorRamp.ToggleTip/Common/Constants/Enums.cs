using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.ToggleTip.Common.Constants;

/// <summary>
/// Defines the position of the toggletip popover relative to its trigger element.
/// </summary>
public enum ToggleTipPosition : int
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

/// <summary>
/// Defines the display order of the icon and label text within the toggletip trigger button.
/// </summary>
public enum ToggleTipLabelOrder : int
{
    /// <summary>
    /// Renders the label text before the icon.
    /// </summary>
    LabelFirst = 0,
    /// <summary>
    /// Renders the icon before the label text.
    /// </summary>
    IconFirst = 1
}

/// <summary>
/// Defines the size of the toggletip popover content area.
/// </summary>
public enum ToggleTipSize : int
{
    /// <summary>
    /// Renders the toggletip at its smallest preset size.
    /// </summary>
    Small = 0,
    /// <summary>
    /// Renders the toggletip at a medium preset size.
    /// </summary>
    Medium = 1,
    /// <summary>
    /// Renders the toggletip at its largest preset size.
    /// </summary>
    Large = 2
}
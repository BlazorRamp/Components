namespace BlazorRamp.Tooltip.Common.Constants;


/// <summary>
/// Defines the position of the tooltip relative to its containing element.
/// </summary>
public enum TooltipPosition : int
{
    /// <summary>
    /// Positions the tooltip above and centred on the container.
    /// </summary>
    TopCentre = 0,
    /// <summary>
    /// Positions the tooltip above and aligned to the left of the container.
    /// </summary>
    TopLeft = 1,
    /// <summary>
    /// Positions the tooltip above and aligned to the right of the container.
    /// </summary>
    TopRight = 2,
    /// <summary>
    /// Positions the tooltip to the left and vertically centred on the container.
    /// </summary>
    CentreLeft = 3,
    /// <summary>
    /// Positions the tooltip to the right and vertically centred on the container.
    /// </summary>
    CentreRight = 4,
    /// <summary>
    /// Positions the tooltip below and centred on the container.
    /// </summary>
    BottomCentre = 5,
    /// <summary>
    /// Positions the tooltip below and aligned to the left of the container.
    /// </summary>
    BottomLeft = 6,
    /// <summary>
    /// Positions the tooltip below and aligned to the right of the container.
    /// </summary>
    BottomRight = 7,
}

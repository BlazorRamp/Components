namespace BlazorRamp.BusyIndicator.Common.Constants;

/// <summary>
/// Defines the positioning behaviour of the busy indicator overlay.
/// </summary>
public enum OverlayPosition : int
{
    /// <summary>
    /// The overlay is constrained to the parent container.
    /// </summary>
    Container = 0,

    /// <summary>
    /// The overlay covers the entire screen.
    /// </summary>
    Screen = 1
}

/// <summary>
/// Defines the alignment of the indicator content within the overlay.
/// </summary>
public enum ContentPosition : int
{
    /// <summary>
    /// Positions content at the top of the overlay.
    /// </summary>
    Top = 0,

    /// <summary>
    /// Centres content vertically and horizontally within the overlay.
    /// </summary>
    Centre = 1
}


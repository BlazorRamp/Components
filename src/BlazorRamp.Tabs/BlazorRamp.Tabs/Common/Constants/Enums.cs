namespace BlazorRamp.Tabs.Common.Constants;

/// <summary>
/// Defines the position of the icon relative to the tab title text within each tab button.
/// </summary>
public enum TabIconPosition : int
{
    /// <summary>
    /// Renders the icon to the left of the tab title.
    /// </summary>
    Left = 0,
    /// <summary>
    /// Renders the icon to the right of the tab title.
    /// </summary>
    Right = 1,
    /// <summary>
    /// Renders the icon above the tab title.
    /// </summary>
    Top = 2,
    /// <summary>
    /// Renders the icon below the tab title.
    /// </summary>
    Bottom = 3
}
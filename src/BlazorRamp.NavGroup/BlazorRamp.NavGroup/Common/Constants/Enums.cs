namespace BlazorRamp.NavGroup.Common.Constants;

/// <summary>
/// Specifies the browsing context for the HTML <c>target</c> attribute on a navigation link.
/// </summary>
public enum TargetType : int
{
    /// <summary>
    /// Opens the link in the same browsing context. Equivalent to <c>target="_self"</c>.
    /// This is the default behaviour.
    /// </summary>
    Self = 0,

    /// <summary>
    /// Opens the link in a new tab or window. Equivalent to <c>target="_blank"</c>.
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

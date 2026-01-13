namespace BlazorRamp.Core.Common.Constants;

/// <summary>
/// Specifies how a component should be styled based on its background context.
/// If the value specified is OnDark or OnLight the data attribute 'data-br-style' will be populated
/// with either the value : 'on-dark' or 'on-light' so it can be detected and styled accordingly. For the
/// value of Dynamic null is used the attribute is omitted.
/// </summary>
public enum StyleAs : int
{
    /// <summary>
    /// The component dynamically adapts to the current theme (light or dark) using CSS variables.
    /// </summary>
    Dynamic = 0,

    /// <summary>
    /// The component is always displayed on a dark background and styles are adjusted accordingly.
    /// </summary>
    OnDark,

    /// <summary>
    /// The component is always displayed on a light background and styles are adjusted accordingly.
    /// </summary>
    OnLight
}

/// <summary>
/// Specifies the type or category of an announcement for semantic classification.
/// </summary>
public enum AnnouncementType : int
{
    /// <summary>
    /// General informational announcement.
    /// </summary>
    Info = 0,

    /// <summary>
    /// Indicates an operation has started.
    /// </summary>
    OperationStarted = 1,

    /// <summary>
    /// Indicates an operation has completed successfully.
    /// </summary>
    OperationCompleted = 2,

    /// <summary>
    /// Indicates an operation has failed.
    /// </summary>
    OperationFailed = 3,

    /// <summary>
    /// Indicates an operation was cancelled.
    /// </summary>
    OperationCancelled = 4,

    /// <summary>
    /// System-level warning message.
    /// </summary>
    SystemWarning = 5,

    /// <summary>
    /// System-level error message.
    /// </summary>
    SystemError = 6
}


/// <summary>
/// Specifies the priority level for ARIA live region announcements to screen readers.
/// </summary>
public enum LiveRegionType : int
{
    /// <summary>
    /// Polite announcements wait for the screen reader to finish current speech before announcing.
    /// </summary>
    Polite = 0,

    /// <summary>
    /// Assertive announcements interrupt the screen reader immediately.
    /// </summary>
    Assertive
}

/// <summary>
/// Specifies the visibility behaviour of a UI element.
/// </summary>
public enum Visibility: int
{
    /// <summary>
    /// The element is never visible.
    /// </summary>
    Never = 0,

    /// <summary>
    /// The element is always visible.
    /// </summary>
    Visible,

    /// <summary>
    /// The element is hidden from view.
    /// </summary>
    Hidden,

    /// <summary>
    /// The element is only visible when focused.
    /// </summary>
    FocusVisible
}
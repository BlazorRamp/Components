namespace BlazorRamp.Pager.Common.Constants;

/// <summary>
/// Specifies the type of element the <see cref="BlazorRamp.Pager.Components.Pager"/> uses to render its
/// navigation controls.
/// </summary>
public enum PagerSelectorType : int
{
    /// <summary>
    /// Navigation controls are rendered as <c>button</c> elements that raise
    /// <see cref="BlazorRamp.Pager.Components.Pager.CurrentPageChanged"/> without navigating the browser. Use
    /// this for content that stays on the current page, such as a data table.
    /// </summary>
    Button = 0,

    /// <summary>
    /// Navigation controls are rendered as <c>anchor</c> (<c>a href</c>) elements that update the page's query
    /// string. Use this for content where paging may navigate to a different page, such as a list of search
    /// results.
    /// </summary>
    Link = 1
}

/// <summary>
/// Specifies whether the <see cref="BlazorRamp.Pager.Components.Pager"/> announces page changes to assistive
/// technology.
/// </summary>
public enum PagerAnnouncementType : int
{
    /// <summary>
    /// Page changes are announced to assistive technology through a live region.
    /// </summary>
    WithAnnouncement = 0,

    /// <summary>
    /// Page changes are not announced by the pager. Useful for when two pagers
    /// are being used for the same data set.
    /// </summary>
    WithoutAnnouncement = 1
}



/// <summary>
/// Specifies the horizontal alignment of the <see cref="BlazorRamp.Pager.Components.Pager"/> within its
/// container.
/// </summary>
public enum PageAlignment : int
{
    /// <summary>
    /// The pager is centred within its container.
    /// </summary>
    Centred = 0,

    /// <summary>
    /// The pager is aligned to the start of its container.
    /// </summary>
    Start = 1,

    /// <summary>
    /// The pager is aligned to the end of its container.
    /// </summary>
    End = 2
}

/// <summary>
/// Identifies which navigation control within the <see cref="BlazorRamp.Pager.Components.Pager"/> is being
/// interacted with.
/// </summary>
internal enum NavSelectorType : int
{
    /// <summary>
    /// No navigation control; the default/unset value.
    /// </summary>
    None = 0,

    /// <summary>
    /// The control that requests the first page.
    /// </summary>
    First = 1,

    /// <summary>
    /// The control that requests the previous page.
    /// </summary>
    Previous = 2,

    /// <summary>
    /// The control that requests the next page.
    /// </summary>
    Next = 3,

    /// <summary>
    /// The control that requests the last page.
    /// </summary>
    Last = 4
}
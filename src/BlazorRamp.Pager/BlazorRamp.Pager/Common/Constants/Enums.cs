namespace BlazorRamp.Pager.Common.Constants;

public enum PagerSelectorType : int
{
    Button = 0,
    Link   = 1
}

public enum PagerAnnouncmentType : int
{
    WithAnnouncement    = 0,
    WithoutAnnouncement = 1
}

internal enum NavFocusType : int
{
    None = 0,
    First = 1,
    Previous = 2,
    Next = 3,
    Last = 4
}

public enum PageAlignment : int
{
    Centred = 0,
    Start   = 1,
    End     = 2
}
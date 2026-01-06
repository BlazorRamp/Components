namespace BlazorRamp.Core.Common.Constants;

public enum StyleAs : int 
{ 
    Dynamic = 0, 
    OnDark, 
    OnLight 
};

public enum AnnouncementType : int
{
    Info = 0,
    OperationStarted =1,
    OperationCompleted = 2,
    OperationFailed = 3,
    OperationCancelled = 4,
    SystemWarning = 5,
    SystemError = 6
}

public enum LiveRegionType : int
{
    Polite = 0,
    Assertive 
}

public enum Visibility: int
{
    Never = 0,
    Visible,
    Hidden,
    FocusVisible
}
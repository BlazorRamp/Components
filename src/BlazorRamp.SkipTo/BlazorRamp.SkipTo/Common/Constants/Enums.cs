namespace BlazorRamp.SkipTo.Common.Constants;


/// <summary>
/// Specifies the positioning level and behaviour for the skip to component.
/// </summary>
public enum SkipToType : int 
{
    /// <summary>
    /// Site level positioning uses fixed placement at the top of the screen.
    /// </summary>
    Site = 0,
    /// <summary>
    /// Section level positioning uses absolute placement relative to the parent container.
    /// </summary>
    Section = 1
}
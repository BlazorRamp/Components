namespace BlazorRamp.DebounceFilter.Common.Constants;

/// <summary>
/// Sets the alignment of the text/data in the input.
/// </summary>
public enum FilterDataPosition : int
{

    /// <summary>
    /// left aligned in LTR, right aligned in RTL
    /// </summary>
    Start = 0,
    /// <summary>
    /// The text/data is centred.
    /// </summary>
    Centre = 1,
    /// <summary>
    /// Right aligned in LTR, left aligned in RTL
    /// </summary>
    End = 2
}
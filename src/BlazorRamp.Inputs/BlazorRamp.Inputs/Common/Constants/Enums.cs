namespace BlazorRamp.Inputs.Common.Constants;

public enum TextInputType : int
{
    Text = 0,
    Email = 1,
    Url = 2, 
    Tel = 3
}


public enum ValidationDisplayMode: int
{
    DescribedbyWithHint       = 0,
    DescribedbyHintSuppressed = 1,
    TabbableWithHint          = 2

}
public enum PasswordAutoComplete
{
    CurrentPassword,
    NewPassword
}
namespace BlazorRamp.Inputs.Common.Constants;

public enum TextInputType : int
{
    Text = 0,
    Email = 1,
    Url = 2, 
    Tel = 3
}
public enum NumericInputModeType : int
{
    Numeric = 0,
    Decimal = 1

}


public enum ValidationDisplayMode: int
{
    DescribedByWithHint       = 0,
    DescribedByHintSuppressed = 1,
    TabbableWithHint          = 2

}
public enum PasswordAutoComplete
{
    CurrentPassword,
    NewPassword
}
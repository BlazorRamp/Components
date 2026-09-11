using BlazorRamp.CssClasses.Common.Constants;

namespace BlazorRamp.CssClasses.Common.Utilities;

public static class GridRow
{
    public const string Base = "br-grid-row";

    public static string ColSpan(UnitBreakpoint unitBreakpoint, UnitColSpan unitColSpan)
    {
        string infix = unitBreakpoint switch
        {
            UnitBreakpoint.Xs  => "xs",
            UnitBreakpoint.Sm  => "sm",
            UnitBreakpoint.Md  => "md",
            UnitBreakpoint.Lg  => "lg",
            UnitBreakpoint.Xl  => "xl",
            UnitBreakpoint.Xxl => "xxl",
            _ => "xs",
        };

        return $"br-grid-col-{infix}-{(int)unitColSpan}";
    }
}

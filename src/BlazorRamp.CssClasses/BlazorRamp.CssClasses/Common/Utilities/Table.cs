using BlazorRamp.CssClasses.Common.Constants;

namespace BlazorRamp.CssClasses.Common.Utilities;

public static class Table
{
    public const string Base = "br-table";
    public const string StripedRows = $"{Base}--striped-rows";
    
    public const string Scrollable = $"{Base}--scrollable";

    public static string AlignColumn(UnitColumn unitColumn, UnitTextAlign unitTextAlign)
    {
        string alignment = unitTextAlign switch
        {
            UnitTextAlign.Start   => "start",
            UnitTextAlign.Centre  => "center",
            UnitTextAlign.End     => "end",
            UnitTextAlign.Justify => "justify",
            _                     => "start",
        };

        return $"{Base}--align-col-{(int)unitColumn}-{alignment}";
    }
}

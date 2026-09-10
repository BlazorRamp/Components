using BlazorRamp.CssClasses.Common.Utilities;
namespace BlazorRamp.DocSite.Common.Extensions;

public static class TableExtensions
{
    // Notice the extension block has NO receiver parameter name (extension(Table))
    extension(Table)
    {
        public static string CodeTable => "br-table--code-table";
    }
}
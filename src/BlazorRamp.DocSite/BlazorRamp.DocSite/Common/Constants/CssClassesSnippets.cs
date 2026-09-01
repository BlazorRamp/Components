namespace BlazorRamp.DocSite.Common.Constants;

public class CssClassesSnippets
{
    public const string Add_CssClasses_Style_Sheet = """
        <head>
            <link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
            <link rel="stylesheet" href="_content/BlazorRamp.CssClasses/assets/css/css-classes.min.css" />
        </head>
        """;

    public const string Without_The_Helper_Class = """
        <div class="br-section br-section--bordered br-section--content">
        """;

    public const string With_The_Helper_Classes = """
        <div class="@Section.Base @Section.Bordered @Section.Radius(SectionRadius.Content)">
        """;
}

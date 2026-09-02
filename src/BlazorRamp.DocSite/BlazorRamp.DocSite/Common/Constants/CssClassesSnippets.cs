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

    public const string Section_Basic_Example = """
        <div class="@Section.Base">
            <p>Basic section content.</p>
        </div>
        """;


    public const string Section_Container_With_Header_Example = """
        <div class="@Section.Base @Section.Bordered @Section.Radius(SectionRadius.Container)">
            <h3 class="@Section.Heading.Base">Section title</h3>
            <p>Content goes here.</p>
        </div>
        """;


    public const string Section_Coloured_Example = """
        <div class="@Section.Base
            @Section.Background(SectionBackground.PrimaryLighter)
            @Section.Text(SectionText.Primary)
            @Section.ColumnCentred
            @Section.Radius(SectionRadius.Content)">
            <h3 class="@Section.Heading.Base @Section.Heading.Text(SectionHeadingText.Primary)">
                Accent callout
            </h3>
            <p>This section uses the primary colour family throughout.</p>
        </div>
        """;
}

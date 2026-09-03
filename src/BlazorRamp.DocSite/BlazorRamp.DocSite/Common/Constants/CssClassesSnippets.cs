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
        <section class="@Section.Base @Section.Bordered @Section.Radius(SectionRadius.Container)">
            <h3 class="@Section.Heading.Base">Section title</h3>
            <p>Content goes here.</p>
        </section>
        """;


    public const string Section_Coloured_Example = """
        <section class="@Section.Base
            @Section.Background(SectionBackground.PrimaryLighter)
            @Section.Text(SectionText.PrimaryDark)
            @Section.ColumnCentred
            @Section.Radius(SectionRadius.Content)">
            <h3 class="@Section.Heading.Base @Section.Heading.Text(SectionHeadingText.PrimaryDark)">
                Accent callout
            </h3>
            <p>This section uses the primary colour family throughout.</p>
        </section>
        """;


    public const string Kbd_Basic_Example = """
        <p>
            Press <span class="@Kbd.Base">Esc</span> to close the dialog.
        </p>
        """;

    public const string Kbd_Modifier_Example = """
        <p>
            Press <span class="@Kbd.Base @Kbd.Scheme(KbdScheme.Accent) @Kbd.FontSize(KbdFontSize.Two)">Ctrl</span>
            + 
            <span class="@Kbd.Base @Kbd.Scheme(KbdScheme.Accent) @Kbd.FontSize(KbdFontSize.Two)">K</span> to open the command palette.
        </p>
        """;


    public const string CodeSpan_Basic_Example = """
        <p>
            Set the <span class="@CodeSpan.Base">ControlID</span> parameter to override the generated id.
        </p>
        """;

    public const string CodeSpan_Scheme_Font_Example = """
        <p>
            Call <span class="@CodeSpan.Base @CodeSpan.Scheme(CodeSpanScheme.Primary) @CodeSpan.FontSize(CodeSpanFontSize.Two)">ClearFilter() </span>to reset the input.
        </p>
        """;
}

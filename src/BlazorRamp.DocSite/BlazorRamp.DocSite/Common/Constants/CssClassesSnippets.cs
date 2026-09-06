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
            Press <span class="@Kbd.Base @Kbd.Scheme(KbdScheme.Accent) @Kbd.FontSize(UnitFontSize.Two) @Kbd.FixedRadius(UnitRadius.Two)">Ctrl</span>
            + 
            <span class="@Kbd.Base @Kbd.Scheme(KbdScheme.Accent) @Kbd.FontSize(UnitFontSize.Two) @Kbd.FixedRadius(UnitRadius.Two)">K</span> to open the command palette.
        </p>
        """;


    public const string CodeSpan_Basic_Example = """
        <p>
            Set the <span class="@CodeSpan.Base">ControlID</span> parameter to override the generated id.
        </p>
        """;

    public const string CodeSpan_Scheme_Font_Example = """
        <p>
            Call <span class="@CodeSpan.Base @CodeSpan.Scheme(CodeSpanScheme.Primary) @CodeSpan.FontSize(UnitFontSize.Two) @CodeSpan.FixedRadius(UnitRadius.Full)">ClearFilter()</span>to reset the input.
        </p>
        """;


    public const string Button_Regular_Default_Example = """
        <button class="@Button.Base @Button.Size(ButtonSize.Regular)">Settings</button>
        """;

    public const string Margin_Code_Example = """
        <div class="@Section.Base @Section.Bordered @Section.FixedRadius(UnitRadius.One) @Section.NoPadding">
            <button class="@Button.Base @Button.FixedRadius(UnitRadius.One) 
                @Margin.InlineStart(UnitSpace.Three) @Margin.BlockStart(UnitSpace.Five)">
                A button inside a section
            </button>
        </div>
        """;

    public const string Padding_Code_Example = """
        <div class="@Section.Base @Section.Bordered @Section.FixedRadius(UnitRadius.One) @Section.NoPadding 
            @Padding.InlineStart(UnitSpace.Three) @Padding.BlockStart(UnitSpace.Five)">
            <button class="@Button.Base @Button.FixedRadius(UnitRadius.One)">A button inside a section</button>
        </div>
        """;

    public const string Position_Code_Example = """
        <div style="width:400px;height:200px;" class="@Overflow.Y(UnitScroll.Auto)" tabindex="0" role="region" aria-label="Scrollable region for position sticky example.">
            <p>
                Text inside a 200px * 200px div with a scrollbar and red box below it that uses position sticky. It has a top of 50px which means the red box will never 
                move above this. Below the red box is just some lorem ipsum text for you to scroll.
            </p>
            <div style="background-color:red;height:100px;width:100px;top:50px;" class="@Position.SetAs(UnitPosition.Sticky)"></div>
            <p>
                Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent facilisis lectus sit amet varius convallis. Suspendisse tempor neque sit amet neque dignissim, ac accumsan enim pellentesque. 
                Nam a nisl vitae magna lacinia placerat ac in erat. Donec vehicula pulvinar venenatis. Maecenas vehicula vehicula nibh. Nullam vehicula leo ex, sit amet consequat mi lacinia et. Cras aliquam 
                lacus nec turpis vestibulum feugiat. Suspendisse sit amet felis at arcu viverra ornare eu imperdiet arcu. Nullam pulvinar odio in nisi pharetra sodales. Fusce tempor ex in ligula rhoncus 
                ultrices. Cras enim elit, consectetur at vestibulum eget, laoreet sit amet erat. Integer id turpis urna.
            </p>
        </div>
        """;

    public const string Overflow_Code_Example = """
        <div style="width:200px;height:75px;padding:var(--br-unit-space-3); border:1px solid var(--br-comp-all-divider-colour);" 
            class="@Overflow.XY(UnitScroll.Hidden)">
            This text will have some of its content hidden because its too big to fit in the box.
        </div>
        """;
    public const string Radius_Code_Example = """
         <div style="border:5px solid var(--br-comp-all-divider-colour);width:400px;height:150px;display:flex;justify-content:center;align-items:center;" 
            class="@Radius.BlockStartInlineStart(UnitRadius.Five) @Radius.BlockEndInlineEnd(UnitRadius.Five)">
            Text in a box with two rounded corners
        </div>
        """;
}

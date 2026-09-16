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
        <button type="button" class="@Button.Base @Button.Size(ButtonSize.Regular)">Settings</button>
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


    public const string VerticalAlign_Code_Example = """
        <table class="@Table.Base">
            <tr>
                <td class="@VerticalAlign.SetAs(UnitVerticalAlign.Top)">Top<br>extra line<br>extra line</td>
                <td class="@VerticalAlign.SetAs(UnitVerticalAlign.Middle)">Middle</td>
                <td class="@VerticalAlign.SetAs(UnitVerticalAlign.Bottom)">Bottom</td>
            </tr>
        </table>
        """;

    public const string TextAlign_Code_Example = """
        <div style="width:400px; height:100px;border:1px solid var(--br-comp-all-divider-colour);" 
        class="@TextAlign.SetAs(UnitTextAlign.Centre)">
            Text horizontally centred in a box.
        </div>
        """;



    public const string Table_Basic_Example = """
        <div class="@Section.Base @Section.NoPadding @Section.OverflowX(UnitScroll.Auto) @Padding.Block(UnitSpace.Four)">

            <table tabindex="0" aria-labelledby="basic-example" class="@Table.Base @Table.StripedRows
                @Table.AlignColumn(UnitColumn.One, UnitTextAlign.Start)
                @Table.AlignColumn(UnitColumn.Two, UnitTextAlign.Start)
                @Table.AlignColumn(UnitColumn.Three, UnitTextAlign.Start)
                @Table.AlignColumn(UnitColumn.Four, UnitTextAlign.Start)
                @Table.AlignColumn(UnitColumn.Five, UnitTextAlign.Centre)
                @Table.AlignColumn(UnitColumn.Six, UnitTextAlign.End)">

                <thead>
                    <tr>
                        <th scope="col">ID</th>
                        <th scope="col">First Name:</th>
                        <th scope="col">Surname</th>
                        <th scope="col">Country</th>
                        <th scope="col">Date of Birth</th>
                        <th scope="col">Hourly Rate</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>1</td>
                        <td>Gabriel</td>
                        <td>Kennedy</td>
                        <td>Dominican Republic</td>
                        <td>15/12/1913</td>
                        <td>£57.88</td>
                    </tr>
                    <tr>
                        <td>2</td>
                        <td>Abigail</td>
                        <td>Thompson</td>
                        <td>Guinea-Bissau</td>
                        <td>05/12/2018</td>
                        <td>£26.43</td>
                    </tr>
                    <tr>
                        <td>3</td>
                        <td>Luke</td>
                        <td>Simpson</td>
                        <td>Georgia</td>
                        <td>17/09/2024</td>
                        <td>£39.14</td>
                    </tr>
                    <tr>
                        <td>4</td>
                        <td>Sophia</td>
                        <td>Phillips</td>
                        <td>Philippines</td>
                        <td>05/06/1988</td>
                        <td>£33.05</td>
                    </tr>
                    <tr>
                        <td>5</td>
                        <td>Michael</td>
                        <td>Yates</td>
                        <td>Singapore</td>
                        <td>10/05/1921</td>
                        <td>£66.15</td>
                    </tr>
                    <tr>
                        <td>6</td>
                        <td>Olivia</td>
                        <td>Collins</td>
                        <td>Malta</td>
                        <td>23/04/1990</td>
                        <td>£74.59</td>
                    </tr>
                    <tr>
                        <td>7</td>
                        <td>Hudson</td>
                        <td>Wood</td>
                        <td>Sweden</td>
                        <td>17/11/1907</td>
                        <td>£38.64</td>
                    </tr>
                    <tr>
                        <td>8</td>
                        <td>Isla</td>
                        <td>Richards</td>
                        <td>Papua New Guinea</td>
                        <td>18/09/1948</td>
                        <td>£58.87</td>
                    </tr>
                    <tr>
                        <td>9</td>
                        <td>Benjamin</td>
                        <td>Gray</td>
                        <td>Dominica</td>
                        <td>16/07/1944</td>
                        <td>£42.33</td>
                    </tr>
                    <tr>
                        <td>10</td>
                        <td>Sophia</td>
                        <td>Phillips</td>
                        <td>Iran</td>
                        <td>27/07/1919</td>
                        <td>£73.68</td>
                    </tr>
                </tbody>
            </table>

        </div>
        """;


    public const string Table_Vertical_Scroll_Example = """
        <div class="@Section.Base @Section.NoPadding @Section.NoMarginBlock @Section.Bordered @Section.Radius(SectionRadius.Container) @Overflow.XY(UnitScroll.Hidden)">

            <div class="@Section.Base @Section.NoPadding @Section.NoMarginBlock @Section.OverflowY(UnitScroll.Auto) @Section.Radius(SectionRadius.Container)" style="height:300px;">

                <table tabindex="0" aria-labelledby="scrollable-table" class="@Table.Base @Table.StripedRows @Table.NoBorder
                    @Table.AlignColumn(UnitColumn.One, UnitTextAlign.Start)
                    @Table.AlignColumn(UnitColumn.Two, UnitTextAlign.Start)
                    @Table.AlignColumn(UnitColumn.Three, UnitTextAlign.Start)
                    @Table.AlignColumn(UnitColumn.Four, UnitTextAlign.Start)
                    @Table.AlignColumn(UnitColumn.Five, UnitTextAlign.Centre)
                    @Table.AlignColumn(UnitColumn.Six, UnitTextAlign.End)">
                    <thead>
                        <tr>
                            <th scope="col" style="width:100px;">ID</th>
                            <th scope="col">First Name:</th>
                            <th scope="col">Surname</th>
                            <th scope="col">Country</th>
                            <th scope="col">Date of Birth</th>
                            <th scope="col">Hourly Rate</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>1</td>
                            <td>Gabriel</td>
                            <td>Kennedy</td>
                            <td>Dominican Republic</td>
                            <td>15/12/1913</td>
                            <td>£57.88</td>
                        </tr>
                        <tr>
                            <td>2</td>
                            <td>Abigail</td>
                            <td>Thompson</td>
                            <td>Guinea-Bissau</td>
                            <td>05/12/2018</td>
                            <td>£26.43</td>
                        </tr>
                        <tr>
                            <td>3</td>
                            <td>Luke</td>
                            <td>Simpson</td>
                            <td>Georgia</td>
                            <td>17/09/2024</td>
                            <td>£39.14</td>
                        </tr>
                        <tr>
                            <td>4</td>
                            <td>Sophia</td>
                            <td>Phillips</td>
                            <td>Philippines</td>
                            <td>05/06/1988</td>
                            <td>£33.05</td>
                        </tr>
                        <tr>
                            <td>5</td>
                            <td>Michael</td>
                            <td>Yates</td>
                            <td>Singapore</td>
                            <td>10/05/1921</td>
                            <td>£66.15</td>
                        </tr>
                        <tr>
                            <td>6</td>
                            <td>Olivia</td>
                            <td>Collins</td>
                            <td>Malta</td>
                            <td>23/04/1990</td>
                            <td>£74.59</td>
                        </tr>
                        <tr>
                            <td>7</td>
                            <td>Hudson</td>
                            <td>Wood</td>
                            <td>Sweden</td>
                            <td>17/11/1907</td>
                            <td>£38.64</td>
                        </tr>
                        <tr>
                            <td>8</td>
                            <td>Isla</td>
                            <td>Richards</td>
                            <td>Papua New Guinea</td>
                            <td>18/09/1948</td>
                            <td>£58.87</td>
                        </tr>
                        <tr>
                            <td>9</td>
                            <td>Benjamin</td>
                            <td>Gray</td>
                            <td>Dominica</td>
                            <td>16/07/1944</td>
                            <td>£42.33</td>
                        </tr>
                        <tr>
                            <td>10</td>
                            <td>Sophia</td>
                            <td>Phillips</td>
                            <td>Iran</td>
                            <td>27/07/1919</td>
                            <td>£73.68</td>
                        </tr>
                    </tbody>
                </table>

            </div>

        </div>
        """;



    public const string Focus_Outline_Example = """
        <a href="/" class="@FocusOutline.Base @FocusOutline.OnFocus @CodeSpan.Base @CodeSpan.Scheme(CodeSpanScheme.Primary)">Home page link</a>
        """;


    public const string Gap_Example = """
        <div class="@FlexContent.Base @FlexContent.Wrap(UnitFlexWrap.Wrap) @LayoutAlignment.JustifyContent(UnitJustifyContent.Centre)

            @Gap.SetGaps(UnitGapSize.Five)">

            <div style="color:black;background-color:var(--br-unit-colour-warning-lighter);width:200px;" class="@FlexContent.Base @LayoutAlignment.JustifyContent(UnitJustifyContent.Centre)">
                Item One
            </div>
            <div style="color:black;background-color:var(--br-unit-colour-success-lighter);width:200px" class="@FlexContent.Base @LayoutAlignment.JustifyContent(UnitJustifyContent.Centre)">
                Item Two
            </div>
            <div style="color:black;background-color:var(--br-unit-colour-danger-lighter);width:200px" class="@FlexContent.Base @LayoutAlignment.JustifyContent(UnitJustifyContent.Centre)">
                Item Three
            </div>
            <div style="color:black;background-color:var(--br-unit-colour-info-lighter);width:200px" class="@FlexContent.Base @LayoutAlignment.JustifyContent(UnitJustifyContent.Centre)">
                Item Four
            </div>
        </div>
        """;

    public const string LayoutAlignment_Justify_Content_Example = """
        <div class="@FlexContent.Base @FlexContent.Wrap(UnitFlexWrap.Wrap) @Gap.SetRowGap(UnitGapSize.Five) 

            @LayoutAlignment.JustifyContent(UnitJustifyContent.SpaceBetween)">

            <div style="color:black;background-color:var(--br-unit-colour-warning-lighter);width:200px;" class="@FlexContent.Base @LayoutAlignment.JustifyContent(UnitJustifyContent.Centre)">
                Item One
            </div>
            <div style="color:black;background-color:var(--br-unit-colour-success-lighter);width:200px" class="@FlexContent.Base @LayoutAlignment.JustifyContent(UnitJustifyContent.Centre)">
                Item Two
            </div>
        </div>
        """;

    public const string FlexContent_Columns_Example = """
        <div class="@FlexContent.Base @FlexContent.UseColumns @Gap.SetRowGap(UnitGapSize.Five)">
            <span style="color:black;background-color:var(--br-unit-colour-warning-lighter);width:200px;" class="@FlexContent.Base @LayoutAlignment.JustifyContent(UnitJustifyContent.Centre)">
                Item One
            </span>
            <span style="color:black;background-color:var(--br-unit-colour-success-lighter);width:200px" class="@FlexContent.Base @LayoutAlignment.JustifyContent(UnitJustifyContent.Centre)">
                Item Two
            </span>
        </div>
        """;

    public const string GidRow_Code_Example = """
        <div class="@Section.Base @Section.Bordered @GridRow.Base">
            <div class="@GridRow.ColSpan(UnitBreakpoint.Md, UnitColSpan.Five)" style="padding: var(--br-unit-space-1); border:1px solid var(--br-comp-all-divider-colour);">
                colspan md 5
            </div>
            <div class="@GridRow.ColSpan(UnitBreakpoint.Md, UnitColSpan.Seven)" style="padding: var(--br-unit-space-1); border:1px solid var(--br-comp-all-divider-colour);">
                colspan md 7
            </div>
            <div class="@GridRow.ColSpan(UnitBreakpoint.Lg, UnitColSpan.Four)" style="padding: var(--br-unit-space-1); border:1px solid var(--br-comp-all-divider-colour);">
                colspan lg 4
            </div>
        </div>
        """;

    public const string Svg_Icon_Code_Example = """
        <a class="@Button.Base @Button.LinkStyle @Gap.SetColGap(UnitGapSize.Four)" href="css-classes/section/api">
            <span class="@SvgIcon.Base @SvgIcon.Size(UnitIconSize.ExtraLarge)" aria-hidden="true" style="--_svg-icon-source:var(--svg-left-arrow-circle-icon)"></span>
            <span>@GlobalValues.CSS_Name_Section - @GlobalValues.Common_Page_Title_Api</span>
        </a>
        """;


    public const string Link_Code_Example = """
        <a class="@Link.Base" href="/">Blazor Ramp home page</a>    
        """;

}

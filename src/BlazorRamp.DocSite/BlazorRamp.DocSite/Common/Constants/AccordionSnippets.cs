namespace BlazorRamp.DocSite.Common.Constants;

public class AccordionSnippets
{

    public const string Add_Accordion_Style_Sheet = """
        <head>
            <link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
            <link rel="stylesheet" href="_content/BlazorRamp.Accordion/assets/css/accordion.min.css" />
        </head>
        """;


    public const string Accordion_Setting_Parameters_Example = """
        <Accordion HeadingLevel="HeadingLevel.H3" ExpandMode="ExpandMode.Multiple" style="margin-bottom:var(--br-unit-space-7)">
            <AccordionItem HeadingText="Food" PanelHasTabIndex="true" PanelIsRegion="false" SvgIcon="--svg-food-icon" PersistContent="true">
                <PanelContent>
                    <p>
                        Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin sit amet sem vulputate, interdum justo vitae, sagittis nulla. 
                        Ut lectus augue, consectetur ut massa id,tristique luctus eros. Donec tempus augue velit, vel vehicula tellus consequat vel. 
                        Integer tristique sem ac nisi sodales placerat. Integer nulla ex, tincidunt ut eleifend ut, vestibulum vel leo. Proin diam 
                        urna, finibus porttitor purus nec, tristique varius justo. Nunc vel dui sed dui elementum facilisis. Curabitur ac fermentum
                        urna. Integer vestibulum fermentum massa, quis auctor elit viverra eget.
                    </p>
                </PanelContent>
            </AccordionItem>
            <AccordionItem HeadingText="Travel" PanelHasTabIndex="true" PanelIsRegion="false" SvgIcon="--svg-airplane-icon" PersistContent="true">
                <PanelContent>
                    <p>
                        Nulla eu est lacus. Mauris sodales tortor in sem viverra porta. Nam in dapibus massa. Vivamus et augue quis mauris luctus pretium
                        eu eu dolor. Maecenas gravida nisl non ante facilisis, pulvinar vestibulum nibh sagittis. Pellentesque mollis sem at arcu mattis, 
                        id molestie augue cursus. Pellentesque dolor urna, ultricies quis iaculis ac, rutrum luctus nulla. Integer et consequat erat. Sed 
                        eget metus in est pulvinar ultrices at nec lorem. Sed aliquam massa eget dui aliquet ullamcorper. Sed elit nisi, maximus et erat 
                        molestie, bibendum rutrum diam. Phasellus cursus eleifend porta.
                    </p>
                </PanelContent>
            </AccordionItem>
            <AccordionItem HeadingText="Exercise" PanelHasTabIndex="true" PanelIsRegion="false" SvgIcon="--svg-runner-icon" PersistContent="true">
                <PanelContent>
                    <p>
                        Mauris imperdiet nisi nec pulvinar porta. Sed semper viverra venenatis. Sed accumsan, erat condimentum ornare malesuada, erat mauris 
                        tincidunt enim, non interdum quam sem ac velit. Nunc et enim lorem. Maecenas gravida tortor eget ligula efficitur ullamcorper. Cras ut 
                        nisi elementum, maximus ante eu, vestibulum nibh. Vestibulum nec iaculis elit, sit amet sagittis ex.
                    </p>
                </PanelContent>
            </AccordionItem>
        </Accordion>
        """;
}

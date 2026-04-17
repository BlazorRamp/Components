namespace BlazorRamp.DocSite.Common.Constants;

public class TabsSnippets
{

    public const string Add_Tabs_Style_Sheet = """
        <head>
            <link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
            <link rel="stylesheet" href="_content/BlazorRamp.Tabs/assets/css/tabs.min.css" />
        </head>
        """;


    public const string Tabs_Setting_Parameters_Example = """
        <Tabs @bind-ActiveTabIndex="_tabindex" AriaLabelledBy="lifestyle-tabs" AutoActivatePanel="true" TabIconPosition="TabIconPosition.Left"">
            <Tab TabTitle="Food" SvgIcon="--svg-food-icon" HasPanelTabIndex="true" PersistContent="true">
                <TabPanelContent>
                    <h3>Recipes</h3>
                    <p>
                        Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin sit amet sem vulputate, interdum justo vitae, 
                        sagittis nulla. Ut lectus augue, consectetur ut massa id, tristique luctus eros. Donec tempus augue velit, vel 
                        vehicula tellus consequat vel. Integer tristique sem ac nisi sodales placerat. Integer nulla ex, tincidunt ut
                        eleifend ut, vestibulum vel leo. Proin diam urna, finibus porttitor purus nec, tristique varius justo. Nunc vel 
                        dui sed dui elementum facilisis. Curabitur ac fermentum urna. Integer vestibulum fermentum massa, quis auctor elit 
                        viverra eget.
                    </p>
                </TabPanelContent>
            </Tab>
            <Tab TabTitle="Travel" SvgIcon="--svg-airplane-icon" HasPanelTabIndex="true" PersistContent="true">

                <TabPanelContent>
                    <h3>Itinerary</h3>
                    <p>
                        Nulla eu est lacus. Mauris sodales tortor in sem viverra porta. Nam in dapibus massa. Vivamus et augue quis mauris 
                        luctus pretium eu eu dolor. Maecenas gravida nisl non ante facilisis, pulvinar vestibulum nibh sagittis. Pellentesque 
                        mollis sem at arcu mattis, id molestie augue cursus. Pellentesque dolor urna, ultricies quis iaculis ac, rutrum luctus 
                        nulla. Integer et consequat erat. Sed eget metus in est pulvinar ultrices at nec lorem. Sed aliquam massa eget dui aliquet 
                        ullamcorper. Sed elit nisi, maximus et erat molestie, bibendum rutrum diam. Phasellus cursus eleifend porta.
                    </p>

                </TabPanelContent>
            </Tab>
            <Tab TabTitle="Exercise" SvgIcon="--svg-runner-icon" HasPanelTabIndex="true" PersistContent="true">

                <TabPanelContent>
                    <h3>Workouts</h3>
                    <p>
                        Mauris imperdiet nisi nec pulvinar porta. Sed semper viverra venenatis. Sed accumsan, erat condimentum ornare malesuada, 
                        erat mauris tincidunt enim, non interdum quam sem ac velit. Nunc et enim lorem. Maecenas gravida tortor eget ligula 
                        efficitur ullamcorper. Cras ut nisi elementum, maximus ante eu, vestibulum nibh. Vestibulum nec iaculis elit, sit amet 
                        sagittis ex.
                    </p>
                </TabPanelContent>
            </Tab>
        </Tabs>
        </Tabs>
        """;

}

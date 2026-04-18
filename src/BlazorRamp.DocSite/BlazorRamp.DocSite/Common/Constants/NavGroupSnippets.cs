namespace BlazorRamp.DocSite.Common.Constants;

public class NavGroupSnippets
{

    public const string Add_Style_Sheet = """
        <head>
            <link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
            <link rel="stylesheet" href="_content/BlazorRamp.NavGroup/assets/css/nav-group.min.css" />
        </head>
        """;

    public const string Framework_Section = """
        <h2 id="framework-section" class="main-menu__heading">Frameworks</h2>
        <NavGroup AriaLabelledby="framework-section">
            <NavSeparator />
            <NavSection Title="Modal Dialog">
                <NavGroupLink Href="/frameworks/modal-dialog/overview" VisuallyHiddenPrefix="Modal Dialogl" LinkText="Overview" />
                <NavGroupLink Href="/frameworks/modal-dialog/installation" VisuallyHiddenPrefix="Modal Dialog" LinkText="Installation" />
                <NavGroupLink Href="/frameworks/modal-dialog/accessibility" VisuallyHiddenPrefix="Modal Dialog" LinkText="Accessibility" />
                <NavGroupLink Href="/frameworks/modal-dialog/api" VisuallyHiddenPrefix="Modal Dialog" LinkText="API Reference" />
                <NavGroupLink Href="/frameworks/modal-dialog/usage" VisuallyHiddenPrefix="Modal Dialog" LinkText="Usage" />
            </NavSection>    
        </NavGroup>
        """;
}

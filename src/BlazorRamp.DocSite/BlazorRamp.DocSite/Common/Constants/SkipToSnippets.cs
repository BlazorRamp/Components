namespace BlazorRamp.DocSite.Common.Constants;

public class SkipToSnippets
{

    public const string Add_Skip_To_Style_Sheet = """
        <head>
            <link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
            <link rel="stylesheet" href="_content/BlazorRamp.SkipTo/assets/css/skip-to.min.css" />
        </head>
        """;

    public const string Skip_To_Params_Example = """
        <SkipTo IconVisible="true" SkipToText="Skip to content" 
            SkipToType="SkipToType.Site" TargetID="app__main" />
        """;

    public const string Skip_To_Section_Example = """
        <SkipTo IconVisible="true" SkipToText="Skip to section content" 
            SkipToType="SkipToType.Section" TargetID="section-one" />
        """;
}

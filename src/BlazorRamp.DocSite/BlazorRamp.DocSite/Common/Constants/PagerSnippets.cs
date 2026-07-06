namespace BlazorRamp.DocSite.Common.Constants;

public class PagerSnippets
{
    public const string Add_Pager_Style_Sheet = """
        <head>
            <link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
            <link rel="stylesheet" href="_content/BlazorRamp.Pager/assets/css/pager.min.css" />
        </head>
        """;


    public const string Overview_Code_Example = """

        <Pager @bind-CurrentPage="@_fullButtonCurrentPage" AriaLabel="Four button pager" TotalItemCount="100" CurrentItemCount="80" ItemsPerPage="10" 
            PagerSelectorType="PagerSelectorType.Button" ShowFirstLast="true" />

        <Pager @bind-CurrentPage="_minButtonCurrentPage" AriaLabel="Two button pager" TotalItemCount="50" CurrentItemCount="50" ItemsPerPage="10" 
            PagerSelectorType="PagerSelectorType.Button" ShowFirstLast="false" />

        <Pager @bind-CurrentPage="_fullLinkCurrentPage" AriaLabel="Four links pager" TotalItemCount="100" CurrentItemCount="100" ItemsPerPage="10" 
            PagerSelectorType="PagerSelectorType.Link" ShowFirstLast="true" />

        <Pager @bind-CurrentPage="_minLinkCurrentPage" QueryParamName="another-page" AriaLabel="Two links pager" TotalItemCount="50" CurrentItemCount="50" 
            ItemsPerPage="10" PageCountText="Current page {currentpage} of {lastpage}" PagerSelectorType="PagerSelectorType.Link" ShowFirstLast="false" />

        @code {

            [Inject] NavigationManager _navigationManager { get; set; } = default!;

            private int _fullButtonCurrentPage = 1;
            private int _minButtonCurrentPage  = 1;
            private int _fullLinkCurrentPage   = 1;
            private int _minLinkCurrentPage    = 1;

            protected override void OnInitialized()
            {
                /*
                    * Only used if you open in new window.
                    * 
                    * One time thing uses binding there after - you can also use [SupplyParameterFromQuery(Name = "page")] but that will tigger OnParametersSet on this page
                 */

                var pageValueFull = HttpUtility.ParseQueryString(new Uri(_navigationManager.Uri).Query).Get("page");
                var pageValueMin = HttpUtility.ParseQueryString(new Uri(_navigationManager.Uri).Query).Get("another-page");

                _fullLinkCurrentPage = int.TryParse(pageValueFull, out int full) ? full : 1;
                _minLinkCurrentPage = int.TryParse(pageValueMin, out int min) ? min : 1;
            }
        }

        """;
}

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


    public const string Usage_Code_Example = """

                
        <h3 id="table-title">Weather table</h3>

        <div class="br-input-row" >
            <DebounceFilter class="br-col-xs-12 br-col-sm-6" FilterLabelText="Filter table" HintText="Data filtered across all columns on pause of typing."
                            DebounceDelayMs="250" SvgIcon="--svg-filter-icon" Replayable="false"
                            RegexPattern="^[A-Za-z0-9]*$" ValidationMessage="Invalid filter, filtering paused, letters and numbers only"
                            OnDebounceFilterResult="HandleDebounce" ParseErrorMessage="System error, filtering is unavailable at this time." />

        </div>

        <div class="weather-table">
            <table aria-labelledby="table-title">
                <thead>
                    <tr>
                        <th scope="col">Date</th>
                        <th scope="col">Temp. (C)</th>
                        <th scope="col">Temp. (F)</th>
                        <th scope="col">Summary</th>
                    </tr>
                </thead>
                <tbody>
                    @foreach (var forecast in _pagedForecasts)
                    {
                        <tr>
                            <td>@forecast.Date.ToShortDateString()</td>
                            <td>@forecast.TemperatureC</td>
                            <td>@forecast.TemperatureF</td>
                            <td>@forecast.Summary</td>
                        </tr>
                    }
                </tbody>
            </table>
        </div>
        <Pager CurrentPage="@_currentPage" CurrentPageChanged="HandleCurrentPageChanged" AriaLabel="Weather pager" TotalItemCount="@_totalItemCount" 
        CurrentItemCount="@_currentItemCount" ItemsPerPage="10" PagerSelectorType="PagerSelectorType.Button" ShowFirstLast="true" PageAlignment="PageAlignment.End" />
                       
        
        @code {

            private List<WeatherForecast> _forecasts         = [];
            private List<WeatherForecast> _pagedForecasts    = [];
            private List<WeatherForecast> _filteredForecasts = [];

            private string _filterText = String.Empty;
            private int _currentPage      = 1;
            private int _totalItemCount   = 0;
            private int _currentItemCount = 0;
            private int _itemsPerPage     = 10;

            protected override void OnInitialized()
            {
                var startDate = DateOnly.FromDateTime(DateTime.Now);
                var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

                _forecasts = Enumerable.Range(1, 100)
                                .Select(index => new WeatherForecast(startDate.AddDays(index), Random.Shared.Next(-20, 55), summaries[Random.Shared.Next(summaries.Length)])).ToList();

                _totalItemCount = _forecasts.Count;
                _currentItemCount = _totalItemCount;
                _pagedForecasts = _forecasts.Take(_itemsPerPage).ToList();
            }
            private async Task HandleCurrentPageChanged(int requestedPage)
            {
                if (String.IsNullOrWhiteSpace(_filterText))
                {
                    _pagedForecasts = _forecasts.Skip((requestedPage - 1) * _itemsPerPage).Take(_itemsPerPage).ToList();
                }
                else
                {
                    _pagedForecasts = _filteredForecasts.Skip((requestedPage - 1) * _itemsPerPage).Take(_itemsPerPage).ToList();
                }
                _currentPage = requestedPage;
            }

            private async Task HandleDebounce(DebouncedFilterResult result)
            {
                if (result.ExceptionMessage is not null || false == result.IsValid) return;

                await FilterTable(result.FilterValue);
            }

            private async Task FilterTable(string filterValue)
            {
                _filteredForecasts = _forecasts!.Where(a => a.Summary!.ToLower().Contains(filterValue.ToLower())
                                         || a.TemperatureC.ToString().Contains(filterValue)
                                         || a.TemperatureF.ToString().Contains(filterValue)
                                         || a.Date.ToString().Contains(filterValue)
                                         || filterValue == String.Empty).ToList();

                _currentItemCount = _filteredForecasts.Count;
                _pagedForecasts   = _filteredForecasts.Take(_itemsPerPage).ToList();
                _currentPage      = _currentItemCount > 0 ? 1 : 0;
                _filterText       = filterValue;
                await InvokeAsync(StateHasChanged);
                /*
                    * Debounce uses a Func not an EventCallback so no renders occur if the filter is invalid 
                    * As we are here its valid so call state as changed to render the page and child components. 
                */
            }

            private record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
            {
                public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
            }
        }
        """;
}

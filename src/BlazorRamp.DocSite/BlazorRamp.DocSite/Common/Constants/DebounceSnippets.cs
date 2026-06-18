namespace BlazorRamp.DocSite.Common.Constants;

public class DebounceSnippets
{
    public const string Add_Debounce_Filter_Style_Sheet = """
        <head>
            <link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
            <link rel="stylesheet" href="_content/BlazorRamp.DebounceFilter/assets/css/debounce-filter.min.css" />
        </head>
        """;



    public const string Usage_Example = """

        <div class="br-input-row">
            <DebounceFilter class="br-col-xs-6"  @ref="DebounceFilterRef" 
                            FilterLabelText="Filter table" HintText="Data filtered across all columns on pause of typing."
                            DebounceDelayMs="500" SvgIcon="--svg-filter-icon" Replayable="true"
                            RegexPattern="^[A-Za-z0-9]*$" ValidationMessage="Invalid filter, filtering paused, letters and numbers only"
                            OnDebounceFilterResult="HandleDebounce" ParseErrorMessage="System error, filtering is unavailable at this time." />

        </div>
        <div class="weather-table">

            <table>
                <caption>Weather Table</caption>
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>Temp. (C)</th>
                        <th>Temp. (F)</th>
                        <th>Summary</th>
                    </tr>
                </thead>
                <tbody>
                    @foreach (var forecast in _filteredForecasts)
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
        <div style="display:flex;justify-content:space-between">
            <span> @_filterRowMessage</span>
            <button class="normal-button" @onclick="ClearFilter">Clear Filter</button>
        </div>
                       
        
        @code {
            [Inject] ILiveRegionService _liveRegionService { get; set; } = default!;

            private DebounceFilter DebounceFilterRef { get; set; } = default!;

            private List<WeatherForecast> _forecasts = [];
            private List<WeatherForecast> _filteredForecasts = [];

            private string _filterRowMessage = String.Empty;

            private async Task HandleDebounce(DebouncedFilterResult result)
            {
                if (result.ExceptionMessage is not null || false == result.IsValid) return;

                await FilterTable(result.FilterValue);

                _filterRowMessage = result.ClearCalled ? "No filter applied showing all 10 rows" : $"Showing {_filteredForecasts.Count} rows";

                await _liveRegionService.MakeAnnouncement(new Announcement(_filterRowMessage));
                await InvokeAsync(StateHasChanged);
            }

            private async Task FilterTable(string filterValue)
            {
                _filteredForecasts = _forecasts!.Where(a => a.Summary!.ToLower().Contains(filterValue)
                                         || a.TemperatureC.ToString().Contains(filterValue)
                                         || a.TemperatureF.ToString().Contains(filterValue)
                                         || a.Date.ToString().Contains(filterValue)
                                         || filterValue == String.Empty).ToList();
            }

            private async Task ClearFilter()
            {
                await DebounceFilterRef.ClearFilter();
            }  


            protected override void OnInitialized()
            {
                var startDate = DateOnly.FromDateTime(DateTime.Now);
                var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

                _forecasts = Enumerable.Range(1, 10)
                                .Select(index => new WeatherForecast(startDate.AddDays(index), Random.Shared.Next(-20, 55), summaries[Random.Shared.Next(summaries.Length)])).ToList();

                _filteredForecasts = [.. _forecasts];
                _filterRowMessage = "No filter applied showing all 10 rows";
            }
            private record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
            {
                public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
            }
        }
        
        """;
}

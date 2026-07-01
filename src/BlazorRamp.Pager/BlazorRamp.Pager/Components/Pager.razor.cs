using BlazorRamp.Core.Common.Constants;
using BlazorRamp.Core.Common.Models;
using BlazorRamp.Core.Services;
using BlazorRamp.Pager.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorRamp.Pager.Components;

public partial class Pager : IAsyncDisposable
{
    [Parameter] public PagerSelectorType PagerSelectorType { get; set; } = PagerSelectorType.Button;
    [Parameter] public PagerAnnouncementType PagerAnnouncementType { get; set; } = PagerAnnouncementType.WithAnnouncement;
    [Parameter] public PageAlignment PageAlignment { get; set; } = PageAlignment.Centred;

    [Parameter] public string AriaLabel { get; set; } = GlobalValues.Pager_Aria_Label;
    [Parameter] public string NextText { get; set; } = GlobalValues.Pager_Selector_Next_Text;
    [Parameter] public string PreviousText { get; set; } = GlobalValues.Pager_Selector_Prev_Text;
    [Parameter] public string FirstText { get; set; } = GlobalValues.Pager_Selector_First_Text;
    [Parameter] public string LastText { get; set; } = GlobalValues.Pager_Selector_Last_Text;

    [Parameter] public bool ShowFirstLast { get; set; } = true;
    [Parameter] public string QueryParamName { get; set; } = GlobalValues.Pager_Query_String_Param_Name;

    [Parameter] public string PageCountText { get; set; } = default!;
    [Parameter] public string FilterCountText { get; set; } = default!;
    [Parameter] public string NoRecordsText { get; set; } = default!;

    [Parameter] public int TotalItemCount { get; set; } = 0;
    [Parameter] public int CurrentItemCount { get; set; } = 0;
    [Parameter] public int ItemsPerPage { get; set; } = GlobalValues.Pager_Rows_Per_Page;
    [Parameter] public int CurrentPage { get; set; } = 0;

    [Parameter] public EventCallback<int> CurrentPageChanged { get; set; }

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private ILiveRegionService LiveRegionService { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private IJSObjectReference? _jSModule = null;

    private ElementReference FirstNavRef { get; set; }
    private ElementReference LastNavRef { get; set; }
    private ElementReference NextNavRef { get; set; }
    private ElementReference PreviousNavRef { get; set; }


    private NavSelectorType _setFocusOn = NavSelectorType.None;
    private PagerSelectorType _pagerSelectorType = PagerSelectorType.Button;
    private PagerAnnouncementType _pagerAnnouncementType = PagerAnnouncementType.WithAnnouncement;

    private string _queryParamName = GlobalValues.Pager_Query_String_Param_Name;
    private string _nextText = GlobalValues.Pager_Selector_Next_Text;
    private string _prevText = GlobalValues.Pager_Selector_Prev_Text;
    private string _firstText = GlobalValues.Pager_Selector_First_Text;
    private string _lastText = GlobalValues.Pager_Selector_Last_Text;

    private string _ariaLabel = GlobalValues.Pager_Aria_Label;
    private string _ariaLabelID = Guid.NewGuid().ToString();
    private string _infoTextID = Guid.NewGuid().ToString();

    private string _navSelectorFirstID = Guid.NewGuid().ToString();
    private string _navSelectorPreviousID = Guid.NewGuid().ToString();
    private string _navSelectorNextID = Guid.NewGuid().ToString();
    private string _navSelectorLastID = Guid.NewGuid().ToString();

    private string _pageCountText = GlobalValues.Pager_Count_Text;
    private string _filterCountText = GlobalValues.Pager_Filter_Count_Text;
    private string _informationText = GlobalValues.Pager_No_Records_Text;
    private string _noRecordsText = GlobalValues.Pager_No_Records_Text;

    private int _lastPage = 7;
    private int _currentPage = 0;
    private int _currentRowCount = 0;
    private int _totalRowCount = 0;
    private int _rowsPerPage = GlobalValues.Pager_Rows_Per_Page;

    private bool _userChangeRequest = false;
    private bool _showFirstLast     = true;

    private int _pageRequested = 0;

    private CancellationTokenSource _announceDebounceTS = new();

    protected override void OnParametersSet()
    {
        _rowsPerPage = ItemsPerPage < 1 ? GlobalValues.Pager_Rows_Per_Page : ItemsPerPage;
        _totalRowCount = TotalItemCount;
        _currentRowCount = CurrentItemCount > TotalItemCount ? TotalItemCount : CurrentItemCount;

        _pageCountText = String.IsNullOrWhiteSpace(PageCountText) ? GlobalValues.Pager_Count_Text : PageCountText.Trim();
        _filterCountText = String.IsNullOrWhiteSpace(FilterCountText) ? GlobalValues.Pager_Filter_Count_Text : FilterCountText.Trim();
        _noRecordsText = String.IsNullOrWhiteSpace(NoRecordsText) ? GlobalValues.Pager_No_Records_Text : NoRecordsText.Trim();

        _nextText = String.IsNullOrWhiteSpace(NextText) ? GlobalValues.Pager_Selector_Next_Text : NextText.Trim();
        _prevText = String.IsNullOrWhiteSpace(PreviousText) ? GlobalValues.Pager_Selector_Prev_Text : PreviousText.Trim();
        _lastText = String.IsNullOrWhiteSpace(LastText) ? GlobalValues.Pager_Selector_Last_Text : LastText.Trim();
        _firstText = String.IsNullOrWhiteSpace(FirstText) ? GlobalValues.Pager_Selector_First_Text : FirstText.Trim();

        _pagerAnnouncementType = PagerAnnouncementType;

        _lastPage = (_currentRowCount < 1) ? 1 : (int)Math.Ceiling((double)_currentRowCount / _rowsPerPage);

        (_currentPage, _setFocusOn) = SetPageAndFocus(CurrentPage, _currentPage, _lastPage);

        _informationText = SetInformationText(_currentPage, _lastPage, _rowsPerPage, _currentRowCount, _totalRowCount, _pageCountText, _filterCountText, _noRecordsText);
    }

    protected override void OnInitialized()
    {
        _pagerSelectorType = PagerSelectorType;
        _ariaLabel = String.IsNullOrWhiteSpace(AriaLabel) ? GlobalValues.Pager_Aria_Label : AriaLabel.Trim();
        _showFirstLast = ShowFirstLast;
        _queryParamName = String.IsNullOrWhiteSpace(QueryParamName) ? GlobalValues.Pager_Query_String_Param_Name : QueryParamName.Trim();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {

      

        if (_setFocusOn != NavSelectorType.None && _userChangeRequest)
        {

            if (_setFocusOn == NavSelectorType.Previous) await PreviousNavRef.FocusAsync();

            if (_setFocusOn == NavSelectorType.Next) await NextNavRef.FocusAsync();

            _setFocusOn = NavSelectorType.None;
          
        }
        await MakeAnnouncement(_pagerAnnouncementType, _informationText, _ariaLabel, _userChangeRequest);

        _userChangeRequest = false;
    }
    private bool CheckSetShowLinks(bool isStartGroup, int currentPage, int lastPage)

        => isStartGroup ? (currentPage < 2 ? false : true)
                        : (currentPage >= lastPage ? false : true);


    private string UpdateUriQueryParams(int pageNo, string queryParamName)

        => NavigationManager.GetUriWithQueryParameter(queryParamName, pageNo);

    private static string SetInformationText(int currentPage, int totalPages, int rowsPerPage, int currentRows, int totalRows,
                                         string pageCountText, string filterCountText, string noRecordsText)
    {
        if (currentRows < 1 || totalRows < 1) return GlobalValues.Pager_No_Records_Text;

        var rowStart = (rowsPerPage * currentPage) - (rowsPerPage - 1);
        var rowEnd = Math.Min(rowsPerPage * currentPage, currentRows);
        var isFiltered = currentRows != totalRows;

        var filteredString = isFiltered ? $"{filterCountText.Replace("{filteredrows}", currentRows.ToString()).Replace("{totalrows}", totalRows.ToString())}"
                                        : String.Empty;

        var infoText = pageCountText.Replace("{firstpage}", currentPage.ToString())
                                    .Replace("{lastpage}", totalPages.ToString())
                                    .Replace("{startrow}", rowStart.ToString())
                                    .Replace("{endrow}", rowEnd.ToString())
                                    .Replace("{totalrows}", currentRows.ToString());

        return $"{infoText} {filteredString}".TrimEnd();
    }

    private async Task RequestPageChange(NavSelectorType currentSelector, int pageRequested, int lastPage)
    {
        var shouldReturn = (currentSelector, pageRequested) switch
        {
            (NavSelectorType.First, < 2) => true,
            (NavSelectorType.Last, _) when pageRequested >= lastPage => true,
            (NavSelectorType.Previous, < 1) => true,
            (NavSelectorType.Next, _) when pageRequested >= lastPage => true,
            _ => false
        };

        _pageRequested = pageRequested;
        _userChangeRequest = true;

        if (CurrentPageChanged.HasDelegate) await CurrentPageChanged.InvokeAsync(pageRequested);
    }

    private (int currentPage, NavSelectorType setFocusOn) SetPageAndFocus(int currentPageParam, int previousPage, int lastPage)
    {
        var currentPage = Math.Clamp(currentPageParam, 1, lastPage);

        if (currentPage != previousPage)
        {
            var setFocusOn = currentPage > previousPage
                ? (currentPage >= lastPage ? NavSelectorType.Previous : NavSelectorType.Next)
                : (currentPage <= 1 ? NavSelectorType.Next : NavSelectorType.Previous);

            return (currentPage, setFocusOn);
        }

        return (currentPage, NavSelectorType.None);
    }

    private string? CheckSetDisableButton(NavSelectorType navItem, int currentPage, int lastPage, int currentRowCount, int totalRowCount)
    {
        if (currentRowCount < 1 || totalRowCount < 1) return "true";

        return navItem switch
        {
            NavSelectorType.First => currentPage < 2 ? "true" : null,
            NavSelectorType.Previous => currentPage < 2 ? "true" : null,
            NavSelectorType.Next => currentPage >= lastPage ? "true" : null,
            NavSelectorType.Last => currentPage >= lastPage ? "true" : null,
            _ => null
        };
    }
    private string CheckSetTabIndex(NavSelectorType navItem, int currentPage, int lastPage, int currentRowCount, int totalRowCount)
    {
        if (currentRowCount < 1 || totalRowCount < 1) return "-1";

        return navItem switch
        {
            NavSelectorType.First => currentPage < 2 ? "-1" : "0",
            NavSelectorType.Previous => currentPage < 2 ? "-1" : "0",
            NavSelectorType.Next => currentPage >= lastPage ? "-1" : "0",
            NavSelectorType.Last => currentPage >= lastPage ? "-1" : "0",
            _ => "0"
        };
    }
    private static string GetPagerClasses(PageAlignment alignment)

        => alignment switch
        {
            PageAlignment.Start => $"{GlobalValues.Pager_Class} {GlobalValues.Pager_Align_Start_Modifier}",
            PageAlignment.End => $"{GlobalValues.Pager_Class} {GlobalValues.Pager_Align_End_Modifier}",
            _ => $"{GlobalValues.Pager_Class}",
        };


    private async Task MakeAnnouncement(PagerAnnouncementType announcementType, string informationText, string triggerLabel, bool pageChanged = true)
    {
        if (pageChanged && announcementType == PagerAnnouncementType.WithAnnouncement)
        {

            _announceDebounceTS.Cancel();
            _announceDebounceTS = new CancellationTokenSource();

            try
            {
                await Task.Delay(400, _announceDebounceTS.Token);
                Announcement announcement = new(informationText, AnnouncementType.Info, triggerLabel, LiveRegionType.Assertive);
                await LiveRegionService.MakeAnnouncement(announcement, true);
            }
            catch (TaskCanceledException) { }
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_jSModule is not null)
        {
            try
            {
                await _jSModule.DisposeAsync();
            }
            catch { }
        }
    }
}



using BlazorRamp.Core.Common.Constants;
using BlazorRamp.Core.Common.Models;
using BlazorRamp.Core.Services;
using BlazorRamp.Pager.Common.Constants;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;

namespace BlazorRamp.Pager.Components;

public partial class Pager
{
    [Parameter] public PagerSelectorType    PagerSelectorType    { get; set; } = PagerSelectorType.Button;
    [Parameter] public PagerAnnouncmentType PagerAnnouncmentType { get; set; } = PagerAnnouncmentType.WithAnnouncement;

    [Parameter] public string AriaLabel    { get; set; } 
    [Parameter] public string NextText      { get; set; } = GlobalValues.Pager_Selector_Next_Text;
    [Parameter] public string PreviousText  { get; set; } = GlobalValues.Pager_Selector_Prev_Text;
    [Parameter] public string FirstText     { get; set; } = GlobalValues.Pager_Selector_First_Text;
    [Parameter] public string LastText      { get; set; } = GlobalValues.Pager_Selector_Last_Text;

    [Parameter] public bool ShowFirstLast   { get; set; } = true;
    [Parameter] public string QueryParamName { get; set; } = GlobalValues.Pager_Query_String_Param_Name;

    [Parameter] public string PageCountText  { get; set; } = default;
    [Parameter] public string FilterCountText { get; set; } = default;
    [Parameter] public string NoRecordsText   { get; set; } = default;

    [Parameter] public int TotalItemCount   { get; set; } = 0;
    [Parameter] public int CurrentItemCount { get; set; } = 0;
    [Parameter] public int ItemsPerPage     { get; set; } = 0;
    [Parameter] public int CurrentPage      { get; set; } = 0;

    [Parameter] public EventCallback<int> CurrentPageChanged { get; set; }

    [Inject] private NavigationManager NavigationManager  { get; set; } = default!;
    [Inject] private ILiveRegionService LiveRegionService { get; set; } = default!;

    private ElementReference FirstNavRef    { get; set; }
    private ElementReference LastNavRef     { get; set; }
    private ElementReference NextNavRef     { get; set; }
    private ElementReference PreviousNavRef { get; set; }

    private NavFocusType         _setFocusOn           = NavFocusType.None;
    private PagerSelectorType    _pagerSelectorType    = PagerSelectorType.Button;
    private PagerAnnouncmentType _pagerAnnouncmentType = PagerAnnouncmentType.WithAnnouncement;

    private string _queryParamName = GlobalValues.Pager_Query_String_Param_Name;
    private string _nextText       = GlobalValues.Pager_Selector_Next_Text;
    private string _prevText       = GlobalValues.Pager_Selector_Prev_Text;
    private string _firstText      = GlobalValues.Pager_Selector_First_Text;
    private string _lastText       = GlobalValues.Pager_Selector_Last_Text;

    private string _ariaLabel      = GlobalValues.Pager_Aria_Label;
    private string _ariaLabelID    = Guid.NewGuid().ToString();

    private string _pageCountText   = GlobalValues.Pager_Count_Text;
    private string _filterCountText = GlobalValues.Pager_Filter_Count_Text;
    private string _informationText = GlobalValues.Pager_No_Records_Text;
    private string _noRecordsText   = GlobalValues.Pager_No_Records_Text;

    private string _infoTextID = Guid.NewGuid().ToString();

    private int _lastPage         = 0;
    private int _currentPage      = 0;
    private int _currentItemCount = 0;
    private int _totalItemCount   = 0;

    private bool _pageChanged = false;
    private bool _showFirstLast = true;

    private CancellationTokenSource _announceDebounceTS = new();

    protected override async Task OnParametersSetAsync()
    {
        _totalItemCount   = TotalItemCount;
        _currentItemCount = CurrentItemCount > TotalItemCount ? TotalItemCount : CurrentItemCount;

        _pageCountText   = String.IsNullOrWhiteSpace(PageCountText)   ? GlobalValues.Pager_Count_Text        : PageCountText.Trim();
        _filterCountText = String.IsNullOrWhiteSpace(FilterCountText) ? GlobalValues.Pager_Filter_Count_Text : FilterCountText.Trim();
        _noRecordsText   = String.IsNullOrWhiteSpace(NoRecordsText)   ? GlobalValues.Pager_No_Records_Text   : NoRecordsText.Trim();

        _nextText  = String.IsNullOrWhiteSpace(NextText)     ? GlobalValues.Pager_Selector_Next_Text  : NextText.Trim();
        _prevText  = String.IsNullOrWhiteSpace(PreviousText) ? GlobalValues.Pager_Selector_Prev_Text  : PreviousText.Trim();
        _lastText  = String.IsNullOrWhiteSpace(LastText)     ? GlobalValues.Pager_Selector_Last_Text  : LastText.Trim();
        _firstText = String.IsNullOrWhiteSpace(FirstText)    ? GlobalValues.Pager_Selector_First_Text : FirstText.Trim();

        _pagerAnnouncmentType = PagerAnnouncmentType;
        _lastPage             = (CurrentItemCount < 1 || ItemsPerPage < 1) ? 1 : (int)Math.Ceiling((double)CurrentItemCount / ItemsPerPage);


       _currentPage = Math.Clamp(CurrentPage, 1, _lastPage);

        _informationText = SetInformationText(_currentPage, _lastPage, ItemsPerPage, _currentItemCount, _totalItemCount, _pageCountText, _filterCountText, _noRecordsText);

    }

    protected override void OnInitialized()
    {
        _pagerSelectorType = PagerSelectorType;
        _ariaLabel = String.IsNullOrWhiteSpace(AriaLabel) ? GlobalValues.Pager_Aria_Label : AriaLabel.Trim();
        _showFirstLast = ShowFirstLast;
    }
    

    private static string SetInformationText(int currentPage, int totalPages, int itemsPerPage, int currentItems, int totalItems, 
                                             string pageCountText, string filterCountText, string noRecordsText)
    {
        if(currentItems < 1 || totalItems < 1) return GlobalValues.Pager_No_Records_Text;

        var rowStart   = (itemsPerPage * currentPage) - (itemsPerPage -1);
        var rowEnd     = Math.Min(itemsPerPage * currentPage, currentItems);
        var isFiltered = currentItems != totalItems;

        var filteredString = isFiltered ? $"{filterCountText.Replace("{filteredrows}",currentItems.ToString()).Replace("{totalrows}", totalItems.ToString())}" 
                                        : String.Empty;

        var infoText = pageCountText.Replace("{firstpage}", currentPage.ToString())
                                    .Replace("{lastpage}", totalPages.ToString())
                                    .Replace("{startrow}", rowStart.ToString())
                                    .Replace("{endrow}", rowEnd.ToString())
                                    .Replace("{totalrows}", currentItems.ToString());

        return $"{infoText} {filteredString}".TrimEnd(); 
    }
    private async Task RequestPageChange(NavFocusType setFocusOn = NavFocusType.None)
    {
        var currentPage = _currentPage;

        switch (setFocusOn)
        {
            case NavFocusType.First:
                _currentPage = 1;
                _setFocusOn = NavFocusType.Next;
                break;

            case NavFocusType.Last:
                _currentPage = _lastPage;
                _setFocusOn = NavFocusType.Previous;
                break;

            case NavFocusType.Previous:
                _currentPage = _currentPage - 1 <= 1 ? 1 : _currentPage - 1;
                _setFocusOn = _currentPage == 1 ? NavFocusType.Next : setFocusOn;
                break;

            case NavFocusType.Next:
                _currentPage = _currentPage + 1 >= _lastPage ? _lastPage : _currentPage + 1;
                _setFocusOn = _currentPage == _lastPage ? NavFocusType.Previous : setFocusOn;
                break;
        }

        _pageChanged = currentPage != _currentPage;

        if (CurrentPageChanged.HasDelegate) await CurrentPageChanged.InvokeAsync(_currentPage);

        await Task.Yield();

        switch (_setFocusOn)
        {
            case NavFocusType.First: await FirstNavRef.FocusAsync(); break;
            case NavFocusType.Last: await LastNavRef.FocusAsync(); break;
            case NavFocusType.Previous: await PreviousNavRef.FocusAsync(); break;
            case NavFocusType.Next: await NextNavRef.FocusAsync(); break;
        }

        _setFocusOn = NavFocusType.None;
        
        if (_pagerSelectorType == PagerSelectorType.Link) NavigationManager.NavigateTo(UpdateUriQueryParams(_currentPage, _queryParamName));
    }

    private string? CheckSetDisableButton(NavFocusType navItem, int currentPage, int lastPage, int currentItemCount, int totalItemCount)
    {
        if (currentItemCount < 1 || totalItemCount < 1) return "true";

        return navItem switch
           {
               NavFocusType.First => currentPage < 2 ? "true" : null,
               NavFocusType.Previous => currentPage < 2 ? "true" : null,
               NavFocusType.Next => currentPage >= lastPage ? "true" : null,
               NavFocusType.Last => currentPage >= lastPage ? "true" : null,
               _ => null
           };
    }

    private bool CheckSetShowLinks(bool isStartGroup, int currentPage, int lastPage)

        => isStartGroup ? (currentPage < 2 ? false : true)
                        : (currentPage >= lastPage ? false : true);


    private string UpdateUriQueryParams(int pageNo, string queryParamName)

        => NavigationManager.GetUriWithQueryParameter(queryParamName, pageNo);


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(firstRender) _informationText = SetInformationText(_currentPage, _lastPage, ItemsPerPage,_currentItemCount,_totalItemCount,_pageCountText,_filterCountText,_noRecordsText);

        await MakeAnnouncement(_pagerAnnouncmentType, _informationText, _ariaLabel, _pageChanged);

    }

    private async Task MakeAnnouncement(PagerAnnouncmentType announcementType, string informationText, string triggerLabel, bool pageChanged = true)
    {
        if (pageChanged && announcementType == PagerAnnouncmentType.WithAnnouncement)
        {
            _pageChanged = false;
            _announceDebounceTS.Cancel();
            _announceDebounceTS = new CancellationTokenSource();

            try
            {
                await Task.Delay(400, _announceDebounceTS.Token);
                Announcement announcement = new(informationText, AnnouncementType.Info, triggerLabel, LiveRegionType.Assertive);
                await LiveRegionService.MakeAnnouncement(announcement, false);
            }
            catch (TaskCanceledException) { }
        }
    }
}

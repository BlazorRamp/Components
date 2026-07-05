using BlazorRamp.Core.Common.Constants;
using BlazorRamp.Core.Common.Models;
using BlazorRamp.Core.Services;
using BlazorRamp.Pager.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorRamp.Pager.Components;


/// <summary>
/// Renders an accessible pagination control for navigating between pages of records, exposing either
/// interactive <c>button</c> elements (for paging content that stays on the current page, such as a data table)
/// or <c>anchor</c> elements (for paging content that may navigate to a different page, such as a list of
/// search results). Page changes are announced through a live region, with the announcement mechanism tailored
/// to the current Blazor render mode to avoid a known JAWS focus issue under interactive server rendering.
/// </summary>
public partial class Pager : IAsyncDisposable
{
    /// <summary>
    /// Gets or sets whether the pager renders its navigation controls as <c>button</c> elements or as
    /// <c>anchor</c> elements. Use <see cref="PagerSelectorType.Button"/> when paging does not change the
    /// current page (e.g. a data table), or <see cref="PagerSelectorType.Link"/> when paging may navigate to a
    /// different page (e.g. a listing page). Defaults to <see cref="PagerSelectorType.Button"/>.
    /// </summary>
    [Parameter] public PagerSelectorType PagerSelectorType         { get; set; } = PagerSelectorType.Button;

    /// <summary>
    /// Gets or sets whether page changes are announced to assistive technology via the live region. Defaults to
    /// <see cref="PagerAnnouncementType.WithAnnouncement"/>; set to
    /// <see cref="PagerAnnouncementType.WithoutAnnouncement"/> to suppress announcements, for example when you
    /// have two pagers on the page usin the same current page variable.
    /// </summary>
    [Parameter] public PagerAnnouncementType PagerAnnouncementType { get; set; } = PagerAnnouncementType.WithAnnouncement;

    /// <summary>
    /// Gets or sets the horizontal alignment of the pager within its container. Defaults to
    /// <see cref="PageAlignment.Centred"/>.
    /// </summary>
    [Parameter] public PageAlignment PageAlignment                 { get; set; } = PageAlignment.Centred;

    /// <summary>
    /// Gets or sets the accessible label describing the purpose of the pager, exposed to assistive technology
    /// via the pager's <c>nav</c> landmark. Defaults to <c>"Results Pager"</c>.
    /// </summary>
    [Parameter] public string AriaLabel { get; set; } = GlobalValues.Pager_Aria_Label;

    /// <summary>
    /// Gets or sets the visible and accessible text for the "next page" control. Defaults to <c>"Next"</c>.
    /// </summary>
    [Parameter] public string NextText { get; set; } = GlobalValues.Pager_Selector_Next_Text;

    /// <summary>
    /// Gets or sets the visible and accessible text for the "previous page" control. Defaults to
    /// <c>"Previous"</c>.
    /// </summary>
    [Parameter] public string PreviousText { get; set; } = GlobalValues.Pager_Selector_Prev_Text;

    /// <summary>
    /// Gets or sets the visible and accessible text for the "first page" control. Only rendered when
    /// <see cref="ShowFirstLast"/> is <c>true</c>. Defaults to <c>"First"</c>.
    /// </summary>
    [Parameter] public string FirstText { get; set; } = GlobalValues.Pager_Selector_First_Text;

    /// <summary>
    /// Gets or sets the visible and accessible text for the "last page" control. Only rendered when
    /// <see cref="ShowFirstLast"/> is <c>true</c>. Defaults to <c>"Last"</c>.
    /// </summary>
    [Parameter] public string LastText { get; set; } = GlobalValues.Pager_Selector_Last_Text;

    /// <summary>
    /// Gets or sets whether the "first page" and "last page" controls are rendered in addition to
    /// "previous"/"next". Defaults to <c>true</c>.
    /// </summary>
    [Parameter] public bool ShowFirstLast     { get; set; } = true;

    /// <summary>
    /// Gets or sets the name of the query string parameter used to encode the page number when
    /// <see cref="PagerSelectorType"/> is <see cref="PagerSelectorType.Link"/>. Defaults to <c>"page"</c>.
    /// </summary>
    [Parameter] public string QueryParamName  { get; set; } = GlobalValues.Pager_Query_String_Param_Name;


    /// <summary>
    /// Gets or sets the template used to build the pager's information text. Supports the placeholders
    /// <c>{firstpage}</c>, <c>{lastpage}</c>, <c>{startrow}</c>, <c>{endrow}</c>, and <c>{totalrows}</c>, which
    /// are replaced with the current values at render time. When not supplied, a default English template is
    /// used.
    /// </summary>
    [Parameter] public string PageCountText   { get; set; } = GlobalValues.Pager_Count_Text;

    /// <summary>
    /// Gets or sets the template appended to the information text when <see cref="CurrentItemCount"/> differs
    /// from <see cref="TotalItemCount"/>, indicating that the current row count reflects an active filter.
    /// Supports the placeholders <c>{filteredrows}</c> and <c>{totalrows}</c>. When not supplied, a default
    /// English template is used.
    /// </summary>
    [Parameter] public string FilterCountText { get; set; } = GlobalValues.Pager_Filter_Count_Text;

    /// <summary>
    /// Gets or sets the text displayed and announced in place of the information text when there are no
    /// records to page through. When not supplied, a default English message is used.
    /// </summary>
    [Parameter] public string NoRecordsText { get; set; } = GlobalValues.Pager_No_Records_Text;


    /// <summary>
    /// Gets or sets the total number of records available, before any filtering is applied. Used together with
    /// <see cref="CurrentItemCount"/> to determine whether the information text should indicate that the
    /// results are filtered. Defaults to <c>0</c>.
    /// </summary>
    [Parameter] public int TotalItemCount     { get; set; } = 0;

    /// <summary>
    /// Gets or sets the number of records available after any filtering is applied. Used to calculate the
    /// total number of pages and the row range shown in the information text. Values greater than
    /// <see cref="TotalItemCount"/> are clamped to <see cref="TotalItemCount"/>. Defaults to <c>0</c>.
    /// </summary>
    [Parameter] public int CurrentItemCount   { get; set; } = 0;

    /// <summary>
    /// Gets or sets the number of records shown per page. Values less than <c>1</c> fall back to the default
    /// of <c>10</c>.
    /// </summary>
    [Parameter] public int ItemsPerPage       { get; set; } = GlobalValues.Pager_Rows_Per_Page;

    /// <summary>
    /// Gets or sets the currently displayed page number, using a 1-based index. Values outside the valid range
    /// are clamped between <c>1</c> and the last available page.
    /// </summary>
    [Parameter] public int CurrentPage        { get; set; } = 0;


    /// <summary>
    /// Gets or sets the callback invoked when the user requests a page change, receiving the newly requested
    /// 1-based page number. The pager does not update <see cref="CurrentPage"/> itself; consumers are expected
    /// to handle this callback and pass the new value back in via the <see cref="CurrentPage"/> parameter.
    /// </summary>
    [Parameter] public EventCallback<int> CurrentPageChanged { get; set; }

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private ILiveRegionService LiveRegionService { get; set; } = default!;
        
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
    private IJSObjectReference? _jSModule = null;
    
    private DotNetObjectReference<Pager>? _dotNetObjectRef;

    private ElementReference NavElementRef   { get; set; }

    private PagerSelectorType     _pagerSelectorType     = PagerSelectorType.Button;
    private PagerAnnouncementType _pagerAnnouncementType = PagerAnnouncementType.WithAnnouncement;

    private string _queryParamName = GlobalValues.Pager_Query_String_Param_Name;
    private string _nextText       = GlobalValues.Pager_Selector_Next_Text;
    private string _prevText       = GlobalValues.Pager_Selector_Prev_Text;
    private string _firstText      = GlobalValues.Pager_Selector_First_Text;
    private string _lastText       = GlobalValues.Pager_Selector_Last_Text;

    private string _ariaLabel   = GlobalValues.Pager_Aria_Label;
    private string _ariaLabelID = Guid.NewGuid().ToString();
    private string _infoTextID  = Guid.NewGuid().ToString();

    private string _pageCountText   = GlobalValues.Pager_Count_Text;
    private string _filterCountText = GlobalValues.Pager_Filter_Count_Text;
    private string _informationText = GlobalValues.Pager_No_Records_Text;
    private string _noRecordsText   = GlobalValues.Pager_No_Records_Text;

    private int _lastPage        = 0;
    private int _currentPage     = 0;
    private int _currentRowCount = 0;
    private int _totalRowCount   = 0;
    private int _rowsPerPage     = GlobalValues.Pager_Rows_Per_Page;

    private bool _showFirstLast = true;
    private bool _isServer      = true;
    private bool _pageChanged   = false;

    private int  _previousCurrent = 0;
    private string _ariaLabelledbyIDs = String.Empty;

    private CancellationTokenSource _announceDebounceTS = new();



    /// <summary>
    /// Recomputes derived paging state (row counts, last page, clamped current page, and the information text)
    /// whenever the component's parameters change.
    /// </summary>
    protected override void OnParametersSet()
    {
        _pageChanged     = _previousCurrent != CurrentPage;
        _previousCurrent = CurrentPage;

        _rowsPerPage     = ItemsPerPage < 1 ? GlobalValues.Pager_Rows_Per_Page : ItemsPerPage;
        _totalRowCount   = TotalItemCount;
        _currentRowCount = CurrentItemCount > TotalItemCount ? TotalItemCount : CurrentItemCount;

        _pageCountText   = String.IsNullOrWhiteSpace(PageCountText)   ? GlobalValues.Pager_Count_Text        : PageCountText.Trim();
        _filterCountText = String.IsNullOrWhiteSpace(FilterCountText) ? GlobalValues.Pager_Filter_Count_Text : FilterCountText.Trim();
        _noRecordsText   = String.IsNullOrWhiteSpace(NoRecordsText)   ? GlobalValues.Pager_No_Records_Text   : NoRecordsText.Trim();

        _nextText        = String.IsNullOrWhiteSpace(NextText)     ? GlobalValues.Pager_Selector_Next_Text  : NextText.Trim();
        _prevText        = String.IsNullOrWhiteSpace(PreviousText) ? GlobalValues.Pager_Selector_Prev_Text  : PreviousText.Trim();
        _lastText        = String.IsNullOrWhiteSpace(LastText)     ? GlobalValues.Pager_Selector_Last_Text  : LastText.Trim();
        _firstText       = String.IsNullOrWhiteSpace(FirstText)    ? GlobalValues.Pager_Selector_First_Text : FirstText.Trim();

        _pagerAnnouncementType = PagerAnnouncementType;

        _lastPage = (_currentRowCount < 1) ? 1 : (int)Math.Ceiling((double)_currentRowCount / _rowsPerPage);

        _currentPage = Math.Clamp(CurrentPage, 1, _lastPage);

        _informationText = SetInformationText(_currentPage, _lastPage, _rowsPerPage, _currentRowCount, _totalRowCount, _pageCountText, _filterCountText, _noRecordsText);
    }

    /// <summary>
    /// Captures the component's one-time render-mode-dependent state: which selector type and first/last
    /// visibility to use, the trimmed aria label and query parameter name, whether the component is running
    /// under a non-browser (server) render mode, and, based on that, which element IDs make up the <c>nav</c>
    /// element's <c>aria-labelledby</c>.
    /// </summary>
    protected override void OnInitialized()
    {
        _pagerSelectorType  = PagerSelectorType;
        _ariaLabel          = String.IsNullOrWhiteSpace(AriaLabel) ? GlobalValues.Pager_Aria_Label : AriaLabel.Trim();
        _showFirstLast      = ShowFirstLast;
        _queryParamName     = String.IsNullOrWhiteSpace(QueryParamName) ? GlobalValues.Pager_Query_String_Param_Name : QueryParamName.Trim();
        _isServer           = OperatingSystem.IsBrowser() ? false : true;

        _ariaLabelledbyIDs  = _isServer ? _ariaLabelID : $"{_ariaLabelID} {_infoTextID}";
    }
    /// <summary>
    /// Refreshes the information text after every render, and, on the first render under a non-browser (server)
    /// render mode, imports the pager's JavaScript module and registers the focus-in handler used to announce
    /// page state when focus enters the <c>nav</c> element. Also raises the debounced announcement following a
    /// user-initiated page change.
    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        _informationText = SetInformationText(_currentPage, _lastPage, _rowsPerPage, _currentRowCount, _totalRowCount, _pageCountText, _filterCountText, _noRecordsText);

        if (true == firstRender && _isServer)
        {
            _jSModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_File_Path);

            _dotNetObjectRef = DotNetObjectReference.Create(this);

            if (_jSModule is not null) await _jSModule.InvokeVoidAsync(GlobalValues.JS_Register_Focus_In_Callback, NavElementRef, _dotNetObjectRef, nameof(NavigationEntered));

            return;
        }

       if (_pageChanged) await MakeAnnouncement(_pagerAnnouncementType, _informationText, _ariaLabel);
       
        _pageChanged = false;

     }

    private string UpdateUriQueryParams(int pageNo, int currentPage, int lastPage, string queryParamName)
    {
        pageNo =  (pageNo < 1 || pageNo > lastPage) ? currentPage : pageNo;

        return NavigationManager.GetUriWithQueryParameter(queryParamName, pageNo);
    }
    private static string SetInformationText(int currentPage, int totalPages, int rowsPerPage, int currentRows, int totalRows,
                                         string pageCountText, string filterCountText, string noRecordsText)
    {
        if (currentRows < 1 || totalRows < 1) return noRecordsText;

        var rowStart = (rowsPerPage * currentPage) - (rowsPerPage - 1);
        var rowEnd = Math.Min(rowsPerPage * currentPage, currentRows);
        var isFiltered = currentRows != totalRows;

        var filteredString = isFiltered ? $"{filterCountText.Replace("{filteredrows}", currentRows.ToString()).Replace("{totalrows}", totalRows.ToString())}"
                                        : String.Empty;

        var infoText = pageCountText.Replace("{currentpage}", currentPage.ToString())
                                    .Replace("{lastpage}", totalPages.ToString())
                                    .Replace("{startrow}", rowStart.ToString())
                                    .Replace("{endrow}", rowEnd.ToString())
                                    .Replace("{totalrows}", currentRows.ToString());

        return $"{infoText} {filteredString}".TrimEnd();
    }

    private async Task RequestPageChange(NavSelectorType currentSelector, int currentPage, int pageRequested, int lastPage)
    {
        var shouldReturn = (currentSelector, pageRequested) switch
        {
            (NavSelectorType.First, _) when currentPage <= 1 => true,
            (NavSelectorType.Previous, _) when pageRequested < 1 => true,
            (NavSelectorType.Last, _) when currentPage >= lastPage => true,
            (NavSelectorType.Next, _) when pageRequested > lastPage => true,
            _ => false
        };

        if (shouldReturn)
        {
            await MakeAnnouncement(_pagerAnnouncementType, _informationText, _ariaLabel);
            return;
        }

        if (CurrentPageChanged.HasDelegate) await CurrentPageChanged.InvokeAsync(pageRequested);
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
    private static string GetPagerClasses(PageAlignment alignment)

        => alignment switch
        {
            PageAlignment.Start => $"{GlobalValues.Pager_Class} {GlobalValues.Pager_Align_Start_Modifier}",
            PageAlignment.End => $"{GlobalValues.Pager_Class} {GlobalValues.Pager_Align_End_Modifier}",
            _ => $"{GlobalValues.Pager_Class}",
        };


    private async Task MakeAnnouncement(PagerAnnouncementType announcementType, string informationText, string triggerLabel)
    {
        if (announcementType == PagerAnnouncementType.WithAnnouncement)
        {
            _announceDebounceTS.Cancel();
            _announceDebounceTS = new CancellationTokenSource();

            try
            {
                await Task.Delay(400, _announceDebounceTS.Token);
                Announcement announcement = new(informationText, AnnouncementType.Info, triggerLabel, LiveRegionType.Polite);
                await LiveRegionService.MakeAnnouncement(announcement, false);
            }
            catch (TaskCanceledException) { }
        }
    }

    /// <summary>
    /// Invoked via JavaScript interop when keyboard or screen-reader focus enters the pager's <c>nav</c>
    /// element from outside it, so its current state can be announced. This is used only under interactive
    /// server rendering, where the information text is deliberately excluded from the <c>nav</c> element's
    /// <c>aria-labelledby</c> to avoid a known issue where JAWS loses focus when a referenced accessible name
    /// changes dynamically.
    /// </summary>

    [JSInvokable]
    public async Task NavigationEntered()
        
        => await MakeAnnouncement(PagerAnnouncementType.WithAnnouncement, _informationText, _ariaLabelID);


    /// <summary>
    /// Releases the JavaScript module reference and unregisters the focus-in handler registered during
    /// <c>OnAfterRenderAsync</c>.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        _dotNetObjectRef?.Dispose();
        _announceDebounceTS.Cancel();

        if (_jSModule is not null)
        {
            try
            {
                await _jSModule.InvokeVoidAsync(GlobalValues.JS_Unregister_Focus_In_Callback);
                await _jSModule.DisposeAsync();

            }
            catch { }
        }

    }
}


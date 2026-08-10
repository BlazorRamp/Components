using BlazorRamp.Core.Common.Constants;
using BlazorRamp.Core.Common.Models;
using BlazorRamp.Core.Services;
using BlazorRamp.DataTable.Common.Constants;
using BlazorRamp.DataTable.Common.Models;
using BlazorRamp.DataTable.Common.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Diagnostics;

namespace BlazorRamp.DataTable.Components;


/// <summary>
/// An accessible, sortable, filterable data table for an in-memory list of <typeparamref name="TData"/> items.
/// Supports optional paging, virtualization, row selection, and screen reader announcements for sort,
/// filter, and empty-result states.
/// </summary>
/// <typeparam name="TData">The row item type displayed by the table.</typeparam>
public partial class DataTable<TData> : ComponentBase,  IAsyncDisposable
{
    /// <summary>
    /// The <see cref="DataColumn{TData}"/>/<see cref="TemplateColumn{TData}"/> definitions for this table.
    /// </summary>
    [Parameter] public RenderFragment TableColumns { get; set; } = default!;

    /// <summary>
    /// The table's title, shown above the content and used as the accessible name for the table.
    /// </summary>
    [Parameter] public string           Title                 { get; set; } = GlobalValues.DataTable_Title_Text;

    /// <summary>
    /// Horizontal alignment of the title.
    /// </summary>
    [Parameter] public TitleAlignment   TitleAlignment        { get; set; } = TitleAlignment.Start;

    /// <summary>
    /// Whether the title is visually hidden while remaining available to assistive technology.
    /// </summary>
    [Parameter] public bool             TitleHidden           { get; set; } = false;

    /// <summary>
    /// Optional markup for a custom filter UI, rendered above the table.
    /// </summary>
    [Parameter] public RenderFragment?  Filter                { get; set; }

    /// <summary>
    /// Horizontal alignment of the filter area.
    /// </summary>
    [Parameter] public FilterAlignment  FilterAlignment       { get; set; } = FilterAlignment.End;

    /// <summary>
    /// Optional markup for a pager rendered above the table.
    /// </summary>
    [Parameter] public RenderFragment?  TopPager              { get; set; }

    /// <summary>
    /// Optional markup for a pager rendered below the table.
    /// </summary>
    [Parameter] public RenderFragment?  BottomPager           { get; set; }

    /// <summary>
    /// The estimated pixel height of each row, used by the virtualized rendering path. Set below 1 to disable virtualization.
    /// </summary>
    [Parameter] public int              VirtualizeItemSizePX  { get; set; } = GlobalValues.DataTable_VirtualizePX;

     /// <summary>
    /// The number of extra rows rendered above and below the visible viewport when virtualization is active, to reduce blank space during fast scrolling.
    /// </summary>
    [Parameter] public int OverscanCount { get; set; } = GlobalValues.DataTable_OverscanCount;

    /// <summary>
    /// The table's height, in any valid CSS unit. Applied as a fixed <c>height</c> when using virtualized
    /// rendering (required, since the Virtualize component needs a definite height to measure against),
    /// or as a <c>max-height</c> when paging is enabled, allowing the table to shrink for fewer rows.
    /// Defaults if null, empty, or whitespace.
    /// </summary>
    [Parameter] public string? TableHeight { get; set; } = GlobalValues.DataTable_Max_Table_Height;

    /// <summary>
    /// When supplied, enables paging and binds the table to the given paging state.
    /// </summary>
    [Parameter] public PagerBinding?    PagerBinding          { get; set; } = null;

    /// <summary>
    /// Header text for the row-selection column. Defaults to Select, if null, empty, or whitespace
    /// .</summary>
    [Parameter] public string? RowSelectHeading { get; set; } = GlobalValues.DataTable_Row_Selector_Header_Text;

    /// <summary>
    /// Optional function producing a per-row accessible label for that row's selection checkbox
    /// to make a better/unique accessible name.
    /// </summary>
    [Parameter] public Func<TData, string>? RowIdentifierFunc { get; set; }


    /// <summary>
    /// Text shown when there are no rows to display. Defaults to "No entries found." 
    /// if null, empty, or whitespace.
    /// </summary>
    [Parameter] public string?          NoRecordText         { get; set; } = GlobalValues.DataTable_No_Records_Text;

    /// <summary>
    /// Template for the row-count message shown when a filter is active. Supports {filteredrows} and {totalrows} tokens.
    /// </summary>
    [Parameter] public string?          FilterCountText      { get; set; } = GlobalValues.DataTable_Filter_Count_Text;

    /// <summary>
    /// Template for the row-count message shown when no filter is active. Supports the {totalrows} token.
    /// </summary>
    [Parameter] public string?          RecordCountText      { get; set; } = GlobalValues.DataTable_Record_Count_Text;

    /// <summary>
    /// The full list of rows to display. Required. Treated as immutable: to add, remove, or
    /// otherwise change rows, assign a new List instance rather than mutating this one in place —
    /// only a new reference is detected as a data change. If you mutate DataSource in place instead,
    /// call Refresh() (via @ref) afterwards.
    /// </summary>
    [Parameter, EditorRequired] public List<TData> DataSource { get; set; } = [];

    /// <summary>
    /// Whether rows can be selected, and if so, whether selection is single or multiple.
    /// </summary>
    [Parameter] public RowSelectionMode            RowSelectionMode { get; set; } = RowSelectionMode.None;

    /// <summary>
    /// The currently selected rows.
    /// </summary>
    [Parameter] public List<TData>                 SelectedRows     { get; set; } = [];

    /// <summary>
    /// Optional function returning inline CSS for a row's <c>&lt;tr&gt;</c> element.
    /// </summary>
    [Parameter] public Func<TData, string?>?       RowStyleFunc     { get; set; } = null;

    /// <summary>
    /// Predicate used to filter <see cref="DataSource"/>. Supplying a new delegate instance triggers
    /// re-filtering; reusing the same instance across renders will not.
    /// </summary>
    [Parameter] public Func<TData, bool>?          FilterRule       { get; set; }

    /// <summary>
    /// Screen reader announcement text used when a filter has been applied.
    /// </summary>
    [Parameter] public string FilteredStatusText    { get; set; } = GlobalValues.DataTable_Filtered_Status_Text;

    /// <summary>
    /// Screen reader announcement text used when a column is sorted ascending.
    /// </summary>
    [Parameter] public string SortUpStatusText      { get; set; } = GlobalValues.DataTable_Sort_Up_Status_Text;

    /// <summary>
    /// Screen reader announcement text used when a column is sorted descending.
    /// </summary>
    [Parameter] public string SortDownStatusText    { get; set; } = GlobalValues.DataTable_Sort_Down_Status_Text;

    /// <summary>
    /// Screen reader announcement text used when a column's sort is cleared.
    /// </summary>
    [Parameter] public string SortRemovedStatusText { get; set; } = GlobalValues.DataTable_Sort_Removed_Status_Text;

    /// <summary>
    /// Hidden hint text describing sort buttons to assistive technology, referenced via <c>aria-describedby</c>.
    /// </summary>
    [Parameter] public string PressToSortText       { get; set; } = GlobalValues.DataTable_Press_To_Sort_Text;

    /// <summary>
    /// Changes the tabindex of -1 to 0 on the table element. You would only
    /// need to do this if the table was scrollable without any interactive
    /// focusable elements such as sortable headers, selection checkboxes or
    /// custom row actions. A focusable item on the table allows keyboard only
    /// users the ability move any scrollbars. Defaults to <c>false</c>
    /// </summary>
    [Parameter] public bool AddTableTabIndex { get; set; } = false;

    /// <summary>
    /// Gets or sets additional attributes that are applied to the underlying table element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object>? AdditionalAttributes { get; set; }
    /// <summary>
    /// Raised whenever the selected rows change.
    /// </summary>
    [Parameter] public EventCallback<List<TData>> SelectedRowsChanged { get; set; }

    [Inject] private ILiveRegionService? LiveRegionService { get; set; }
    [Inject] private IJSRuntime          JSRuntime         { get; set; } = default!;

    private ElementReference ContentElementRef { get; set; }
    private ElementReference TableElementRef   { get; set; }

    private IJSObjectReference? _jSModule = null;


    private readonly List<ColumnBase<TData>>    _tableColumns = [];
    private readonly Dictionary<string, string> _columnAlignments = [];

    private Func<TData, bool>? _previousFilterRule = null;

    private List<TData> _filteredRows               = []; // filtered-but-unsorted cache; written ONLY where DataSource/FilterRule actually change
    private List<TData> _dataPage                   = [];
    private List<TData> _dataSource                 = [];
    private List<TData> _previousDataRef            = [];
    private List<TData> _selectedRows               = [];

    private string _noDataText            = GlobalValues.DataTable_No_Records_Text;
    private string _rowSelectHeading      = GlobalValues.DataTable_Row_Selector_Header_Text;
    private string _selectRowLabelID      = Guid.NewGuid().ToString();
    private string _tableTitleID          = Guid.NewGuid().ToString();
    private string _pressSortTextID       = Guid.NewGuid().ToString();
    private string _tableTitle            = GlobalValues.DataTable_Title_Text;
    private int    _lastSortedColumnIndex = -1;
    private bool   _usePaging             = false;
    private string _filterCountText       = GlobalValues.DataTable_Filter_Count_Text;
    private string _recordCountText       = GlobalValues.DataTable_Record_Count_Text;
    private string _displayCountMessage   = String.Empty;
    private bool   _hasAnnouncedNoData    = false;

    private string _sortUpCompleted      =  GlobalValues.DataTable_Sort_Up_Status_Text;
    private string _sortDownCompleted    = GlobalValues.DataTable_Sort_Down_Status_Text;
    private string _sortRemovedCompleted = GlobalValues.DataTable_Sort_Removed_Status_Text;
    private string _filteringCompleted   = GlobalValues.DataTable_Filtered_Status_Text;
    private string _pressToSortText      = GlobalValues.DataTable_Press_To_Sort_Text;

    private string _tableHeight      = GlobalValues.DataTable_Max_Table_Height;
    private string _tableHeightStyle = String.Empty;

    private string _operationCompletedAnnouncement = String.Empty;

    private bool _showTableSpinner = false;


    /// <summary>
    /// Resolves default text parameters, detects data source/filter changes, and refreshes paging and row-count state on every parameter set.
    /// </summary>
    protected override async Task OnParametersSetAsync()
    {
        _tableTitle           = String.IsNullOrWhiteSpace(Title)                 ? GlobalValues.DataTable_Title_Text               : Title.Trim();
        _noDataText           = String.IsNullOrWhiteSpace(NoRecordText)          ? GlobalValues.DataTable_No_Records_Text          : NoRecordText.Trim();
        _filterCountText      = String.IsNullOrWhiteSpace(FilterCountText)       ? GlobalValues.DataTable_Filter_Count_Text        : FilterCountText.Trim();
        _recordCountText      = String.IsNullOrWhiteSpace(RecordCountText)       ? GlobalValues.DataTable_Record_Count_Text        : RecordCountText.Trim();
        _sortUpCompleted      = String.IsNullOrWhiteSpace(SortUpStatusText)      ? GlobalValues.DataTable_Sort_Up_Status_Text      : SortUpStatusText.Trim();
        _sortDownCompleted    = String.IsNullOrWhiteSpace(SortDownStatusText)    ? GlobalValues.DataTable_Sort_Down_Status_Text    : SortDownStatusText.Trim();
        _sortRemovedCompleted = String.IsNullOrWhiteSpace(SortRemovedStatusText) ? GlobalValues.DataTable_Sort_Removed_Status_Text : SortRemovedStatusText.Trim();
        _filteringCompleted   = String.IsNullOrWhiteSpace(FilteredStatusText)    ? GlobalValues.DataTable_Filtered_Status_Text     : FilteredStatusText.Trim();
        _pressToSortText      = String.IsNullOrWhiteSpace(PressToSortText)       ? GlobalValues.DataTable_Press_To_Sort_Text       : PressToSortText.Trim();
        _tableHeight          = String.IsNullOrWhiteSpace(TableHeight)           ? GlobalValues.DataTable_Max_Table_Height         : TableHeight.Trim();

        if (false == _tableHeight.EndsWith(';')) _tableHeight += ';';

        _tableHeightStyle = _usePaging ? $"max-height: {_tableHeight}" : $"height: {_tableHeight}";

        var dataSourceChanged = ReferenceEquals(_previousDataRef, DataSource) == false;
        var filterRuleChanged = ReferenceEquals(_previousFilterRule, FilterRule) == false;

        if (dataSourceChanged)
        {
            _selectedRows.Clear();
            _previousDataRef = DataSource;
        }

        if (dataSourceChanged || filterRuleChanged)
        {
            if (filterRuleChanged) await ToggleBusyIndicator(true, "");

            _previousFilterRule = FilterRule;
            _filteredRows       = GetFilteredRows();
            _dataSource         = ApplyLastSort(_filteredRows);

            if (_usePaging) CheckSetPagingInfo(_dataSource, dataSourceChanged || filterRuleChanged);

            if (filterRuleChanged)
            {
                var filterAnnouncement = _dataSource.Count == 0 ? String.Empty : GetDisplayCountMessage(_dataSource.Count, DataSource.Count);
                if (false == _usePaging) await MakeAnnouncement(filterAnnouncement, _tableTitle);
                await ToggleBusyIndicator(false, _filteringCompleted);
            }
        }

        _selectedRows = SelectedRows ?? [];

        _displayCountMessage = _dataSource.Count == 0 ? String.Empty : GetDisplayCountMessage(_dataSource.Count, DataSource.Count); // ← now runs every pass, not just when the gate opens

        _dataPage = _usePaging ? GetDataPage(PagerBinding!.CurrentPage, PagerBinding.ItemsPerPage, _dataSource) : _dataSource;
    }

    /// <summary>
    /// Builds the "showing N of M rows" style message, choosing the filtered or unfiltered template as appropriate.
    /// </summary>
    private string GetDisplayCountMessage(int currentRowCount, int originalRowCount)

        => currentRowCount == originalRowCount ? _recordCountText.Replace("{totalrows}", originalRowCount.ToString())
                                               : _filterCountText.Replace("{filteredrows}", currentRowCount.ToString()).Replace("{totalrows}", originalRowCount.ToString());

    /// <summary>
    /// Resolves default text parameters and builds the initial display rows from <see cref="DataSource"/>.
    /// </summary>
    protected override void OnInitialized()
    {
        _rowSelectHeading       = String.IsNullOrWhiteSpace(RowSelectHeading) ? GlobalValues.DataTable_Row_Selector_Header_Text : RowSelectHeading.Trim();
        _usePaging              = PagerBinding != null;

    }

    /// <summary>
    /// Applies the default sort on first render, and announces the no-data state once per empty result while avoiding duplicate announcements from extra render passes.
    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(firstRender == true) _jSModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_Module_File_Path);

        if (_dataSource.Count == 0 && false == _usePaging)
        {
            /*
                * Needed as the spinner causes another OnAfterRenderAsync - without the OnParametersSetAsync being triggered
              */
            if (false == _hasAnnouncedNoData)
            {
                _hasAnnouncedNoData = true;
                await MakeAnnouncement(_noDataText, _tableTitle);
                return;
            }

            _hasAnnouncedNoData = false;
        }

    }

    /// <summary>
    /// Sends a polite live-region announcement via <see cref="ILiveRegionService"/>, if one is available.
    /// </summary>
    private async Task MakeAnnouncement(string message, string trigger)
    {
        var announcement = new Announcement(message, AnnouncementType.Info, trigger, LiveRegionType.Polite);
        if (LiveRegionService is not null) await LiveRegionService.MakeAnnouncement(announcement, false);
    }


    /// <summary>
    /// Registers a column with this table. Called by <see cref="ColumnBase{TData}"/> during initialization.
    /// </summary>
    internal void AddDataTableColumn(ColumnBase<TData> dataColumn)
    {
        _columnAlignments[dataColumn.FieldName] = DataTableHelper.GetDataPosition(dataColumn.ColumnAlignment);

        if (false == _tableColumns.Contains(dataColumn)) _tableColumns.Add(dataColumn);

        StateHasChanged();
    }

    /// <summary>
    /// Unregisters a column from this table. Called by <see cref="ColumnBase{TData}"/> when disposed.
    /// </summary>
    internal void RemoveDataTableColumn(ColumnBase<TData> dataColumn)
    { 
        if (true == _tableColumns.Contains(dataColumn)) _tableColumns.Remove(dataColumn);
    }

    private async Task HandleColumnHeaderClick(DataColumn<TData> column)
    {
        if (false == column.IsSortable) return;

        await ToggleBusyIndicator(true);

        var previousDirection = column.ColumnSortDirection;
        foreach (var col in _tableColumns) col.ColumnSortDirection = ColumnSortDirection.NotSorted;

        column.ColumnSortDirection = previousDirection switch
        {
            ColumnSortDirection.NotSorted => ColumnSortDirection.Ascending,
            ColumnSortDirection.Ascending => ColumnSortDirection.Descending,
            _ => ColumnSortDirection.NotSorted
        };

        _lastSortedColumnIndex = _tableColumns.IndexOf(column);
        _dataSource = ApplyLastSort(_filteredRows);

        if (_usePaging) PagerBinding!.CurrentPage = 1; // comment out if you do not like sorting to restart the paging

        _dataPage = _usePaging ? GetDataPage(PagerBinding!.CurrentPage, PagerBinding.ItemsPerPage, _dataSource) : _dataSource;

        var sortingText = column.ColumnSortDirection switch
        {
            ColumnSortDirection.Ascending => _sortUpCompleted,
            ColumnSortDirection.Descending => _sortDownCompleted,
            _ => _sortRemovedCompleted
        };

        await ToggleBusyIndicator(false, sortingText);
    }

    /// <summary>
    /// Shows or hides the busy indicator. Uses <see cref="Task.Delay(int)"/> rather than <see cref="Task.Yield"/>
    /// to guarantee the render batch has flushed to the DOM before returning — see inline comment for details.
    /// </summary>
    private async Task ToggleBusyIndicator(bool showSpinner, string endMessage = "")
    {
        _operationCompletedAnnouncement = endMessage;
        
        _showTableSpinner = showSpinner;
        // Task.Yield() only guarantees the continuation runs asynchronously — it does not
        // guarantee Blazor has flushed the pending render batch to the DOM. Both the visual
        // spinner AND the live-region announcement depend on that DOM flush actually happening;
        // without it, screen reader announcements can be silently dropped, not just delayed.
        // Task.Delay(10) uses the timer queue instead of SynchronizationContext.Post, which
        // reliably allows the render batch to flush before the (synchronous) filter/sort
        // work resumes. Confirmed via testing: Task.Yield() was unreliable for both paint
        // and SR announcements, especially when filtering+sorting together.
        await Task.Delay(10);
    }
        

    /// <summary>
    /// Returns the slice of <paramref name="dataSource"/> for the given page.
    /// </summary>
    private static List<TData> GetDataPage(int currentPage, int itemsPerPage, List<TData> dataSource)
    {
        if (dataSource.Count == 0) return [];

        int start = (currentPage - 1) * itemsPerPage;

        return [.. dataSource.Skip(start).Take(itemsPerPage)];
    }


    private void CheckSetPagingInfo(List<TData> dataSource, bool resetToFirstPage)
    {
        if (PagerBinding is null) return;

       // var totalPages = dataSource.Count == 0 ? 0 : (int)Math.Ceiling(dataSource.Count / (double)PagerBinding.ItemsPerPage);

        var totalPages = dataSource.Count == 0 || PagerBinding.ItemsPerPage <= 0 ? 0 : (int)Math.Ceiling(dataSource.Count / (double)PagerBinding.ItemsPerPage);

        switch (PagerBinding.CurrentPage)
        {
            case var _ when resetToFirstPage:
                PagerBinding.CurrentPage = dataSource.Count > 0 ? 1 : 0;
                break;

            case var page when page > totalPages:
                PagerBinding.CurrentPage = totalPages; // clamp to new last page, don't jump to page 1
                break;

            case var page when page < 1 && dataSource.Count > 0:
                PagerBinding.CurrentPage = 1;
                break;
        }

        PagerBinding.CurrentItemCount = dataSource.Count;  // filtered count — always current
        PagerBinding.TotalItemCount = DataSource.Count;  // true row count — note: DataSource, not dataSource
    }


    /// <summary>
    /// Toggles selection for a row clicked directly (not via its checkbox). Ignores clicks where
    /// <see cref="MouseEventArgs.Buttons"/> is non-zero, which per the UI Events spec indicates a
    /// synthetic/keyboard/assistive-technology-triggered click rather than a genuine mouse click —
    /// this prevents screen reader table-navigation activation (Enter/Space on a cell) from also
    /// toggling the row's checkbox via bubbling.
    /// </summary>
    private async Task HandleOnSelectedRow(MouseEventArgs args, TData rowItem)
    {
        if (RowSelectionMode == RowSelectionMode.None || args.Buttons != 0) return;

        if(_selectedRows.Contains(rowItem))
        {
            _selectedRows.Remove(rowItem);
        }
        else
        {
            _selectedRows.Add(rowItem);
        }

        if (RowSelectionMode == RowSelectionMode.Single) _selectedRows.RemoveAll(row => !row!.Equals(rowItem));

        if (SelectedRowsChanged.HasDelegate) await SelectedRowsChanged.InvokeAsync(_selectedRows);
    }

    /// <summary>
    /// Sets selection state for a row via its checkbox's change event.
    /// </summary>
    private async Task ToggleSelection(bool isSelected, TData rowItem)
    {
        if (true == isSelected && false == _selectedRows.Contains(rowItem)) _selectedRows.Add(rowItem);

        if(false == isSelected) _selectedRows.Remove(rowItem);

        if (RowSelectionMode == RowSelectionMode.Single) _selectedRows.RemoveAll(row => !row!.Equals(rowItem));

        if (SelectedRowsChanged.HasDelegate)
        {
            await SelectedRowsChanged.InvokeAsync(_selectedRows);
        }
        else
        {
            await InvokeAsync(StateHasChanged);
        }

    }


    /// <summary>
    /// Filters <see cref="DataSource"/> by <see cref="FilterRule"/>, or returns a plain copy if no filter is set.
    /// </summary>
    private List<TData> GetFilteredRows()

        => FilterRule is null ? [.. DataSource] : [.. DataSource.Where(FilterRule)];

    /// <summary>
    /// Returns <paramref name="filtered"/> ordered by the last-sorted column, or unchanged
    /// (i.e. in DataSource's own order) if no column is currently sorted.
    /// </summary>
    private List<TData> ApplyLastSort(List<TData> filtered)
    {
        if (_lastSortedColumnIndex == -1) return filtered;

        var column = _tableColumns[_lastSortedColumnIndex];

        return column.ColumnSortDirection == ColumnSortDirection.NotSorted ? filtered : SortDataSource(filtered, column.ColumnSortDirection, column);

    }

    /// <summary>
    /// Returns <paramref name="dataSource"/> ordered by <paramref name="dataColumn"/>'s bound property.
    /// Uses a stable comparison (ties broken by original index, unaffected by sort direction) so equal
    /// values retain their relative order regardless of ascending/descending.
    /// </summary>
    private static List<TData> SortDataSource(List<TData> dataSource, ColumnSortDirection sortDirection, ColumnBase<TData> dataColumn)
    {

        if (dataColumn.PropertyInfo is null || dataSource.Count <= 1) return dataSource;

        var sortAscending = sortDirection == ColumnSortDirection.Ascending;
        var getter        = dataColumn.ValueGetter!;

        if (dataColumn.PropertyInfo.PropertyType == typeof(string))
        {
            var sortArray = new (TData Item, string Key, int OriginalIndex)[dataSource.Count];
            for (int i = 0; i < dataSource.Count; i++) sortArray[i] = (dataSource[i], (string)getter(dataSource[i]), i);

            Array.Sort(sortArray, (x, y) =>
            {
                int compare = string.Compare(x.Key, y.Key, StringComparison.Ordinal);
                return compare == 0 ? x.OriginalIndex.CompareTo(y.OriginalIndex) : (sortAscending ? compare : -compare);
            });

            return [.. sortArray.Select(x => x.Item)];
        }
        else
        {
            var sortArray = new (TData Item, IComparable Key, int OriginalIndex)[dataSource.Count];
            for (int i = 0; i < dataSource.Count; i++) sortArray[i] = (dataSource[i], (IComparable)getter(dataSource[i]), i);

            Array.Sort(sortArray, (x, y) =>
            {
                int compare = x.Key is null ? (y.Key is null ? 0 : -1) : y.Key is null ? 1 : x.Key.CompareTo(y.Key);
                return compare == 0 ? x.OriginalIndex.CompareTo(y.OriginalIndex) : (sortAscending ? compare : -compare);
            });

            return [.. sortArray.Select(x => x.Item)];
        }
    }

    /// <summary>
    /// Forces the table to re-filter, re-sort, and re-page from <see cref="DataSource"/>. Call this
    /// after mutating the existing DataSource list in place (e.g. Add/Remove/Clear) rather than
    /// assigning a new List instance, since in-place mutation is not detected automatically.
    /// </summary>
    public async Task Refresh()
    {
        _filteredRows = GetFilteredRows();
        _dataSource = ApplyLastSort(_filteredRows);

        if (_usePaging) CheckSetPagingInfo(_dataSource, false);

        _dataPage = _usePaging ? GetDataPage(PagerBinding!.CurrentPage, PagerBinding.ItemsPerPage, _dataSource) : _dataSource;

        _displayCountMessage = _dataSource.Count == 0 ? String.Empty : GetDisplayCountMessage(_dataSource.Count, DataSource.Count);

        await InvokeAsync(StateHasChanged);
    }


    /// <summary>
    /// Programmatically using JavaScript sets focus on the table element.
    /// and applies a temporary modifier class to ensure that a focus indicator 
    /// is visible around the table whilst it has this focus.
    /// </summary>
    /// <returns></returns>
    public async Task SetTableFocus()
    {

        if(_jSModule != null && TableElementRef.Context != null && ContentElementRef.Context != null)
        {
            await _jSModule.InvokeVoidAsync(GlobalValues.JS_Set_Temp_Content_Focus_Modifier, TableElementRef, ContentElementRef, GlobalValues.DataTable_Content_Focused_Modifier);
        }

    }

    /// <summary>
    /// Returns the row adjacent to <paramref name="rowItem"/> in the currently displayed order — the
    /// next row if one exists, otherwise the previous row, otherwise default if rowItem was the only
    /// displayed row. 
    /// </summary>
    public TData? GetAdjacentDisplayedRow(TData rowItem)
    {
        var index = _dataSource.IndexOf(rowItem);

        if (index < 0) return default;
        if (index < _dataSource.Count - 1) return _dataSource[index + 1];
        if (index > 0) return _dataSource[index - 1];

        return default;
    }

    /// <summary>
    /// Performs asynchronous disposal of resources, including the JS module reference
    /// </summary>
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

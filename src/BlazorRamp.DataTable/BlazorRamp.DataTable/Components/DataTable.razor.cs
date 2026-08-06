using BlazorRamp.Core.Common.Constants;
using BlazorRamp.Core.Common.Models;
using BlazorRamp.Core.Services;
using BlazorRamp.DataTable.Common.Constants;
using BlazorRamp.DataTable.Common.Models;
using BlazorRamp.DataTable.Common.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorRamp.DataTable.Components;


/// <summary>
/// An accessible, sortable, filterable data table for an in-memory list of <typeparamref name="TData"/> items.
/// Supports optional paging, virtualization, row selection, and screen reader announcements for sort,
/// filter, and empty-result states.
/// </summary>
/// <typeparam name="TData">The row item type displayed by the table.</typeparam>
public partial class DataTable<TData> : ComponentBase
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

    /// <summary>The number of extra rows rendered above and below the visible viewport when virtualization is active, to reduce blank space during fast scrolling.</summary>
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
    /// Header text for the row-selection column. Defaults if null, empty, or whitespace
    /// .</summary>
    [Parameter] public string?          RowSelectHeading      { get; set; }

    /// <summary>
    /// Optional function producing a per-row accessible label for that row's selection checkbox or text
    /// added to a column button to make a better/unique accessible name.
    /// </summary>
    [Parameter] public Func<TData, string>? RowIdentifierFunc { get; set; }


    /// <summary>
    /// Text shown when there are no rows to display. Defaults if null, empty, or whitespace.
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
    /// The full, list of rows to display. Required.
    /// </summary>
    [Parameter, EditorRequired] public List<TData> DataSource       { get; set; } = [];

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
    /// Raised whenever the selected rows change.
    /// </summary>
    [Parameter] public EventCallback<List<TData>> SelectedRowsChanged { get; set; }

    [Inject] private ILiveRegionService? LiveRegionService { get; set; }

    private readonly List<ColumnBase<TData>>    _tableColumns = [];
    private readonly Dictionary<string, string> _columnAlignments = [];

    private Func<TData, bool>? _previousFilterRule = null;

    private List<TData> _filteredUnsortedDataSource = [];
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
        var rowsChanged = false;

        _tableTitle           = String.IsNullOrWhiteSpace(Title)                 ? GlobalValues.DataTable_Title_Text               : Title.Trim();
        _noDataText           = String.IsNullOrWhiteSpace(NoRecordText)          ? GlobalValues.DataTable_No_Records_Text          : NoRecordText.Trim();
        _filterCountText      = String.IsNullOrWhiteSpace(FilterCountText)       ? GlobalValues.DataTable_Filter_Count_Text        : FilterCountText.Trim();
        _recordCountText      = String.IsNullOrWhiteSpace(RecordCountText)       ? GlobalValues.DataTable_Record_Count_Text        : RecordCountText.Trim();
        _sortUpCompleted      = String.IsNullOrWhiteSpace(SortUpStatusText)      ? GlobalValues.DataTable_Sort_Up_Status_Text      : SortUpStatusText.Trim();
        _sortDownCompleted    = String.IsNullOrWhiteSpace(SortDownStatusText)    ? GlobalValues.DataTable_Sort_Down_Status_Text    : SortDownStatusText.Trim();
        _sortRemovedCompleted = String.IsNullOrWhiteSpace(SortRemovedStatusText) ? GlobalValues.DataTable_Sort_Removed_Status_Text : SortRemovedStatusText.Trim();
        _filteringCompleted   = String.IsNullOrWhiteSpace(FilteredStatusText)    ? GlobalValues.DataTable_Filtered_Status_Text     : FilteredStatusText;
        _pressToSortText      = String.IsNullOrWhiteSpace(PressToSortText)       ? GlobalValues.DataTable_Press_To_Sort_Text       : PressToSortText.Trim();
        _tableHeight          = String.IsNullOrWhiteSpace(TableHeight)           ? GlobalValues.DataTable_Max_Table_Height         : TableHeight.Trim();

        if (false == _tableHeight.EndsWith(';')) _tableHeight += ';'; 
        
        _tableHeightStyle = _usePaging ? $"max-height: {_tableHeight}" : $"height: {_tableHeight}";

        var isSameDataSource = ReferenceEquals(_previousDataRef, DataSource);


        if (false == isSameDataSource) //new search / datasource
        {
            _selectedRows.Clear();//new datasource so different equality, we could end up with duplicates if left.
            _previousDataRef = DataSource;
            _dataSource = [.. DataSource];   

            await CheckAndResortLastColumn(_dataSource, _tableColumns, _lastSortedColumnIndex, DataSource, null, FilterRule);

            if (_usePaging) CheckSetPagingInfo(DataSource, true);
        }
        else if ((_usePaging == true && DataSource.Count != PagerBinding!.TotalItemCount))//existing data source with items added or deleted
        {
            rowsChanged = true;
            if (DataSource.Count < (PagerBinding.CurrentPage * PagerBinding.ItemsPerPage))//move to new last page if page is no good
            {
                PagerBinding.CurrentPage = DataSource.Count < 1 ? 0 : (int)Math.Ceiling((double)DataSource.Count / PagerBinding.ItemsPerPage);
            }
            PagerBinding!.TotalItemCount = DataSource.Count;
            PagerBinding.CurrentItemCount = DataSource.Count;
        }

        _displayCountMessage = GetDisplayCountMessage(_dataSource.Count, DataSource.Count);

        await CheckSetApplyFilterRule(FilterRule, _previousFilterRule, rowsChanged);

        _selectedRows = SelectedRows ?? [];
        _dataPage     = _dataSource;

        if (_dataSource.Count == 0) _displayCountMessage = String.Empty;
        if (_usePaging) _dataPage = GetDataPage(PagerBinding!.CurrentPage, PagerBinding.ItemsPerPage, _dataSource);

    }


    /// <summary>
    /// Applies <paramref name="currentFilterRule"/> to <see cref="DataSource"/> if it has changed (or
    /// <paramref name="rowsChanged"/> is true), caches the filtered-but-unsorted result, re-applies
    /// the last active sort, and announces the resulting row count.
    /// </summary>
    private async Task CheckSetApplyFilterRule(Func<TData, bool>? currentFilterRule, Func<TData, bool>? previousFilterRule, bool rowsChanged)
    {
        if (currentFilterRule is null || (currentFilterRule == previousFilterRule && false == rowsChanged)) return;

        await ToggleBusyIndicator(true, "");

        _previousFilterRule         = FilterRule;
        _dataSource                 = [.. DataSource.Where(FilterRule!)];
        _filteredUnsortedDataSource = [.. _dataSource];

        await CheckAndResortLastColumn(_dataSource, _tableColumns, _lastSortedColumnIndex, DataSource, _filteredUnsortedDataSource, currentFilterRule);

        if (_usePaging) CheckSetPagingInfo(_dataSource, false);

        if (_dataSource.Count > 0 && false == _usePaging)
        {
            _displayCountMessage = GetDisplayCountMessage(_dataSource.Count, DataSource.Count);

            await MakeAnnouncement(_displayCountMessage, _tableTitle);
        }

        await ToggleBusyIndicator(false, _filteringCompleted);
    }

    /// <summary>
    /// Builds the "showing N of M rows" style message, choosing the filtered or unfiltered template as appropriate.
    /// </summary>
    private string GetDisplayCountMessage(int currentRowCount, int originalRowCount)

        => currentRowCount == originalRowCount ? _recordCountText.Replace("{totalrows}", originalRowCount.ToString())
                                               : _filterCountText.Replace("{filteredrows}", currentRowCount.ToString()).Replace("{totalrows}", originalRowCount.ToString());
    
    /// <summary>
    /// Resolves default text parameters and takes the initial copy of <see cref="DataSource"/> 
    /// on first initialization.</summary>
    protected override void OnInitialized()
    {
        _rowSelectHeading     = String.IsNullOrWhiteSpace(RowSelectHeading) ? GlobalValues.DataTable_Row_Selector_Header_Text : RowSelectHeading.Trim();
        _usePaging            = PagerBinding != null;
        _previousDataRef      = DataSource;
         _dataSource          = [.. DataSource];
    }

    /// <summary>
    /// Applies the default sort on first render, and announces the no-data state once per empty result while avoiding duplicate announcements from extra render passes.
    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
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
        if (true == column.IsSortable)
        {
            _lastSortedColumnIndex = await ToggleSortData(column, _dataSource, DataSource, _filteredUnsortedDataSource, FilterRule);

            if (true == _usePaging)
            {
                PagerBinding!.CurrentPage = 1;//comment out if you do not like sorting to restart the paging
                _dataPage = GetDataPage(PagerBinding.CurrentPage, PagerBinding.ItemsPerPage, _dataSource);
            }
        }
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
    /// Cycles the given column's sort state (NotSorted → Ascending → Descending → NotSorted), applies the
    /// resulting sort (or restores original order), and returns the column's index for use as <c>_lastSortedColumnIndex</c>.
    /// </summary>
    private async Task<int> ToggleSortData(ColumnBase<TData> dataColumn, List<TData> dataSource, List<TData> originalDataSource, List<TData>? filteredUnsortedDataSource, Func<TData, bool>? filterRule)
    {
        await ToggleBusyIndicator(true);

        var sortDirection = dataColumn.ColumnSortDirection;

        foreach (var column in _tableColumns) column.ColumnSortDirection = ColumnSortDirection.NotSorted;

        dataColumn.ColumnSortDirection = sortDirection switch { ColumnSortDirection.NotSorted => ColumnSortDirection.Ascending, ColumnSortDirection.Ascending => ColumnSortDirection.Descending, _ => ColumnSortDirection.NotSorted };

        if(dataColumn.ColumnSortDirection == ColumnSortDirection.NotSorted)
        {
            UnSortDataSource(dataSource, originalDataSource, filteredUnsortedDataSource, filterRule);
        }
        else
        {
            await SortDataSource(dataColumn.ColumnSortDirection, dataSource, dataColumn);
        }

        var sortingText = dataColumn.ColumnSortDirection switch { ColumnSortDirection.Ascending => _sortUpCompleted, ColumnSortDirection.Descending => _sortDownCompleted, _ => _sortRemovedCompleted };

        await ToggleBusyIndicator(false, sortingText);

        return _tableColumns.IndexOf(dataColumn);
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

    private void CheckSetPagingInfo(List<TData> dataSource, bool isNewData)
    {
        if (PagerBinding is not null)
        {
            PagerBinding.CurrentPage      = dataSource.Count > 0 ? 1 : 0;
            PagerBinding.CurrentItemCount = dataSource.Count;

            if (isNewData) PagerBinding.TotalItemCount = dataSource.Count;
        }
    }

    /// <summary>
    /// Re-applies the sort (or restores original order) for the last-sorted column against a freshly
    /// filtered or replaced <paramref name="dataSource"/>, so sort state survives filter and data source changes.
    /// </summary>
    private async Task CheckAndResortLastColumn(List<TData> dataSource, List<ColumnBase<TData>> tableColumns, int lastSortedColumnIndex, List<TData> originalDataSource, List<TData>? filteredUnsortedDataSource, Func<TData,bool>? filterRule)
    {
        if (lastSortedColumnIndex == -1) return;

        var dataColumn = tableColumns[lastSortedColumnIndex];

        if (dataColumn.ColumnSortDirection == ColumnSortDirection.NotSorted)
        {
            UnSortDataSource(dataSource, originalDataSource, filteredUnsortedDataSource,filterRule);
        }
        else
        {
            await SortDataSource(dataColumn.ColumnSortDirection, dataSource, dataColumn);
        }
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
    /// Restores <paramref name="dataSource"/> to its original (pre-sort) order. Uses the cached
    /// <paramref name="filteredUnsortedDataSource"/> when a filter is active to avoid re-filtering the
    /// full <paramref name="originalDataSource"/>; falls back to re-deriving it if the cache is unavailable.
    /// </summary>
    private void UnSortDataSource(List<TData> dataSource, List<TData> originalDataSource, List<TData>? filteredUnsortedDataSource = null, Func<TData, bool>? filterRule = null)
    {
        var sourceData = filterRule is null ? originalDataSource : filteredUnsortedDataSource ?? [.. originalDataSource.Where(filterRule)];

        for (int index = 0; index < dataSource.Count; index++) dataSource[index] = sourceData[index];
    }

    /// <summary>
    /// Sorts <paramref name="dataSource"/> in place by <paramref name="dataColumn"/>'s bound property.
    /// Uses a stable comparison (ties broken by original index, unaffected by sort direction) so equal
    /// values retain their relative order regardless of ascending/descending.
    /// </summary>
    private async Task SortDataSource(ColumnSortDirection sortDirection, List<TData> dataSource, ColumnBase<TData> dataColumn)
    {

        if (dataColumn.PropertyInfo == null || dataSource.Count <= 1) return;

        var sortAscending = sortDirection == ColumnSortDirection.Ascending ? true : false;

        var propertyInfo = dataColumn.PropertyInfo;

        var getter = dataColumn.ValueGetter!;

        if (propertyInfo.PropertyType == typeof(string))
        {
            var sortArray = new (TData Item, string Key, int OriginalIndex)[dataSource.Count];
            for (int i = 0; i < dataSource.Count; i++)
            {
                sortArray[i] = (dataSource[i], (string)getter(dataSource[i]), i);
            }

            Array.Sort(sortArray, (x, y) =>
            {
                int compare = string.Compare(x.Key, y.Key, StringComparison.Ordinal);

                if (compare == 0)
                {
                    return x.OriginalIndex.CompareTo(y.OriginalIndex);
                }

                return sortAscending ? compare : -compare;
            });

            for (int i = 0; i < dataSource.Count; i++)
            {
                dataSource[i] = sortArray[i].Item;
            }
        }
        else
        {
            var sortArray = new (TData Item, IComparable Key, int OriginalIndex)[dataSource.Count];
            for (int i = 0; i < dataSource.Count; i++)
            {
                sortArray[i] = (dataSource[i], (IComparable)getter(dataSource[i]), i);
            }

            Array.Sort(sortArray, (x, y) =>
            {
                int compare;

                if (x.Key == null)
                {
                    compare = (y.Key == null) ? 0 : -1;
                }
                else if (y.Key == null)
                {
                    compare = 1;
                }
                else
                {
                    compare = x.Key.CompareTo(y.Key);
                }

                if (compare == 0)
                {
                    return x.OriginalIndex.CompareTo(y.OriginalIndex);
                }

                return sortAscending ? compare : -compare;
            });

            for (int i = 0; i < dataSource.Count; i++)
            {
                dataSource[i] = sortArray[i].Item;
            }
        }

        await Task.CompletedTask;
    }


}

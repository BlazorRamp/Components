using BlazorRamp.Core.Common.Constants;
using BlazorRamp.Core.Common.Models;
using BlazorRamp.Core.Services;
using BlazorRamp.DataTable.Common.Constants;
using BlazorRamp.DataTable.Common.Models;
using BlazorRamp.DataTable.Common.Utilities;
using Microsoft.AspNetCore.Components;
using System.Data.Common;
using System.Diagnostics;
using System.Formats.Asn1;

namespace BlazorRamp.DataTable.Components;

public partial class DataTable<TData> : ComponentBase
{

    [Parameter] public RenderFragment TableColumns { get; set; } = default!;

    [Parameter] public string           Title                 { get; set; } = GlobalValues.DataTable_Title_Text;
    [Parameter] public TitleAlignment   TitleAlignment        { get; set; } = TitleAlignment.Start;
    [Parameter] public bool             TitleHidden           { get; set; } = false;
    [Parameter] public RenderFragment?  Filter                { get; set; }
    [Parameter] public FilterAlignment  FilterAlignment       { get; set; } = FilterAlignment.End;
    [Parameter] public RenderFragment?  TopPager              { get; set; }
    [Parameter] public RenderFragment?  BottomPager           { get; set; }
    [Parameter] public int              VirtualizeItemSizePX  { get; set; } = 32;
    [Parameter] public PagerBinding?    PagerBinding          { get; set; } = null;
    [Parameter] public int              DefaultSortIndex      { get; set; } = -1;
    [Parameter] public string?          RowSelectHeading      { get; set; }
    [Parameter] public string?          RowIdentifierHeading  { get; set; }
    [Parameter] public Func<TData, string>? RowIdentifierFunc { get; set; }

    //[Parameter] public Func<TData,string>? Ar
    [Parameter] public string?          NoRecordText         { get; set; } = GlobalValues.DataTable_No_Records_Text;
    [Parameter] public string?          FilterCountText      { get; set; } = GlobalValues.DataTable_Filter_Count_Text;
    [Parameter] public string?          RecordCountText      { get; set; } = GlobalValues.DataTable_Record_Count_Text;

    [Parameter, EditorRequired] public List<TData> DataSource       { get; set; } = [];
    [Parameter] public RowSelectionMode            RowSelectionMode { get; set; } = RowSelectionMode.None;
    [Parameter] public List<TData>                 SelectedRows     { get; set; } = [];
    [Parameter] public Func<TData, string?>?       RowStyleFunc     { get; set; } = null;
    [Parameter] public Func<TData, bool>?          FilterRule       { get; set; }


    [Parameter] public string FilteredStatusText    { get; set; } = GlobalValues.DataTable_Filtered_Status_Text;
    [Parameter] public string SortUpStatusText      { get; set; } = GlobalValues.DataTable_Sort_Up_Status_Text;
    [Parameter] public string SortDownStatusText    { get; set; } = GlobalValues.DataTable_Sort_Down_Status_Text;
    [Parameter] public string SortRemovedStatusText { get; set; } = GlobalValues.DataTable_Sort_Removed_Status_Text;
    [Parameter] public string PressToSortText       { get; set; } = GlobalValues.DataTable_Press_To_Sort_Text;

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
    private string _rowIdentifierHeading = GlobalValues.DataTable_Row_Identifier_Heading_Text;
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


    private string _headerKey = Guid.NewGuid().ToString();

    private string _operationCompletedAnnouncement = String.Empty;

    private bool _showTableSpinner = false;
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
        _filteringCompleted   = String.IsNullOrWhiteSpace(RecordCountText)       ? GlobalValues.DataTable_Filtered_Status_Text     : FilteredStatusText;
        _pressToSortText      = String.IsNullOrWhiteSpace(PressToSortText)       ? GlobalValues.DataTable_Press_To_Sort_Text       : PressToSortText.Trim();

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

    private string GetDisplayCountMessage(int currentRowCount, int originalRowCount)

        => currentRowCount == originalRowCount ? _recordCountText.Replace("{totalrows}", originalRowCount.ToString())
                                               : _filterCountText.Replace("{filteredrows}", currentRowCount.ToString()).Replace("{totalrows}", originalRowCount.ToString());

    protected override void OnInitialized()
    {
        _rowSelectHeading     = String.IsNullOrWhiteSpace(RowSelectHeading) ? GlobalValues.DataTable_Row_Selector_Header_Text : RowSelectHeading.Trim();
        _rowIdentifierHeading = String.IsNullOrWhiteSpace(RowIdentifierHeading) ? GlobalValues.DataTable_Row_Identifier_Heading_Text : RowIdentifierHeading.Trim();//TODO ??
        _usePaging            = PagerBinding != null;
        _previousDataRef      = DataSource;
         _dataSource          = [.. DataSource];

        //_displayCountMessage = GetDisplayCountMessage(_dataSource.Count, DataSource.Count);
    }


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        Debug.WriteLine("In OnAfterRenderAsync");

        if (true == firstRender)
        {

            if (DefaultSortIndex > -1 && DefaultSortIndex < _tableColumns.Where(a => a is DataColumn<TData>).ToList().Count)
            {
                _lastSortedColumnIndex = await ToggleSortData(_tableColumns[DefaultSortIndex], _dataSource, DataSource, _filteredUnsortedDataSource, FilterRule);

                if(true == _usePaging)  _dataPage = GetDataPage(PagerBinding!.CurrentPage, PagerBinding.ItemsPerPage, _dataSource);

                await InvokeAsync(StateHasChanged);
            }

            return;
        }


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

    private async Task MakeAnnouncement(string message, string trigger)
    {
        var announcement = new Announcement(message, AnnouncementType.Info, trigger, LiveRegionType.Polite);
        if (LiveRegionService is not null) await LiveRegionService.MakeAnnouncement(announcement, false);
    }

    internal void AddDataTableColumn(ColumnBase<TData> dataColumn)
    {
        _columnAlignments[dataColumn.FieldName] = DataTableHelper.GetDataPosition(dataColumn.ColumnAlignment);

        if (false == _tableColumns.Contains(dataColumn)) _tableColumns.Add(dataColumn);

        StateHasChanged();
    }

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

    private async Task ToggleBusyIndicator(bool showSpinner, string endMessage = "")
    {

        Debug.WriteLine("In ToggleBusyIndicator showSpinner: " + showSpinner.ToString());
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

    private async Task<int> ToggleSortData(ColumnBase<TData> dataColumn, List<TData> dataSource, List<TData> originalDataSource, List<TData>? filteredUnsortedDataSource, Func<TData, bool>? filterRule)
    {
        Debug.WriteLine("ToggleSortData Start for column: " + dataColumn.DisplayName);
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
        Debug.WriteLine("ToggleSortData Stopped for column: " + dataColumn.DisplayName);
        return _tableColumns.IndexOf(dataColumn);
    }


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

    private async Task HandleOnSelectedRow(TData rowItem)
    {
        if (RowSelectionMode == RowSelectionMode.None) return;

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


    private void UnSortDataSource(List<TData> dataSource, List<TData> originalDataSource, List<TData>? filteredUnsortedDataSource = null, Func<TData, bool>? filterRule = null)
    {
        var sourceData = filterRule is null ? originalDataSource : filteredUnsortedDataSource ?? [.. originalDataSource.Where(filterRule)];

        for (int index = 0; index < dataSource.Count; index++) dataSource[index] = sourceData[index];
    }
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


    //private static async Task SortDataSource(bool sortAscending, List<TData> dataSource, ColumnBase<TData> dataColumn)
    //{
    //    Stopwatch stopwatch = Stopwatch.StartNew();
    //    if (dataColumn.PropertyInfo == null) return;

    //    var propertyValueGetter = dataColumn.ValueGetter!;//DataTableHelper.CreatePropertyValueGetter<TData>(dataColumn.PropertyInfo);

    //    List<TData> sortedData = [];

    //    if(sortAscending == true)
    //    {
    //        sortedData = [.. dataSource.OrderBy(item => propertyValueGetter(item))];
    //    }
    //    else
    //    {
    //        sortedData = [.. dataSource.OrderByDescending(item => propertyValueGetter(item))];
    //    }

    //    //Do in place swap with sorted results to keep equality reference
    //    for (int index = 0; index < dataSource.Count; index++) dataSource[index] = sortedData[index];

    //    Console.WriteLine("Time: " + stopwatch.ElapsedMilliseconds);
    //    await Task.CompletedTask;
    //}
}

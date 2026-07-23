using BlazorRamp.DataTable.Common.Constants;
using BlazorRamp.DataTable.Common.Models;
using BlazorRamp.DataTable.Common.Utilities;
using Microsoft.AspNetCore.Components;
using System.Data.Common;
using System.Diagnostics;

namespace BlazorRamp.DataTable.Components;

public partial class DataTable<TData> : ComponentBase
{

    [Parameter] public RenderFragment TableColumns { get; set; } = default!;

    [Parameter] public string           Title                { get; set; } = default!;
    [Parameter] public TitleAlignment  TitleAlignment       { get; set; } = TitleAlignment.Start;
    [Parameter] public bool             TitleHidden          { get; set; } = false;
    [Parameter] public RenderFragment?  Filter               { get; set; }
    [Parameter] public FilterAlignment  FilterAlignment      { get; set; } = FilterAlignment.End;
    [Parameter] public RenderFragment?  TopPager             { get; set; }
    [Parameter] public RenderFragment?  BottomPager          { get; set; }
    [Parameter] public int              VirtualizeItemSizePX { get; set; } = 32;
    [Parameter] public PagerBinding?    PagerBinding         { get; set; } = null;

    [Parameter, EditorRequired] public List<TData> DataSource       { get; set; } = [];
    [Parameter] public RowSelectionMode            RowSelectionMode { get; set; } = RowSelectionMode.None;
    [Parameter] public List<TData>                 SelectedRows     { get; set; } = [];
    [Parameter] public Func<TData, string?>?       RowStyleFunc     { get; set; } = null;
    [Parameter] public Func<TData, bool>?          FilterRule       { get; set; }

    [Parameter] public int DefaultSortIndex { get; set; } = -1;
    [Parameter] public EventCallback<List<TData>> SelectedRowsChanged { get; set; }



    private readonly List<ColumnBase<TData>>    _tableColumns = [];
    private readonly Dictionary<string, string> _columnAlignments = [];

    private Func<TData, bool>? _previousFilterRule = null;

    private List<TData> _dataPage        = [];
    private List<TData> _dataSource      = [];
    private List<TData> _previousDataRef = [];
    private List<TData> _selectedRows    = [];

    private string _tableTitleID          = Guid.NewGuid().ToString();
    private string _title                 = GlobalValues.DataTable_Title_Text;
    private int    _lastSortedColumnIndex = -1;
    private bool    _usePaging            = false;
    protected override async Task OnParametersSetAsync()
    {
        var rowsChanged = false;

        _title = String.IsNullOrWhiteSpace(Title) ? GlobalValues.DataTable_Title_Text : Title.Trim();

        if (false == ReferenceEquals(_previousDataRef, DataSource)) //new search / datasource
        {
            _selectedRows.Clear();//new datasource so different equality, we could end up with duplicates if left.
            _previousDataRef = DataSource;
            _dataSource      = DataSource;

            await CheckAndResortLastColumn(_dataSource, _tableColumns, _lastSortedColumnIndex);

            if(_usePaging) CheckSetPagingInfo(DataSource, true);
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

        if (FilterRule is not null && (FilterRule != _previousFilterRule || true == rowsChanged))
        {
            //await ToggleTableSpinner(true, DataSource.Count);
            _previousFilterRule = FilterRule;
            _dataSource = [.. DataSource.Where(FilterRule)];
            
            await CheckAndResortLastColumn(_dataSource, _tableColumns, _lastSortedColumnIndex);
            
            //await ToggleTableSpinner(false);
            if(_usePaging) CheckSetPagingInfo(_dataSource, false);
        }

        _selectedRows = SelectedRows ?? [];
        _dataPage     = _dataSource;

        if (_usePaging) _dataPage = GetDataPage(PagerBinding!.CurrentPage, PagerBinding.ItemsPerPage, _dataSource);

    }

    protected override void OnInitialized()
    {
        _usePaging       = PagerBinding != null;
        _previousDataRef = DataSource;
        _dataSource      = DataSource;
    }


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(true == firstRender)
        {
            if (DefaultSortIndex > -1 && DefaultSortIndex < _tableColumns.Where(a => a is DataColumn<TData>).ToList().Count)
            {
                _lastSortedColumnIndex = await ToggleSortData(_tableColumns[DefaultSortIndex], _dataSource);

                if(true == _usePaging)  _dataPage = GetDataPage(PagerBinding!.CurrentPage, PagerBinding.ItemsPerPage, _dataSource);

                await InvokeAsync(StateHasChanged);
            }
        }
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
            _lastSortedColumnIndex = await ToggleSortData(column, _dataSource);

            if (true == _usePaging)
            {
                PagerBinding!.CurrentPage = 1;//comment out if you do not like sorting to restart the paging
                _dataPage = GetDataPage(PagerBinding.CurrentPage, PagerBinding.ItemsPerPage, _dataSource);
            }
        }
    }
    private async Task<int> ToggleSortData(ColumnBase<TData> dataColumn, List<TData> dataSource)
    {
        //await ToggleTableSpinner(true, dataSource.Count);

        var sortDirection = dataColumn.ColumnSortDirection;

        foreach (var column in _tableColumns) column.ColumnSortDirection = ColumnSortDirection.NotSorted;

        dataColumn.ColumnSortDirection = sortDirection == ColumnSortDirection.Ascending ? ColumnSortDirection.Descending : ColumnSortDirection.Ascending;

        await SortDataSource((dataColumn.ColumnSortDirection == ColumnSortDirection.Ascending), dataSource, dataColumn);

        //await ToggleTableSpinner(false);

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
    private static async Task CheckAndResortLastColumn(List<TData> dataSource, List<ColumnBase<TData>> tableColumns, int lastSortedColumnIndex)
    {
        if (lastSortedColumnIndex != -1)
        {
            var dataColumn = tableColumns[lastSortedColumnIndex];
            await SortDataSource((dataColumn.ColumnSortDirection == ColumnSortDirection.Ascending), dataSource, dataColumn);
        }
    }

    private async Task HandleOnSelectedRow(TData rowItem)
    {
        if (RowSelectionMode == RowSelectionMode.None) return;

        if(_selectedRows.Contains(rowItem)) _selectedRows.Remove(rowItem);

        _selectedRows.Add(rowItem);

        if (RowSelectionMode == RowSelectionMode.Single) _selectedRows.RemoveAll(row => !row!.Equals(rowItem));

        if (SelectedRowsChanged.HasDelegate) await SelectedRowsChanged.InvokeAsync(_selectedRows);
    }

    private static async Task SortDataSource(bool sortAscending, List<TData> dataSource, ColumnBase<TData> dataColumn)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        if (dataColumn.PropertyInfo == null || dataSource.Count <= 1) return;

        var propertyInfo = dataColumn.PropertyInfo;
        var getter = DataTableHelper.CreatePropertyValueGetter<TData>(propertyInfo);

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
                    return sortAscending  ? x.OriginalIndex.CompareTo(y.OriginalIndex)  : y.OriginalIndex.CompareTo(x.OriginalIndex);
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
                    return sortAscending
                        ? x.OriginalIndex.CompareTo(y.OriginalIndex)
                        : y.OriginalIndex.CompareTo(x.OriginalIndex);
                }

                return sortAscending ? compare : -compare;
            });

            for (int i = 0; i < dataSource.Count; i++)
            {
                dataSource[i] = sortArray[i].Item;
            }
        }
        stopwatch.Stop();

        Console.WriteLine("Time: " + stopwatch.ElapsedMilliseconds);
        Debug.WriteLine("Time: " + stopwatch.ElapsedMilliseconds);

        await Task.CompletedTask;
    }
}

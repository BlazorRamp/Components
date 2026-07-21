using BlazorRamp.DataTable.Common.Constants;
using BlazorRamp.DataTable.Common.Utilities;
using Microsoft.AspNetCore.Components;

namespace BlazorRamp.DataTable.Components;

public partial class DataTable<TData>
{

    [Parameter] public RenderFragment TableColumns { get; set; } = default!;

    [Parameter] public string           Title                { get; set; } = default!;
    [Parameter] public ContentAlignment TitleAlignment       { get; set; } = ContentAlignment.Start;
    [Parameter] public RenderFragment?  TopPager             { get; set; }
    [Parameter] public ContentAlignment TopPagerAlignment    { get; set; } = ContentAlignment.Start;
    [Parameter] public RenderFragment?  BottomPager          { get; set; }
    [Parameter] public ContentAlignment BottomPagerAlignment { get; set; } = ContentAlignment.Start;
    [Parameter] public RenderFragment?  Filter               { get; set; }
    [Parameter] public ContentAlignment FilterAlignment      { get; set; } = ContentAlignment.Start;


    private readonly List<ColumnBase<TData>>    _tableColumns = [];
    private readonly Dictionary<string, string> _columnAlignments = [];
    
    private string _title = GlobalValues.DataTable_Title_Text;


    protected override void OnParametersSet()
    {
        _title = String.IsNullOrWhiteSpace(Title) ? GlobalValues.DataTable_Title_Text : Title.Trim();
    }

    internal void AddDataTableColumn(ColumnBase<TData> dataColumn)
    {
        _columnAlignments[dataColumn.FieldName] = DataTableHelper.GetDataPosition(dataColumn.ColumnAlignment);

        if (false == _tableColumns.Contains(dataColumn)) _tableColumns.Add(dataColumn);
    }

    internal void RemoveDataTableColumn(ColumnBase<TData> dataColumn)
    { 
        if (true == _tableColumns.Contains(dataColumn)) _tableColumns.Remove(dataColumn);
    }
}

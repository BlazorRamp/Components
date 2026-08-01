using BlazorRamp.DataTable.Common.Constants;
using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace BlazorRamp.DataTable.Components;

public abstract class ColumnBase<TData> : ComponentBase, IDisposable
{
    [CascadingParameter] protected DataTable<TData> ParentTable { get; set; } = default!;

    [Parameter] public RenderFragment<TData>? CellTemplate { get; set; } = default!;
    [Parameter] public string? HeaderStyle                 { get; set; } = null;
    [Parameter] public string? CellStyle                   { get; set; } = null;
    [Parameter] public string DisplayName                  { get; set; } = default!;
    [Parameter] public string? CellFormat                  { get; set; } = null;
    [Parameter] public ColumnAlignment ColumnAlignment    { get; set; } = ColumnAlignment.Start;

    public PropertyInfo?        PropertyInfo       { get; protected set; } = null;
    public ColumnSortDirection ColumnSortDirection { get; set; } = ColumnSortDirection.NotSorted;

    public Func<TData, object>? ValueGetter { get; protected set; } = null;

    internal string FieldName     => _fieldName;
    internal string Title         => _title;
    internal bool HasFormatString => _hasStringFormat;
    internal string FormatString  => _formatString;

    protected string _fieldName        = String.Empty;
    protected string _title            = String.Empty;
    protected string _formatString     = String.Empty;
    protected bool   _hasStringFormat  = false;


    protected override void OnInitialized()
    {
        _formatString    = String.IsNullOrWhiteSpace(CellFormat) ? String.Empty : CellFormat.Trim();
        _hasStringFormat = !string.IsNullOrEmpty(_formatString);

        ParentTable.AddDataTableColumn(this);
    }

    public void Dispose()
    { 
       if (ParentTable is not null) ParentTable.RemoveDataTableColumn(this);
    }
}
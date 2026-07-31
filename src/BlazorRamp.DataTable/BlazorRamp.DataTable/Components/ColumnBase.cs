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
    [Parameter] public string? Format                      { get; set; } = null;
    [Parameter] public ContentAlignment ColumnAlignment     { get; set; } = ContentAlignment.Start;

    public PropertyInfo?        PropertyInfo       { get; protected set; } = null;
    public ColumnSortDirection ColumnSortDirection { get; set; } = ColumnSortDirection.NotSorted;

    public Func<TData, object>? ValueGetter { get; protected set; } = null;

    public string FieldName     => _fieldName;
    public string Title         => _title;
    public bool HasFormatString => _hasStringFormat;
    public string FormatString  => _formatString;

    protected string _fieldName        = String.Empty;
    protected string _title            = String.Empty;
    protected string _formatString     = String.Empty;
    protected bool   _hasStringFormat  = false;


    protected override void OnInitialized()
    {
        _formatString    = String.IsNullOrWhiteSpace(Format) ? String.Empty : Format.Trim();
        _hasStringFormat = !string.IsNullOrEmpty(_formatString);

        ParentTable.AddDataTableColumn(this);
    }

    public void Dispose()
    { 
       if (ParentTable is not null) ParentTable.RemoveDataTableColumn(this);
    }
}
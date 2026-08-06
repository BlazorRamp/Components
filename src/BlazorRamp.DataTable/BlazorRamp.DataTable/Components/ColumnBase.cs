using BlazorRamp.DataTable.Common.Constants;
using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace BlazorRamp.DataTable.Components;

/// <summary>
/// Base class for all columns that can be added to a <see cref="DataTable{TData}"/>.
/// Handles registration with the parent table and the common parameters shared by every column type.
/// </summary>
/// <typeparam name="TData">The row item type displayed by the parent table.</typeparam>
public abstract class ColumnBase<TData> : ComponentBase, IDisposable
{
    /// <summary>
    /// The parent <see cref="DataTable{TData}"/> this column is cascaded into.
    /// </summary>
    [CascadingParameter] protected DataTable<TData> ParentTable { get; set; } = default!;

    /// <summary>
    /// Optional template used to render each cell's content instead of just the raw value.
    /// </summary>
    [Parameter] public RenderFragment<TData>? CellTemplate { get; set; } = default;

    /// <summary>
    /// Optional inline CSS values applied to a style attribute on the column's header cell.
    /// </summary>
    [Parameter] public string? HeaderStyle                 { get; set; } = null;

    /// <summary>
    /// Optional inline CSS values applied to a style attribute added to every cell in the column.
    /// </summary>
    [Parameter] public string? CellStyle                   { get; set; } = null;

    /// <summary>
    /// The text shown in the column header.
    /// </summary>
    [Parameter] public string DisplayName                  { get; set; } = default!;

    /// <summary>
    /// Optional .NET format string applied to the cell's value, e.g. for dates or numbers.
    /// </summary>
    [Parameter] public string? CellFormat                  { get; set; } = null;

    /// <summary>
    /// Horizontal alignment applied to the column's header and cells.
    /// </summary>
    [Parameter] public ColumnAlignment ColumnAlignment    { get; set; } = ColumnAlignment.Start;

    /// <summary>
    /// Reflection metadata for the bound property, resolved once the column initialises. Null for template-only columns.
    /// </summary>
    public PropertyInfo?        PropertyInfo       { get; protected set; } = null;

    /// <summary>
    /// The column's current sort state.
    /// </summary>
    public ColumnSortDirection ColumnSortDirection { get; set; } = ColumnSortDirection.NotSorted;

    /// <summary>
    /// Compiled delegate that reads this column's value from a row item.
    /// </summary>
    public Func<TData, object>? ValueGetter { get; protected set; } = null;

    internal string FieldName     => _fieldName;
    internal string Title         => _title;
    internal bool HasFormatString => _hasStringFormat;
    internal string FormatString  => _formatString;

    /// <summary>
    /// The underlying property/field name used as this column's dictionary key for alignment lookups.
    /// </summary>
    protected string _fieldName = String.Empty;

    /// <summary>
    /// The resolved header text for this column.
    /// </summary>
    protected string _title = String.Empty;

    /// <summary>
    /// The trimmed format string derived from <see cref="CellFormat"/>.
    /// </summary>
    protected string _formatString = String.Empty;

    /// <summary>
    /// Whether a non-empty <see cref="CellFormat"/> was supplied.
    /// </summary>
    protected bool _hasStringFormat = false;

    /// <summary>
    /// Resolves the cell format and registers this column with its parent table.
    /// </summary>
    protected override void OnInitialized()
    {
        _formatString    = String.IsNullOrWhiteSpace(CellFormat) ? String.Empty : CellFormat.Trim();
        _hasStringFormat = !string.IsNullOrEmpty(_formatString);

        ParentTable.AddDataTableColumn(this);
    }


    /// <summary>
    /// Unregisters this column from its parent table when the column is removed from the render tree.
    /// </summary>
    public void Dispose()
    { 
       if (ParentTable is not null) ParentTable.RemoveDataTableColumn(this);
    }
}
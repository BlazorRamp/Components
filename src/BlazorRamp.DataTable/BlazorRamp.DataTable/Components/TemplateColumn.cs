using Microsoft.AspNetCore.Components;

namespace BlazorRamp.DataTable.Components;

/// <summary>
/// A column not bound to any property, rendered entirely via <see cref="ColumnBase{TData}.CellTemplate"/>
/// and an optional <see cref="HeaderTemplate"/>. Not sortable, since there is no underlying value to sort by.
/// </summary>
/// <typeparam name="TData">The row item type displayed by the parent table.</typeparam>
public class TemplateColumn<TData> : ColumnBase<TData>
{
    /// <summary>
    /// Optional template used to render the column header. Receives the column's display name.
    /// </summary>
    [Parameter] public RenderFragment<string>? HeaderTemplate { get; set; } = default!;
    protected override void OnInitialized()
    {
        _fieldName  = Guid.NewGuid().ToString();//just something to get alignments as these get added to a dictionary with the field as the key
        _title      = String.IsNullOrWhiteSpace(DisplayName) ? String.Empty : DisplayName;
        
        base.OnInitialized();//Other stuff gets set in the column base and the column is added to the parent, the data table.
    }
}

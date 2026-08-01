
using BlazorRamp.DataTable.Common.Constants;
using BlazorRamp.DataTable.Common.Utilities;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace BlazorRamp.DataTable.Components;

/// <summary>
/// A column bound to a specific property on <typeparamref name="TData"/>. Supports sorting, formatting,
/// per-row cell styling, and an optional cell template.
/// </summary>
/// <typeparam name="TData">The row item type displayed by the parent table.</typeparam>
public partial class DataColumn<TData> : ColumnBase<TData>
{
    /// <summary>
    /// The property on <typeparamref name="TData"/> this column reads its value from. Required.
    /// </summary>
    [Parameter, EditorRequired] public Expression<Func<TData, object>>? DataProperty { get; set; } = default;

    /// <summary>
    /// Optional function returning inline CSS for a specific row's cell in this column.
    /// </summary>
    [Parameter] public Func<TData, string?>? CellStyleFunc { get; set; } = null;

    /// <summary>
    /// Whether clicking the column header sorts the table by this column.
    /// </summary>
    [Parameter] public bool IsSortable { get; set; } = false;

    protected override void OnInitialized()
    {
        if (DataProperty is null) throw new ArgumentNullException(nameof(DataProperty), GlobalValues.DataTable_Data_Property_Exception_Message);
        
        _fieldName = DataTableHelper.GetPropertyName(DataProperty);
        _title     = String.IsNullOrWhiteSpace(DisplayName) ? _fieldName : DisplayName;
        
        PropertyInfo = typeof(TData).GetProperty(_fieldName);

        ValueGetter = DataTableHelper.CreatePropertyValueGetter<TData>(PropertyInfo!);

        base.OnInitialized();//Other stuff gets set in the column base and the column is added to the parent, the data table.

    }

}


using BlazorRamp.DataTable.Common.Constants;
using BlazorRamp.DataTable.Common.Utilities;
using BlazorRamp.DataTable.Components;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace BlazorRamp.DataTable.Components;

public partial class DataColumn<TData> : ColumnBase<TData>
{
    [Parameter, EditorRequired] public Expression<Func<TData, object>>? DataProperty { get; set; } = default;
    [Parameter] public Func<TData, string?>? CellStyleFunc { get; set; } = null;
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

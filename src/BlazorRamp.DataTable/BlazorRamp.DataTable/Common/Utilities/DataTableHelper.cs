using BlazorRamp.DataTable.Common.Constants;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace BlazorRamp.DataTable.Common.Utilities;

internal static class DataTableHelper
{
    public static string? FormatValue<T>(T value, string? format)
    {
        if (value is null) return null;

        if (string.IsNullOrWhiteSpace(format)) return value.ToString();

        if (value is IFormattable formattable) return formattable.ToString(format, System.Globalization.CultureInfo.CurrentCulture);

        return value.ToString();
    }
    public static Func<T, object> CreatePropertyValueGetter<T>(PropertyInfo propertyInfo)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyAccess = Expression.Property(parameter, propertyInfo);
        var convert = Expression.Convert(propertyAccess, typeof(object));

        return Expression.Lambda<Func<T, object>>(convert, parameter).Compile();
    }

    public static string GetPropertyName<TData>(Expression<Func<TData, object>> expression)
    {
        MemberExpression memberExpression;

        if (expression.Body is UnaryExpression unaryExpression)
        {
            memberExpression = unaryExpression.Operand as MemberExpression ?? throw new InvalidOperationException("Invalid expression format");
        }
        else
        {
            memberExpression = expression.Body as MemberExpression ?? throw new InvalidOperationException("Invalid expression format");
        }

        return memberExpression.Member.Name;
    }


    public static string GetDataPosition(ContentAlignment alignment)

        => alignment switch
        {
            ContentAlignment.End    => GlobalValues.DataTable_Data_Position_End,
            ContentAlignment.Centre => GlobalValues.DataTable_Data_Position_Centre,
            _                       => GlobalValues.DataTable_Data_Position_Start
        };

    public static string BuildClassList(params string[] classList)

        => String.Join(" ", classList.Where(c => !string.IsNullOrWhiteSpace(c)));
}

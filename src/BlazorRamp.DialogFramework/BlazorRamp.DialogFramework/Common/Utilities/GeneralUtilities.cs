using BlazorRamp.DialogFramework.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.DialogFramework.Common.Utilities;


public static class GeneralUtilities
{
    public static (string paramName, Type paramType) GetModalDialogParamType<T>(Expression<Func<T, object>> expression)
    {
        if (expression.Body is UnaryExpression unary && unary.Operand is MemberExpression member)
        {
            return new(member.Member.Name, GetMemberType(member.Member));
        }
        else if (expression.Body is MemberExpression directMember)
        {
            return new(directMember.Member.Name, GetMemberType(directMember.Member));
        }

        throw new ArgumentException("Expression is not a valid member expression", nameof(expression));
    }

    private static Type GetMemberType(MemberInfo memberInfo)
    {
        return memberInfo switch
        {
            PropertyInfo propertyInfo => propertyInfo.PropertyType,
            FieldInfo fieldInfo => fieldInfo.FieldType,
            _ => throw new ArgumentException("Expression does not refer to a property or field")
        };
    }

    public static T ThrowIfNullEmptyOrWhitespace<T>(T argument, [CallerArgumentExpression(nameof(argument))] string argumentName = "")

        => argument is null || typeof(T).Name == "String" && string.IsNullOrWhiteSpace(argument as string)
                ? throw new ArgumentException(GlobalValues.Argument_Null_Empty_Exception_Message, argumentName)
                    : argument;

}

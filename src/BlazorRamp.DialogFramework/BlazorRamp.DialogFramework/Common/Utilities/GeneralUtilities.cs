using BlazorRamp.DialogFramework.Common.Constants;
using BlazorRamp.DialogFramework.Framework;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BlazorRamp.DialogFramework.Common.Utilities;

/// <summary>
/// General utility methods used internally by the dialog framework.
/// </summary>
internal static class GeneralUtilities
{
    /// <summary>
    /// Extracts the parameter name and type from a member expression targeting a component property.
    /// Used internally for type-safe parameter binding in <see cref="ModalDialogParameters{TDialog}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The component type.
    /// </typeparam>
    /// <param name="expression">
    /// A lambda expression referencing a component property, e.g. <c>x => x.MyParam</c>.
    /// </param>
    /// <returns>A tuple containing the property name and its <see cref="Type"/>.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the expression is not a valid member expression.
    /// </exception>
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

    /// <summary>
    /// Returns the argument if valid, otherwise throws an <see cref="ArgumentException"/>.
    /// Considers null, empty strings, and whitespace-only strings as invalid.
    /// </summary>
    /// <typeparam name="T">
    /// The argument type.
    /// </typeparam>
    /// <param name="argument">The value to validate.</param>
    /// <param name="argumentName">The argument name, automatically captured by the compiler.</param>
    /// <returns>
    /// The original argument if valid.
    /// </returns>
    /// <exception cref="ArgumentException"
    /// >Thrown if the argument is null, empty, or whitespace.
    /// </exception>
    public static T ThrowIfNullEmptyOrWhitespace<T>(T argument, [CallerArgumentExpression(nameof(argument))] string argumentName = "")

        => argument is null || typeof(T).Name == "String" && string.IsNullOrWhiteSpace(argument as string)
                ? throw new ArgumentException(GlobalValues.Argument_Null_Empty_Exception_Message, argumentName)
                    : argument;

}

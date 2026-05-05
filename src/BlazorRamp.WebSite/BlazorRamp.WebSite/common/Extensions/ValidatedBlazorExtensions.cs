using System.Linq.Expressions;
using Validated.Blazor.Builders;
using Validated.Core.Types;

namespace BlazorRamp.WebSite.common.Extensions;

public static class ValidatedBlazorExtensions
{
    public static BlazorValidationBuilder<TEntity> ForComparisonWithMemberAndValidate<TEntity, TMember>(this BlazorValidationBuilder<TEntity> builder, Expression<Func<TEntity, TMember>> selectorExpression,
                                                    MemberValidator<TEntity> comparisonValidator, Expression<Func<TEntity, TMember>> validateMemberSelector,
                                                    MemberValidator<TMember> memberValidator, string shortCircuitMessage, string displayName) where TEntity : notnull where TMember : notnull
    {
        var memberName = selectorExpression.Body switch
        {
            MemberExpression m => m.Member.Name,
            UnaryExpression { Operand: MemberExpression m } => m.Member.Name,
            _ => throw new ArgumentException("Expression must be a simple member access", nameof(selectorExpression))
        };

        var compiledSelector = selectorExpression.Compile();
        var compiledValidateMember = validateMemberSelector.Compile();

        MemberValidator<TEntity> combined = async (entity, path, compareTo, cancellationToken) =>
        {
            var sourceValue = compiledValidateMember(entity);
            var memberResult = await memberValidator(sourceValue, path, default, cancellationToken);

            if (memberResult.IsInvalid)
                return Validated<TEntity>.Invalid(
                    new InvalidEntry(shortCircuitMessage, path, memberName, displayName));

            return await comparisonValidator(entity, path, compareTo, cancellationToken);
        };

        return builder.ForComparisonWithMember(selectorExpression, combined);
    }
}


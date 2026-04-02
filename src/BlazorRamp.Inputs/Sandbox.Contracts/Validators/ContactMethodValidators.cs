using Sandbox.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Validated.Blazor.Builders;
using Validated.Blazor.Types;
using Validated.Core.Common.Constants;
using Validated.Core.Factories;
using Validated.Core.Types;
using Validated.Core.Validators;


public static class ContactMethodValidators
{
    public static MemberValidator<string> ContactMethodValueValidator { get; }
    public static MemberValidator<string> ContactMethodTypeValidator { get; }

    static ContactMethodValidators()
    {
        ContactMethodValueValidator = MemberValidators.CreateNotNullOrEmptyValidator<string>("MethodValue", "Method value", "Required, cannot be missing, null or empty");

        ContactMethodTypeValidator = MemberValidators.CreateNotNullOrEmptyValidator<string>("MethodType", "Method type", "Required, cannot be missing, null or empty");
    }

    public static ImmutableDictionary<string, BoxedValidator> GetBoxedContactMethodValidators()

        => BlazorValidationBuilder<ContactMethodDto>.Create()
                    .ForMember(c => c.MethodType, ContactMethodTypeValidator)
                        .ForMember(c => c.MethodValue, ContactMethodValueValidator)
                            .GetBoxedValidators();

    public static ImmutableDictionary<string, BoxedValidator> GetBoxedTenantContactMethodValidators(ImmutableList<ValidationRuleConfig> ruleConfigs,
                                                                                                    IValidatorFactoryProvider validationFactoryProvider,
                                                                                                    string tenantID = ValidatedConstants.Default_TenantID, string cultureID = ValidatedConstants.Default_CultureID)

        => BlazorTenantValidationBuilder<ContactMethodDto>.Create(ruleConfigs, validationFactoryProvider)
                    .ForMember(c => c.MethodType)
                        .ForMember(c => c.MethodValue)
                            .GetBoxedValidators();
}
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

namespace Sandbox.Contracts.Validators;


public static class AddressValidators
{
    public static MemberValidator<string> AddressLineValidator { get; }
    public static MemberValidator<string> TownCityValidator { get; }
    public static MemberValidator<string> CountyValidator { get; }
    public static MemberValidator<string> UKPostcodeValidator { get; }




    static AddressValidators()
    {
        AddressLineValidator = MemberValidators.CreateStringRegexValidator(@"^(?=.{5,250}$)(?!.* {2})(?!.*[,\-']{2})[A-Za-z0-9][A-Za-z0-9 ,\-\n']+[A-Za-z0-9]$", "AddressLine",
                                                                     "Address Line", "Must start with a letter or number and be 5 to 250 characters in length.");

        TownCityValidator = MemberValidators.CreateStringRegexValidator(@"^(?=.{3,100}$)[A-Z](?!.* {2})(?!.*'{2})(?!.*-{2})[\-A-Za-z ']+[a-z]+$", "TownCity",
                                                                        "Town / City", "Must start with a capital letter and be between 3 to 100 characters in length.");

        CountyValidator = MemberValidators.CreateStringRegexValidator(@"^(?=.{3,100}$)[A-Z](?!.* {2})(?!.*'{2})(?!.*-{2})[\-A-Za-z ']+[a-z]+$", "County",
                                                                      "County", "Must start with a capital letter and be between 3 to 100 characters in length.");

        UKPostcodeValidator = MemberValidators.CreateStringRegexValidator(@"^(GIR 0AA)|((([ABCDEFGHIJKLMNOPRSTUWYZ][0-9][0-9]?)|(([ABCDEFGHIJKLMNOPRSTUWYZ][ABCDEFGHKLMNOPQRSTUVWXY][0-9][0-9]?)|(([ABCDEFGHIJKLMNOPRSTUWYZ][0-9][ABCDEFGHJKSTUW])|([ABCDEFGHIJKLMNOPRSTUWYZ][ABCDEFGHKLMNOPQRSTUVWXY][0-9][ABEHMNPRVWXY])))) [0-9][ABDEFGHJLNPQRSTUWXYZ]{2})$",
                                                                          "Postcode", "Postcode", "Must be a valid UK formatted postcode.");
    }

    public static ImmutableDictionary<string, BoxedValidator> GetBoxedAddressValidators()

        => BlazorValidationBuilder<AddressDto>.Create()
                    .ForMember(a => a.AddressLine, AddressLineValidator)
                        .ForMember(a => a.TownCity, TownCityValidator)
                            .ForMember(a => a.County, CountyValidator)
                                .ForNullableStringMember(a => a.NullablePostcode, UKPostcodeValidator)
                                    .GetBoxedValidators();

    public static ImmutableDictionary<string, BoxedValidator> GetBoxedTenantAddressValidators(ImmutableList<ValidationRuleConfig> ruleConfigs,
                                                                                                IValidatorFactoryProvider validationFactoryProvider,
                                                                                                string tenantID = ValidatedConstants.Default_TenantID, string cultureID = ValidatedConstants.Default_CultureID)

        => BlazorTenantValidationBuilder<AddressDto>.Create(ruleConfigs, validationFactoryProvider)
                .ForMember(a => a.AddressLine)
                    .ForMember(a => a.TownCity)
                        .ForMember(a => a.County)
                            .ForNullableStringMember(a => a.NullablePostcode).GetBoxedValidators();
}
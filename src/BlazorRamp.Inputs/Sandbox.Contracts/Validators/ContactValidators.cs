using Sandbox.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Validated.Core.Common.Constants;
using Validated.Core.Extensions;
using Validated.Core.Types;
using Validated.Core.Validators;

namespace Sandbox.Contracts.Validators;

public static class ContactValidators
{
    public static MemberValidator<string> TitleValidator { get; }
    public static MemberValidator<string> GivenNameValidator { get; }
    public static MemberValidator<string> FamilyNameValidator { get; }

    public static MemberValidator<string> PasswordValidator { get; }
    public static MemberValidator<int> AgeValidator { get; }
    public static MemberValidator<int> NullableAgeValidator { get; }

    public static MemberValidator<decimal> SalaryValidator { get; }

    public static MemberValidator<ContactDto> CompareDOBValidator { get; }
    public static MemberValidator<DateOnly> DOBValidator { get; }

    public static MemberValidator<bool> IsAliveValidator { get; }

    public static MemberValidator<int> RadioGroupValue { get; }

    public static MemberValidator<TimeOnly> TimeOnlyValidator { get; }
    public static MemberValidator<int> ContactIDValidator { get; }


    /*
        * All of these validator are good for multiple things. Validating individual values, used in the Validated.Core's ValidationBuilder
        * or as in this demo the BlazorValidationBuilder
    */
    static ContactValidators()
    {
        TitleValidator = MemberValidators.CreateStringRegexValidator("^(Mr|Mrs|Ms|Dr|Prof)$", "Title", "Title", "Must be one of Mr, Mrs, Ms, Dr, Prof");

        GivenNameValidator = MemberValidators.CreateStringRegexValidator(@"^(?=.{2,50}$)[A-Z]+['\- ]?[A-Za-z]*['\- ]?[A-Za-z]+$", "GivenName", "First name", "Must start with a capital letter and be between 2 and 50 characters in length");

        FamilyNameValidator = MemberValidators.CreateStringRegexValidator(@"^[A-Z].*$", "FamilyName", "Surname", "Must start with a capital letter")
                                .AndThen(MemberValidators.CreateStringLengthValidator(2, 50, "FamilyName", "Surname", "Must be between 2 and 50 characters in length"));

        NullableAgeValidator = MemberValidators.CreateRangeValidator<int>(25, 50, "Age", "Age", "Must be between 25 and 50");
        AgeValidator = MemberValidators.CreateRangeValidator<int>(25, 50, "Age", "Age", "Must be between 25 and 50");

        CompareDOBValidator = MemberValidators.CreateMemberComparisonValidator<ContactDto, DateOnly>(c => c.CompareDOB, c => c.DOB, CompareType.LessThan, "Compare DOB", "Must be less than Date of birth");

        DOBValidator = MemberValidators.CreateCompareToValidator<DateOnly>(DateOnly.Parse("2022-01-01"), CompareType.EqualTo, "DOB", "Date of birth", "Must be equal to 2022-01-01");

        PasswordValidator = MemberValidators.CreateStringRegexValidator(@"^(?=.{2,50}$)[A-Z]+['\- ]?[A-Za-z]*['\- ]?[A-Za-z]+$", "Password", "Password", "Must start with a capital letter and be between 2 and 50 characters in length");


        SalaryValidator = MemberValidators.CreatePrecisionScaleValidator<decimal>(7, 2, "Salary", "Salary", "Must be pounds and pence");


        IsAliveValidator = MemberValidators.CreateCompareToValidator<bool>(true, CompareType.EqualTo, "IsAlive", "Is Alive", "You should be alive.");


        ContactIDValidator = MemberValidators.CreatePredicateValidator<int>(x => x > 0, "Contact ID", "Contact ID", "Must be in the list of options");


        RadioGroupValue = MemberValidators.CreatePredicateValidator<int>(x => x == 1 && x < 4, "RadioButtonValue", "Radio Button Value", "Required, you must select an option");

        //TimeOnlyValidator = MemberValidators.CreateRangeValidator<TimeOnly>(TimeOnly.FromTimeSpan(new TimeSpan(10, 10, 10)), TimeOnly.FromTimeSpan(new TimeSpan(23, 59, 59)), "TimeValue", "Time Value", "Needs to be over 10 seconds past 10 past 10"); ;// MemberValidators.CreatePredicateValidator<TimeOnly>(x => x.Hour > 10 && x.Minute > 10 & x.Second > 10, "TimeValue", "Time Value", "Needs to be over 10 seconds past 10 past 10");

        TimeOnlyValidator = MemberValidators.CreatePredicateValidator<TimeOnly>(x => x.Hour > 9, "TimeValue", "Time Value", "Needs to be over 10 seconds past 10 past 10"); ;// MemberValidators.CreatePredicateValidator<TimeOnly>(x => x.Hour > 10 && x.Minute > 10 & x.Second > 10, "TimeValue", "Time Value", "Needs to be over 10 seconds past 10 past 10");
    }
}
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
    public static MemberValidator<ContactDto> CompareDOBValidator { get; }
    public static MemberValidator<DateOnly> DOBValidator { get; }

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

        AgeValidator = MemberValidators.CreateRangeValidator(25, 50, "Age", "Age", "Must be between 25 and 50");

        CompareDOBValidator = MemberValidators.CreateMemberComparisonValidator<ContactDto, DateOnly>(c => c.CompareDOB, c => c.DOB, CompareType.LessThan, "Compare DOB", "Must be less than Date of birth");

        DOBValidator = MemberValidators.CreateCompareToValidator<DateOnly>(DateOnly.Parse("2022-01-01"), CompareType.EqualTo, "DOB", "Date of birth", "Must be equal to 2022-01-01");

        PasswordValidator = MemberValidators.CreateStringRegexValidator(@"^(?=.{2,50}$)[A-Z]+['\- ]?[A-Za-z]*['\- ]?[A-Za-z]+$", "Password", "Password", "Must start with a capital letter and be between 2 and 50 characters in length");

    }
}
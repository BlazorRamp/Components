using Sandbox.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Sandbox.Contracts.Data;


public class StaticData
{
    public static ContactDto CreateContactObjectGraph()
    {
        var dob = new DateOnly(2000, 1, 1);
        var olderDob = new DateOnly(2000, 1, 2);

        var nullableAge = DateTime.Now.Year - dob.Year - (DateTime.Now.DayOfYear < dob.DayOfYear ? 1 : 0);
        var age = DateTime.Now.Year - dob.Year - (DateTime.Now.DayOfYear < dob.DayOfYear ? 1 : 0);

        AddressDto address = new() { AddressLine = "Some AddressLine", County = "Some County", NullablePostcode = "Some PostCode", TownCity = "Some Town" };
        AddressDto nullableAddress = new() { AddressLine = "Some AddressLine", County = "Some County", NullablePostcode = "Some PostCode", TownCity = "Some Town" };

        List<ContactMethodDto> contactMethods = [new() { MethodType = "MethodTypeOne", MethodValue = "MethodValueOne" }, new() { MethodType = "MethodTypeTwo", MethodValue = "MethodValueTwo" }];

        return new()
        {
            Address = address,
            NullableAge = nullableAge,
            Age = age,
            ContactMethods = contactMethods,
            DOB = dob,
            CompareDOB = olderDob,
            Email = "john.doe@gmail.com",
            FamilyName = "Doe",
            GivenName = "John",
            NullableMobile = "123456789",
            Title = "Mr",
            Entries = []
        };

    }


    //public static ImmutableList<ValidationRuleConfig> ValidationRuleConfigsForTenantValidationBuilder()

    //    => [
    //        new("Validated.Contracts.Models.ContactDto", "Title", "Title", "RuleType_Regex", "MinMaxToValueType_String", @"^(Mr|Mrs|Ms|Dr|Prof)$", "Must be one of Mr, Mrs, Ms, Dr, Prof", 2, 4),
    //        new("Validated.Contracts.Models.ContactDto", "GivenName", "First name", "RuleType_Regex", "MinMaxToValueType_String", @"^(?=.{2,50}$)[A-Z]+['\- ]?[A-Za-z]*['\- ]?[A-Za-z]+$", "Must start with a capital letter and be between 2 and 50 characters in length", 2, 50),
    //        new("Validated.Contracts.Models.ContactDto", "FamilyName", "Surname", "RuleType_Regex", "MinMaxToValueType_String", @"^[A-Z]+['\- ]?[A-Za-z]*['\- ]?[A-Za-z]*$", "Must start with a capital letter (TenantOne)", 2, 50, "", "", "", "","", ValidatedConstants.TargetType_Item,"TenantOne"),
    //        new("Validated.Contracts.Models.ContactDto", "FamilyName", "Surname", "RuleType_Regex", "MinMaxToValueType_String", @"^[A-Z]+['\- ]?[A-Za-z]*['\- ]?[A-Za-z]*$", "Must start with a capital letter", 2, 50),
    //        new("Validated.Contracts.Models.ContactDto", "FamilyName", "Surname", "RuleType_StringLength", "", "", "Must be between 2 and 50 characters long", 2, 50),
    //        new("Validated.Contracts.Models.ContactDto", "Email", "Email", "RuleType_Regex", "MinMaxToValueType_String", @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", "Must be a valid email format", 4, 75),
    //        new("Validated.Contracts.Models.ContactDto", "DOB", "Date of birth", "RuleType_CompareTo", "MinMaxToValueType_DateOnly", "", "Must be equal to 2022-01-01", 10, 10, "", "","2022-01-01","","CompareType_EqualTo"),
    //        new("Validated.Contracts.Models.ContactDto", "Mobile", "Mobile Number", "RuleType_Regex", "MinMaxToValueType_String", @"^(?:\+[1-9]\d{1,3}[ -]?7\d{9}|07\d{9})$", "Must be a valid UK mobile number format", 11, 16, "", ""),
    //        new("Validated.Contracts.Models.ContactDto", "Age", "Age", "RuleType_Range", "MinMaxToValueType_Int32", "", "Must be between 25 and 50", 0, 0, "25", "50"),

    //        new("Validated.Contracts.Models.ContactDto", "CompareDOB", "Compare DOB", "RuleType_MemberComparison", "", "", "Must be less than Date of birth", 0, 0, "", "","","DOB", "CompareType_LessThan"),

    //        new("Validated.Contracts.Models.ContactDto", "Entries", "Entries", "RuleType_StringLength", "", "", "Must be between 1 and 10 characters in length", 1, 10, "", ""),
    //        new("Validated.Contracts.Models.ContactDto", "Entries", "Entries", ValidatedConstants.RuleType_CollectionLength, "", "", "Must have at least 1 item but no more than 5", 1, 5, "", "","","", "",ValidatedConstants.TargetType_Collection),

    //        new("Validated.Contracts.Models.ContactDto", "ContactMethods", "Contact methods", "RuleType_CollectionLength", "", "", "Must have at least 1 contact method but no more than 3", 1, 3, "", "","","", "",ValidatedConstants.TargetType_Collection),

    //        new("Validated.Contracts.Models.AddressDto", "AddressLine", "Address Line", "RuleType_Regex", "MinMaxToValueType_String", @"^(?=.{5,250}$)(?!.* {2})(?!.*[,\-']{2})[A-Za-z0-9][A-Za-z0-9 ,\-\n']+[A-Za-z0-9]$", "Must start with a letter or number and be 5 to 250 characters in length.", 5, 250),
    //        new("Validated.Contracts.Models.AddressDto", "TownCity", "Town / City", "RuleType_Regex", "MinMaxToValueType_String", @"^(?=.{3,100}$)[A-Z](?!.* {2})(?!.*'{2})(?!.*-{2})[\-A-Za-z ']+[a-z]+$", "Must start with a capital letter and be between 3 to 100 characters in length.", 3, 100),
    //        new("Validated.Contracts.Models.AddressDto", "County", "County", "RuleType_Regex", "MinMaxToValueType_String", @"^(?=.{3,100}$)[A-Z](?!.* {2})(?!.*'{2})(?!.*-{2})[\-A-Za-z ']+[a-z]+$", "Must start with a capital letter and be between 3 to 100 characters in length.", 3, 100),
    //        new("Validated.Contracts.Models.AddressDto", "NullablePostcode", "Postcode", "RuleType_Regex", "MinMaxToValueType_String", @"^(GIR 0AA)|((([ABCDEFGHIJKLMNOPRSTUWYZ][0-9][0-9]?)|(([ABCDEFGHIJKLMNOPRSTUWYZ][ABCDEFGHKLMNOPQRSTUVWXY][0-9][0-9]?)|(([ABCDEFGHIJKLMNOPRSTUWYZ][0-9][ABCDEFGHJKSTUW])|([ABCDEFGHIJKLMNOPRSTUWYZ][ABCDEFGHKLMNOPQRSTUVWXY][0-9][ABEHMNPRVWXY])))) [0-9][ABDEFGHJLNPQRSTUWXYZ]{2})$",
    //                                                                        "Must be a valid UK formatted postcode.", 5, 15),

    //        new("Validated.Contracts.Models.ContactMethodDto", "MethodType", "Method type", "RuleType_StringLength", "", "", "Must be between 2 and 20 characters", 2, 20, "", "","","", ""),
    //        new("Validated.Contracts.Models.ContactMethodDto", "MethodValue", "Method value","RuleType_StringLength", "", "", "Must be between 2 and 20 characters", 2, 20, "", "","","", ""),

    //    ];
}


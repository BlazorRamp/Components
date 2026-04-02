using Validated.Core.Extensions;
using Validated.Core.Types;
using Validated.Core.Validators;

namespace BlazorRamp.WebSite.common.Validators;

public class ContactValidators
{
    public static MemberValidator<string> SurnameValidator { get; }
    public static MemberValidator<string> EmailValidator   { get; }
    
    static ContactValidators() 
    {
        SurnameValidator = MemberValidators.CreateStringRegexValidator(@"^[A-Z].*$", "Surname", "Surname", "Must start with a capital letter.")
                               .AndThen(MemberValidators.CreateStringLengthValidator(2, 50, "FamilyName", "Surname", "Must be between 2 and 50 characters in length but you entered {ActualLength} characters"));

        EmailValidator = MemberValidators.CreateStringLengthValidator(5, 255, "Email", "Email address", "Must be between 5 and 30 characters in length but you entered {ActualLength} characters.")
                                .AndThen(MemberValidators.CreatePredicateValidator<string>(s => s.EndsWith(".com"), "Email", "Email address", "Must end in with .com (lower case)"))
                                    .AndThen(MemberValidators.CreateStringRegexValidator("^[^@]+@[^@]*$", "Email", "Email address", "Must contain one @ symbol"));
    }
}

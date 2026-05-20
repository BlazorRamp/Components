namespace BlazorRamp.DocSite.Common.Constants;

public class InputSnippets
{
    public const string Add_Style_Sheet = """
        <head>
            <link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
            <link rel="stylesheet" href="_content/BlazorRamp.Inputs/assets/css/inputs.min.css" />
        </head>
        """;


    public const string Inputs_Overview_Example = """
        <EditForm Model="@_contactData" OnValidSubmit="HandleSubmit">

            <DataAnnotationsValidator />

            <div class="br-input-row">
                <TextInput class="br-col-xs-12 br-col-sm-6" LabelText="First name:" Required = "true" ErrorsLabel="errors" UpdateOnInput="true" 
                            TextInputType="TextInputType.Text" ValidationDisplayMode="ValidationDisplayMode.TabbableWithHint" 
                                @bind-Value="_contactData.FirstName" HintText="The name you use on official documents" SvgIcon="--svg-user-icon" />

                <TextInput class="br-col-xs-12 br-col-sm-6" LabelText="Surname:" Required="true" UpdateOnInput="true" 
                            TextInputType="TextInputType.Text" ValidationDisplayMode="ValidationDisplayMode.DescribedByWithHint"
                                @bind-Value="_contactData.Surname" HintText="The surname you use on official documents" SvgIcon="--svg-user-icon" />
            </div>

        </EditForm>

        @code {

            private ContactDto _contactData = new();

            private void HandleSubmit() { }

            public class ContactDto
            {
                [Required(ErrorMessage = "First name is a required field.")]
                [StringLength(maximumLength:20,ErrorMessage ="First name must be between 2 and 20 characters in length", MinimumLength = 2)]
                public string FirstName { get; set; }


                [Required(ErrorMessage = "Surname is a required field.")]
                [StringLength(maximumLength: 20, ErrorMessage = "Surname must be between 2 and 20 characters in length", MinimumLength = 2)]
                public string Surname   { get; set; }
            }


        }
        """;


    public const string Numeric_Code_Example = """

        <EditForm Model="@_inventoryData" OnValidSubmit="HandleSubmit">

            <BlazorValidated TEntity="InventoryDto" BoxedValidators="_boxedValidators" AddDisplayName="true" />

            <div class="br-input-row">

                <NumericInput class="br-col-xs-12 br-col-sm-6" @bind-Value="_inventoryData.UnitsInStock" Required="false" 
                    LabelText="Units in stock" ErrorsLabel="errors" HintText="Required, the number of units currently in stock." UpdateOnInput="true" 
                        ValidationDisplayMode="ValidationDisplayMode.TabbableWithHint" ParseErrorMessage="Please enter a valid number for this field type."/>

                <NumericInput class="br-col-xs-12 br-col-sm-6" @bind-Value="_inventoryData.UnitPrice" Required="true" Format="C"
                    LabelText="Unit Price" ErrorsLabel="errors" HintText="The price per unit." UpdateOnInput="true"
                        ValidationDisplayMode="ValidationDisplayMode.TabbableWithHint" />

            </div>

            <div class="br-input-row">
                <button class="br-col-xs-12 test-button" type="submit">Fake Submit to trigger validation on unmodified fields</button>
            </div>

        </EditForm>

        @code {

            public InventoryDto _inventoryData = new();
            private ImmutableDictionary<string, BoxedValidator> _boxedValidators = default!;

            protected void OnInitialized()
            {

                var unitValidator   = MemberValidators.CreateRangeValidator<int>(1, 999, "UnitsInStock", "Units in stock", "Must be between 1 and 999");

                var priceValidator  = MemberValidators.CreatePrecisionScaleValidator<decimal>(5, 2, "UnitPrice", "Unit price", "Cannot be more than than one hundred thousand with a maximum of 2 decimal places")
                                        .AndThen(MemberValidators.CreatePredicateValidator<decimal>(c => c > 1M, "UnitPrice", "Unit Price", "Must be greater than " + String.Format("{0:C}", 1)));

                _boxedValidators = BlazorValidationBuilder<InventoryDto>.Create()
                                        .ForMember(c => c.UnitsInStock, unitValidator)
                                        .ForMember(c => c.UnitPrice, priceValidator)
                                        .GetBoxedValidators();
            }

            private void HandleSubmit() { }

            public class InventoryDto
            {
                public decimal UnitPrice    { get; set; }
                public int     UnitsInStock { get; set; }
            }
        }
        """;


    public const string Password_Code_Example = """
        <EditForm Model="@_registrationData" OnValidSubmit="HandleSubmit">

            <BlazorValidated TEntity="UserRegistrationDto" BoxedValidators="_boxedValidators" AddDisplayName="true" />

            <div class="br-input-row">

                <PasswordInput class="br-col-xs-12 br-col-sm-6"  @bind-Value="_registrationData.Password" Required="true" LabelText="Password" ErrorsLabel="errors" HintText="Choose something only you know" 
                                UpdateOnInput="false" PasswordAutoComplete="PasswordAutoComplete.NewPassword"  ValidationDisplayMode="ValidationDisplayMode.DescribedByHintSuppressed" SvgIcon="--svg-padlock-icon" />



                <PasswordInput class="br-col-xs-12 br-col-sm-6" " @bind-Value="_registrationData.ConfirmPassword" Required="true" LabelText="Password Confirmation" ErrorsLabel="errors" HintText="Please confirm your password." 
                                UpdateOnInput="false" PasswordAutoComplete="PasswordAutoComplete.NewPassword" ValidationDisplayMode="ValidationDisplayMode.DescribedByHintSuppressed" SvgIcon="--svg-key-icon" />
            </div>

        <div class="br-input-row">
            <button class="br-col-xs-12 test-button" type="submit">Fake Submit to trigger validation on unmodified fields</button>
        </div>

        </EditForm>

        @code {

            public UserRegistrationDto _registrationData = new();
            private ImmutableDictionary<string, BoxedValidator> _boxedValidators = default!;

            protected override async Task OnInitializedAsync()
            {
                var passwordValidator = MemberValidators.CreateStringLengthValidator(7, 25, "Password", "Password", "Must be between 7 and 25 characters in length but you entered {ActualLength} characters");
                var compareValidator  = MemberValidators.CreateMemberComparisonValidator<UserRegistrationDto, string>(c => c.Password, c => c.ConfirmPassword, CompareType.EqualTo, "Confirm Password", "Passwords do not match");
                /*
                    * The ForComparisonWithMemberAndValidate is an extension I made just for this demo its not part of the Validated.Blazor library and I probably will not add it to the library
                    * as it would not be possible to add a matching BlazorTenantValidatorBuilder version, and I prefer the parity.
                 */
                _boxedValidators = BlazorValidationBuilder<UserRegistrationDto>.Create()
                                       .ForMember(c => c.Password, passwordValidator)
                                       .ForComparisonWithMemberAndValidate(c => c.ConfirmPassword, compareValidator, c => c.Password, passwordValidator, "Password must be valid before confirming", "Password Confirmation")
                                       .GetBoxedValidators();
            }

            private void HandleSubmit() { }

            public class UserRegistrationDto
            {
                public string Password        { get; set; }
                public string ConfirmPassword { get; set; }
            }
        }
        """;






    public const string Validation_Extension_Method = """

        public static class ValidatedBlazorExtension
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

                var compiledSelector       = selectorExpression.Compile();
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
        """;



    public const string Text_Input_Example = """
        <EditForm Model="@_contactData" OnValidSubmit="HandleSubmit">

            <BlazorValidated TEntity="ContactDto" BoxedValidators="_boxedValidators" AddDisplayName="true" />

            <div class="br-input-row">
                <TextInput class="br-col-xs-12 br-col-sm-6" LabelText="First Name" Required="true" ErrorsLabel="errors" UpdateOnInput="true" 
                    TextInputType="TextInputType.Text" ValidationDisplayMode="ValidationDisplayMode.TabbableWithHint" @bind-Value="_contactData.FirstName" 
                    HintText="The name you use on official documents" SvgIcon="--svg-user-icon" ReadOnly="false" autocomplete="given-name" />

                <TextInput class="br-col-xs-12 br-col-sm-6" LabelText="Surname" Required="true" ErrorsLabel="errors" UpdateOnInput="true" 
                    TextInputType="TextInputType.Text" ValidationDisplayMode="ValidationDisplayMode.TabbableWithHint" @bind-Value="_contactData.Surname" 
                    HintText="The surname you use on official documents" SvgIcon="--svg-user-icon" autocomplete="family-name" />
            </div>
            <div class="br-input-row">
                <button class="br-col-xs-12 test-button" type="submit">Fake Submit to trigger validation on unmodified fields</button>
            </div>
        </EditForm>

        @code {

            private ContactDto _contactData = new();
            private ImmutableDictionary<string, BoxedValidator> _boxedValidators = default!;

            protected override void OnInitialized()
            {

                var firstNameValidator = MemberValidators.CreateStringRegexValidator(@"^(?=.{2,50}$)[A-Z][A-Za-z]*(?:['\- ][A-Za-z]+)*$", "FirstName", "First Name", "Must start with a capital letter and be between 2 and 50 characters in length");
                /*
                    * Although its easy to create a regex that does everything I prefer to break things down into separate rules, so below creates a validator made up of two rules. 
                */
                var surnameValidator = MemberValidators.CreateStringRegexValidator(@"^[A-Z]+['\- ]?[A-Za-z]*['\- ]?[A-Za-z]*$", "Surname", "Surname", "Must start with a capital letter and no double spaces.")
                                        .AndThen(MemberValidators.CreateStringLengthValidator(2, 50, "Surname", "Surname", "Must be between 2 and 50 characters in length"));

                _boxedValidators = BlazorValidationBuilder<ContactDto>.Create()
                                        .ForMember(c => c.FirstName, firstNameValidator)
                                        .ForMember(c => c.Surname, surnameValidator, true)
                                        .GetBoxedValidators();
            }


            private void HandleSubmit() { }

            public class ContactDto
            {

                public string FirstName { get; set; }
                public string Surname { get; set; }
            }

        }
        
        """;

    public const string Error_Summary_Example = """
            
        <EditForm Model="@_contactData" OnValidSubmit="HandleSubmit">

            <BlazorValidated TEntity="ContactDto" BoxedValidators="_boxedValidators" AddDisplayName="true" />

            <InputErrorsSummary InputSuffix="Field" TitleHeadingLevel="TitleHeadingLevel.H3" SummaryDisplay="SummaryDisplay.OnModelValidated">

                <div class="br-input-row">
                    <TextInput class="br-col-xs-12 br-col-sm-6" LabelText="First Name" Required="true" ErrorsLabel="errors" UpdateOnInput="true" 
                    TextInputType="TextInputType.Text" ValidationDisplayMode="ValidationDisplayMode.TabbableWithHint" @bind-Value="_contactData.FirstName" 
                    HintText="The name you use on official documents" SvgIcon="--svg-user-icon" autocomplete="given-name"/>

                    <TextInput class="br-col-xs-12 br-col-sm-6" LabelText="Surname" Required="true" ErrorsLabel="errors" UpdateOnInput="true" 
                    TextInputType="TextInputType.Text" ValidationDisplayMode="ValidationDisplayMode.TabbableWithHint" @bind-Value="_contactData.Surname" 
                    HintText="The surname you use on official documents" SvgIcon="--svg-user-icon" autocomplete="familiy-name" />
                </div>
                <div class="br-input-row">

                    <NumericInput class="br-col-xs-12 br-col-sm-6" @bind-Value="_contactData.Age" Required="true"
                                  LabelText="Age" ErrorsLabel="errors" HintText="Your current age in years." UpdateOnInput="true"
                                  ValidationDisplayMode="ValidationDisplayMode.TabbableWithHint" ParseErrorMessage="Please enter a valid number for this field type." />

                    <NumericInput class="br-col-xs-12 br-col-sm-6" @bind-Value="_contactData.HourlyRate" Required="false" Format="C" 
                                    LabelText="Hourly Rate" ErrorsLabel="errors" HintText="Optional, how much you charge per hour." UpdateOnInput="true" 
                                  ValidationDisplayMode="ValidationDisplayMode.TabbableWithHint" ParseErrorMessage="Please enter a valid number for this field type." maxlength="50"/>

                </div>
            </InputErrorsSummary>

            <div class="br-input-row">
                <button class="br-col-xs-12 normal-button" type="submit">Fake Submit to trigger validation on the model</button>
            </div>
        </EditForm>

        @code {

            private ContactDto _contactData = new();
            private ImmutableDictionary<string, BoxedValidator> _boxedValidators = default!;

            protected override void OnInitialized()
            {

                var firstNameValidator = MemberValidators.CreateStringRegexValidator(@"^(?=.{2,55}$)[A-Z][A-Za-z]*(?:['\- ][A-Za-z]+)*$", "FirstName", "First Name", "Must start with a capital letter and be between 2 and 25 characters in length.");
                /*
                    * Although its easy to create a regex that does everything I prefer to break things down into separate rules, so below creates a validator made up of two rules.
                */
                var surnameValidator = MemberValidators.CreateStringRegexValidator(@"^(?=.{2,})[A-Z][A-Za-z]*(?:['\- ][A-Za-z]+)*\z", "Surname", "Surname", "Must start with a capital letter and no double spaces.")
                                .AndThen(MemberValidators.CreateStringLengthValidator(2, 25, "Surname", "Surname", "Must be between 2 and 25 characters in length but you entered {ActualLength} characters."));

                var ageValidator = MemberValidators.CreateRangeValidator<int>(16, 120, "Age", "Age", "Must be between 16 and 120");

                var salaryValidator = MemberValidators.CreatePrecisionScaleValidator<decimal>(50, 2, "HourlyRate", "Hourly Rate", "Can only contain a maximum of 2 decimal places")
                                .AndThen(MemberValidators.CreateRangeValidator<decimal>(10.00M, 200.00M, "HourlyRate", "Hourly Rate", $"Must be between {String.Format("{0:C}", 10)} and {String.Format("{0:C}", 100)}"));


                _boxedValidators = BlazorValidationBuilder<ContactDto>.Create()
                                        .ForMember(c => c.FirstName, firstNameValidator)
                                        .ForMember(c => c.Surname, surnameValidator)
                                        .ForMember(c => c.Age, ageValidator)
                                        .ForNullableMember(c => c.HourlyRate, salaryValidator)
                                    .GetBoxedValidators();
            }


            private void HandleSubmit() { }

            public class ContactDto
            {
                public string   FirstName  { get; set; }
                public string   Surname    { get; set; }
                public int      Age        { get; set; }
                public decimal? HourlyRate { get; set; }
            }

        }
        """;



    public const string Checkbox_Input_Example = """

        <EditForm Model="@_userAccountData">

            <div class="br-input-row">

                <CheckboxInput class="br-col-xs-12 br-col-sm-6" Value="@_userAccountData.Disabled" ValueChanged="HandleValueChanged" ValueExpression="() => _userAccountData.Disabled"
                LabelText="Disable Account" HintText="Tick if the user account should be disabled." SvgIcon="@_disabledIcon" Required="false" />

                <CheckboxInput class="br-col-xs-12 br-col-sm-6" @bind-Value="_userAccountData.IncreaseSalary" LabelText="Increase Salary" HintText="Tick to give yourself a pay rise."
                                SvgIcon="--svg-money-icon" AriaDisabled="true" Required="false" />

         </div>

        </EditForm>
               
        @code {

            public UserAccountDto _userAccountData = new();

            private string _userIcon            = "--svg-user-icon";
            private string _accountLockedIcon   = "--svg-lock-account-icon";
            private string _disabledIcon        = "--svg-user-icon";

            private void HandleValueChanged(bool value)
            {
                _disabledIcon = value ? _accountLockedIcon : _userIcon;
                _userAccountData.Disabled = value;
            }

            public class UserAccountDto
            {
                public bool Disabled       { get; set; }
                public bool IncreaseSalary { get; set; }
            }
        }

        """;
}

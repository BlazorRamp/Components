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

            <BlazorValidated TEntity="ContactDto" BoxedValidators="_boxedValidators" AddDisplayName="true" DeferFieldValidation="false" />

            <InputErrorsSummary InputSuffix="Field" TitleHeadingLevel="TitleHeadingLevel.H3" SummaryDisplay="SummaryDisplay.OnModelValidated">

                <div class="br-input-row">
                    <TextInput class="br-col-xs-12 br-col-sm-6" LabelText="First Name" Required="true" ErrorsLabel="errors" UpdateOnInput="true" TextInputType="TextInputType.Text" ValidationDisplayMode="ValidationDisplayMode.TabbableWithHint"
                               @bind-Value="_contactData.FirstName" HintText="The name you use on official documents" SvgIcon="--svg-user-icon" autocomplete="given-name"/>

                    <TextInput class="br-col-xs-12 br-col-sm-6" LabelText="Surname" Required="true" ErrorsLabel="errors" UpdateOnInput="true" TextInputType="TextInputType.Text" ValidationDisplayMode="ValidationDisplayMode.TabbableWithHint"
                               @bind-Value="_contactData.Surname" HintText="The surname you use on official documents" SvgIcon="--svg-user-icon" autocomplete="family-name" />
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
                <button class="br-col-xs-12 normal-button" type="submit">Fake submit to trigger validation on the model</button>
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
                var surnameValidator = MemberValidators.CreateStringRegexValidator(@"^[A-Z]+['\- ]?[A-Za-z]*['\- ]?[A-Za-z]*$", "Surname", "Surname", "Must start with a capital letter and no double spaces.")
                                .AndThen(MemberValidators.CreateStringLengthValidator(2, 25, "Surname", "Surname", "Must be between 2 and 25 characters in length but you entered {ActualLength} characters."));

                var ageValidator = MemberValidators.CreateRangeValidator<int>(16, 120, "Age", "Age", "Must be between 16 and 120");

                var salaryValidator = MemberValidators.CreatePrecisionScaleValidator<decimal>(50, 2, "HourlyRate", "Hourly Rate", "Can only contain a maximum of 2 decimal places")
                                .AndThen(MemberValidators.CreateRangeValidator<decimal>(10.00M, 200.00M, "HourlyRate", "Hourly Rate", $"Must be between {String.Format("{0:C}", 10)} and {String.Format("{0:C}", 100)}"));


                _boxedValidators = BlazorValidationBuilder<ContactDto>.Create()
                                        .ForMember(c => c.FirstName, firstNameValidator, true)
                                        .ForMember(c => c.Surname, surnameValidator, true)
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


    public const string Error_Summary_Example_Two = """
        
        <EditForm EditContext="@_editContext" OnValidSubmit="HandleSubmit">

            <BlazorValidated TEntity="ContactDto" BoxedValidators="_boxedValidators" AddDisplayName="true" DeferFieldValidation="true" />

            <InputErrorsSummary InputSuffix="Field" TitleHeadingLevel="TitleHeadingLevel.H3" SummaryDisplay="SummaryDisplay.OnModelValidated">

                <div class="br-input-row">
                    <TextInput class="br-col-xs-12 br-col-sm-6" LabelText="First Name" Required="true" ErrorsLabel="errors" UpdateOnInput="true" TextInputType="TextInputType.Text" ValidationDisplayMode="ValidationDisplayMode.TabbableWithHint"
                               @bind-Value="_contactDataTwo.FirstName" HintText="The name you use on official documents" SvgIcon="--svg-user-icon" autocomplete="given-name" />

                    <TextInput class="br-col-xs-12 br-col-sm-6" LabelText="Surname" Required="true" ErrorsLabel="errors" UpdateOnInput="true" TextInputType="TextInputType.Text" ValidationDisplayMode="ValidationDisplayMode.TabbableWithHint"
                               @bind-Value="_contactDataTwo.Surname" HintText="The surname you use on official documents" SvgIcon="--svg-user-icon" autocomplete="family-name" />
                </div>
                <div class="br-input-row">

                    <NumericInput class="br-col-xs-12 br-col-sm-6" @bind-Value="_contactDataTwo.Age" Required="true"
                                  LabelText="Age" ErrorsLabel="errors" HintText="Your current age in years." UpdateOnInput="true"
                                  ValidationDisplayMode="ValidationDisplayMode.TabbableWithHint" ParseErrorMessage="Please enter a valid number for this field type." />

                    <NumericInput class="br-col-xs-12 br-col-sm-6" @bind-Value="_contactDataTwo.HourlyRate" Required="false" Format="C"
                                  LabelText="Hourly Rate" ErrorsLabel="errors" HintText="Optional, how much you charge per hour." UpdateOnInput="true"
                                  ValidationDisplayMode="ValidationDisplayMode.TabbableWithHint" ParseErrorMessage="Please enter a valid number for this field type." maxlength="50" />

                </div>
            </InputErrorsSummary>

            <div style="display:flex;justify-content:space-between;flex-wrap:wrap;gap:1rem;">
                <button class="normal-button" type="submit">Fake submit to trigger validation on the model</button>
                <button @ref="ResetButton" class="normal-button" type="button" @onclick="ResetExampleTwo">Reset example</button>
            </div>
        </EditForm>

        @code {

            private ElementReference ResetButton { get; set; }

            private EditContext _editContext    = default!;
            private ContactDto  _contactDataTwo = new();

            private ImmutableDictionary<string, BoxedValidator> _boxedValidators = default!;

            protected override void OnInitialized()
            {

                _editContext = new EditContext(_contactDataTwo);

                var firstNameValidator = MemberValidators.CreateStringRegexValidator(@"^(?=.{2,55}$)[A-Z][A-Za-z]*(?:['\- ][A-Za-z]+)*$", "FirstName", "First Name", "Must start with a capital letter and be between 2 and 25 characters in length.");
                /*
                * Although its easy to create a regex that does everything I prefer to break things down into separate rules, so below creates a validator made up of two rules.
                */
                var surnameValidator = MemberValidators.CreateStringRegexValidator(@"^[A-Z]+['\- ]?[A-Za-z]*['\- ]?[A-Za-z]*$", "Surname", "Surname", "Must start with a capital letter and no double spaces.")
                    .AndThen(MemberValidators.CreateStringLengthValidator(2, 25, "Surname", "Surname", "Must be between 2 and 25 characters in length but you entered {ActualLength} characters."));

                var ageValidator = MemberValidators.CreateRangeValidator<int>(16, 120, "Age", "Age", "Must be between 16 and 120");

                var salaryValidator = MemberValidators.CreatePrecisionScaleValidator<decimal>(50, 2, "HourlyRate", "Hourly Rate", "Can only contain a maximum of 2 decimal places")
                                .AndThen(MemberValidators.CreateRangeValidator<decimal>(10.00M, 200.00M, "HourlyRate", "Hourly Rate", $"Must be between {String.Format("{0:C}", 10)} and {String.Format("{0:C}", 100)}"));


                _boxedValidators = BlazorValidationBuilder<ContactDto>.Create()
                                        .ForMember(c => c.FirstName, firstNameValidator, true)
                                        .ForMember(c => c.Surname, surnameValidator, true)
                                        .ForMember(c => c.Age, ageValidator)
                                        .ForNullableMember(c => c.HourlyRate, salaryValidator)
                                    .GetBoxedValidators();
            }


            private void HandleSubmit() { }

            private async Task ResetExampleTwo()
            {
                _contactDataTwo = new ContactDto();
                _editContext    = new EditContext(_contactDataTwo);
                await Task.Yield();
                await ResetButton.FocusAsync();
            }

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


    public const string Radio_Input_Group_Code_Example = """

        <EditForm Model="@_profileData">
            <BlazorValidated TEntity="ProfileDto" BoxedValidators="_boxedValidators" AddDisplayName="true" />
            <div class="br-input-row">
                <RadioInputGroup class=" br-col-xs-12 br-col-sm-6" LabelText="Favourite Food" HintText="What food do you like the most?" Required="true"
                                 @bind-Value="@_profileData.FavouriteFoodID" ValidationDisplayMode="ValidationDisplayMode.TabbableWithHint">
                <OptionValues>
                        <RadioInput LabelText="Fish & Chips" Value="1" />
                        <RadioInput LabelText="Burger & Fries" Value="2" />
                        <RadioInput LabelText="Pizza" Value="3" />
                </OptionValues>
                </RadioInputGroup>

            </div>
        </EditForm>

        @code {


        private ProfileDto _profileData = new();
        private ImmutableDictionary<string, BoxedValidator> _boxedValidators = default!;

        protected override void OnInitialized()
        {

            var foodValidator = MemberValidators.CreatePredicateValidator<int>(c => c == 1, "FavouriteFoodID", "Favourite Food", "Wrong answer, it has to be Fish & Chips");

            _boxedValidators = BlazorValidationBuilder<ProfileDto>.Create()
                                    .ForMember(c => c.FavouriteFoodID, foodValidator)
                                    .GetBoxedValidators();
        }

        public class ProfileDto
        {
            public int FavouriteFoodID { get; set; }
        }
        """;


    public const string Time_Input_Code_Example = """

            <EditForm EditContext="@_editContext" OnValidSubmit="HandleSubmit">

                <BlazorValidated TEntity="ScheduleDto" BoxedValidators="_boxedValidators" AddDisplayName="true" />

                <div class="br-input-row">
                    <TimeInput class="br-col-xs-12 br-col-sm-6" LabelText="Start Time" @bind-Value="@_scheduleData.StartTime" HintText="Required, the time (24-hour format) you want the scheduled action to start." 
                        Required="false" EnableSeconds="true" DataPosition="DataPosition.Start" UpdateOnInput="true" ParseErrorMessage="Must be a valid time."  
                        ValidationDisplayMode="ValidationDisplayMode.TabbableWithHint" />

                    <TimeInput class="br-col-xs-12 br-col-sm-6" LabelText="End Time" @bind-Value="@_scheduleData.EndTime" HintText="Required, the time (24-hour format) you want the scheduled action to finish"
                        Required="false" EnableSeconds="true" DataPosition="DataPosition.Start" UpdateOnInput="false" ParseErrorMessage="Must be a valid time."   
                        ValidationDisplayMode="ValidationDisplayMode.TabbableWithHint" />
                </div>
                 <div class="br-input-row">

                    <TimeInput class="br-col-xs-12 br-col-sm-6" LabelText="Start Work" @bind-Value="@_scheduleData.StartWork" HintText="Optional, the time (24-hour format) you start work." Required="false"
                               EnableSeconds="false" DataPosition="DataPosition.Start" UpdateOnInput="true"  ParseErrorMessage="Must be a valid time."  
                               ValidationDisplayMode="ValidationDisplayMode.TabbableWithHint" />

                    <TimeInput class="br-col-xs-12 br-col-sm-6" LabelText="End Work" @bind-Value="@_scheduleData.EndWork" HintText="Optional, the time (24-hour format) you finish work." Required="false"
                               EnableSeconds="false" DataPosition="DataPosition.Start" UpdateOnInput="false" ParseErrorMessage="Must be a valid time."  
                               ValidationDisplayMode="ValidationDisplayMode.TabbableWithHint" />
                </div>
                <div class="br-input-row">
                    <button class="br-col-xs-12 normal-button" type="submit">Fake Submit to trigger validation on unmodified fields</button>
                </div>
            </EditForm>

            @code {

            private EditContext _editContext = default!;
            private ScheduleDto _scheduleData = new();
            private ImmutableDictionary<string, BoxedValidator> _boxedValidators = default!;

            protected override void OnInitialized()
            {
                 var startTimeValidator = MemberValidators.CreateRangeValidator<TimeOnly>(new TimeOnly(7, 0, 0), new TimeOnly(10, 0, 0), "StartTime", "Start Time", "Must be between 7am and 10am.");
                 var endTimeValidator = MemberValidators.CreateMemberComparisonValidator<ScheduleDto, TimeOnly>(y => y.EndTime, x => x.StartTime, CompareType.GreaterThan, "End Time", "Must be greater than the start time.");
                 /*
                     * Different ways to do the same thing 
                 */
                 var startWorkValidator = MemberValidators.CreatePredicateValidator<TimeOnly>(x => x <= new TimeOnly(11, 0, 0), "StartWork", "Start Work", "If entered must be 11am or earlier.");
                 var endWorkValidator   = MemberValidators.CreateCompareToValidator<TimeOnly>(new TimeOnly(13, 0, 0),CompareType.GreaterThanOrEqual, "EndWork", "End Work", "If entered must be 1pm or later.");

                _editContext = new EditContext(_scheduleData);

                _boxedValidators = BlazorValidationBuilder<ScheduleDto>.Create()
                                        .ForMember(c => c.StartTime, startTimeValidator)
                                        .ForComparisonWithMember(c => c.EndTime, endTimeValidator)
                                        .ForNullableMember(c => c.StartWork, startWorkValidator) //Nullable in my library means optional
                                        .ForNullableMember(c => c.EndWork, endWorkValidator)
                                        .GetBoxedValidators();
            }

            private void HandleSubmit() { }

            public class ScheduleDto
            {
                public TimeOnly  StartTime { get; set; }
                public TimeOnly  EndTime   { get; set; }

                public TimeOnly? StartWork { get; set; } = null;
                public TimeOnly? EndWork   { get; set; } = null;
            }

        }
        
        """;


    public const string Date_Input_Code_Example = """

            <EditForm EditContext="@_editContext" OnValidSubmit="HandleSubmit">

            <BlazorValidated TEntity="ScheduleDto" BoxedValidators="_boxedValidators" AddDisplayName="true" />

            <div class="br-input-row">
                <DateInput class="br-col-xs-12 br-col-sm-6" LabelText="Start Date" @bind-Value="@_scheduleData.StartDate" HintText="Required, the date (numbers only) you want the scheduled action to start." 
                     Required="false" DataPosition="DataPosition.Start" UpdateOnInput="true" ParseErrorMessage="Must be a valid date."  
                     ValidationDisplayMode="ValidationDisplayMode.TabbableWithHint" />

                <DateInput class="br-col-xs-12 br-col-sm-6" LabelText="End Date" @bind-Value="@_scheduleData.EndDate" HintText="Required, the date (numbers only) you want the scheduled action to end"
                    Required="false" DataPosition="DataPosition.Start" UpdateOnInput="false" ParseErrorMessage="Must be a valid date."   
                    ValidationDisplayMode="ValidationDisplayMode.TabbableWithHint" />
            </div>
             <div class="br-input-row">

                <DateInput class="br-col-xs-12 br-col-sm-6" LabelText="Date Started" @bind-Value="@_scheduleData.DateStarted" HintText="Optional, the date you started the project (numbers only)." 
                        Required="false" DataPosition="DataPosition.Start" UpdateOnInput="true"  ParseErrorMessage="Must be a valid date."  
                           ValidationDisplayMode="ValidationDisplayMode.TabbableWithHint" />

                <DateInput class="br-col-xs-12 br-col-sm-6" LabelText="Date Completed" @bind-Value="@_scheduleData.DateCompleted" HintText="Optional, the date you completed the project (numbers only)." Required="false"
                           DataPosition="DataPosition.Start" UpdateOnInput="false" ParseErrorMessage="Must be a valid date."  
                           ValidationDisplayMode="ValidationDisplayMode.TabbableWithHint" />
            </div>
            <div class="br-input-row">
                <button class="br-col-xs-12 normal-button" type="submit">Fake Submit to trigger validation on unmodified fields</button>
            </div>
        </EditForm>

        @code {

            private EditContext _editContext = default!;
            private ScheduleDto _scheduleData = new();
            private ImmutableDictionary<string, BoxedValidator> _boxedValidators = default!;

            protected override void OnInitialized()
            {
                var startDateValidator = MemberValidators.CreateRangeValidator<DateOnly>(new DateOnly(2022, 2, 2), new DateOnly(2026, 6, 6), "StartTime", "Start Date", "Must be between 2022-02-02 and 2026-06-06.");
                var endDateValidator = MemberValidators.CreateMemberComparisonValidator<ScheduleDto, DateOnly>(y => y.EndDate, x => x.StartDate, CompareType.GreaterThan, "End Date", "Must be greater than the start date.");
                /*
                    * Different ways to do similar things. 
                 */
                var dateStartedValidator   = MemberValidators.CreatePredicateValidator<DateOnly>(x => x < new DateOnly(2026, 06, 06), "DateStarted", "Date Started", "If entered must be before to 2026-06-06.");
                var dateCompletedValidator = MemberValidators.CreateCompareToValidator<DateOnly>(new DateOnly(2026, 6, 6),CompareType.GreaterThan, "DateCompleted", "Date Completed", "If entered must be after 2026-06-06.");

                _editContext = new EditContext(_scheduleData);

                _boxedValidators = BlazorValidationBuilder<ScheduleDto>.Create()
                                         .ForMember(c => c.StartDate, startDateValidator)
                                         .ForComparisonWithMember(c => c.EndDate, endDateValidator)
                                         .ForNullableMember(c => c.DateStarted, dateStartedValidator) //Nullable in my library means optional
                                         .ForNullableMember(c => c.DateCompleted, dateCompletedValidator)
                                        .GetBoxedValidators();
            }

            private void HandleSubmit() { }

            public class ScheduleDto
            {
                public DateOnly  StartDate { get; set; }
                public DateOnly  EndDate   { get; set; }

                public DateOnly? DateStarted   { get; set; } = null;
                public DateOnly? DateCompleted { get; set; } = null;
            }
        }
        
        """;


    public const string TextArea_Input_Code_Example = """
        
        <EditForm EditContext="@_editContext"  OnValidSubmit="HandleSubmit">

            <BlazorValidated TEntity="ContactDto" BoxedValidators="_boxedValidators" AddDisplayName="true" />

            <div class="br-input-row">
                <TextAreaInput class="br-col-xs-12" LabelText="Notes" HintText="Tell us what happened (maximum of 40 characters)" @bind-Value="@_contactData.Notes" 
                DataPosition="DataPosition.Centre" MaxCharacters="40" ValidationDisplayMode="ValidationDisplayMode.DescribedByHintSuppressed" UpdateOnInput="false" 
                ReadOnly="false" AriaDisabled="false" TextAreaRows="5" AutoSize="true" />

            </div>
            <div style="display:flex; justify-content:space-between;">
                <button class="br-col-xs-12 normal-button" type="submit">Fake Submit to trigger validation on unmodified fields</button>
                <button class="br-col-xs-12 normal-button" type="button" @ref="ResetButtonRef" @onclick="ResetExample">Reset example</button>
            </div>
        </EditForm>

        
        @code {

            private ElementReference ResetButtonRef { get; set; }

            private EditContext _editContext = default!;
            private ContactDto  _contactData = new();

            private ImmutableDictionary<string, BoxedValidator> _boxedValidators = default!;

            protected override void OnInitialized()
            {
                _editContext = new EditContext(_contactData);

                var notesValidator = MemberValidators.CreateStringLengthValidator(1, 40, "Notes", "Notes", "Must contain text with a maximum of 40 characters.");

                _boxedValidators = BlazorValidationBuilder<ContactDto>.Create()
                                        .ForMember(c => c.Notes,notesValidator)
                                        .GetBoxedValidators();
            }

            private void HandleSubmit() { }

            private async Task ResetExample()
            {
                _contactData = new ContactDto();
                _editContext = new EditContext(_contactData);
                await Task.Yield();
                await ResetButtonRef.FocusAsync();

            }

            public class ContactDto
            {
                public string Notes { get; set; }
            }

        }
        """;

    public const string Select_Input_Code_Example = """

            <EditForm Model="@_profileData" OnSubmit="HandleSubmit">
            <BlazorValidated TEntity="ProfileDto" BoxedValidators="_boxedValidators" AddDisplayName="true" />
            <div class="br-input-row">
                <SelectInput  class=" br-col-xs-12 br-col-sm-6" LabelText="Favourite Food" HintText="What food do you like the most?" Required="true" 
                    @bind-Value="@_profileData.FavouriteFoodID" SvgIcon="--svg-food-icon">
                     <OptionValues>
                         <option value="0">Please select your favourite food</option>
                        @foreach(var item in _foodListItems)
                        {
                            <option value="@item.Key" >@item.Value</option>
                        }
                    </OptionValues>
                </SelectInput>
            </div>
            <div class="br-input-row">
                <SelectInput class=" br-col-xs-12 br-col-sm-6" LabelText="Favourite Food" HintText="What food do you like the most?" Required="true" ReadOnly="true"
                             @bind-Value="@_profileData.FavouriteFoodID" SvgIcon="--svg-food-icon">
                    <OptionValues>
                        <option value="0">Please select your favourite food</option>
                        @foreach (var item in _foodListItems)
                        {
                            <option value="@item.Key">@item.Value</option>
                        }
                    </OptionValues>
                </SelectInput>
            </div>
            <div class="br-input-row">
                <SelectInput class=" br-col-xs-12 br-col-sm-6" LabelText="Favourite Food" HintText="What food do you like the most?" Required="true" AriaDisabled="true"
                             @bind-Value="@_profileData.FavouriteFoodID" SvgIcon="--svg-food-icon">
                    <OptionValues>
                        <option value="0">Please select your favourite food</option>
                        @foreach (var item in _foodListItems)
                        {
                            <option value="@item.Key">@item.Value</option>
                        }
                    </OptionValues>
                </SelectInput>
            </div>
        </EditForm>
            
        @code {


            private ProfileDto _profileData               = new();
            private Dictionary<int,string> _foodListItems = new(){[1]="Fish & Chips", [2]="Burger & Fries", [3]="Pizza"};

            private ImmutableDictionary<string, BoxedValidator> _boxedValidators = default!;

            protected override void OnInitialized()
            {
                var foodValidator = MemberValidators.CreatePredicateValidator<int>(c => c == 1, "FavouriteFoodID", "Favourite Food", "Wrong answer, it has to be Fish & Chips");

                _boxedValidators = BlazorValidationBuilder<ProfileDto>.Create()
                                        .ForMember(c => c.FavouriteFoodID, foodValidator)
                                        .GetBoxedValidators();
            }

            private void HandleSubmit() { }

            public class ProfileDto
            {
                public int FavouriteFoodID { get; set; }
            }

        }
        

        """;
        
        
}

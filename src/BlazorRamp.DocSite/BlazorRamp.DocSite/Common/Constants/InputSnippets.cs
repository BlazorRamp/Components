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
        <EditForm Model="@_contactData">

            <DataAnnotationsValidator />

            <div class="br-input-row">
                <TextInput class="br-col-xs-12 br-col-sm-6" LabelText="First name:" Required = "true" ErrorsLabel="errors" UpdateOnInput="true" 
                            TextInputType="TextInputType.Text" ValidationDisplayMode="ValidationDisplayMode.TabbableWithHint" 
                                @bind-Value="_contactData.FirstName" HintText="The name you use on official documents" />

                <TextInput class="br-col-xs-12 br-col-sm-6" LabelText="Surname:" Required="true" UpdateOnInput="true" 
                            TextInputType="TextInputType.Text" ValidationDisplayMode="ValidationDisplayMode.DescribedByWithHint"
                                @bind-Value="_contactData.Surname" HintText="The surname you use on official documents" />
            </div>

        </EditForm>

        @code {

            private ContactDto _contactData = new();

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

            protected override async Task OnInitializedAsync()
            {

                var unitValidator   = MemberValidators.CreateRangeValidator<int>(1, 999, "UnitsInStock", "Units in stock", "Must be between 1 and 999");

                var priceValidation = MemberValidators.CreatePrecisionScaleValidator<decimal>(5, 2, "UnitPrice", "Unit price", "Cannot be more than than one hundred thousand with a maximum of 2 decimal places")
                                        .AndThen(MemberValidators.CreatePredicateValidator<decimal>(c => c > 1M, "UnitPrice", "Unit Price", "Must be greater than " + String.Format("{0:C}", 1)));

                _boxedValidators = BlazorValidationBuilder<InventoryDto>.Create()
                                        .ForMember(c => c.UnitsInStock, unitValidator)
                                        .ForMember(c => c.UnitPrice, priceValidation)
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

}

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
}

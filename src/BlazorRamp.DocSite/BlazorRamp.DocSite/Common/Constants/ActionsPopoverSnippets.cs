namespace BlazorRamp.DocSite.Common.Constants;

public class ActionsPopoverSnippets
{
    public const string Add_Actions_Popover_Style_Sheet = """
        <head>
            <link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
            <link rel="stylesheet" href="_content/BlazorRamp.ActionsPopover/assets/css/actions-popover.min.css" />
        </head>
        """;


    public const string Usage_Example_Row_Actions = """
        <h2 id="table-title">Contacts table (Northwind lives on)</h2>

        <div class="data-table">
            <table aria-labelledby="table-title">
                <thead>
                    <tr>
                        <th scope="col">Contact ID</th>
                        <th scope="col">Given Name</th>
                        <th scope="col">Family Name</th>
                        <th scope="col">Job Title</th>
                        <th scope="col">Actions</th>
                    </tr>
                </thead>
                <tbody>
                    @foreach (var contact in _contacts)
                    {
                        <tr>
                            <td>@contact.ContactID</td>
                            <td>@contact.GivenName</td>
                            <td>@contact.FamilyName</td>
                            <td>@contact.JobTitle</td>
                            <td style="width:1%;white-space:nowrap;">                            
                                <ActionsPopover TriggerText="@($"For: {contact.GivenName} {contact.FamilyName}")" Stretch="true" ActionsPopoverPosition="ActionsPopoverPosition.BottomRight">
                                    <PopoverItems>
                                        <ActionPopoverButton TData="Contact" ButtonText="Edit" SvgIcon="--svg-pencil-icon" OnClick="HandleOnEdit" ItemData="@contact"/>
                                         <ActionPopoverButton TData="Contact" ButtonText="Delete" SvgIcon="--svg-trash-can-icon" OnClick="HandleOnDelete" ItemData="@contact" />
                                    </PopoverItems>
                                </ActionsPopover>
                            </td>
                        </tr>
                    }
                </tbody>
            </table>
        </div>
        <div style="margin-top:var(--br-unit-space-5);">
            <p> @_output</p>
        </div>
                
        @code {
            [Inject] ILiveRegionService LiveRegionService { get; set; } = default!;

            private string _output = String.Empty;

            private Contact[] _contacts = new[]
            {
                new Contact(1, "Nancy", "Davolio", "Sales Representative"),
                new Contact(2, "Andrew", "Fuller", "Vice President, Sales"),
                new Contact(3, "Janet", "Leverling", "Sales Representative"),
                new Contact(4, "Margaret", "Peacock", "Sales Representative"),
                new Contact(5, "Steven", "Buchanan", "Sales Manager")
            };

            private async Task HandleOnEdit(ButtonActionData<Contact> itemData)
            {
                var data = itemData.GetValueOr(null);

                _output = $"You activated the {itemData.ButtonText} option that contained the payload value of {(data is null ? "[null]" : data.ToString())}";
                await LiveRegionService.MakeAnnouncement(new Announcement(_output, AnnouncementType.Info, $"My Actions - {itemData.ButtonText}"), true);
                await InvokeAsync(StateHasChanged);
            }
            private async Task HandleOnDelete(ButtonActionData<Contact> itemData)
            {
                var data = itemData.GetValueOr(null);
                _output = $"You activated the {itemData.ButtonText} option that contained the payload value of {(data is null ? "[null]" : data.ToString())}";
                await LiveRegionService.MakeAnnouncement(new Announcement(_output, AnnouncementType.Info, $"My Actions - {itemData.ButtonText}"), true);
                await InvokeAsync(StateHasChanged);
            }

            private record Contact(int ContactID, string GivenName, string FamilyName, string JobTitle);

        }

        """;
}

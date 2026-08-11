namespace BlazorRamp.DocSite.Common.Constants;

public class DataTableSnippets
{
    public const string Add_Data_Table_Style_Sheets = """
        <head>
            <link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
            <link rel="stylesheet" href="_content/BlazorRamp.DataTable/assets/css/data-table.min.css" />
            <link rel="stylesheet" href="_content/BlazorRamp.BusyIndicator/assets/css/busy-indicator.min.css" />
        </head>
        """;


    public const string Basic_Usage_Example = """

        <DataTable TData="Contact" DataSource="@_dataSource" Title="Contact Results" PagerBinding="@_pagerBinding" FilterRule="_filterRule">
            <Filter>
                <DebounceFilter HintText="Filters across first name, surname and country" RegexPattern="^[A-Za-z ]*$" OnDebounceFilterResult="HandleOnDebounce"
                                ValidationMessage="Invalid filter, filtering paused, letters and spaces only"  />
            </Filter>
            <TableColumns>
                <DataColumn DataProperty="c => c.ContactID"   DisplayName="ID"            IsSortable="true" />
                <DataColumn DataProperty="c => c.GivenName"   DisplayName="First Name"    IsSortable="true" />
                <DataColumn DataProperty="c => c.FamilyName"  DisplayName="Surname"       IsSortable="true" />
                <DataColumn DataProperty="c => c.Country"     DisplayName="Country"       IsSortable="true" />
                <DataColumn DataProperty="c => c.DateOfBirth" DisplayName="Data of Birth" ColumnAlignment="ColumnAlignment.Centre" IsSortable="true" />
                <DataColumn DataProperty="c => c.Rate" DisplayName="Hourly Rate" IsSortable="true" CellFormat="C" ColumnAlignment="ColumnAlignment.End" />
            </TableColumns>
            <BottomPager>
                <Pager @bind-CurrentPage="@_pagerBinding.CurrentPage" AriaLabel="Contacts pager" TotalItemCount="@_pagerBinding.TotalItemCount" 
                CurrentItemCount="@_pagerBinding.CurrentItemCount" ItemsPerPage="@_pagerBinding.ItemsPerPage" PagerSelectorType="PagerSelectorType.Button" 
                ShowFirstLast="true" PageAlignment="PageAlignment.End" />
            </BottomPager>
        </DataTable>

        @code{

            private Func<Contact, bool>? _filterRule   = null;
            private PagerBinding         _pagerBinding = new(itemsPerPage: 10);
            private List<Contact>        _dataSource   = [];

            protected override void OnInitialized()

                => _dataSource = StaticData.GetContacts(100_000);

            private async Task HandleOnDebounce(DebouncedFilterResult debounceResult)
            {
                if (debounceResult.IsValid)
                {
                    _filterRule = (data) => data.GivenName.Contains(debounceResult.FilterValue, StringComparison.OrdinalIgnoreCase)
                                            || data.FamilyName.Contains(debounceResult.FilterValue, StringComparison.OrdinalIgnoreCase)
                                            || data.Country.ToString().Contains(debounceResult.FilterValue, StringComparison.OrdinalIgnoreCase);

                /*
                    * The debounce uses a Func not an EventCallback to negate renders if the filter value is invalid so
                    * you call StateHasChanged if all is well. 
                */
                await InvokeAsync(StateHasChanged);
                }
            }
        }
        """;



    public const string Virtualize_Usage_Example = """

        <DataTable TData="Contact" DataSource="@_dataSource" Title="Contact Results" FilterRule="_filterRule">
            <Filter>
                <DebounceFilter HintText="Filters across first name, surname and country" RegexPattern="^[A-Za-z ]*$" OnDebounceFilterResult="HandleOnDebounce"
                                ValidationMessage="Invalid filter, filtering paused, letters and spaces only" />
            </Filter>
            <TableColumns>
                <DataColumn DataProperty="c => c.ContactID"   DisplayName="ID"         IsSortable="true" />
                <DataColumn DataProperty="c => c.GivenName"   DisplayName="First Name" IsSortable="true" />
                <DataColumn DataProperty="c => c.FamilyName"  DisplayName="Surname"    IsSortable="true" />
                <DataColumn DataProperty="c => c.Country"     DisplayName="Country"    IsSortable="true" />
                <DataColumn DataProperty="c => c.DateOfBirth" DisplayName="Data of Birth" ColumnAlignment="ColumnAlignment.Centre" IsSortable="true" />
                <DataColumn DataProperty="c => c.Rate" DisplayName="Hourly Rate" IsSortable="true" CellFormat="C" ColumnAlignment="ColumnAlignment.End" />
            </TableColumns>
        </DataTable>

        @code {

            private Func<Contact, bool>? _filterRule = null;
            private List<Contact>        _dataSource = [];

            protected override void OnInitialized()

                => _dataSource = StaticData.GetContacts(100_000);

            private async Task HandleOnDebounce(DebouncedFilterResult debounceResult)
            {
                if (debounceResult.IsValid)
                {
                    _filterRule = (data) => data.GivenName.Contains(debounceResult.FilterValue, StringComparison.OrdinalIgnoreCase)
                                            || data.FamilyName.Contains(debounceResult.FilterValue, StringComparison.OrdinalIgnoreCase)
                                            || data.Country.ToString().Contains(debounceResult.FilterValue, StringComparison.OrdinalIgnoreCase);

                    /*
                        * The debounce uses a Func not an EventCallback to negate renders if the filter value is invalid so
                        * you call StateHasChanged if all is well.
                    */
                    await InvokeAsync(StateHasChanged);
                }
            }
        }
        
        """;

    public const string Paging_Usage_Example = """

        <DataTable TData="Contact" DataSource="@_dataSource" Title="Contact Results" PagerBinding="@_pagerBinding">
            <TopPager>
                <Pager @bind-CurrentPage="@_pagerBinding.CurrentPage" AriaLabel="Contacts top pager" TotalItemCount="@_pagerBinding.TotalItemCount" CurrentItemCount="@_pagerBinding.CurrentItemCount"
                       ItemsPerPage="@_pagerBinding.ItemsPerPage" PagerSelectorType="PagerSelectorType.Button" ShowFirstLast="false" PageAlignment="PageAlignment.End" 
                       PagerAnnouncementType="PagerAnnouncementType.WithoutAnnouncement" />
            </TopPager>
            <TableColumns>
                <DataColumn DataProperty="c => c.ContactID"   DisplayName="ID"         IsSortable="true" />
                <DataColumn DataProperty="c => c.GivenName"   DisplayName="First Name" IsSortable="true" />
                <DataColumn DataProperty="c => c.FamilyName"  DisplayName="Surname"    IsSortable="true" />
                <DataColumn DataProperty="c => c.Country"     DisplayName="Country"    IsSortable="true" />
                <DataColumn DataProperty="c => c.DateOfBirth" DisplayName="Data of Birth" ColumnAlignment="ColumnAlignment.Centre" IsSortable="true" />
                <DataColumn DataProperty="c => c.Rate" DisplayName="Hourly Rate" IsSortable="true" CellFormat="C" ColumnAlignment="ColumnAlignment.End" />
            </TableColumns>
            <BottomPager>
                <Pager @bind-CurrentPage="@_pagerBinding.CurrentPage" AriaLabel="Contacts bottom pager" TotalItemCount="@_pagerBinding.TotalItemCount" CurrentItemCount="@_pagerBinding.CurrentItemCount"
                       ItemsPerPage="@_pagerBinding.ItemsPerPage" PagerSelectorType="PagerSelectorType.Button" ShowFirstLast="false" PageAlignment="PageAlignment.Start" 
                       PagerAnnouncementType="PagerAnnouncementType.WithAnnouncement" />
            </BottomPager>
        </DataTable>

        @code{

            private PagerBinding   _pagerBinding = new(itemsPerPage: 25);
            private List<Contact>  _dataSource   = [];

            protected override void OnInitialized()

                => _dataSource = StaticData.GetContacts(100);

        }

        """;

    public const string Filtering_Usage_Example = """

        <DataTable TData="Contact" DataSource="@_dataSource" Title="Contact Results" PagerBinding="@_pagerBinding" FilterRule="_filterRule"
            FilterAlignment="FilterAlignment.Start" TitleAlignment="TitleAlignment.Centre">
            <Filter>
                <DebounceFilter HintText="Filters across all columns" RegexPattern="^[A-Za-z0-9 ]*$" OnDebounceFilterResult="HandleOnDebounce"
                                ValidationMessage="Invalid filter, filtering paused, letters, numbers and spaces only"  />
            </Filter>
            <TableColumns>
                <DataColumn DataProperty="c => c.ContactID"   DisplayName="ID"            IsSortable="true" />
                <DataColumn DataProperty="c => c.GivenName"   DisplayName="First Name"    IsSortable="true" />
                <DataColumn DataProperty="c => c.FamilyName"  DisplayName="Surname"       IsSortable="true" />
                <DataColumn DataProperty="c => c.Country"     DisplayName="Country"       IsSortable="true" />
                <DataColumn DataProperty="c => c.DateOfBirth" DisplayName="Date of Birth" ColumnAlignment="ColumnAlignment.Centre" IsSortable="true" />
                <DataColumn DataProperty="c => c.Rate" DisplayName="Hourly Rate" IsSortable="true" CellFormat="C" ColumnAlignment="ColumnAlignment.End" />
            </TableColumns>
            <BottomPager>
                <Pager @bind-CurrentPage="@_pagerBinding.CurrentPage" AriaLabel="Contacts pager" TotalItemCount="@_pagerBinding.TotalItemCount" CurrentItemCount="@_pagerBinding.CurrentItemCount"
                ItemsPerPage="@_pagerBinding.ItemsPerPage" PagerSelectorType="PagerSelectorType.Button" ShowFirstLast="true" PageAlignment="PageAlignment.End" AddApplicationRole="true" />
            </BottomPager>
        </DataTable>

         @code {

            private Func<Contact, bool>? _filterRule = null;
            private PagerBinding _pagerBinding = new(itemsPerPage: 10);
            private List<Contact> _dataSource = [];

            protected override void OnInitialized()

                => _dataSource = StaticData.GetContacts(10_000);

            private async Task HandleOnDebounce(DebouncedFilterResult debounceResult)
            {
                if (debounceResult.IsValid)
                {
                    _filterRule = (data) => data.ContactID.ToString().Contains(debounceResult.FilterValue, StringComparison.OrdinalIgnoreCase)
                                            || data.GivenName.Contains(debounceResult.FilterValue, StringComparison.OrdinalIgnoreCase)
                                            || data.FamilyName.Contains(debounceResult.FilterValue, StringComparison.OrdinalIgnoreCase)
                                            || data.Country.ToString().Contains(debounceResult.FilterValue, StringComparison.OrdinalIgnoreCase)
                                            || data.DateOfBirth.ToString().Contains(debounceResult.FilterValue, StringComparison.OrdinalIgnoreCase)
                                            || data.Rate.ToString().Contains(debounceResult.FilterValue, StringComparison.OrdinalIgnoreCase);
                    /*
                        * The debounce uses a Func not an EventCallback to negate renders if the filter value is invalid so
                        * you call StateHasChanged if all is well.
                    */
                    await InvokeAsync(StateHasChanged);
                }
            }
        }
        """;


    public const string Selection_Usage_Example = """

        <DataTable TData="Contact" DataSource="@_dataSource" Title="Contact Results" PagerBinding="@_pagerBinding" FilterRule="_filterRule" 
                   RowSelectionMode="RowSelectionMode.Multiple" RowSelectHeading="Pick" SelectedRowsChanged="HandleSectionChanged" RowIdentifierFunc="@_checkboxLabel">
            <Filter>
                <DebounceFilter HintText="Filters across first name, surname and country" RegexPattern="^[A-Za-z ]*$" OnDebounceFilterResult="HandleOnDebounce"
                                ValidationMessage="Invalid filter, filtering paused, letters and spaces only" />
            </Filter>
            <TableColumns>
                <DataColumn DataProperty="c => c.ContactID"   DisplayName="ID"         IsSortable="true" />
                <DataColumn DataProperty="c => c.GivenName"   DisplayName="First Name" IsSortable="true" />
                <DataColumn DataProperty="c => c.FamilyName"  DisplayName="Surname"    IsSortable="true" />
                <DataColumn DataProperty="c => c.Country"     DisplayName="Country"    IsSortable="true" />
                <DataColumn DataProperty="c => c.DateOfBirth" DisplayName="Date of Birth" ColumnAlignment="ColumnAlignment.Centre" IsSortable="true" />
                <DataColumn DataProperty="c => c.Rate"        DisplayName="Hourly Rate" IsSortable="true" CellFormat="C" ColumnAlignment="ColumnAlignment.End" />
            </TableColumns>
            <BottomPager>
                <Pager @bind-CurrentPage="@_pagerBinding.CurrentPage" AriaLabel="Contacts pager" TotalItemCount="@_pagerBinding.TotalItemCount" CurrentItemCount="@_pagerBinding.CurrentItemCount"
                       ItemsPerPage="@_pagerBinding.ItemsPerPage" PagerSelectorType="PagerSelectorType.Button" ShowFirstLast="true" PageAlignment="PageAlignment.End" AddApplicationRole="true" />
            </BottomPager>
        </DataTable>

        <p>
            Selected row(s):@if (_selectedRows.Count == 0) {<text>None selected;</text>}

            @if (_selectedRows.Count > 0)
            {
                <ul>
                    @foreach (var row in _selectedRows)
                    {
                        <li>@row.ToString()</li>
                    }
                </ul>
            }
        </p>
        
        @code {

            private Func<Contact, string> _checkboxLabel = c => $"For {c.GivenName} {c.FamilyName}, ID {c.ContactID}";
            private Func<Contact, bool>? _filterRule     = null;
            private PagerBinding         _pagerBinding   = new(itemsPerPage: 10);
            private List<Contact>        _dataSource     = [];
            private List<Contact>        _selectedRows   = [];

            protected override void OnInitialized()

                => _dataSource = StaticData.GetContacts(10_000);

            private async Task HandleOnDebounce(DebouncedFilterResult debounceResult)
            {
                if (debounceResult.IsValid)
                {
                    _filterRule = (data) => data.GivenName.Contains(debounceResult.FilterValue, StringComparison.OrdinalIgnoreCase)
                                            || data.FamilyName.Contains(debounceResult.FilterValue, StringComparison.OrdinalIgnoreCase)
                                            || data.Country.ToString().Contains(debounceResult.FilterValue, StringComparison.OrdinalIgnoreCase);

                    /*
                    * The debounce uses a Func not an EventCallback to negate renders if the filter value is invalid so
                    * you call StateHasChanged if all is well.
                    */
                    await InvokeAsync(StateHasChanged);
                }
            }

            private async Task HandleSectionChanged(List<Contact> selectedRows)

                => _selectedRows = selectedRows;

        }
        
        """;


    public const string Sorting_Usage_Example = """

         <DataTable TData="Contact" DataSource="@_dataSource" Title="Contact Results" FilterRule="_filterRule" >
            <Filter>
                <DebounceFilter HintText="Filters across first name, surname and country" RegexPattern="^[A-Za-z ]*$" OnDebounceFilterResult="HandleOnDebounce"
                                ValidationMessage="Invalid filter, filtering paused, letters and spaces only" />
            </Filter>
            <TableColumns>
                <DataColumn DataProperty="c => c.ContactID"   DisplayName="ID" />
                <DataColumn DataProperty="c => c.GivenName"   DisplayName="First Name" IsSortable="true" />
                <DataColumn DataProperty="c => c.FamilyName"  DisplayName="Surname"    IsSortable="true" />
                <DataColumn DataProperty="c => c.Country"     DisplayName="Country" />
                <DataColumn DataProperty="c => c.DateOfBirth" DisplayName="Date of Birth" ColumnAlignment="ColumnAlignment.Centre" />
                <DataColumn DataProperty="c => c.Rate"        DisplayName="Hourly Rate" CellFormat="C" ColumnAlignment="ColumnAlignment.End" />
            </TableColumns>
        </DataTable>

        @code {

            private Func<Contact, bool>? _filterRule = null;
            private List<Contact>        _dataSource = [];

            protected override void OnInitialized()

                => _dataSource = StaticData.GetContacts(10_000);

            private async Task HandleOnDebounce(DebouncedFilterResult debounceResult)
            {
                if (debounceResult.IsValid)
                {
                    _filterRule = (data) => data.GivenName.Contains(debounceResult.FilterValue, StringComparison.OrdinalIgnoreCase)
                                            || data.FamilyName.Contains(debounceResult.FilterValue, StringComparison.OrdinalIgnoreCase)
                                            || data.Country.ToString().Contains(debounceResult.FilterValue, StringComparison.OrdinalIgnoreCase);

                    /*
                        * The debounce uses a Func not an EventCallback to negate renders if the filter value is invalid so
                        * you call StateHasChanged if all is well.
                    */
                    await InvokeAsync(StateHasChanged);
                }
            }
        }
        """;

    public const string Styling_Usage_Example = """
        <DataTable TData="Contact" DataSource="@_dataSource" Title="Contact Results" PagerBinding="@_pagerBinding" FilterRule="_filterRule" RowStyleFunc=UKAvailability>
            <Filter>
                <DebounceFilter HintText="Filters across first name, surname and country" RegexPattern="^[A-Za-z ]*$" OnDebounceFilterResult="HandleOnDebounce"
                                ValidationMessage="Invalid filter, filtering paused, letters and spaces only" />
            </Filter>
            <TableColumns>
                <DataColumn DataProperty="c => c.ContactID"   DisplayName="ID"            IsSortable="true" />
                <DataColumn DataProperty="c => c.GivenName"   DisplayName="First Name"    IsSortable="true" />
                <DataColumn DataProperty="c => c.FamilyName"  DisplayName="Surname"       IsSortable="true" />
                <DataColumn DataProperty="c => c.Country"     DisplayName="Country"       IsSortable="true" CellStyle="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;" HeaderStyle="min-width:16ch;" /> 
                <DataColumn DataProperty="c => c.DateOfBirth" DisplayName="Date of Birth" ColumnAlignment="ColumnAlignment.Centre" IsSortable="true" CellFormat="yyyy-MM-dd" HeaderStyle="min-width:16ch;" />
                <DataColumn DataProperty="c => c.Rate"        DisplayName="Hourly Rate"   IsSortable="true" CellFormat="C" ColumnAlignment="ColumnAlignment.End" />
                <DataColumn DataProperty="c => c.Availability" IsSortable="true" ColumnAlignment="ColumnAlignment.Centre" 
                    CellStyleFunc='r => r.Availability == "Unavailable" ? "font-weight:bold; color:red;" : String.Empty' />
            </TableColumns>
            <BottomPager>
                <Pager @bind-CurrentPage="@_pagerBinding.CurrentPage" AriaLabel="Contacts pager" TotalItemCount="@_pagerBinding.TotalItemCount" CurrentItemCount="@_pagerBinding.CurrentItemCount"
                       ItemsPerPage="@_pagerBinding.ItemsPerPage" PagerSelectorType="PagerSelectorType.Button" ShowFirstLast="true" PageAlignment="PageAlignment.End" AddApplicationRole="true" />
            </BottomPager>
        </DataTable>
        
        @code {

            private Func<Contact, bool>? _filterRule = null;
            private PagerBinding _pagerBinding = new(itemsPerPage: 10);
            private List<Contact> _dataSource = [];

            private Func<Contact, string?>? UKAvailability = c => c.Availability == "Available" && c.Country == "United Kingdom" ? "background-color:var(--br-unit-colour-success); color:white;" : null;

            protected override void OnInitialized()

                => _dataSource = StaticData.GetContacts(10_000);

            private async Task HandleOnDebounce(DebouncedFilterResult debounceResult)
            {
                if (debounceResult.IsValid)
                {
                    _filterRule = (data) => data.GivenName.Contains(debounceResult.FilterValue, StringComparison.OrdinalIgnoreCase)
                                            || data.FamilyName.Contains(debounceResult.FilterValue, StringComparison.OrdinalIgnoreCase)
                                            || data.Country.ToString().Contains(debounceResult.FilterValue, StringComparison.OrdinalIgnoreCase);

                    /*
                        * The debounce uses a Func not an EventCallback to negate renders if the filter value is invalid so
                        * you call StateHasChanged if all is well.
                    */
                    await InvokeAsync(StateHasChanged);
                }
            }
        }
        """;
    
    public const string Templating_Usage_Example = """
    
        <style>
            /*
                * The actions popover uses focus visible which is not triggered programmatically only by a keyobard, so we need to add a bit of style.
                * just for this page so we see the indicator on focus. Or you can create a class and add it ot its class list and remove it on lost focus etc.
            */
            .br-actions-popover__trigger:focus {
                border-radius:  var(--br-unit-radius-2);
                outline-offset: var(--br-comp-all-outline-offset);
                outline-width:  calc(var(--br-comp-all-outline-width));
                outline-style:  var(--br-comp-all-outline-style);
                outline-color:  var(--br-comp-all-outline-colour);
            }

            .united-kingdom-flag-icon {
                display: inline-block;
                min-height:3em;
                min-width: 3em;
                background-repeat: no-repeat;
                background-size: 100% 100%;
                background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 36 36'%3E%3Cpath fill='%2300247d' d='M0 9.059V13h5.628zM4.664 31H13v-5.837zM23 25.164V31h8.335zM0 23v3.941L5.63 23zM31.337 5H23v5.837zM36 26.942V23h-5.631zM36 13V9.059L30.371 13zM13 5H4.664L13 10.837z'/%3E%3Cpath fill='%23cf1b2b' d='m25.14 23l9.712 6.801a4 4 0 0 0 .99-1.749L28.627 23zM13 23h-2.141l-9.711 6.8c.521.53 1.189.909 1.938 1.085L13 23.943zm10-10h2.141l9.711-6.8a4 4 0 0 0-1.937-1.085L23 12.057zm-12.141 0L1.148 6.2a4 4 0 0 0-.991 1.749L7.372 13z'/%3E%3Cpath fill='%23eee' d='M36 21H21v10h2v-5.836L31.335 31H32a4 4 0 0 0 2.852-1.199L25.14 23h3.487l7.215 5.052c.093-.337.158-.686.158-1.052v-.058L30.369 23H36zM0 21v2h5.63L0 26.941V27c0 1.091.439 2.078 1.148 2.8l9.711-6.8H13v.943l-9.914 6.941c.294.07.598.116.914.116h.664L13 25.163V31h2V21zM36 9a3.98 3.98 0 0 0-1.148-2.8L25.141 13H23v-.943l9.915-6.942A4 4 0 0 0 32 5h-.663L23 10.837V5h-2v10h15v-2h-5.629L36 9.059zM13 5v5.837L4.664 5H4a4 4 0 0 0-2.852 1.2l9.711 6.8H7.372L.157 7.949A4 4 0 0 0 0 9v.059L5.628 13H0v2h15V5z'/%3E%3Cpath fill='%23cf1b2b' d='M21 15V5h-6v10H0v6h15v10h6V21h15v-6z'/%3E%3C/svg%3E");
                forced-color-adjust: none;
            }
            .uk-data{

                display:flex;
                gap:0.5rem;
                align-items: center;
                white-space: nowrap;
                > a{
                    display:flex;
                    flex-direction:column;
                    color:inherit;
                }
            }
        </style>
        <DataTable @ref="DataTableRef" TData="Contact" DataSource="@_dataSource" Title="Contact Results" PagerBinding="@_pagerBinding" FilterRule="_filterRule" TableHeight="800px;">
            <Filter>
                <DebounceFilter HintText="Filters across first name, surname and country" RegexPattern="^[A-Za-z ]*$" OnDebounceFilterResult="HandleOnDebounce"
                                ValidationMessage="Invalid filter, filtering paused, letters and spaces only" />
            </Filter>
            <TableColumns>
                <DataColumn DataProperty="c => c.ContactID"  DisplayName="ID" IsSortable="true" />
                <DataColumn DataProperty="c => c.GivenName"  DisplayName="First Name" IsSortable="true" />
                <DataColumn DataProperty="c => c.FamilyName" DisplayName="Surname" IsSortable="true" />
                <DataColumn DataProperty="c => c.Country"    DisplayName="Country" IsSortable="true" HeaderStyle="min-width:30ch;" >

                    <CellTemplate Context="rowItem">

                        @if(rowItem.Country == "United Kingdom")
                        {
                            <div class="uk-data">
                                <span aria-hidden="true" class="united-kingdom-flag-icon"></span>
                                <a href="@($"https://en.wikipedia.org/wiki/{rowItem.Country}")" target="_blank" rel="noopener noreferrer">
                                    <span>@rowItem.Country</span>
                                    <span>(opens in new window)</span>
                                </a>
                            </div>
                            return;
                        }
                        @rowItem.Country

                    </CellTemplate>
                </DataColumn>

                <DataColumn DataProperty="c => c.DateOfBirth" DisplayName="Date of Birth" ColumnAlignment="ColumnAlignment.Centre" IsSortable="true" />
                <DataColumn DataProperty="c => c.Rate"        DisplayName="Hourly Rate" IsSortable="true" CellFormat="C" ColumnAlignment="ColumnAlignment.End" />

                <TemplateColumn DisplayName="Actions" ColumnAlignment="ColumnAlignment.Centre" CellStyle="white-space: nowrap;">
                    <HeaderTemplate Context="displayName">
                        @displayName
                    </HeaderTemplate>
                    <CellTemplate Context="rowItem">
                        @{
                            var identifier = CreateIdentifier(rowItem);
                        }
                        <ActionsPopover TriggerText="@identifier" Stretch="true" ActionsPopoverPosition="ActionsPopoverPosition.BottomRight" id="@identifier">
                            <PopoverItems>
                                <ActionPopoverButton TData="Contact" ButtonText="View" SvgIcon="--svg-overview-icon" ItemData="@rowItem" onclick="HandleRowView" />
                                <ActionPopoverButton TData="Contact" ButtonText="Delete" SvgIcon="--svg-trash-can-icon"  ItemData="@rowItem" OnClick="HandleRowDelete"/>
                            </PopoverItems>
                        </ActionsPopover>
                    </CellTemplate>
                </TemplateColumn>

            </TableColumns>
            <BottomPager>
                <Pager @bind-CurrentPage="@_pagerBinding.CurrentPage" AriaLabel="Contacts pager" TotalItemCount="@_pagerBinding.TotalItemCount" CurrentItemCount="@_pagerBinding.CurrentItemCount"
                       ItemsPerPage="@_pagerBinding.ItemsPerPage" PagerSelectorType="PagerSelectorType.Button" ShowFirstLast="true" PageAlignment="PageAlignment.End" AddApplicationRole="true" />
            </BottomPager>
        </DataTable>

        <p style="margin-top:var(--br-unit-space-9);">
            <b>Last viewed row: </b>@_dataRowDetails
        </p>

                
        @code {

            @implements IAsyncDisposable

            [Inject] IJSRuntime        JSRuntime    { get; set; }
            private DataTable<Contact> DataTableRef { get; set; }

            private IJSObjectReference?  _jsModule      = null;
            private Func<Contact, bool>? _filterRule    = null;
            private PagerBinding         _pagerBinding  = new(itemsPerPage: 10);
            private List<Contact>        _dataSource    = [];
            private string?              _dataRowDetails = String.Empty;

            private string CreateIdentifier(Contact contact)

              => $"For: {contact.GivenName} {contact.FamilyName}, ID {contact.ContactID}";

            protected override void OnInitialized()

                => _dataSource = StaticData.GetContacts(10_000);

            private async Task HandleRowView(ButtonActionData<Contact> payload)
            {
                var rowData = payload.GetValueOr(null);

                if(rowData is not null)
                {
                    _dataRowDetails = rowData.ToString();
                    await InvokeAsync(StateHasChanged);//Actions popover uses Func not EventCallBack so you can chooses whether or not to render.
                }
            }        

            private async Task HandleRowDelete(ButtonActionData<Contact> payload)
            {
                var rowItem = payload.GetValueOr(null);

                if (rowItem is null) return;

                var adjacentRow = DataTableRef.GetAdjacentDisplayedRow(rowItem);

                _dataSource.Remove(rowItem);

                await DataTableRef.Refresh();

                // OR _dataSource = [.._dataSource];
                //await InvokeAsync(StateHasChanged);

                if (adjacentRow != null && _jsModule != null)
                {
                    await _jsModule.InvokeVoidAsync(GlobalValues.JS_Set_Focus, CreateIdentifier(adjacentRow));
                    return;
                }

                await DataTableRef.SetTableFocus();
            }

            private async Task HandleOnDebounce(DebouncedFilterResult debounceResult)
            {
                if (debounceResult.IsValid)
                {
                    _filterRule = (data) => data.GivenName.Contains(debounceResult.FilterValue, StringComparison.OrdinalIgnoreCase)
                                            || data.FamilyName.Contains(debounceResult.FilterValue, StringComparison.OrdinalIgnoreCase)
                                            || data.Country.ToString().Contains(debounceResult.FilterValue, StringComparison.OrdinalIgnoreCase);

                    /*
                        * The debounce uses a Func not an EventCallback to negate renders if the filter value is invalid so
                        * you call StateHasChanged if all is well.
                    */
                    await InvokeAsync(StateHasChanged);
                }
            }

            protected override async Task OnAfterRenderAsync(bool firstRender)
            {
                if (true == firstRender) _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_Module_File_Path);
            }

            public async ValueTask DisposeAsync()
            {
                try
                {
                    if (_jsModule is not null) await _jsModule.DisposeAsync();
                }
                catch { }
            }

        }
        """;

}

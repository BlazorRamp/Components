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
            private PagerBinding         _pagerBinding = new(currentPage: 0,currentItemCount: 0, totalItemCount: 0,itemsPerPage: 10);
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

            private PagerBinding   _pagerBinding = new(currentPage: 0,currentItemCount: 0, totalItemCount: 0,itemsPerPage: 25);
            private List<Contact>  _dataSource   = [];

            protected override void OnInitialized()

                => _dataSource = StaticData.GetContacts(100);

        }

        """;

    public const string Filtering_Usage_Example = """

        <DataTable TData="Contact" DataSource="@_dataSource" Title="Contact Results" PagerBinding="@_pagerBinding" FilterRule="_filterRule">
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
            private PagerBinding _pagerBinding = new(currentPage: 0, currentItemCount: 0, totalItemCount: 0, itemsPerPage: 10);
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
                   RowSelectionMode="RowSelectionMode.Multiple" RowSelectHeading="Pick" SelectedRowsChanged="HandleSectionChanged">
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

            private Func<Contact, bool>? _filterRule   = null;
            private PagerBinding         _pagerBinding = new(currentPage: 0, currentItemCount: 0, totalItemCount: 0, itemsPerPage: 10);
            private List<Contact>        _dataSource   = [];
            private List<Contact>        _selectedRows = [];

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
            private PagerBinding _pagerBinding = new(currentPage: 0, currentItemCount: 0, totalItemCount: 0, itemsPerPage: 10);
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

        """;

    public const string Typical_Usage_Example = """

        """;


}

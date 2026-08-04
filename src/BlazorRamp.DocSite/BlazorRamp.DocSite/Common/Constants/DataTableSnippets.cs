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

        <div style="height:700px;">
            <DataTable TData="Contact" DataSource="@_dataSource" Title="Contact Results" PagerBinding="@_pagerBinding" FilterRule="_filterRule">
                <Filter>
                    <DebounceFilter HintText="Filters across first name, surname and country" RegexPattern="^[A-Za-z ]*$" OnDebounceFilterResult="HandleOnDebounce"
                                    ValidationMessage="Invalid filter, filtering paused, letters and spaces only"  />
                </Filter>
                <TableColumns>
                    <DataColumn DataProperty="c => c.ContactID"   DisplayName="Contact ID"    IsSortable="true" />
                    <DataColumn DataProperty="c => c.GivenName"   DisplayName="First Name"    IsSortable="true" />
                    <DataColumn DataProperty="c => c.FamilyName"  DisplayName="Surname"       IsSortable="true" />
                    <DataColumn DataProperty="c => c.Country"     DisplayName="Country"       IsSortable="true" />
                    <DataColumn DataProperty="c => c.DateOfBirth" DisplayName="Data of Birth" ColumnAlignment="ColumnAlignment.Centre" IsSortable="true" />
                    <DataColumn DataProperty="c => c.Rate" DisplayName="Hourly Rate" IsSortable="true" CellFormat="C" ColumnAlignment="ColumnAlignment.End" />
                </TableColumns>
                <BottomPager>
                    <Pager @bind-CurrentPage="@_pagerBinding.CurrentPage" AriaLabel="Two button pager" TotalItemCount="@_pagerBinding.TotalItemCount" 
                    CurrentItemCount="@_pagerBinding.CurrentItemCount" ItemsPerPage="@_pagerBinding.ItemsPerPage" PagerSelectorType="PagerSelectorType.Button" 
                    ShowFirstLast="true" PageAlignment="PageAlignment.End" AddApplicationRole="true" />
                </BottomPager>
            </DataTable>
        </div>
        @*  
            Note that the DataTable is inside a container with a height specified. You adjust to suit your needs and the tables contents. 
        *@
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

}

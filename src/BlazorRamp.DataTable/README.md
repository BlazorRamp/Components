# Blazor Ramp - Data Table

The Blazor Ramp project aims to provide a suite of modular, accessibility-first Blazor components. 


## Requirements
It is a requirement that the Blazor Ramp Core script, Live Region Service, and associated Announcement History component are added alongside this component’s specific 
requirements (a stylesheet reference), as outlined below.

**Note**: Every package includes a reference to the Blazor Ramp Core project (where the aforementioned items reside) so there is no need to install 
this package separately (but it can be if you only require the Live Regions Service and Announcement History component).

**The full documentation is available at:** https://docs.blazorramp.uk 

## Installation


1. Add the BlazorRamp.DataTable nuget package to your project using the Nuget Package Manager or the dotnet CLI.

```c#
dotnet add package BlazorRamp.DataTable
```
2. Add the following Core and Data Table style references to the `<head>` section of your application:
- Blazor Web App / Blazor Server → App.razor
- Blazor WebAssembly → wwwroot/index.html

**Note:** The data table makes use of the BlazorRamp.BusyIndicator package, which is a transitive so need to include its style sheet as well.
```html
<head>
	<link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
	<link rel="stylesheet" href="_content/BlazorRamp.DataTable/assets/css/data-table.min.css" />
	<link rel="stylesheet" href="_content/BlazorRamp.BusyIndicator/assets/css/busy-indicator.min.css" />
</head>
```
 
3. Add the following Blazor Ramp Core live region script after Blazors script, as follows: 
- Blazor Web App / Blazor Server → App.razor
- Blazor WebAssembly → wwwroot/index.html

```html
<script src="_framework/blazor.web.js"></script>
<script type="module" src="_content/BlazorRamp.Core/assets/js/core-live-region.js"></script>
```
4. Register BlazorRamp services in the Program.cs file (Both server and client if using Server and WebAssembly interactive rendermode)

Add the following line to the service registration section:

```
@using BlazorRamp.Core.Common.Extensions;

builder.Services.AddBlazorRampCore();
```

5. Add the `<AnnouncementHistory />` component with your parameter values above the Router component contained in either:
- Blazor Web App / Blazor Server → Routes.razor
- Blazor WebAssembly → App.razor
 
```html
<AnnouncementHistory RefreshText="Refresh" ClearCloseText="Clear & Close" CloseText="Close" NoDataText="No announcements" 
Title="Recent Announcements" TriggerVisible="true" TriggerText="Alerts" />

<Router AppAssembly . . .
```

## Data Table overview

The Data Table is an accessible, sortable, filterable table for an in-memory list of items. It handles sorting, filtering, paging or virtualization, 
row selection, and templating – while treating screen reader and keyboard-only users as first-class citizens throughout, not as an afterthought bolted on at the end.

It is deliberately *not* a kitchen-sink enterprise grid. There is no built-in remote data fetching, no server-side paging or filtering, and no `IQueryable` pushdown 
to a database. `DataSource` is a plain in-memory `List<TData>` - you bring the data, the Data Table handles displaying, sorting, filtering, and paging it. 
If you genuinely need server-side evaluation of millions of rows, this isn't that component; see the [About These Examples](https://docs.blazorramp.uk/components/data-table/usage/about page for my thoughts on
why that's a smaller slice of real-world use cases than table demos tend to suggest.

## At a glance

- **Sorting** – set `IsSortable` on any `DataColumn`. Cycles unsorted → ascending → descending → back to the original order, one column at a time, with a screen reader announcement on every change.
- **Filtering** – bring your own filter UI via the `Filter` render fragment (a built-in `DebounceFilter` component is included) and supply a `FilterRule` predicate.
- **Paging or virtualization** – supply a `PagerBinding` for a traditional paged table, or leave it out and set `VirtualizeItemSizePX` for a virtualized, scrollable one. The two are mutually exclusive by design.
- **Row selection** – single or multiple, with a dedicated accessible selection column.
- **Templating** – override any column's cell content via `CellTemplate`, or add entirely new columns (row actions, say) that don't exist in your data source via `TemplateColumn`.
- **Like all components, accessibility by default** – live region announcements for sorting, filtering, and empty results (all fully customisable, with sensible defaults 
if you change nothing), proper table semantics, and documented, tested focus-management guidance for anything that changes the row count while the table has focus.

The following snippet is the exmaple on the Basic Usage page on the documentation site.

```
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
        <DataColumn DataProperty="c => c.DateOfBirth" DisplayName="Date of Birth" ColumnAlignment="ColumnAlignment.Centre" IsSortable="true"  />
        <DataColumn DataProperty="c => c.Rate" DisplayName="Hourly Rate" IsSortable="true" CellFormat="C" ColumnAlignment="ColumnAlignment.End" />
    </TableColumns>
    <BottomPager>
        <Pager @bind-CurrentPage="@_pagerBinding.CurrentPage" AriaLabel="Contacts pager" TotalItemCount="@_pagerBinding.TotalItemCount" CurrentItemCount="@_pagerBinding.CurrentItemCount"
        ItemsPerPage="@_pagerBinding.ItemsPerPage" PagerSelectorType="PagerSelectorType.Button" ShowFirstLast="true" PageAlignment="PageAlignment.End" AddApplicationRole="true" />
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
```


**Note:** For the full documentation and example usage, please see https://docs.blazorramp.uk



## Using the Live Region Service (directly)

Inject the `ILiveRegionService` into your desired component or class and make the appropriate calls by passing the `ILiveRegionSerivce.MakeAnnouncement` method an announcement object.

```
@inject ILiveRegionService _liveRegionService

@code{

	private async Task MakeAnnouncement()
	{
		var announcement = new Announcement("The site is now using a dark coloured theme.", AnnouncementType.Info, "Dark Theme Switch", LiveRegionType.Polite);
		await _liveRegionService.MakeAnnouncement(announcement);
	}
}

```
**Note:** Where possible make announcements using `LiveRegionType.Polite` and keep your messages brief and to the point. Long verbose messages are annoying and just slow the user down. 

The announcement object has the following constructor parameters:

- **Message** - a string value containing the message to be announced.
- **AnnouncementType** - an enumerated type describing the type category of announcement (for future use) the default is `AnnoucementType.Info`,
- **AnnouncementTrigger** - an optional string value with the user friendly display name of the element that triggered the announcement such as 'Save Button'
- **LiveRegionType** - the urgency of the announcement. Polite announcements wait for the screen reader to finish current speech before announcing where as assertive announcements 
interrupt the screen reader immediately. 

**Full documentation available at:** https://docs.blazorramp.uk 

**Screen Reader Browser Combination Tests:** 
- On Windows 11 - JAWS, NVDA and Narrator each paired with Chrome, Edge and Firefox.
- On macOS (Sequoia) VoiceOver was paired with Safari
- On iPhone, VoiceOver was paired with Safari
- On Android, TalkBack was paired with Chrome
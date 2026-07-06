# Blazor Ramp - Pager

The Blazor Ramp project aims to provide a suite of modular, accessibility-first Blazor components. 


## Requirements
It is a requirement that the Blazor Ramp Core script, Live Region Service, and associated Announcement History component are added alongside this component’s specific 
requirements (a stylesheet reference), as outlined below.

**Note**: Every package includes a reference to the Blazor Ramp Core project (where the aforementioned items reside) so there is no need to install 
this package separately (but it can be if you only require the Live Regions Service and Announcement History component).

**The full documentation is available at:** https://docs.blazorramp.uk 

## Installation


1. Add the BlazorRamp.Pager nuget package to your project using the Nuget Package Manager or the dotnet CLI.

```c#
dotnet add package BlazorRamp.Pager
```
2. Add the following Core and Pager style references to the `<head>` section of your application:
- Blazor Web App / Blazor Server → App.razor
- Blazor WebAssembly → wwwroot/index.html
```html
<head>
	<link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
	<link rel="stylesheet" href="_content/BlazorRamp.Pager/assets/css/pager.min.css" />
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

## Using the Pager component

The Pager has two modes of operation: one that uses buttons for paging, typically used with data tables, and the other that uses links, typically used on product listing pages.

Both require interactivity, and given the nature of Blazor, the link version has its href suppressed so that only the address bar is updated - technically, you could use either 
version for either scenario; the link version simply adds a query parameter to the URL shown in the address bar.

For smaller data sets, rather than displaying all four selectors - First, Previous, Next and Last - you can choose to show only Previous and Next, reducing the number of tab stops.

The information displayed above the selectors is what is announced to screen reader users as they navigate through the pages. As this is a navigation landmark, the provided name is 
announced to screen reader users on entering the pager, along with its current state — the information displayed above the selectors.

You provide the Pager with the total record count for the data set, the number of records per page, and, if filtering is used, the current record count. The Pager uses these to 
calculate the number of pages and to determine whether filtering is in use, which it does by checking whether the current record count equals the total record count; if they differ, filtering is assumed.

The following example shows a typical Pager set-up, using the default text for the displayed selector names and page information, that has a data set that has been filtered so
is providing values for both the `TotalItemCount` and `CurrentItemCount` parameters.

```
<Pager @bind-CurrentPage="@_currentPage" AriaLabel="Customer pager" TotalItemCount="@_totalCount" CurrentItemCount="@_filteredCount" ItemsPerPage="10" 
	PagerSelectorType="PagerSelectorType.Button" ShowFirstLast="true"></Pager>
```


**Note:** For the full documentation please see https://docs.blazorramp.uk



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
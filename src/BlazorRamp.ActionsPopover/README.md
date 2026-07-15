# Blazor Ramp - Actions Popover

The Blazor Ramp project aims to provide a suite of modular, accessibility-first Blazor components. 


## Requirements
It is a requirement that the Blazor Ramp Core script, Live Region Service, and associated Announcement History component are added alongside this component’s specific 
requirements (a stylesheet reference), as outlined below.

**Note**: Every package includes a reference to the Blazor Ramp Core project (where the aforementioned items reside) so there is no need to install 
this package separately (but it can be if you only require the Live Regions Service and Announcement History component).

**The full documentation is available at:** https://docs.blazorramp.uk 

## Installation


1. Add the BlazorRamp.ActionsPopover nuget package to your project using the Nuget Package Manager or the dotnet CLI.

```c#
dotnet add package BlazorRamp.ActionsPopover
```
2. Add the following Core and Pager style references to the `<head>` section of your application:
- Blazor Web App / Blazor Server → App.razor
- Blazor WebAssembly → wwwroot/index.html
```html
<head>
	<link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
	<link rel="stylesheet" href="_content/BlazorRamp.ActionsPopover/assets/css/actions-popover.min.css" />
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

## Using the Actions Popover component

ActionsPopover is a lightweight component for grouping related actions - buttons, links, or a mix of both, behind a single trigger.

It's built on the native HTML Popover API for toggling, top-layer stacking, and dismiss behaviour like click-outside and Escape, 
combined with CSS anchor positioning to place the panel relative to its trigger.

The native behaviour was extended to include closing the popover when tabbing puts the focus on an item outside of the actions popover.

Use it standalone as a page-level actions panel, or embed it anywhere you need a compact set of contextual actions - such as the last column of a table row

Items are reached via standard Tab order. Each action can carry its own icon, colour, and associated data via ItemData, and both buttons and links 
respect forced-colours mode automatically, so high-contrast users always get a legible, system-consistent result regardless of what colours you configure.

**Note:** The actions popover just uses the simple disclosure pattern and as such no ARIA role or custom keyboard navigation support was necessary.

The following example just shows the actions popover with two buttons and a link that opens in a new window, each with an optional icon. 
You have to add a type param `TData` to each item, when its not needed you can just assign it to object without populated the `ItemData` parameter.

```
<ActionsPopover TriggerText="Available Actions" ActionsPopoverPosition="ActionsPopoverPosition.BottomCentre">
    <PopoverItems>
        <ActionPopoverButton TData="object" ButtonText="Edit item" SvgIcon="--svg-pencil-icon" OnClick="HandleOnClick" IconColour="green" />
        <ActionPopoverButton TData="object" ButtonText="Delete item" SvgIcon="--svg-trash-can-icon" OnClick="HandleOnClick" IconColour="red" />
        <ActionPopoverLink   TData="object" LinkText="View (opens in new window)" SvgIcon="--svg-overview-icon" TargetType="PopoverLinkTargetType.Blank" Path="weather" />
    </PopoverItems>
</ActionsPopover>
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
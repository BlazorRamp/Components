# Blazor Ramp - Toggletip

The Blazor Ramp project aims to provide a suite of modular, accessibility-first Blazor components. 

**ToggleTip** is a term coined by Heydon Pickering for a widget that displays supplemental information on click, key press, touch or voice control rather than on on mouseover/hover.
This implmentation uses the the HTML Popover attribute API to show non-modal content.

**Note:** Important: The Toggletip component uses CSS anchor positioning with fallbacks for when it wont fit in the desired location which was only baseline at the start of the year 2026. 
Dependant on your target audience and supported browser versions, you may want to consider referencing the OddBird anchor positioning polyfill see: https://github.com/oddbird/css-anchor-positioning 
to support older browser versions. Without CSS anchor positioning the Toggletip content will be located at the top left of the page, but with all functionality intact.

## Requirements
It is a requirement that the Blazor Ramp Core script, Live Region Service, and associated Announcement History component are added alongside this component’s specific 
requirements (a stylesheet reference), as outlined below.

**Note**: Every package includes a reference to the Blazor Ramp Core project (where the aforementioned items reside) so there is no need to install 
this package separately (but it can be if you only require the Live Regions Service and Announcement History component).

**The full documentation is available at:** https://docs.blazorramp.uk 

## Installation


1. Add the BlazorRamp.ToggleTip nuget package to your project using the Nuget Package Manager or the dotnet CLI.

```c#
dotnet add package BlazorRamp.ToggleTip
```
2. Add the following Core and Busy Indicator stylesheet references to the `<head>` section of your application:
- Blazor Web App / Blazor Server → App.razor
- Blazor WebAssembly → wwwroot/index.html
```html
<head>
	<link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
	<link rel="stylesheet" href="_content/BlazorRamp.ToggleTip/assets/css/toggle-tip.min.css" />
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

## Using the Toggletip

The toggletip can be set apart from text or placed inline within a paragraph as shown on the (full) documentation page. When using the default variable values 
the toggltips optional label and icon button will both match the size and colour of the surrounding text. 

The toggletip uses light dismiss so clicking outside of the popover will close it, as will the escape key. The popover will remain open until closed, 
and as its not modal the user can tab out of the toggletip leaving it open. Opening an other item that uses the popover api, such as the Announcement History dialog 
will close any other open popover.

Below is a simple example of the toggletip showing supplemental information about the keyboard interations for the toggletip:

```
@using BlazorRamp.ToggleTip.Common.Constants
@using BlazorRamp.ToggleTip.Components

<ToggleTip CloseText="Close Keyboard Info." Label="Keyboard info:" ShowClose="true" ShowLabel="true" 
ToggleTipLabelOrder="ToggleTipLabelOrder.LabelFirst" ToggleTipSize="ToggleTipSize.Small">
	
	<h3 id="keyboard-info">Keyboard interaction</h3>
	<ul aria-labelledby="id="keyboard-info"">
		<li><kbd>Space</kbd> - when focus is on the toggletip icon, expands or collapses the content.</li>
		<li><kbd>Enter</kbd> - when focus is on the toggletip icon, expands or collapses the content.</li>
		<li><kbd>Escape</kbd> - closes the toggletip</li>
	</ul>

</ToggleTip>
```



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
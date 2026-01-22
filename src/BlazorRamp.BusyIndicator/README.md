# Blazor Ramp - Busy Indicator

The Blazor Ramp project aims to provide a suite of modular, accessibility-first Blazor components. 

The **Busy Indicator** component provides the ability to cover an entire page, or a specific section of a page, while an action is taking place. This indicator differs from 
others in two ways:

First, it makes everything beneath it (excluding the trigger button) inert, ensuring it is inaccessible to all users until the action is complete. 

Second, it requires you to 
provide text that is sent via a **Live Region Service** to hidden ARIA live regions. This allows assistive technologies, such as screen readers, to monitor and relay updates to 
users.

## Requirements
It is a requirement that the Blazor Ramp Core script, Live Region Service, and associated Announcement History component are added alongside this component’s specific 
requirements (a stylesheet reference), as outlined below.

**Note**: Every package includes a reference to the Blazor Ramp Core project (where the aforementioned items reside) so there is no need to install this separately.

**The full documentation is available at:** https://docs.blazorramp.uk 

## Installation


1. Add the BlazorRamp.BusyIndicator nuget package to your project using the Nuget Package Manager or the dotnet CLI.

```c#
dotnet add package BlazorRamp.BusyIndicator
```
2. Add the following Core and Busy Indicator stylesheet references to the `<head>` section of your application:
- Blazor Web App / Blazor Server → App.razor
- Blazor WebAssembly → wwwroot/index.html
```html
<head>
	<link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
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

## Using the Busy Indicator

Add one or more Busy Indicators to each page where required. You can have multiple indicators running concurrently. Each indicator allows you to specify whether it should 
cover the entire page or just a section of it, as well as the location of its spinner or hourglass and optional text.

It is a requirement that text be provided for the indicator to relay via the Live Region Service to inform screen reader users when the underlying task has completed. 
Optionally, you can also provide a starting announcement, but I do not recommend this unless you know the task is likely to take more than four or five seconds. I also advise 
keeping any message short and to the point; for example, upon the completion and submission of edited data, an announcement such as "Saved successfully" is sufficient.

The following example is taken from the test site https://blazorramp.uk where I created some simple tests for any user to try with their device setup.

```
<BusyIndicator AriaStartText="Dummy task has started, please wait." AriaEndText="Dummy task completed successfully." BusyText=". . .Processing please wait . . " ContentPosition="ContentPosition.Centre"
               OverlayPosition="OverlayPosition.Screen"  IndicatorTrigger="Run Page Test button" ShowIndicator="@_showIndicator" />

@code {
    private bool _showIndicator = false;

    private async Task RunTest()
    {
        _showIndicator = true;

        await Task.Delay(8000);

        _showIndicator = false;
    }
}
```
**Note:** In production, unlike the test you would use a variable for the `AriaEndText` and assign it with correct status dependant on success or failure.

If the user has there system settings for reduced motion instead of a spinning circle they will see a static hour gloss.

For the full description of all of the component parameters and events, please see the documentation for the Busy Indicator: https://docs.blazorramp.uk/components/busy-indicator


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
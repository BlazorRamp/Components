# Blazor Ramp - GoTo

The Blazor Ramp project aims to provide a suite of modular, accessibility-first Blazor components. 

The **GoTo** component gives keyboard users a way to jump straight to the parts of a page that matter to them. You place a `<GoTo />` marker, with a short label such 
as "Filters" or "Order summary", at each destination. A single `<GoToViewer />` added once to your layout, lists the markers on the current page. It opens with the default 
shortcut key `Alt/Option + Z` (configurable), or with a trigger button that stays hidden until it receives keyboard focus, so it adds nothing to the page for anyone who doesn't need it. 

## Requirements
It is a requirement that the Blazor Ramp Core script, Live Region Service, and associated Announcement History component are added alongside this component’s specific 
requirements, as outlined below.

**Note**: Every package includes a reference to the Blazor Ramp Core project (where the aforementioned items reside) so there is no need to install 
this package separately (but it can be if you only require the Live Regions Service and Announcement History component).

**The full documentation is available at:** https://docs.blazorramp.uk 

## Installation


1. Add the BlazorRamp.GoTo nuget package to your project using the Nuget Package Manager or the dotnet CLI.

```c#
dotnet add package BlazorRamp.GoTo
```
2. Add the following Core and GoTo stylesheet references to the `<head>` section of your application:
- Blazor Web App / Blazor Server → App.razor
- Blazor WebAssembly → wwwroot/index.html
```html
<head>
	<link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
	<link rel="stylesheet" href="_content/BlazorRamp.GoTo/assets/css/goto.min.css" />
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

5. Add both the `<AnnouncementHistory />` and the `<GoToViewer />`component with your preferred parameter values above the Router component contained in either:
- Blazor Web App / Blazor Server → Routes.razor
- Blazor WebAssembly → App.razor
 
 **Note:** If you are using Global InteractiveServer or Global InteractiveWebAssembly then no `@rendermode` will need assigning to
 the `<GoToViewer />` component. 
 
 If you are using per page with interactiveAuto, InteractiveServer and InteractiveWebAssembly then you will need to 
 add `@rendermode="InteractiveAuto"` to the GoToViewer Component. Note this is not needed for the the `<AnnouncementHistory />` component.


```html
<AnnouncementHistory RefreshText="Refresh" ClearCloseText="Clear & Close" CloseText="Close" NoDataText="No announcements" 
Title="Recent Announcements" TriggerVisible="true" TriggerText="Alerts" />

<GoToViewer CloseButtonText="Close" ExcludeTriggerButton="false" GoToShortcut="@(new(Key:"z", Alt:true))" 
            HeadingText="Go to Markers" NoGoToMarkersText="No go to markers defined for this page." TriggerButtonText="Go to viewer"/>

<Router AppAssembly . . .
```

## Using the GoTo components (viewer and marker/label)

Once the viewer has been configured and placed as shown above then this can then be accessed either via the triggering button (shown when it recieves focus) or
via the configurable global shorcut key, which by default is Alt+Z (Option+Z on macOS).

You then simply place `<GoTo />` components where you would like the user to be able to jump to and then start tabbing from that location.

```html
<GoTo LabelText="Components section main menu" />
```


For the full description of all the component parameters and events, please see the documentation for the GoTo: https://docs.blazorramp.uk/components/goto


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
- On Android, TalkBack was paired with Chrome

**Note:** Unlike the other components no testing has yet been performed using VoiceOver on iPhone.

The testing matrix also includes Windows Voice access.

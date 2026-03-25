# Blazor Ramp - Accordion

The Blazor Ramp project aims to provide a suite of modular, accessibility-first Blazor components. 


## Requirements
It is a requirement that the Blazor Ramp Core script, Live Region Service, and associated Announcement History component are added alongside this component’s specific 
requirements (a stylesheet reference), as outlined below.

**Note**: Every package includes a reference to the Blazor Ramp Core project (where the aforementioned items reside) so there is no need to install 
this package separately (but it can be if you only require the Live Regions Service and Announcement History component).

**The full documentation is available at:** https://docs.blazorramp.uk 

## Installation


1. Add the BlazorRamp.Accordion nuget package to your project using the Nuget Package Manager or the dotnet CLI.

```c#
dotnet add package BlazorRamp.Accordion
```
2. Add the following Core and Accordion style references to the `<head>` section of your application:
- Blazor Web App / Blazor Server → App.razor
- Blazor WebAssembly → wwwroot/index.html
```html
<head>
	<link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
	<link rel="stylesheet" href="_content/BlazorRamp.Accordion/assets/css/accordion.min.css" />
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

## Using the Accordion component

Add the Accordion component to your page and set the desired parameters. Each Accordion heading can have an associated icon. There are other
paramters that control persistance of collapsed items and for two way binding for each items expanded state.

The example belwo is taken from the test site: https://blazorramp.uk

```<Accordion HeadingLevel="HeadingLevel.H3" ExpandMode="ExpandMode.Multiple">
        <AccordionItem HeadingText="Food" PanelHasTabIndex="true" PanelIsRegion="false" SvgIcon="--svg-food-icon">
            <PanelContent>
                <p>
                    Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin sit amet sem vulputate, interdum justo vitae, sagittis nulla. 
                    Ut lectus augue, consectetur ut massa id, tristique luctus eros. Donec tempus augue velit, vel vehicula tellus consequat vel. 
                    Integer tristique sem ac nisi sodales placerat. Integer nulla ex, tincidunt ut eleifend ut, vestibulum vel leo. Proin diam urna,
                    finibus porttitor purus nec, tristique varius justo. Nunc vel dui sed dui elementum facilisis. Curabitur ac fermentum
                    urna. Integer vestibulum fermentum massa, quis auctor elit viverra eget.
                </p>
            </PanelContent>
        </AccordionItem>
        <AccordionItem HeadingText="Travel" PanelHasTabIndex="true" PanelIsRegion="false" SvgIcon="--svg-airplane-icon">
            <PanelContent>
                <p>
                    Nulla eu est lacus. Mauris sodales tortor in sem viverra porta. Nam in dapibus massa. Vivamus et augue quis mauris luctus pretium
                    eu eu dolor. Maecenas gravida nisl non ante facilisis, pulvinar vestibulum nibh sagittis. Pellentesque mollis sem at arcu mattis, 
                    id molestie augue cursus. Pellentesque dolor urna, ultricies quis iaculis ac, rutrum luctus nulla. Integer et consequat erat. Sed 
                    eget metus in est pulvinar ultrices at nec lorem. Sed aliquam massa eget dui aliquet ullamcorper. Sed elit nisi, maximus
                    et erat molestie, bibendum rutrum diam. Phasellus cursus eleifend porta.
                </p>
            </PanelContent>
        </AccordionItem>
        <AccordionItem HeadingText="Exercise" PanelHasTabIndex="true" PanelIsRegion="false" SvgIcon="--svg-runner-icon">
            <PanelContent>
                <p>
                    Mauris imperdiet nisi nec pulvinar porta. Sed semper viverra venenatis. Sed accumsan, erat condimentum ornare malesuada, erat mauris 
                    tincidunt enim, non interdum quam sem ac velit. Nunc et enim lorem. Maecenas gravida tortor eget ligula efficitur ullamcorper. Cras 
                    ut nisi elementum, maximus ante eu, vestibulum nibh. Vestibulum nec iaculis elit, sit amet sagittis ex.
                </p>
            </PanelContent>
        </AccordionItem>
    </Accordion>

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
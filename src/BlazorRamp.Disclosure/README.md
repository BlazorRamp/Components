# Blazor Ramp - Disclosure

The Blazor Ramp project aims to provide a suite of modular, accessibility-first Blazor components. 

The **Disclosure** component provides an accessible way to show and hide a single region of content. It's made up of a button, the trigger, and the 
content that the button controls. Activating the trigger with a mouse, touch, or the Enter or Space keys toggles the content, while the trigger's 
`aria-expanded` attribute tells assistive technologies whether the content is currently shown and `aria-controls` associates the trigger with the content it controls. 


## Requirements
It is a requirement that the Blazor Ramp Core script, Live Region Service, and associated Announcement History component are added alongside this component’s specific 
requirements (a stylesheet reference), as outlined below.

**Note**: Every package includes a reference to the Blazor Ramp Core project (where the aforementioned items reside) so there is no need to install 
this package separately (but it can be if you only require the Live Regions Service and Announcement History component).

**The full documentation is available at:** https://docs.blazorramp.uk 

## Installation


1. Add the BlazorRamp.Disclosure nuget package to your project using the Nuget Package Manager or the dotnet CLI.

```c#
dotnet add package BlazorRamp.Disclosure
```
2. Add the following Core and disclosure stylesheet references to the `<head>` section of your application:
- Blazor Web App / Blazor Server → App.razor
- Blazor WebAssembly → wwwroot/index.html
```html
<head>
	<link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
	<link rel="stylesheet" href="_content/BlazorRamp.Disclosure/assets/css/disclosure.min.css" />
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

## Using the Disclosure

The following example has set the content to have a maximum height of 40vh units. Dependant on the window size, if there is not 
enough room for the content a vertical scrollbar will appear. This addition and removal of a scrollbar is monitored with attributes 
automatically added to ensure that the scrollable region is keyboard accessible. if there is no focusable content detected a `role="group"` 
attribute is added (if no existing role defined). A tabindex="0" is also added along with the `aria-labelledby`, so the trigger text is used for the groups 
accessible name. These attributes are removed if the page is sized as such to not need a scrollbar.

```
<Disclosure TriggerText="Lorem ipson text in a disclosure" ContentMaxHeight="40vh">
    <p style="margin:0">
        Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut pretium pharetra ullamcorper. Quisque lectus 
        enim, laoreet eu nisi id, hendrerit convallis est. Nam et gravida purus, eget tincidunt erat. Ut mattis diam 
        at est ullamcorper, et ultrices est ultricies. Nam vel ultricies metus. Cras at aliquam sem. Curabitur
        consectetur velit vulputate vestibulum sagittis. In et scelerisque libero. Vestibulum condimentum venenatis 
        metus, sit amet commodo tortor rutrum vel.
    </p>
    <p>
        Suspendisse felis nibh, molestie id sapien at, venenatis condimentum erat. Nam id dui mi. Vestibulum accumsan 
        lacus nec turpis pharetra, eget scelerisque quam gravida.Sed eget libero condimentum, sodales lorem non, bibendum dui. 
        Nunc iaculis lacinia turpis non mattis. Pellentesque ut lectus nec quam rutrum sagittis eget non mi. Aliquam
        at euismod purus. Curabitur maximus, risus eget mattis fermentum, lorem lorem vehicula sapien, id consequat nisi 
        est at tellus. In a volutpat enim. Aliquam vitae nisl in nisl vulputate mollis. Donec sapien quam, sagittis at est eget, 
        egestas placerat quam. Donec ipsum orci, facilisis sit amet viverra a, ultrices auctor nisi. Duis volutpat
        rutrum dui ac scelerisque. Vivamus dapibus faucibus massa sit amet efficitur.

    </p>
    <p>
        Donec eu turpis leo. Morbi vehicula sem feugiat, aliquet erat malesuada, pellentesque metus. Mauris vitae dolor sodales,
        imperdiet magna consectetur, tristique felis. Nunc aliquam elit vitae orci tristique, ut porta nisi interdum. Donec tellus 
        tortor, lobortis vel augue sed, imperdiet cursus massa. Duis quis magna porttitor, molestie tortor et, rutrum ipsum. Nunc 
        et nibh porta, dignissim purus et, rutrum sapien. Vestibulum a orci libero. Suspendisse quis scelerisque quam. Quisque purus lorem, 
        accumsan vel  mauris ut, pharetra volutpat mauris. Aliquam venenatis lacus at magna aliquet, nec semper tortor dapibus. Donec
        eget consectetur metus. Duis pulvinar aliquet augue quis accumsan.Quisque risus odio, dapibus sit amet consequat vel, elementum id libero.
    </p>

</Disclosure>
```

For the full description of all the component parameters and events, please see the documentation for the Disclosure component: https://docs.blazorramp.uk


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
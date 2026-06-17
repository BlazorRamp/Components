# Blazor Ramp - Debounce Filter

The Blazor Ramp project aims to provide a suite of modular, accessibility-first Blazor components. 

The **BlazorRamp.DebounceFilter** NuGet package contains a single component, the **DebounceFilter**. This is, in essence, a single
text input styled like the other input components, however, there is no binding as the keystrokes and input value are handled in
JavaScript using a timer. On each keystroke the timer is reset; once the timer elapses, the input value is checked against an
optional Regex pattern, with the result of any validation and the input value handed to Blazor.

The component, rather than using a traditional `EventCallback`, uses a `Func<DebouncedFilterResult, Task>` to provide you with the
result. Using a `Func` does not cause a re-render, which in this case is useful, as if the filter value is invalid you do not need
your component to re-render. If the value is valid, you can then hand it to your component and call `StateHasChanged` to trigger


## Requirements
It is a requirement that the Blazor Ramp Core script, Live Region Service, and associated Announcement History component are added alongside this component’s specific 
requirements (a stylesheet reference), as outlined below.

**Note**: Every package includes a reference to the Blazor Ramp Core project (where the aforementioned items reside) so there is no need to install 
this package separately (but it can be if you only require the Live Regions Service and Announcement History component).

**The full documentation is available at:** https://docs.blazorramp.uk 

## Installation


1. Add the BlazorRamp.DebounceFilter NuGet package to your project using the NuGet Package Manager or the dotnet CLI.

```c#
dotnet add package BlazorRamp.Inputs
```
2. Add the following Core and NavGroup style references to the `<head>` section of your application:
- Blazor Web App / Blazor Server → App.razor
- Blazor WebAssembly → wwwroot/index.html
```html
<head>
	<link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
	<link rel="stylesheet" href="_content/BlazorRamp.DebounceFilter/assets/css/debounce-filter.min.css" />
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

## Using the Debounce Filter

Add the filter next to or inside your component that needs a text filter. Set the paramters as desired and a handler for the `OnDebounceFilterResult`
parameter in order to get the result of the filter after each debounce delay.

Validation is turned on if the `RegexPattern` parameter is provided.

```
<DebounceFilter DebounceDelayMs="500" 
                ValidationMessage="Invalid entry filtering paused, numbers only" 
                RegexPattern="^[A-Za-z0-9]*$"
                FilterLabelText="Filter Results" 
                HintText="Data filtered on pause of typing." 
                OnDebounceFilterResult="HandleDebounce"  
                ParseErrorMessage="System error, filtering is unavailable at this time." />

@code{

    public async Task HandleDebounce(DebouncedFilterResult result)
    {
        if(result.IsValid)
        {
            var inputValue = result.FilterValue;
            /*
                * Pass the input value to your filtering mechanism 
                * And remember to inform Blazor as callback is using a Func not EventCallback.
            */
            await InvokeAsync(StateHasChanged); 
        }
    }

}

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
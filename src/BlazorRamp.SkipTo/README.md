# Blazor Ramp - Skip To

The Blazor Ramp project aims to provide a suite of modular, accessibility-first Blazor components. 

The **Skip To** component is a utility component that allows navigation to a named target on the page enabling its users to skip repetitive blocks of elements such as navigation
menus. The focused target will either scroll into view or just snap into view dependent on if the user has reduced motion settings enabled on their device.

## Requirements
It is a requirement that the Blazor Ramp Core script, Live Region Service, and associated Announcement History component are added alongside this component’s specific 
requirements (a stylesheet reference), as outlined below.

**Note**: Every package includes a reference to the Blazor Ramp Core project (where the aforementioned items reside) so there is no need to install 
this package separately (but it can be if you only require the Live Regions Service and Announcement History component).

**The full documentation is available at:** https://docs.blazorramp.uk 

## Installation


1. Add the BlazorRamp.SkipTo NuGet package to your project using the Nuget Package Manager or the dotnet CLI.

```c#
dotnet add package BlazorRamp.SkipTo
```
2. Add the following Core and Skip To stylesheet references to the `<head>` section of your application:
- Blazor Web App / Blazor Server → App.razor
- Blazor WebAssembly → wwwroot/index.html
```html
<head>
	<link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
	<link rel="stylesheet" href="_content/BlazorRamp.SkipTo/assets/css/skip-to.min.css" />
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

## Using the Skip To Component



The Skip To component is designed with two use cases in mind. The first will probably cover 90% of cases, where it simply sits at the top-right corner of the page and 
always targets the main content element. In the case of Blazor, that main content is most likely fixed, with the `@Body` injecting the content.

Given that a skip to for the site may be on a non-interactive area, the Skip To has been designed to work with or without interactivity at the `SkipToType.Site` level.
Currently, I only support having one Skip to at the Site level which it positioned top (LTR/RTL).  

```
<SkipTo IconVisible="true" SkipToText="Skip to content" SkipToType="SkipToType.Site" TargetID="app__main" />
```
The second use case is where you have a block of repeated items on each interactive page and need to skip this content i.e. a secondary skip to. You can have multiple 
skip to components on a page at this level (SkipToType.Section). Each skip to for a section uses absolute positioning relative to its parent (that has a position set, 
such as relative) and will be position top (LTR/RTL) within its parent


```
<SkipTo IconVisible="true" SkipToText="Skip to section content" SkipToType="SkipToType.Section" TargetID="section-one" />
```

**Note**: Don't forget to add tabindex="-1" on the target element.

Please see the full documentation for more information: https://docs.blazorramp.uk


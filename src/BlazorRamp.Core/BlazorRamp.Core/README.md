# Blazor Ramp Core

The Blazor Ramp project aims to provide a suite of modular, accessibility-first Blazor components. This Core package includes the `LiveRegionService`, which enables you to make announcements 
using the aria-live API via elements with the `aria-live` attribute, utilising the values `polite` and `assertive` to indicate the urgency of each announcement.

The Core package also includes the `<AnnouncementHistory />` component, which allows users to view and/or clear a rolling log of the last twenty announcements stored in browser memory.

In addition to the service and component mentioned above, the Core project provides a common set of CSS custom properties that all future components will utilise for their styling. 
Overriding these CSS properties will therefore update the appearance of all components that rely on them. Each component also exposes non-scoped CSS classes, enabling you to apply custom style overrides where necessary.

## Installation


1. Add the BlazorRamp.Core nuget package to your project using the Nuget Package Manager or the dotnet CLI.

```c#
dotnet add package BlazorRamp.Core
```
2. Add the following static stylesheet reference to the `<head>` section of you application:
- Blazor Web App / Blazor Server → App.razor
- Blazor WebAssembly → wwwroot/index.html
```html
<head>
	<link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
</head>
```
 
3. Add the following script tag after `_framework/blazor.web.js"`as follows to you application: 
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
# Blazor Ramp - Modal Dialog Framework

The Blazor Ramp project aims to provide a suite of modular, accessibility-first Blazor components. 

The **Modal Dialog Framework** component and service together make up a framework that allow you to have your content placed within a an HTML dialog element, 
with the framework taking care of all interactions with the HTML Dialog API.

This content is in the form of a standard blazor component and as such you just define your [Parameter] properties as normal as the means of passing data into your component. When 
you want to show your component in a dialog you just invoke the methods on the service providing it with the values for your component parameters.

The framework allows for nested dialogs and as it overrides the default HTML Dialog API escape key mechanism, via a subscription mechanism on the service, it gives you the ability to 
close the dialog when the escape key is pressed, or perhaps open another dialog asking for confirmation if the current dialog was a pop up form with unsaved changes etc.

As the HTML dialog makes everything beneath it inert, this also makes any centralised aria live regions inert. However, the announcement history component and its associated live regions 
react to dialog openings and are moved, so the live regions are always available via the live region service.

## Requirements
It is a requirement that the Blazor Ramp Core script, Live Region Service, and associated Announcement History component are added alongside this component’s specific 
requirements, a stylesheet reference and service, as outlined below.

**Note**: Every package includes a reference to the Blazor Ramp Core project (where the aforementioned items reside) so there is no need to install 
this package separately (but it can be if you only require the Live Regions Service and Announcement History component).

**The full documentation is available at:** https://docs.blazorramp.uk 

## Installation


1. Add the BlazorRamp.DialogFramework nuget package to your project using the Nuget Package Manager or the dotnet CLI.

```c#
dotnet add package BlazorRamp.DialogFramework
```
2. Add the following Core and Dialog Framework stylesheet references to the `<head>` section of your application:
- Blazor Web App / Blazor Server → App.razor
- Blazor WebAssembly → wwwroot/index.html
```html
<head>
	<link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
	<link rel="stylesheet" href="_content/BlazorRamp.DialogFramework/assets/css/dialog-framework.min.css" />
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

Add the following lines to the service registration section for both the live region service contained in the Core project and the Modal Dialog Service
contained in this package:

```
@using BlazorRamp.Core.Common.Extensions;
@using BlazorRamp.DialogFramework.Common.Extensions;

builder.Services.AddBlazorRampCore();
builder.Services.AddBlazorRampDialogService();

```

5. Add the `<AnnouncementHistory />` component with your parameter values above the Router component contained in either:
- Blazor Web App / Blazor Server → Routes.razor
- Blazor WebAssembly → App.razor

**Note:** If your project is a global Server or WebAssembly project then you can just place a single instance of the component with
the appropriate render mode setting here with the Announcement History component. **Please see the full documenation for more details** 
regarding the placement of the `<ModalDialogContainter />` component.
```html
<AnnouncementHistory RefreshText="Refresh" ClearCloseText="Clear & Close" CloseText="Close" NoDataText="No announcements" 
Title="Recent Announcements" TriggerVisible="true" TriggerText="Alerts" />

<ModalDialogContainer @rendermode="InteractiveWebAssembly" />

<Router AppAssembly . . .
```

## Using the Modal Dialog Framework

1. Create a component that is to be the content of the modal dialog, with any `[Parameter]` properties that you want for data that will
be passed into the dialog.
2. Set the options for the dialogs positioning and widths.
3. Create and poplulate a dialog parameters collection with the data for the parameters in your component.
4. await the result of a call to your component via the dialog service.

The example below is taken from the demo on the documenataion, where `<SomeForm />` is the component that is the content of the
modal dialog.

```c#
        @inject ModalDialogService _dialogService

        SomePersonData somePersonData = new("John","Doe", 42,"United Kingdom");

        var dialogOptions     = new ModalDialogOptions(HorizontalAlignment.Centre, VerticalAlignment.Top);
        var dialogParameters  = new ModalDialogParameters<SomeForm>();

        dialogParameters.Add(x => x.SomePersonData, somePersonData);//The parameter/type and the data

        var dialogResult = await _dialogService.ShowDialog<SomeForm>(dialogParameters, dialogOptions);

        if (dialogResult.ButtonClicked == DialogResultButtons.Ok)
        {
            _outputMessage = "The data returned was:" + dialogResult.Data.ToString();
        }
        else
        {
            _outputMessage = "The operation was cancelled.";
        }

```


**Full documentation available at:** https://docs.blazorramp.uk 

**Screen Reader Browser Combination Tests:** 
- On Windows 11 - JAWS, NVDA and Narrator each paired with Chrome, Edge and Firefox.
- On macOS (Sequoia) VoiceOver was paired with Safari
- On iPhone, VoiceOver was paired with Safari
- On Android, TalkBack was paired with Chrome
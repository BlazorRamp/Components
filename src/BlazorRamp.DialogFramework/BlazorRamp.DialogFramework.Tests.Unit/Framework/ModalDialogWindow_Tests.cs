using BlazorRamp.DialogFramework.Framework;
using BlazorRamp.DialogFramework.Tests.SharedDataFixtures.Common.Data;
using BlazorRamp.DialogFramework.Tests.SharedDataFixtures.Components;
using FluentAssertions;
using FluentAssertions.Execution;
using Bunit;

namespace BlazorRamp.DialogFramework.Tests.Unit.Framework;

public class ModalDialogWindow_Tests
{
    [Fact]
    public void Constructor_params_should_set_properties()
    {
        var windowID          = Guid.NewGuid();
        var dialogType        = typeof(SomeForm);
        var dialogParamerters = new ModalDialogParameters<SomeForm>();
        var dialogOptions     = new ModalDialogOptions();
        var somePersonData    = StaticData.ConstructedPersonData();

        dialogParamerters.Add(d => d.SomePersonData, somePersonData);

        var dialogWindow = new ModalDialogWindow(windowID, dialogType, dialogParamerters, dialogOptions);

        using(new AssertionScope())
        {
            dialogWindow.Should().Match<ModalDialogWindow>(d => d.WindowID == windowID && d.DialogType == dialogType);

            var dialogParam = dialogWindow.DialogParameters.First();

            dialogParam.Value.Should().Be(somePersonData);

           dialogWindow.DialogOptions.Should().Match<ModalDialogOptions>(o => o.HorizonalPosition == "center" && o.VerticalPosition == "center" && o.MaxWidth == "70%");
        }
    }

    [Fact]
    public void Dialog_contents_should_return_a_render_fragement()
    {
        var windowID          = Guid.NewGuid();
        var dialogType        = typeof(SomeForm);
        var dialogParamerters = new ModalDialogParameters<SomeForm>();
        var dialogOptions     = new ModalDialogOptions();
        var somePersonData    = StaticData.ConstructedPersonData();

        dialogParamerters.Add(d => d.SomePersonData, somePersonData);

        var dialogWindow = new ModalDialogWindow(windowID, dialogType, dialogParamerters, dialogOptions);

        using (new AssertionScope())
        {
            dialogWindow.DialogContents.Should().NotBeNull();
        }
    }

    [Fact]
    public async Task ModalDialogWindow_ShouldInitialise_WithCorrectDefaults()
    {
        using var context = new BunitContext();

        var windowId         = Guid.NewGuid();
        var dialogType       = typeof(SomeForm);
        var somePersonData   = StaticData.ConstructedPersonData();
        var dialogParameters = new ModalDialogParameters<SomeForm>();
        var dialogOptions    = new ModalDialogOptions();

        dialogParameters.Add(d => d.SomePersonData, somePersonData);

        var dialogWindow = new ModalDialogWindow(windowId, dialogType, dialogParameters, dialogOptions);
        
        var dialogComponent = context.Render<SomeForm>(builder => builder.Add(p => p.SomePersonData, somePersonData));
        
        using (new AssertionScope())
        {

            dialogWindow.ShowDialogTask.Should().NotBeNull();
            dialogWindow.ShowDialogTask.Should().BeSameAs(dialogWindow.TaskSource.Task);
            dialogWindow.ShowDialogTask.IsCompleted.Should().BeFalse();
            dialogWindow.EscapeTrigger.Should().NotBeNull();

            var renderedComponent = context.Render(dialogWindow.DialogContents);

            renderedComponent.Find(".demo-dialog__header").TextContent.Should().Be("Some Form");
        }

    }
}

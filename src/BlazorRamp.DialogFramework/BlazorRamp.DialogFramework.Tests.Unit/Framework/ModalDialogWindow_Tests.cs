using BlazorRamp.DialogFramework.Framework;
using BlazorRamp.DialogFramework.Tests.SharedDataFixtures.Common.Models;
using FluentAssertions;
using FluentAssertions.Execution;

namespace BlazorRamp.DialogFramework.Tests.Unit.Framework;

public class ModalDialogWindow_Tests
{
    [Fact]
    public void Constructor_params_should_set_properties()
    {
        var windowID          = Guid.NewGuid();
        var dialogType        = typeof(FakeDialogComponent);
        var dialogParamerters = new ModalDialogParameters<FakeDialogComponent>();
        var dialogOptions     = new ModalDialogOptions();

        dialogParamerters.Add<string>(d => d.Title, "Dialog Title");

        var dialogWindow = new ModalDialogWindow(windowID, dialogType, dialogParamerters, dialogOptions);

        using(new AssertionScope())
        {
            dialogWindow.Should().Match<ModalDialogWindow>(d => d.WindowID == windowID && d.DialogType == dialogType);

            var dialogParam = dialogWindow.DialogParameters.First();

            dialogParam.Value.Should().Be("Dialog Title");

           dialogWindow.DialogOptions.Should().Match<ModalDialogOptions>(o => o.HorizonalPosition == "center" && o.VerticalPosition == "center" && o.MaxWidth == "70%");
        }
    } 
}

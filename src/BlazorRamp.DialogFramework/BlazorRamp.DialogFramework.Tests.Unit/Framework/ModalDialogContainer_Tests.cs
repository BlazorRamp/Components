using BlazorRamp.DialogFramework.Framework;
using BlazorRamp.DialogFramework.Tests.SharedDataFixtures.Components;
using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.DialogFramework.Tests.Unit.Framework;

public class ModalDialogContainer_Tests
{
    private ModalDialogService CreateService(BunitContext context)
    {
        context.JSInterop.Mode = JSRuntimeMode.Loose;
        context.Services.AddSingleton<ModalDialogService>();

        return context.Services.GetRequiredService<ModalDialogService>();
    }

    [Fact]
    public void Should_render_no_dialogs_when_none_are_open()
    {
        using var context   = new BunitContext();
        var dialogService   = CreateService(context);
        var dialogContainer = context.Render<ModalDialogContainer>();

        dialogContainer.FindAll("dialog").Should().BeEmpty();
    }
    [Fact]
    public void Should_render_a_dialogs_when_one_is_open()
    {
        using var context   = new BunitContext();
        var dialogService   = CreateService(context);
        var task            = dialogService.ShowDialog<SomeForm>();
        var dialogContainer = context.Render<ModalDialogContainer>();

        dialogContainer.FindAll("dialog").Should().HaveCount(1);
    }

    [Fact]
    public void Should_render_a_multiple_dialogs_when_multiple_are_open()
    {
        using var context = new BunitContext();
        var dialogService = CreateService(context);

        var taskOne = dialogService.ShowDialog<SomeForm>();
        var taskTwo = dialogService.ShowDialog<SomeForm>();

        var dialogContainer = context.Render<ModalDialogContainer>();

        dialogContainer.FindAll("dialog").Should().HaveCount(2);
    }

    [Fact]
    public async Task Should_remove_dialog_from_render_when_closed()
    {
        using var context  = new BunitContext();
        var dialogService  = CreateService(context);
        var task            = dialogService.ShowDialog<SomeForm>();
        var dialogContainer = context.Render<ModalDialogContainer>();

        await dialogContainer.InvokeAsync(() => dialogService.CloseDialog(ModalDialogResult.Cancel()));

        dialogContainer.WaitForAssertion(() => dialogContainer.FindAll("dialog").Should().BeEmpty());
    }

    [Fact]
    public void Should_render_dialog_content_inside_dialog_element()
    {
        using var context   = new BunitContext();
        var dialogService   = CreateService(context);
        var task            = dialogService.ShowDialog<SomeForm>();
        var dialogContainer = context.Render<ModalDialogContainer>();

        dialogContainer.FindAll(".demo-dialog").Should().HaveCount(1);
    }

    [Fact]
    public void Should_set_the_dialog_id_attribute_to_the_window_id()
    {
        using var context   = new BunitContext();
        var dialogService   = CreateService(context);
        var task            = dialogService.ShowDialog<SomeForm>();
        var dialogContainer = context.Render<ModalDialogContainer>();

        dialogContainer.Find("dialog").GetAttribute("id").Should().Be(dialogService.DialogWindows[0].WindowID.ToString());
    }
    [Fact]
    public void Should_set_the_dialog_aria_labelledby_attribute()
    {
        using var context   = new BunitContext();
        var dialogService   = CreateService(context);
        var task            = dialogService.ShowDialog<SomeForm>();
        var dialogContainer = context.Render<ModalDialogContainer>();

        dialogContainer.Find("dialog").GetAttribute("aria-labelledby").Should().Be(dialogService.GetAriaLabelledByID());
    }

    [Fact]
    public async Task Should_invoke_the_escape_key_handler_when_the_escape_key_is_pressed()
    {
        using var context   = new BunitContext();
        var dialogService   = CreateService(context);
        var task            = dialogService.ShowDialog<SomeForm>();
        var dialogContainer = context.Render<ModalDialogContainer>();
        bool handlerInvoked = false;

        Func<Task> handler = () => { handlerInvoked = true; return Task.CompletedTask; };
        dialogService.RegisterEscapeHandler(handler);

        await dialogContainer.Find("dialog").KeyDownAsync(new KeyboardEventArgs { Key = "Escape" });

        handlerInvoked.Should().BeTrue();
    }

    [Fact]
    public async Task Should_only_handle_escape_for_the_top_most_dialog()
    {
        using var context   = new BunitContext();
        var dialogService   = CreateService(context);
        var taskOne         = dialogService.ShowDialog<SomeForm>();
        var taskTwo         = dialogService.ShowDialog<SomeForm>();
        var dialogContainer = context.Render<ModalDialogContainer>();
        bool handlerInvoked = false;

        await dialogContainer.FindAll("dialog").First().KeyDownAsync(new KeyboardEventArgs { Key = "Escape" });

        handlerInvoked.Should().BeFalse();
    }

    
}

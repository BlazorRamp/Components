using BlazorRamp.DialogFramework.Common.Constants;
using BlazorRamp.DialogFramework.Framework;
using BlazorRamp.DialogFramework.Tests.SharedDataFixtures.Common.Data;
using BlazorRamp.DialogFramework.Tests.SharedDataFixtures.Common.Models;
using BlazorRamp.DialogFramework.Tests.SharedDataFixtures.Components;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
namespace BlazorRamp.DialogFramework.Tests.Unit.Framework;

public class ModalDialogService_Tests
{
    public class ShowDialog()
    {
        private ModalDialogService CreateService(BunitContext context)
        {
            context.JSInterop.Mode = JSRuntimeMode.Loose;
            context.Services.AddSingleton<ModalDialogService>();

            return context.Services.GetRequiredService<ModalDialogService>();
        }

        private ModalDialogParameters<SomeForm> CreateDialogParameters()
        {
            var dialogParameters = new ModalDialogParameters<SomeForm>();

            dialogParameters.Add(s => s.SomePersonData, StaticData.ConstructedPersonData());

            return dialogParameters;
        }


        [Fact]
        public async Task Should_return_pending_task() 
        {
            using var context    = new BunitContext();
            var dialogOptions    = new ModalDialogOptions();
            var dialogParameters = CreateDialogParameters();
            var dialogService    = CreateService(context);

            var task = dialogService.ShowDialog<SomeForm>(dialogParameters, dialogOptions);

            task.Status.Should().Be(TaskStatus.WaitingForActivation);

        }

        [Fact]
        public async Task Should_throw_an_argument_exception_if_the_type_is_not_razor_component()
        {
            using var context    = new BunitContext();
            var dialogOptions    = new ModalDialogOptions();
            var dialogParameters = CreateDialogParameters();
            var dialogService    = CreateService(context);

            await FluentActions.Invoking(() => dialogService.ShowDialog<FakeDialogComponent>()).Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task Should_notify_state_changed()
        {
            using var context    = new BunitContext();
            var dialogOptions    = new ModalDialogOptions();
            var dialogParameters = CreateDialogParameters();
            var dialogService    = CreateService(context);

            bool notified = false;

            dialogService.OnChanged += () => notified = true;

            _ = dialogService.ShowDialog<SomeForm>(dialogParameters, dialogOptions);

            notified.Should().BeTrue();

        }
        [Fact]
        public async Task Should_add_widnow_to_collection()
        {
            using var context    = new BunitContext();
            var dialogOptions    = new ModalDialogOptions();
            var dialogParameters = CreateDialogParameters();
            var dialogService    = CreateService(context);

            _ = dialogService.ShowDialog<SomeForm>(dialogParameters, dialogOptions);

            dialogService.DialogWindows.Count.Should().Be(1);

        }

                [Fact]
        public async Task Without_dialog_parameters_should_use_an_empty_dialog_parameters_collection()
        {
            using var context    = new BunitContext();
            var dialogOptions    = new ModalDialogOptions();
            var dialogService    = CreateService(context);

            _ = dialogService.ShowDialog<SomeForm>(dialogOptions);

            dialogService.DialogWindows[0].DialogParameters.Should().BeEmpty();

        }

        [Fact]
        public async Task Without_dialog_options_should_use_the_default_options()
        {
            using var context    = new BunitContext();
            var dialogParameters = CreateDialogParameters();
            var dialogService    = CreateService(context);

            _ = dialogService.ShowDialog<SomeForm>(dialogParameters);

            dialogService.DialogWindows[0].DialogOptions.Should().Match<ModalDialogOptions>(o => o.HorizonalPosition == "center" && o.VerticalPosition == "center" 
                                                                                            && o.MaxWidth == "70%");

        }
        [Fact]
        public async Task Should_use_defaults_when_paramters_and_or_options_are_null()
        {
            using var context = new BunitContext();
            var dialogService = CreateService(context);

            _ = dialogService.ShowDialog<SomeForm>(null!, null!);

            using (new AssertionScope())
            {
                dialogService.DialogWindows[0].DialogOptions.Should().Match<ModalDialogOptions>(o => o.HorizonalPosition == "center" && o.VerticalPosition == "center"
                                                                                                && o.MaxWidth == "70%");

                dialogService.DialogWindows[0].DialogParameters.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task With_no_params_should_use_empty_params_and_default_options()
        {
            using var context = new BunitContext();
            var dialogService = CreateService(context);

            _ = dialogService.ShowDialog<SomeForm>();

            using (new AssertionScope())
            {
                dialogService.DialogWindows[0].DialogOptions.Should().Match<ModalDialogOptions>(o => o.HorizonalPosition == "center" && o.VerticalPosition == "center"
                                                                                                && o.MaxWidth == "70%");

                dialogService.DialogWindows[0].DialogParameters.Should().BeEmpty();
            }
        }
    }

    public class CloseDialog()
    {
        private ModalDialogService CreateService(BunitContext context)
        {
            context.JSInterop.Mode = JSRuntimeMode.Loose;
            context.Services.AddSingleton<ModalDialogService>();

            return context.Services.GetRequiredService<ModalDialogService>();
        }

        private ModalDialogParameters<SomeForm> CreateDialogParameters()
        {
            var dialogParameters = new ModalDialogParameters<SomeForm>();

            dialogParameters.Add(s => s.SomePersonData, StaticData.ConstructedPersonData());

            return dialogParameters;
        }

        [Fact]
        public async Task Should_complete_the_show_dialog_task_with_the_result()
        {
            using var context    = new BunitContext();
            var dialogOptions    = new ModalDialogOptions();
            var dialogParameters = CreateDialogParameters();
            var dialogService    = CreateService(context);

            var task         = dialogService.ShowDialog<SomeForm>(dialogParameters, dialogOptions);
            var dialogResult = ModalDialogResult.OK();

            await dialogService.CloseDialog(dialogResult);

            using(new AssertionScope())
            {
                task.Status.Should().Be(TaskStatus.RanToCompletion);
                (await task).Should().Be(dialogResult);
            }

        }

        [Fact]
        public async Task Should_return_without_error_if_there_is_no_window_to_closet()
        {
            using var context = new BunitContext();
            var dialogService = CreateService(context);
            var dialogResult  = ModalDialogResult.OK();

            await FluentActions.Invoking(() => dialogService.CloseDialog(dialogResult)).Should().NotThrowAsync();

        }

        [Fact]
        public async Task Should_close_the_top_most_window()
        {
            using var context    = new BunitContext();
            var dialogOptions    = new ModalDialogOptions();
            var dialogParameters = CreateDialogParameters();
            var dialogService    = CreateService(context);


            var taskOne = dialogService.ShowDialog<SomeForm>(dialogParameters, dialogOptions);
            var taskTwo = dialogService.ShowDialog<SomeForm>(dialogParameters, dialogOptions);

            await dialogService.CloseDialog(ModalDialogResult.Cancel());

            using(new AssertionScope())
            {
                taskOne.Status.Should().Be(TaskStatus.WaitingForActivation);
                taskTwo.Status.Should().Be(TaskStatus.RanToCompletion);

                dialogService.DialogWindows.Should().HaveCount(1);  
            }

        }
    }

    public class Subscriptions()
    {
        private ModalDialogService CreateService(BunitContext context)
        {
            context.JSInterop.Mode = JSRuntimeMode.Loose;
            context.Services.AddSingleton<ModalDialogService>();

            return context.Services.GetRequiredService<ModalDialogService>();
        }

        [Fact]
        public void Should_be_able_to_subscribe_for_update_notifications()
        {
            using var context = new BunitContext();

            var dialogService = CreateService(context);

            bool notified = false;

            var handler = () => { notified = true; };

            dialogService.SubscribeToUpdates(handler);

            _ = dialogService.ShowDialog<SomeForm>(null!, null!);

            notified.Should().BeTrue();

        }
        [Fact]
        public void Should_be_able_to_unsubscribe_from_update_notifications()
        {
            using var context = new BunitContext();

            var dialogService = CreateService(context);

            bool notified = false;

            var handler = () => { notified = true; };

            dialogService.SubscribeToUpdates(handler);
            dialogService.UnsubscribeFromUpdates(handler);

            _ = dialogService.ShowDialog<SomeForm>(null!, null!);

            notified.Should().BeFalse();

        }
    }


    public class EscapeKeyRegistrations()
    {
        private ModalDialogService CreateService(BunitContext context)
        {
            context.JSInterop.Mode = JSRuntimeMode.Loose;
            context.Services.AddSingleton<ModalDialogService>();

            return context.Services.GetRequiredService<ModalDialogService>();
        }

        [Fact]
        public async Task Register_escape_handler_Should_invoke_handler_when_escape_is_raised()
        {
            using var context  = new BunitContext();
            var dialogService  = CreateService(context);
            var handlerInvoked = false;

            var task = dialogService.ShowDialog<SomeForm>();

            Func<Task> handler = () => { handlerInvoked = true; return Task.CompletedTask; };

            dialogService.RegisterEscapeHandler(handler);
    
            await dialogService.DialogWindows.Last().EscapeTrigger.RaiseEscapeKeyPressed();

            handlerInvoked.Should().BeTrue();
        }
        [Fact]
        public async Task Unrgister_escape_handler_Should_remove_the_handler_so_no_notifications_are_received()
        {
            using var context = new BunitContext();
            var dialogService = CreateService(context);
            var handlerInvoked = false;

            var task = dialogService.ShowDialog<SomeForm>();

            Func<Task> handler = () => { handlerInvoked = true; return Task.CompletedTask; };

            dialogService.RegisterEscapeHandler(handler);
            dialogService.UnregisterEscapeHandler(handler);

            await dialogService.DialogWindows.Last().EscapeTrigger.RaiseEscapeKeyPressed();

            handlerInvoked.Should().BeFalse();
        }
    }


    public class GetAriaLabelledByID()
    {
        private ModalDialogService CreateService(BunitContext context)
        {
            context.JSInterop.Mode = JSRuntimeMode.Loose;
            context.Services.AddSingleton<ModalDialogService>();

            return context.Services.GetRequiredService<ModalDialogService>();
        }

        [Fact]
        public async Task Should_return_a_unique_string_guid_for_the_window()
        {
            using var context = new BunitContext();
            var dialogService = CreateService(context);
            
            var task = dialogService.ShowDialog<SomeForm>();

            var windowID = dialogService.GetAriaLabelledByID();

            using(new AssertionScope())
            {
                windowID.Should().NotBeNullOrWhiteSpace();

                windowID.Should().StartWith("dialog-");
                
                Guid.Parse(windowID.Substring("dialog-".Length)).Should().NotBeEmpty();
            }
        }
        [Fact]
        public async Task Should_return_a_non_unique_label_if_there_are_no_windows()
        {
            using var context = new BunitContext();
            var dialogService = CreateService(context);

            dialogService.GetAriaLabelledByID().Should().Be("dialog-");
        }
    }


    public class JsOpenModalDialog()
    {
        private ModalDialogService CreateService(BunitContext context)
        {
            context.JSInterop.Mode = JSRuntimeMode.Loose;
            context.Services.AddSingleton<ModalDialogService>();

            return context.Services.GetRequiredService<ModalDialogService>();
        }
        [Fact]
        public async Task Js_open_modal_dialog_should_do_nothing_when_no_dialogs_are_open()
        {
            using var context = new BunitContext();
            var dialogService = CreateService(context);

            await FluentActions.Invoking(() => dialogService.JsOpenModalDialog()).Should().NotThrowAsync();
        }

        [Fact]
        public async Task Js_open_modal_dialog_should_invoke_js_open_with_topmost_window_id()
        {
            using var context = new BunitContext();
            var dialogService = CreateService(context);
            
            var task = dialogService.ShowDialog<SomeForm>();

            var windowId = dialogService.DialogWindows.Last().WindowID.ToString();

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JavaScript_File_Path);

            moduleInterop.SetupVoid(GlobalValues.JavaScript_Open_Modal_Func, windowId).SetVoidResult();

            await dialogService.JsOpenModalDialog();

            moduleInterop.VerifyInvoke(GlobalValues.JavaScript_Open_Modal_Func).Arguments.First().Should().Be(windowId);
        }
    }

}

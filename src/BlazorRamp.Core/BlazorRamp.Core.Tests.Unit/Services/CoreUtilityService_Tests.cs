using BlazorRamp.Core.Common.Constants;
using BlazorRamp.Core.Services;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;


namespace BlazorRamp.Core.Tests.Unit.Services;

public class CoreUtilityService_Tests
{

    public class RegisterContainerForAutoTabindex
    {
        [Fact]
        public async Task Should_import_module_and_invoke_register_with_element_label_and_add_region()
        {
            using var context = new BunitContext();

            var jsInterop = context.JSInterop;
            var moduleInterop = jsInterop.SetupModule(CoreGlobalValues.JS_Utils_File_Path);
            var utilityService = new CoreUtilityService(jsInterop.JSRuntime);

            var container = new ElementReference("scroll-container");
            var labelName = "Results table";
            var addRegion = false;

            moduleInterop.SetupVoid(CoreGlobalValues.JS_Utils_Register_Container_For_Tab_Func, container, labelName, addRegion)
                            .SetVoidResult();

            await utilityService.RegisterContainerForAutoTabindex(container, labelName, addRegion);

            using (new AssertionScope())
            {
                jsInterop.Invocations.Should().Contain(i => i.Identifier == "import" && i.Arguments[0]!.ToString() == CoreGlobalValues.JS_Utils_File_Path);

                var invocation = moduleInterop.VerifyInvoke(CoreGlobalValues.JS_Utils_Register_Container_For_Tab_Func);

                invocation.Arguments[0].Should().Be(container);
                invocation.Arguments[1].Should().Be(labelName);
                invocation.Arguments[2].Should().Be(addRegion);
            }
        }

        [Fact]
        public async Task Should_use_default_label_and_add_region_when_not_supplied()
        {
            using var context = new BunitContext();

            var jsInterop = context.JSInterop;
            var moduleInterop = jsInterop.SetupModule(CoreGlobalValues.JS_Utils_File_Path);
            var utilityService = new CoreUtilityService(jsInterop.JSRuntime);

            var container = new ElementReference("scroll-container");

            moduleInterop.SetupVoid(CoreGlobalValues.JS_Utils_Register_Container_For_Tab_Func, container, "Scroll region", true)
                            .SetVoidResult();

            await utilityService.RegisterContainerForAutoTabindex(container);

            var invocation = moduleInterop.VerifyInvoke(CoreGlobalValues.JS_Utils_Register_Container_For_Tab_Func);

            using (new AssertionScope())
            {
                invocation.Arguments[1].Should().Be("Scroll region");
                invocation.Arguments[2].Should().Be(true);
            }
        }

        [Fact]
        public async Task Should_only_import_the_js_module_once_when_called_multiple_times()
        {
            using var context = new BunitContext();

            var jsInterop = context.JSInterop;
            var moduleInterop = jsInterop.SetupModule(CoreGlobalValues.JS_Utils_File_Path);
            var utilityService = new CoreUtilityService(jsInterop.JSRuntime);

            var firstContainer = new ElementReference("first-container");
            var secondContainer = new ElementReference("second-container");

            moduleInterop.SetupVoid(CoreGlobalValues.JS_Utils_Register_Container_For_Tab_Func, _ => true)
                            .SetVoidResult();

            await utilityService.RegisterContainerForAutoTabindex(firstContainer);
            await utilityService.RegisterContainerForAutoTabindex(secondContainer);

            using (new AssertionScope())
            {
                jsInterop.Invocations.Count(i => i.Identifier == "import").Should().Be(1);

                moduleInterop.VerifyInvoke(CoreGlobalValues.JS_Utils_Register_Container_For_Tab_Func, calledTimes: 2);
            }
        }
    }


    public class UnregisterContainerForAutoTabindex
    {
        [Fact]
        public async Task Should_invoke_unregister_with_the_element_reference()
        {
            using var context = new BunitContext();

            var jsInterop = context.JSInterop;
            var moduleInterop = jsInterop.SetupModule(CoreGlobalValues.JS_Utils_File_Path);
            var utilityService = new CoreUtilityService(jsInterop.JSRuntime);

            var container = new ElementReference("scroll-container");

            moduleInterop.SetupVoid(CoreGlobalValues.JS_Utils_Unregister_Container_For_Tab_Func, container)
                            .SetVoidResult();

            await utilityService.UnregisterContainerForAutoTabindex(container);

            using (new AssertionScope())
            {
                jsInterop.Invocations.Should().Contain(i => i.Identifier == "import" && i.Arguments[0]!.ToString() == CoreGlobalValues.JS_Utils_File_Path);

                moduleInterop.VerifyInvoke(CoreGlobalValues.JS_Utils_Unregister_Container_For_Tab_Func).Arguments[0].Should().Be(container);
            }
        }

        [Fact]
        public async Task Should_not_throw_if_the_js_runtime_is_disconnected()
        {
            var utilityService = new CoreUtilityService(new FakeJSRuntime(new DisconnectedModule()));

            var container = new ElementReference("scroll-container");

            await FluentActions.Awaiting(() => utilityService.UnregisterContainerForAutoTabindex(container).AsTask()).Should().NotThrowAsync();
        }

        // Hand-rolled fakes: the module imports fine, but every call on it throws as if the circuit had gone away
        private sealed class DisconnectedModule : IJSObjectReference
        {
            public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
                => ValueTask.FromException<TValue>(new JSDisconnectedException("Circuit disconnected"));

            public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
                => ValueTask.FromException<TValue>(new JSDisconnectedException("Circuit disconnected"));

            public ValueTask DisposeAsync() => ValueTask.CompletedTask;
        }

        private sealed class FakeJSRuntime(IJSObjectReference module) : IJSRuntime
        {
            public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
                => new((TValue)(object)module);

            public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
                => new((TValue)(object)module);
        }
    }


    public class DisposeAsync
    {
        [Fact]
        public async Task Should_be_idempotent_and_not_throw_if_called_more_than_once()
        {
            using var context = new BunitContext();

            var utilityService = new CoreUtilityService(context.JSInterop.JSRuntime);

            await utilityService.DisposeAsync();

            await FluentActions.Awaiting(() => utilityService.DisposeAsync().AsTask()).Should().NotThrowAsync();
        }

        [Fact]
        public async Task Should_not_throw_if_the_module_was_never_loaded()
        {
            var utilityService = new CoreUtilityService(null!);

            await FluentActions.Awaiting(() => utilityService.DisposeAsync().AsTask()).Should().NotThrowAsync();
        }

        [Fact]
        public async Task Should_dispose_js_module_after_use_without_throwing()
        {
            using var context = new BunitContext();

            var jsInterop = context.JSInterop;
            var moduleInterop = jsInterop.SetupModule(CoreGlobalValues.JS_Utils_File_Path);
            var utilityService = new CoreUtilityService(jsInterop.JSRuntime);

            var container = new ElementReference("scroll-container");

            moduleInterop.SetupVoid(CoreGlobalValues.JS_Utils_Register_Container_For_Tab_Func, _ => true)
                            .SetVoidResult();

            await utilityService.RegisterContainerForAutoTabindex(container);

            await FluentActions.Awaiting(() => utilityService.DisposeAsync().AsTask()).Should().NotThrowAsync();
        }
    }
}
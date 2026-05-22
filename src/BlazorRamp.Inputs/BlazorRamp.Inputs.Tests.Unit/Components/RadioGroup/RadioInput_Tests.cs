using BlazorRamp.Inputs.Common.Constants;
using BlazorRamp.Inputs.Components.RadioGroup;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.Inputs.Tests.Unit.Components.RadioGroup;

public class RadioInput_Tests
{
    internal class TestModel
    {
        [Required(ErrorMessage = "Please select a value.")]
        [Range(1, 3, ErrorMessage = "Must be one of the options.")]
        public int IntValue { get; set; } = 0;

        public string? StringValue { get; set; } = null;

        public int? NullableIntValue { get; set; } = null;
    }

    public static (IRenderedComponent<RadioInputGroup<int>> Component, EditContext EditContext) CreateIntRadioInputGroup(
     BunitContext context,
     Action<ComponentParameterCollectionBuilder<RadioInputGroup<int>>>? parameters = null)
    {
        var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);
        moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();
        moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Select_Readonly_Disabled_Handlers, _ => true).SetVoidResult();
        moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Unregister_Select_Readonly_Disabled_Handlers, _ => true).SetVoidResult();

        var model = new TestModel();
        var editContext = new EditContext(model);

        editContext.EnableDataAnnotationsValidation(context.Services);

        var component = context.Render<RadioInputGroup<int>>(
            builder =>
            {
                builder
                    .AddCascadingValue(editContext)
                    .Add(p => p.Value, model.IntValue)
                    .Add(p => p.ValueChanged, EventCallback.Factory.Create<int>(context, v => model.IntValue = v))
                    .Add(p => p.ValueExpression, () => model.IntValue);

                parameters?.Invoke(builder);
            });

        return (component, editContext);
    }

    public class Parameters
    {
        [Fact]
        public async Task Should_be_able_to_set_the_label_text()
        {
            using var context = new BunitContext();

            var lableText = "Option Ten";

            var (parentComponent, _) = CreateIntRadioInputGroup(context, parentBuilder =>
            {
                parentBuilder.Add<RadioInput<int>>(p => p.OptionValues, childBuilder =>
                {
                    childBuilder.Add(c => c.LabelText, lableText).Add(c => c.Value, 10);
                });
            });

            using (new AssertionScope())
            {
                parentComponent.FindComponent<RadioInput<int>>().Instance.LabelText.Should().Be(lableText);

                parentComponent.Find($"label.{GlobalValues.Radio_Input_Label_Class}").TextContent.Should().Be(lableText);
            }
        }

        [Fact]
        public async Task Should_throw_an_exception_if_the_label_text_is_not_provided()
        {
            using var context = new BunitContext();

            FluentActions.Invoking(() =>
            {
                CreateIntRadioInputGroup(context, parentBuilder =>
                {
                    parentBuilder.Add<RadioInput<int>>(p => p.OptionValues, childBuilder =>
                    {
                        childBuilder.Add(c => c.Value, 10);
                    });
                });
            }).Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public async Task Should_capture_unmatched_attributes_and_add_only_the_class_to_the_container_class_list_not_the_input()
        {
            using var context = new BunitContext();

            var lableText = "Option Ten";

            var (parentComponent, _) = CreateIntRadioInputGroup(context, parentBuilder =>
            {
                parentBuilder.Add<RadioInput<int>>(p => p.OptionValues, childBuilder =>
                {
                    childBuilder
                    .Add(c => c.LabelText, lableText)
                    .Add(c => c.Value, 10)
                    .AddUnmatched("class", "test")
                    .AddUnmatched("style", "color:red;");
                });
            });

            var inputContainer = parentComponent.Find($"div.{GlobalValues.Radio_Input_Class}");

            using(new AssertionScope())
            {
                inputContainer.GetAttribute("class").Should().Contain("test");
                inputContainer.GetAttribute("style").Should().BeNull();
            }

        }

        [Fact]
        public async Task Should_capture_unmatched_attributes_and_add_non_class_attributes_to_the_input()
        {
            using var context = new BunitContext();

            var lableText = "Option Ten";

            var (parentComponent, _) = CreateIntRadioInputGroup(context, parentBuilder =>
            {
                parentBuilder.Add<RadioInput<int>>(p => p.OptionValues, childBuilder =>
                {
                    childBuilder
                    .Add(c => c.LabelText, lableText)
                    .Add(c => c.Value, 10)
                    .AddUnmatched("class", "test")
                    .AddUnmatched("style", "color:red;");
                });
            });

            var input = parentComponent.Find("input");

            using (new AssertionScope())
            {
                input.GetAttribute("class").Should().NotContain("test");
                input.GetAttribute("style").Should().Contain("color:red;");
            }

        }

    }
}

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
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.Inputs.Tests.Unit.Components.RadioGroup;

public class RadioInputGroup_Tests
{
    internal class TestModel
    {
        [Required(ErrorMessage = "Please select a value.")]
        [Range(1, 3, ErrorMessage = "Must be one of the options.")]
        public int IntValue { get; set; } = 0;

        public string? StringValue { get; set; } = null;

        public int? NullableIntValue { get; set; } = null;
    }

    internal class GenericPropertyModel<TValue>()
    {
        public TValue GenericValue { get; set; } = default!;
    }

    private static RenderFragment BuildRadioOptions() => builder =>
    {
        builder.OpenComponent<RadioInput<int>>(0);
        builder.AddAttribute(1, "LabelText", "First item");
        builder.AddAttribute(2, "Value", 1);
        builder.CloseComponent();

        builder.OpenComponent<RadioInput<int>>(3);
        builder.AddAttribute(4, "LabelText", "Second item");
        builder.AddAttribute(5, "Value", 2);
        builder.CloseComponent();

        builder.OpenComponent<RadioInput<int>>(6);
        builder.AddAttribute(7, "LabelText", "Third item");
        builder.AddAttribute(8, "Value", 3);
        builder.CloseComponent();
    };
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
                    .Add(p => p.ValueExpression, () => model.IntValue)
                    .Add(p => p.OptionValues, BuildRadioOptions());

                parameters?.Invoke(builder);
            });

        return (component, editContext);
    }


    public class Parameters
    {

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_be_able_to_set_required_which_uses_aria_required_if_true(bool required)
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateIntRadioInputGroup(context, p => p.Add(x => x.Required, required));

            var ariaAttribute = inputComponent.Find("div").GetAttribute("aria-required");

            if (required) ariaAttribute.Should().Be("true");

            if (!required) ariaAttribute.Should().BeNull();
        }



        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("MyControl")]
        public async Task Should_be_able_to_set_the_control_id_param_or_have_a_guid_string_if_null_empty_or_whitespace(string? controlID)
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateIntRadioInputGroup(context, p => p.Add(x => x.ControlID, controlID));

            var idAttribute = inputComponent.Find("div").GetAttribute("id");

            if (String.IsNullOrWhiteSpace(controlID)) Guid.Parse(idAttribute!).Should().NotBeEmpty();

            if (!String.IsNullOrWhiteSpace(controlID)) idAttribute.Should().Be(controlID);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("MyControl")]
        public async Task Should_be_able_to_set_the_label_text_which_defaults_to_the_field_identity_if_null_empty_or_whitespace(string? labelText)
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateIntRadioInputGroup(context, p => p.Add(x => x.LabelText, labelText).Add(x => x.Required, false));

            var labelContent = inputComponent.Find($"span.{GlobalValues.Radio_Input_Group_Label_Class}").TextContent;

            if (String.IsNullOrWhiteSpace(labelText)) labelContent.Should().Be("IntValue");

            if (!String.IsNullOrWhiteSpace(labelText)) labelContent.Should().Be(labelText);
        }



        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("MyControl")]
        public async Task Label_text_with_required_set_to_true_should_have_a_space_and_asterisk_appended_to_its_valued(string? labelText)
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateIntRadioInputGroup(context, p => p.Add(x => x.LabelText, labelText).Add(x => x.Required, true));

            var labelContent = inputComponent.Find($"span.{GlobalValues.Radio_Input_Group_Label_Class}").TextContent;

            if (String.IsNullOrWhiteSpace(labelText)) labelContent.Should().Be("IntValue *");

            if (!String.IsNullOrWhiteSpace(labelText)) labelContent.Should().Be(labelText + " *");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("My hint text")]
        [InlineData("My hint text.")]
        public async Task Should_be_able_to_set_the_hint_text_normalised_so_it_ends_with_a_full_stop(string? hintText)
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateIntRadioInputGroup(context, p => p.Add(x => x.HintText, hintText));

            var hints = inputComponent.FindAll($".{GlobalValues.Radio_Input_Group_Hint_Class}");

            if (String.IsNullOrWhiteSpace(hintText)) hints.Should().BeEmpty();

            if (!String.IsNullOrWhiteSpace(hintText))
            {
                using (new AssertionScope())
                {
                    hints[0].TextContent.Should().Contain(hintText);
                    hints[0].TextContent.Should().EndWith(".");
                }
            }
        }


        [Fact]
        public async Task Should_capture_unmatched_attributes_and_add_class_to_the_text_input_class_list()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateIntRadioInputGroup(context, p => p.AddUnmatched("class", "test").AddUnmatched("style", "color:red;"));

            using (new AssertionScope())
            {
                inputComponent.Instance.AdditionalAttributes.Should().ContainKey("class").WhoseValue.Should().Be("test");

                var divElement = inputComponent.Find("div");

                divElement.GetAttribute("style").Should().Be("color:red;");
                divElement.ClassList.Should().Contain("test");

            }
        }

        [Fact]
        public async Task Should_have_a_tabbable_region_for_errors_when_the_Validation_display_mode_is_set_to_tabbable_with_hint()
        {
            await using var context = new BunitContext();

            var (inputComponent,editContext) = CreateIntRadioInputGroup(context, p => p.Add(x => x.ValidationDisplayMode, ValidationDisplayMode.TabbableWithHint));


            await inputComponent.InvokeAsync(() => inputComponent.Instance.SetGroupValue(4));

            await inputComponent.InvokeAsync(() => editContext.Validate());


            inputComponent.WaitForAssertion(() =>
            { 
                var errorDiv = inputComponent.Find($"div.{GlobalValues.Radio_Input_Group_Error_Class}");

                using (new AssertionScope())
                {
                    errorDiv.GetAttribute("tabindex").Should().Be("0");
                    errorDiv.GetAttribute("role").Should().Be("region");

                    inputComponent.Find("div").GetAttribute("aria-invalid").Should().Be("true");
                }
            });
        }

        
    }
    public class Properties()
    {
        [Fact]
        public async Task Should_be_able_to_get_the_control_reference_for_the_underlying_input()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateIntRadioInputGroup(context);

            inputComponent.Instance.ControlReference.Should().NotBeNull();
        }
    }
}

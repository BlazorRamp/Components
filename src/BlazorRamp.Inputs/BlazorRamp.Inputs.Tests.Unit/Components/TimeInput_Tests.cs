using BlazorRamp.Inputs.Common.Constants;
using BlazorRamp.Inputs.Components;
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

namespace BlazorRamp.Inputs.Tests.Unit.Components;

public class TimeInput_Tests
{
    internal class TestModel
    {
        [Range(typeof(TimeOnly), "09:00", "17:00", ErrorMessage = "Time must be between 9 AM and 5 PM.")]
        public TimeOnly TimeValue          { get; set; } = TimeOnly.FromTimeSpan(new TimeSpan(13, 0, 0));
        public TimeOnly? NullableTimeValue { get; set; } = null;
    }

    public static (IRenderedComponent<TimeInput<TimeOnly>> Component, EditContext EditContext) CreateTimeInput(
    BunitContext context,
    Action<ComponentParameterCollectionBuilder<TimeInput<TimeOnly>>>? parameters = null)
    {
        var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);
        moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();
        moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Readonly_Handlers, _ => true).SetVoidResult();

        var model = new TestModel();
        var editContext = new EditContext(model);

        editContext.EnableDataAnnotationsValidation(context.Services);

        var component = context.Render<TimeInput<TimeOnly>>(
            builder =>
            {
                builder
                    .AddCascadingValue(editContext)
                    .Add(p => p.Value, model.TimeValue)
                    .Add(p => p.ValueChanged, EventCallback.Factory.Create<TimeOnly>(context, v => model.TimeValue = v))
                    .Add(p => p.ValueExpression, () => model.TimeValue);

                parameters?.Invoke(builder);
            });

        return (component, editContext);
    }

    public class Parameters
    {
        [Theory]
        [InlineData(DataPosition.End)]
        [InlineData(DataPosition.Centre)]
        [InlineData(DataPosition.Start)]
        public void Should_be_able_to_set_the_data_position(DataPosition dataPosition)
        {
            using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.DataPosition, dataPosition));

            var dataAttribute = inputComponent.Find($"div.{GlobalValues.Time_Input_Field_Area_Class}").GetAttribute("data-br-input-position");

            dataAttribute.Should().Be(dataPosition.ToString().ToLower());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_be_able_to_set_required_which_uses_aria_required_if_true(bool required)
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.Required, required));

            var ariaAttribute = inputComponent.Find("input").GetAttribute("aria-required");

            if (required) ariaAttribute.Should().Be("true");

            if (!required) ariaAttribute.Should().BeNull();
        }


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_be_able_to_set_the_readonly_attribute_when_true(bool readOnly)
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.ReadOnly, readOnly));

            var readOnlyAttribute = inputComponent.Find("input").GetAttribute("readonly");

            if (readOnly) readOnlyAttribute.Should().Be("readonly");

            if (!readOnly) readOnlyAttribute.Should().BeNull();
        }


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_be_able_to_set_the_aria_disabled_attribute_when_true(bool disabled)
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.AriaDisabled, disabled));

            var disabledAttribute = inputComponent.Find("input").GetAttribute("aria-disabled");

            if (disabled) disabledAttribute.Should().Be("true");

            if (!disabled) disabledAttribute.Should().BeNull();
        }


        [Fact]
        public async Task Should_only_use_the_readonly_attribute_if_both_readonly_and_aria_disabled_are_set_to_true()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.AriaDisabled, true).Add(x => x.ReadOnly, true));
            var readonlyAttribute = inputComponent.Find("input").GetAttribute("readonly");
            var ariaDisabledAttribute = inputComponent.Find("input").GetAttribute("aria-disabled");


            using (new AssertionScope())
            {
                readonlyAttribute.Should().Be("readonly");
                ariaDisabledAttribute.Should().BeNull();
            }
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("MyControl")]
        public async Task Should_be_able_to_set_the_control_id_param_or_have_a_guid_string_if_null_empty_or_whitespace(string? controlID)
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.ControlID, controlID));

            var idAttribute = inputComponent.Find("input").GetAttribute("id");

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

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.LabelText, labelText).Add(x => x.Required, false));

            var labelContent = inputComponent.Find("label").TextContent;

            if (String.IsNullOrWhiteSpace(labelText)) labelContent.Should().Be("TimeValue");

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

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.LabelText, labelText).Add(x => x.Required, true));

            var labelContent = inputComponent.Find("label").TextContent;

            if (String.IsNullOrWhiteSpace(labelText)) labelContent.Should().Be("TimeValue *");

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

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.HintText, hintText));

            var hints = inputComponent.FindAll($".{GlobalValues.Time_Input_Hint_Class}");

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
        public async Task Should_have_a_tabbable_region_for_errors_when_the_Validation_display_mode_is_set_to_tabbable_with_hint()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.ValidationDisplayMode, ValidationDisplayMode.TabbableWithHint));

            var input = inputComponent.Find("input");//tried editContext.Validate but that did not get the markup to render 

            input.Change("18:00");//need to change from default 13:00 to a failing time of 18:00

            inputComponent.WaitForAssertion(() =>
            {
   
                // Use FindAll first to debug if it exists at all
                var errorDiv = inputComponent.Find($".{GlobalValues.Time_Input_Error_Class}");

                using (new AssertionScope())
                {
                    errorDiv.GetAttribute("tabindex").Should().Be("0");
                    errorDiv.GetAttribute("role").Should().Be("region");

                    input.GetAttribute("aria-invalid").Should().Be("true");
                }
            });
        }




        [Fact]
        public async Task Should_capture_unmatched_attributes_and_apply_all_to_the_input_element_excluding_class()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.AddUnmatched("class", "test").AddUnmatched("style", "color:red;"));

            using (new AssertionScope())
            {
                inputComponent.Instance.AdditionalAttributes.Should().ContainKey("style").WhoseValue.Should().Be("color:red;");

                var inputElement = inputComponent.Find("input");

                inputElement.GetAttribute("style").Should().Be("color:red;");
                inputElement.ClassList.Should().NotContain("test");

            }
        }


        [Fact]
        public async Task Should_capture_unmatched_attributes_and_add_class_to_the_time_input_class_list()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.AddUnmatched("class", "test").AddUnmatched("style", "color:red;"));

            using (new AssertionScope())
            {
                inputComponent.Instance.AdditionalAttributes.Should().ContainKey("class").WhoseValue.Should().Be("test");

                var textInputElement = inputComponent.Find("div");

                textInputElement.GetAttribute("style").Should().BeNull();
                textInputElement.ClassList.Should().Contain("test");

            }
        }


        [Fact]
        public async Task Should_be_able_to_set_the_error_label_used_for_tabbable_error_regions()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.ValidationDisplayMode, ValidationDisplayMode.TabbableWithHint).Add(x => x.ErrorsLabel, "My Errors"));

            var input = inputComponent.Find("input");//tried editContext.Validate but that did not get the markup to render 

            input.Change("18:00");//need to change from default 13:00 to a failing time of 18:00
            inputComponent.WaitForAssertion(() =>
            {
                // Use FindAll first to debug if it exists at all
                var errorDiv = inputComponent.Find($"div.{GlobalValues.Time_Input_Error_Class}");

                using (new AssertionScope())
                {
                    inputComponent.Instance.ErrorsLabel.Should().Be("My Errors");
                    errorDiv.GetAttribute("aria-label").Should().Contain("My Errors");
                }
            });
        }



        [Theory]
        [InlineData("--svg-icon")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async Task Should_be_able_to_set_the_optional_svg_icon_parameter_which_must_start_with_a_double_dash(string? svgIconVariable)
        {
            await using var context = new BunitContext();
            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.ValidationDisplayMode, ValidationDisplayMode.TabbableWithHint).Add(x => x.SvgIcon, svgIconVariable));

            if (String.IsNullOrWhiteSpace(svgIconVariable))
            {
                inputComponent.FindAll($"span.{GlobalValues.Time_Input_Icon_Class}").Should().BeEmpty();
                return;
            }

            if (svgIconVariable.StartsWith("--"))
            {
                inputComponent.Find($"span.{GlobalValues.Time_Input_Icon_Class}").GetAttribute("style").Should().NotBeEmpty();
                return;
            }

            inputComponent.FindAll($"span.{GlobalValues.Time_Input_Icon_Class}").Should().BeEmpty();
        }


    }

    public class Properties()
    {
        [Fact]
        public async Task Should_be_able_to_get_the_control_reference_for_the_underlying_input()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context);

            inputComponent.Instance.ControlReference.Should().NotBeNull();
        }
    }


}




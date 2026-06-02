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
        moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Time_Segment_Handlers, _ => true).SetVoidResult();
        moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Focus_Out_Callback, _ => true).SetVoidResult();


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
    public static (IRenderedComponent<TimeInput<TimeOnly?>> Component, EditContext EditContext) CreateNullableTimeInput(
    BunitContext context,
    Action<ComponentParameterCollectionBuilder<TimeInput<TimeOnly?>>>? parameters = null)
    {
        var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);
        moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();
        moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Readonly_Handlers, _ => true).SetVoidResult();
        moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Time_Segment_Handlers, _ => true).SetVoidResult();
        moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Focus_Out_Callback, _ => true).SetVoidResult();

        var model = new TestModel();
        var editContext = new EditContext(model);

        editContext.EnableDataAnnotationsValidation(context.Services);

        var component = context.Render<TimeInput<TimeOnly?>>(
            builder =>
            {
                builder
                    .AddCascadingValue(editContext)
                    .Add(p => p.Value, model.NullableTimeValue)
                    .Add(p => p.ValueChanged, EventCallback.Factory.Create<TimeOnly?>(context, v => model.NullableTimeValue = v))
                    .Add(p => p.ValueExpression, () => model.NullableTimeValue);

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
        public async Task Should_be_able_to_set_required_which_uses_sets_aria_required_on_each_input_if_true(bool required)
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.Required, required));

            var inputs = inputComponent.FindAll("input");

            foreach(var input in inputs)
            {
                if (required) input.GetAttribute("aria-required").Should().Be("true");
                if (!required) input.GetAttribute("aria-required").Should().BeNull();
            }
        }


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_be_able_to_set_the_readonly_attribute_on_each_input_when_true(bool readOnly)
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.ReadOnly, readOnly));

            var inputs = inputComponent.FindAll("input");
            
            foreach (var input in inputs)
            {
                if (readOnly) input.GetAttribute("readonly").Should().Be("readonly");
                if (!readOnly) input.GetAttribute("readonly").Should().BeNull();
            }
        }


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_be_able_to_set_the_aria_disabled_attribute_on_each_input_when_true(bool disabled)
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.AriaDisabled, disabled));

            var inputs = inputComponent.FindAll("input");

            foreach (var input in inputs)
            {
                if (disabled) input.GetAttribute("aria-disabled").Should().Be("true");
                if (!disabled) input.GetAttribute("aria-disabled").Should().BeNull();
            }
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

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.LabelText, labelText).Add(x => x.Required, false));

            var labelContent = inputComponent.Find($"span.{GlobalValues.Time_Input_Label_Class}").TextContent;

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

            var labelContent = inputComponent.Find($"span.{GlobalValues.Time_Input_Label_Class}").TextContent;

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

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Readonly_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Time_Segment_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Focus_Out_Callback, _ => true).SetVoidResult();


            var model = new TestModel { TimeValue = new TimeOnly(18, 0, 0) };
            var editContext = new EditContext(model);

            editContext.EnableDataAnnotationsValidation(context.Services);

            var component = context.Render<TimeInput<TimeOnly>>(
                builder =>
                {
                    builder
                        .AddCascadingValue(editContext)
                        .Add(p => p.Value, model.TimeValue)
                        .Add(p => p.ValueChanged, EventCallback.Factory.Create<TimeOnly>(context, v => model.TimeValue = v))
                        .Add(p => p.ValueExpression, () => model.TimeValue)
                        .Add(p => p.ValidationDisplayMode, ValidationDisplayMode.TabbableWithHint)
                        .Add(p => p.ErrorsLabel, "My Errors");

                });

            await component.InvokeAsync(() =>
            {
                editContext.NotifyFieldChanged(new FieldIdentifier(model, nameof(model.TimeValue)));
                editContext.Validate();
            });

            component.WaitForAssertion(() =>
            {
   
                // Use FindAll first to debug if it exists at all
                var errorDiv = component.Find($".{GlobalValues.Time_Input_Error_Class}");

                using (new AssertionScope())
                {
                    errorDiv.GetAttribute("tabindex").Should().Be("0");
                    errorDiv.GetAttribute("role").Should().Be("region");

                    component.Find("div").GetAttribute("aria-invalid").Should().Be("true");
                   
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

                var inputElement = inputComponent.Find("div");

                inputElement.GetAttribute("style").Should().Be("color:red;");

                // the get attributes is used twice, once for the class the other for attribues so we should only have one test class.
                inputElement.ClassList.Where(c => c.Contains("test")).Should().HaveCount(1);
                inputElement.ClassList.Should().Contain(GlobalValues.Time_Input_Class);


            }
        }


        [Fact]
        public async Task Should_capture_unmatched_attributes_and_add_class_to_the_time_input_class_list()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.AddUnmatched("class", "test"));

            using (new AssertionScope())
            {
                inputComponent.Instance.AdditionalAttributes.Should().ContainKey("class").WhoseValue.Should().Be("test");

                var parentElement = inputComponent.Find("div");
                parentElement.ClassList.Should().Contain("test");

            }
        }


        [Fact]
        public async Task Should_be_able_to_set_the_error_label_used_for_tabbable_error_regions()
        {
            await using var context = new BunitContext();

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Readonly_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Time_Segment_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Focus_Out_Callback, _ => true).SetVoidResult();


            var model = new TestModel { TimeValue = new TimeOnly(18, 0, 0) };
            var editContext = new EditContext(model);

            editContext.EnableDataAnnotationsValidation(context.Services);

            var component = context.Render<TimeInput<TimeOnly>>(
                builder =>
                {
                    builder
                        .AddCascadingValue(editContext)
                        .Add(p => p.Value, model.TimeValue)
                        .Add(p => p.ValueChanged, EventCallback.Factory.Create<TimeOnly>(context, v => model.TimeValue = v))
                        .Add(p => p.ValueExpression, () => model.TimeValue)
                        .Add(p => p.ValidationDisplayMode, ValidationDisplayMode.TabbableWithHint)
                        .Add(p => p.ErrorsLabel, "My Errors");

                });

            await component.InvokeAsync(() =>
            {
                editContext.NotifyFieldChanged(new FieldIdentifier(model, nameof(model.TimeValue)));
                editContext.Validate();
            });

            component.WaitForAssertion(() =>
            {
                // Use FindAll first to debug if it exists at all
                var errorDiv = component.Find($"div.{GlobalValues.Time_Input_Error_Class}");

                using (new AssertionScope())
                {
                    component.Instance.ErrorsLabel.Should().Be("My Errors");
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

        [Fact]
        public async Task Should_not_render_autocomplete_attribute_when_autocomplete_off_is_false()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.AutoCompleteOff, false));

            inputComponent.Find("input").GetAttribute("autocomplete").Should().BeNull();
        }

        [Fact]
        public async Task Should_render_autocomplete_off_attribute_when_autocomplete_off_is_true()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context);

            inputComponent.Find("input").GetAttribute("autocomplete").Should().Be("off");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("HH")]
        public async Task Should_be_able_to_set_the_hours_label_text_which_defaults_when_null_empty_or_whitespace(string? labelText)
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.HoursLabelText, labelText));

            var label = inputComponent.Find($"label[for='{inputComponent.FindAll("input")[0].GetAttribute("id")}']").TextContent;

            if (String.IsNullOrWhiteSpace(labelText)) label.Should().Be(GlobalValues.Time_Input_Hours_Text);

            if (!String.IsNullOrWhiteSpace(labelText)) label.Should().Be(labelText);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("MM")]
        public async Task Should_be_able_to_set_the_minutes_label_text_which_defaults_when_null_empty_or_whitespace(string? labelText)
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.MinutesLabelText, labelText));

            var label = inputComponent.Find($"label[for='{inputComponent.FindAll("input")[1].GetAttribute("id")}']").TextContent;

            if (String.IsNullOrWhiteSpace(labelText)) label.Should().Be(GlobalValues.Time_Input_Minutes_Text);

            if (!String.IsNullOrWhiteSpace(labelText)) label.Should().Be(labelText);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("SS")]
        public async Task Should_be_able_to_set_the_seconds_label_text_which_defaults_when_null_empty_or_whitespace(string? labelText)
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.SecondsLabelText, labelText).Add(x => x.EnableSeconds, true));

            var label = inputComponent.Find($"label[for='{inputComponent.FindAll("input")[2].GetAttribute("id")}']").TextContent;

            if (String.IsNullOrWhiteSpace(labelText)) label.Should().Be(GlobalValues.Time_Input_Seconds_Text);

            if (!String.IsNullOrWhiteSpace(labelText)) label.Should().Be(labelText);
        }




        [Theory]
        [InlineData("My parse error text")]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public async Task Should_be_able_to_set_the_parse_error_message_or_use_the_default(string? parseErrorText)
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p =>
                p.Add(x => x.ParseErrorMessage, parseErrorText)
                 .Add(x => x.LabelText, "MyField")
                 .Add(x => x.UpdateOnInput, true)
                 .Add(x => x.ValidationDisplayMode, ValidationDisplayMode.TabbableWithHint));

            await inputComponent.FindAll("input")[0].InputAsync(new ChangeEventArgs { Value = "99" });
            await inputComponent.FindAll("input")[1].InputAsync(new ChangeEventArgs { Value = "99" });

            await inputComponent.InvokeAsync(() => inputComponent.Instance.HandleComponentFocusOut());

            inputComponent.WaitForAssertion(() =>
            {
                var errorItems = inputComponent.FindAll($"div.{GlobalValues.Time_Input_Error_Class} > ul > li");

                if (String.IsNullOrWhiteSpace(parseErrorText))
                {
                    errorItems[0].TextContent.Should().Be($"MyField - {GlobalValues.Input_Parse_time_Error_Message}");
                    return;
                }

                errorItems[0].TextContent.Should().Be($"MyField - {parseErrorText}.");
            });
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


    public class RemoveLeadingZero_
    {

        [Fact]
        public async Task Should_remove_the_leading_zero_from_a_segment_value_on_focus()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context);

            var result = inputComponent.Instance.RemoveLeadingZero("09");

            result.Should().Be("9");
        }
        [Fact]
        public async Task Should_return_value_unchanged_when_readonly_is_true()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.ReadOnly, true));

            var result = inputComponent.Instance.RemoveLeadingZero("09");

            result.Should().Be("09");
        }

        [Fact]
        public async Task Should_return_value_unchanged_when_aria_disabled_is_true()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.AriaDisabled, true));

            var result = inputComponent.Instance.RemoveLeadingZero("09");

            result.Should().Be("09");
        }

        [Fact]
        public async Task Should_return_value_unchanged_when_value_is_not_a_valid_integer()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context);

            var result = inputComponent.Instance.RemoveLeadingZero("abc");

            result.Should().Be("abc");
        }

        [Fact]
        public async Task Should_return_null_when_value_is_null()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context);

            var result = inputComponent.Instance.RemoveLeadingZero(null);

            result.Should().BeNull();
        }
    }

    public class PadMissingWithZero
    {

        [Theory]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("11")]

        public async Task Should_left_pad_with_non_blank_value_with_zero_so_its_2_digit(string value)
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context);

            var result = inputComponent.Instance.PadMissingWithZero(value);

            if(value.Length == 1) result.Should().Be("0" + value);
            if (value.Length == 2) result.Should().Be(value);
        }
        [Fact]
        public async Task Should_return_value_unchanged_when_readonly_is_true()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.ReadOnly, true));

            var result = inputComponent.Instance.PadMissingWithZero("9");

            result.Should().Be("9");
        }


        [Fact]
        public async Task Should_return_value_unchanged_when_aria_disabled_is_true()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.AriaDisabled, true));

            var result = inputComponent.Instance.PadMissingWithZero("9");

            result.Should().Be("9");
        }

        
        [Fact]
        public async Task Should_return_null_when_value_is_null()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context);

            var result = inputComponent.Instance.PadMissingWithZero(null);

            result.Should().BeNull();
        }

    }

    public class HandleHoursSet
    {
        [Fact]
        public async Task Should_update_the_hours_value()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context);

            var hoursInput = inputComponent.FindAll("input")[0];
            await hoursInput.InputAsync(new ChangeEventArgs { Value = "10" });

            inputComponent.WaitForAssertion(() =>
            {
                inputComponent.FindAll("input")[0].GetAttribute("value").Should().Be("10");
            });
        }

        [Fact]
        public async Task Should_set_hours_value_to_empty_string_when_null_is_passed_and_update_on_input_is_true()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.UpdateOnInput, true));

            var hoursInput = inputComponent.FindAll("input")[0];
            await hoursInput.InputAsync(new ChangeEventArgs { Value = null });

            inputComponent.WaitForAssertion(() =>
            {
                inputComponent.FindAll("input")[0].GetAttribute("value").Should().BeNullOrEmpty();
            });
        }
        [Fact]
        public async Task Should_not_update_the_hours_value_when_aria_disabled_is_true()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.AriaDisabled, true));

            var hoursInput = inputComponent.FindAll("input")[0];
            await hoursInput.InputAsync(new ChangeEventArgs { Value = "10" });

            inputComponent.WaitForAssertion(() =>
            {
                inputComponent.FindAll("input")[0].GetAttribute("value").Should().Be("13");
            });
        }
    }


    public class HandleMinutesSet
    {
        [Fact]
        public async Task Should_update_the_minutes_value()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context);

            var minutesInput = inputComponent.FindAll("input")[1];
            await minutesInput.InputAsync(new ChangeEventArgs { Value = "30" });

            inputComponent.WaitForAssertion(() =>
            {
                inputComponent.FindAll("input")[1].GetAttribute("value").Should().Be("30");
            });
        }

        [Fact]
        public async Task Should_set_minutes_value_to_empty_string_when_null_is_passed_and_update_on_input_is_true()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.UpdateOnInput, true));

            var minutesInput = inputComponent.FindAll("input")[1];
            await minutesInput.InputAsync(new ChangeEventArgs { Value = null });

            inputComponent.WaitForAssertion(() =>
            {
                inputComponent.FindAll("input")[1].GetAttribute("value").Should().BeNullOrEmpty();
            });
        }

        [Fact]
        public async Task HandleMinutesSet_should_not_update_the_minutes_value_when_aria_disabled_is_true()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.AriaDisabled, true));

            var minutesInput = inputComponent.FindAll("input")[1];
            await minutesInput.InputAsync(new ChangeEventArgs { Value = "45" });

            inputComponent.WaitForAssertion(() =>
            {
                inputComponent.FindAll("input")[1].GetAttribute("value").Should().Be("00");
            });
        }
    }


    public class HandleSecondsSet
    {
        [Fact]
        public async Task Should_update_the_seconds_value()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.EnableSeconds, true));

            var secondsInput = inputComponent.FindAll("input")[2];
            await secondsInput.InputAsync(new ChangeEventArgs { Value = "45" });

            inputComponent.WaitForAssertion(() =>
            {
                inputComponent.FindAll("input")[2].GetAttribute("value").Should().Be("45");
            });
        }

        [Fact]
        public async Task Should_set_seconds_value_to_empty_string_when_null_is_passed_and_update_on_input_is_true()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.UpdateOnInput, true).Add(x => x.EnableSeconds, true));

            var secondsInput = inputComponent.FindAll("input")[2];
            await secondsInput.InputAsync(new ChangeEventArgs { Value = null });

            inputComponent.WaitForAssertion(() =>
            {
                inputComponent.FindAll("input")[2].GetAttribute("value").Should().BeNullOrEmpty();
            });
        }

        [Fact]
        public async Task HandleSecondsSet_should_not_update_the_seconds_value_when_aria_disabled_is_true()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.AriaDisabled, true).Add(x => x.EnableSeconds, true));

            var secondsInput = inputComponent.FindAll("input")[2];
            await secondsInput.InputAsync(new ChangeEventArgs { Value = "30" });

            inputComponent.WaitForAssertion(() =>
            {
                inputComponent.FindAll("input")[2].GetAttribute("value").Should().Be("00");
            });
        }
    }

    public class HandleComponentFocusOut
    {
        [Fact]
        public async Task Should_pad_single_digit_segment_values_and_commit_the_time()
        {
            await using var context = new BunitContext();

            // Start with a time that has single digit hours and minutes
            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Readonly_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Time_Segment_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Focus_Out_Callback, _ => true).SetVoidResult();

            var model = new TestModel { TimeValue = new TimeOnly(9, 5, 0) };
            var editContext = new EditContext(model);

            var component = context.Render<TimeInput<TimeOnly>>(
                builder =>
                {
                    builder
                        .AddCascadingValue(editContext)
                        .Add(p => p.Value, model.TimeValue)
                        .Add(p => p.ValueChanged, EventCallback.Factory.Create<TimeOnly>(context, v => model.TimeValue = v))
                        .Add(p => p.ValueExpression, () => model.TimeValue);
                });

            await component.InvokeAsync(() => component.Instance.HandleComponentFocusOut());

            component.WaitForAssertion(() =>
            {
                component.Instance.Value.Should().Be(new TimeOnly(9, 5, 0));
            });
        }


        [Fact]
        public async Task Should_return_early_and_not_change_value_when_aria_disabled_is_true()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.AriaDisabled, true));

            await inputComponent.InvokeAsync(() => inputComponent.Instance.HandleComponentFocusOut());

            inputComponent.WaitForAssertion(() =>
            {
                inputComponent.Instance.Value.Should().Be(new TimeOnly(13, 0, 0));
            });
        }

        [Fact]
        public async Task Should_set_value_to_null_when_all_segments_are_empty_and_type_is_nullable()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateNullableTimeInput(context);

            await inputComponent.InvokeAsync(() => inputComponent.Instance.HandleComponentFocusOut());

            inputComponent.WaitForAssertion(() =>
            {
                inputComponent.Instance.Value.Should().BeNull();
            });
        }

        [Fact]
        public async Task Should_default_to_midnight_when_all_segments_are_empty_and_type_is_non_nullable()
        {
            await using var context = new BunitContext();

            // Use a model with a null-ish starting point by rendering with no initial value set
            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Readonly_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Time_Segment_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Focus_Out_Callback, _ => true).SetVoidResult();

            var model = new TestModel { TimeValue = new TimeOnly(0, 0, 0) };
            var editContext = new EditContext(model);

            var component = context.Render<TimeInput<TimeOnly>>(
                builder =>
                {
                    builder
                        .AddCascadingValue(editContext)
                        .Add(p => p.Value, model.TimeValue)
                        .Add(p => p.ValueChanged, EventCallback.Factory.Create<TimeOnly>(context, v => model.TimeValue = v))
                        .Add(p => p.ValueExpression, () => model.TimeValue);
                });

            // Clear the segments then focusout
            await component.FindAll("input")[0].InputAsync(new ChangeEventArgs { Value = "" });
            await component.FindAll("input")[1].InputAsync(new ChangeEventArgs { Value = "" });

            await component.InvokeAsync(() => component.Instance.HandleComponentFocusOut());

            component.WaitForAssertion(() =>
            {
                component.Instance.Value.Should().Be(new TimeOnly(0, 0, 0));
            });
        }

        [Fact]
        public async Task Should_pad_single_digit_segment_values_and_commit_when_focus_leaves_the_group()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.UpdateOnInput, true));

            await inputComponent.FindAll("input")[0].InputAsync(new ChangeEventArgs { Value = "9" });
            await inputComponent.FindAll("input")[1].InputAsync(new ChangeEventArgs { Value = "5" });

            await inputComponent.InvokeAsync(() => inputComponent.Instance.HandleComponentFocusOut());

            inputComponent.WaitForAssertion(() =>
            {
                inputComponent.Instance.Value.Should().Be(new TimeOnly(9, 5, 0));
            });
        }

        [Fact]
        public async Task Should_pad_seconds_and_commit_when_enable_seconds_is_true_and_focus_leaves_the_group()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTimeInput(context, p => p.Add(x => x.UpdateOnInput, true).Add(x => x.EnableSeconds, true));

            await inputComponent.FindAll("input")[0].InputAsync(new ChangeEventArgs { Value = "9" });
            await inputComponent.FindAll("input")[1].InputAsync(new ChangeEventArgs { Value = "5" });
            await inputComponent.FindAll("input")[2].InputAsync(new ChangeEventArgs { Value = "3" });

            await inputComponent.InvokeAsync(() => inputComponent.Instance.HandleComponentFocusOut());

            inputComponent.WaitForAssertion(() =>
            {
                inputComponent.Instance.Value.Should().Be(new TimeOnly(9, 5, 3));
            });
        }


    }
}




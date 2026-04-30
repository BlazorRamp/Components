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

public class NumericInput_Tests
{
    internal class TestModel
    {
        [Required(ErrorMessage = "Needs a value")]
        public int IntValue { get; set; } = 20;

        [Required(ErrorMessage = "Needs a value")]
        public decimal DecimalValue { get; set; } = 20.00M;

        public int? NullableInt { get; set; } = 20;
    }


    public static (IRenderedComponent<NumericInput<decimal>> Component, EditContext EditContext) CreateDecimalInputWithParamByName<TValue>(BunitContext context, string paramName, TValue paramValue)
    {
        var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);
        moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();
        moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Numeric_Handlers, _ => true).SetVoidResult();

        var model = new TestModel();
        var editContext = new EditContext(model);

        editContext.EnableDataAnnotationsValidation(context.Services);

        var component = context.Render<NumericInput<decimal>>(
            builder => builder
                .AddCascadingValue(editContext)
                .Add(p => p.Value, model.DecimalValue)
                .Add(p => p.ValueChanged, EventCallback.Factory.Create<decimal>(context, v => model.DecimalValue = v))
                .Add(p => p.ValueExpression, () => model.DecimalValue)
            .TryAdd(paramName, paramValue));

        return (component, editContext);
    }



    public class Parameters()
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_be_able_to_set_required_which_uses_aria_required_if_true(bool required)
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateDecimalInputWithParamByName<bool>(context, nameof(NumericInput<decimal>.Required), required);

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

            var (inputComponent, _) = CreateDecimalInputWithParamByName<bool>(context, nameof(NumericInput<decimal>.ReadOnly), readOnly);

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

            var (inputComponent, _) = CreateDecimalInputWithParamByName<bool>(context, nameof(NumericInput<decimal>.AriaDisabled), disabled);

            var disabledAttribute = inputComponent.Find("input").GetAttribute("aria-disabled");

            if (disabled) disabledAttribute.Should().Be("true");

            if (!disabled) disabledAttribute.Should().BeNull();
        }


        [Fact]
        public async Task Should_only_use_the_readonly_attribute_if_both_readonly_and_aria_disabled_are_set_to_true()
        {
            await using var context = new BunitContext();

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Numeric_Handlers, _ => true).SetVoidResult();

            var model = new TestModel();
            var editContext = new EditContext(model);

            var component = context.Render<NumericInput<decimal>>(
                builder => builder
                    .AddCascadingValue(editContext)
                    .Add(p => p.Value, model.DecimalValue)
                    .Add(p => p.ValueChanged, EventCallback.Factory.Create<decimal>(context, v => model.DecimalValue = v))
                    .Add(p => p.ValueExpression, () => model.DecimalValue)
                    .Add(p => p.ReadOnly, true)
                    .Add(p => p.AriaDisabled, true));


            var readonlyAttribute = component.Find("input").GetAttribute("readonly");
            var ariaDisabledAttribute = component.Find("input").GetAttribute("aria-disabled");


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

            var (inputComponent, _) = CreateDecimalInputWithParamByName<string?>(context, nameof(NumericInput<decimal>.ControlID), controlID);

            var idAttribute = inputComponent.Find("input").GetAttribute("id");

            if (String.IsNullOrWhiteSpace(controlID)) Guid.Parse(idAttribute!).Should().NotBeEmpty();

            if (!String.IsNullOrWhiteSpace(controlID)) idAttribute.Should().Be(controlID);
        }



        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("MyControl")]
        public async Task Should_be_able_to_set_the_display_dame_which_will_used_the_field_identity_if_null_empty_or_whitespace(string? labelText)
        {
            await using var context = new BunitContext();

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);

            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Numeric_Handlers, _ => true).SetVoidResult();

            var model = new TestModel();
            var editContext = new EditContext(model);

            var component = context.Render<NumericInput<decimal>>(
                builder => builder
                    .AddCascadingValue(editContext)
                    .Add(p => p.Value, model.DecimalValue)
                    .Add(p => p.ValueChanged, EventCallback.Factory.Create<decimal>(context, v => model.DecimalValue = v))
                    .Add(p => p.ValueExpression, () => model.DecimalValue)
                    .Add(p => p.Required, false) // Setting this to false otherwise a * is appended
                    .Add(p => p.LabelText, labelText));

            var labelContent = component.Find("label").TextContent;

            if (String.IsNullOrWhiteSpace(labelText)) labelContent.Should().Be("DecimalValue");

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

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);

            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Numeric_Handlers, _ => true).SetVoidResult();

            var model = new TestModel();
            var editContext = new EditContext(model);

            var component = context.Render<NumericInput<decimal>>(
                builder => builder
                    .AddCascadingValue(editContext)
                    .Add(p => p.Value, model.DecimalValue)
                    .Add(p => p.ValueChanged, EventCallback.Factory.Create<decimal>(context, v => model.DecimalValue = v))
                    .Add(p => p.ValueExpression, () => model.DecimalValue)
                    .Add(p => p.Required, true)
                    .Add(p => p.LabelText, labelText));

            var labelContent = component.Find("label").TextContent;

            if (String.IsNullOrWhiteSpace(labelText)) labelContent.Should().Be("DecimalValue *");

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

            var (inputComponent, _) = CreateDecimalInputWithParamByName<string?>(context, nameof(NumericInput<decimal>.HintText), hintText);

            var hints = inputComponent.FindAll($".{GlobalValues.Numeric_Input_Hint_Class}");

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
        public async Task Should_capture_unmatched_attributes_and_apply_all_to_the_input_element_excluding_class()
        {
            await using var context = new BunitContext();

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);

            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Numeric_Handlers, _ => true).SetVoidResult();

            var model = new TestModel();
            var editContext = new EditContext(model);

            var component = context.Render<NumericInput<decimal>>(
                builder => builder
                    .AddCascadingValue(editContext)
                    .Add(p => p.Value, model.DecimalValue)
                    .Add(p => p.ValueChanged, EventCallback.Factory.Create<decimal>(context, v => model.DecimalValue = v))
                    .Add(p => p.ValueExpression, () => model.DecimalValue)
                    .AddUnmatched("class", "test")
                    .AddUnmatched("style", "color:red;"));

            using (new AssertionScope())
            {
                component.Instance.AdditionalAttributes.Should().ContainKey("style").WhoseValue.Should().Be("color:red;");

                var inputElement = component.Find("input");

                inputElement.GetAttribute("style").Should().Be("color:red;");
                inputElement.ClassList.Should().NotContain("test");

            }
        }


        [Fact]
        public async Task Should_capture_unmatched_attributes_and_add_class_to_the_text_input_class_list()
        {
            await using var context = new BunitContext();

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);

            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Numeric_Handlers, _ => true).SetVoidResult();

            var model = new TestModel();
            var editContext = new EditContext(model);

            var component = context.Render<NumericInput<decimal>>(
                builder => builder
                    .AddCascadingValue(editContext)
                    .Add(p => p.Value, model.DecimalValue)
                    .Add(p => p.ValueChanged, EventCallback.Factory.Create<decimal>(context, v => model.DecimalValue = v))
                    .Add(p => p.ValueExpression, () => model.DecimalValue)
                    .AddUnmatched("class", "test")
                    .AddUnmatched("style", "color:red;"));

            using (new AssertionScope())
            {
                component.Instance.AdditionalAttributes.Should().ContainKey("class").WhoseValue.Should().Be("test");

                var textInputElement = component.Find("div");

                textInputElement.GetAttribute("style").Should().BeNull();
                textInputElement.ClassList.Should().Contain("test");

            }
        }



        [Theory]
        [InlineData(true)]
        [InlineData(false)]

        public async Task Should_be_able_to_set_the_update_on_input_param(bool updateOnInput)
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateDecimalInputWithParamByName<bool?>(context, nameof(NumericInput<decimal>.UpdateOnInput), updateOnInput);

            var hints = inputComponent.FindAll($".{GlobalValues.Numeric_Input_Hint_Class}");

            inputComponent.Instance.UpdateOnInput.Should().Be(updateOnInput);
        }



        [Fact]

        public async Task Input_type_should_be_text_with_an_input_mode_of_decimal()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateDecimalInputWithParamByName<string?>(context, nameof(NumericInput<decimal>.DisplayName), "DisplayName");

            var inputType = inputComponent.Find("input");

            using (new AssertionScope())
            {
                inputType.GetAttribute("type").Should().Be("text");
                inputType.GetAttribute("inputmode").Should().Be("decimal");
            }
        }



        [Fact]
        public async Task Should_be_able_to_set_the_error_label_used_for_tabbable_error_regions()
        {
            await using var context = new BunitContext();

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);

            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Numeric_Handlers, _ => true).SetVoidResult();

            var model = new TestModel();
            var editContext = new EditContext(model);

            var component = context.Render<NumericInput<decimal>>(
                builder => builder
                    .AddCascadingValue(editContext)
                    .Add(p => p.Value, model.DecimalValue)
                    .Add(p => p.ValueChanged, EventCallback.Factory.Create<decimal>(context, v => model.DecimalValue = v))
                    .Add(p => p.ValueExpression, () => model.DecimalValue)
                    .Add(p => p.Required, true)
                    .Add(p => p.ValidationDisplayMode, ValidationDisplayMode.TabbableWithHint)
                    .Add(p => p.ErrorsLabel, "My Errors"));



            var input = component.Find("input");//tried editContext.Validate but that did not get the markup to render 

            input.Change("abc");

            component.WaitForAssertion(() =>
            {
                // Use FindAll first to debug if it exists at all
                var errorDiv = component.Find($"div.{GlobalValues.Numeric_Input_Error_Class}");

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

            var (inputComponent, _) = CreateDecimalInputWithParamByName<string?>(context, nameof(NumericInput<decimal>.SvgIcon), svgIconVariable);

            if (String.IsNullOrWhiteSpace(svgIconVariable))
            {
                inputComponent.FindAll($"span.{GlobalValues.Numeric_Input_Icon_Class}").Should().BeEmpty();
                return;
            }

            if (svgIconVariable.StartsWith("--"))
            {
                inputComponent.Find($"span.{GlobalValues.Numeric_Input_Icon_Class}").GetAttribute("style").Should().NotBeEmpty();
                return;
            }

            inputComponent.FindAll($"span.{GlobalValues.Numeric_Input_Icon_Class}").Should().BeEmpty();
        }


        [Fact]
        public async Task Should_have_a_tabbable_region_for_errors_when_the_Validation_display_mode_is_set_to_tabbable_with_hint()
        {
            await using var context = new BunitContext();

            var (inputComponent, editContext) = CreateDecimalInputWithParamByName<ValidationDisplayMode?>(context, nameof(NumericInput<decimal>.ValidationDisplayMode), ValidationDisplayMode.TabbableWithHint);

            var input = inputComponent.Find("input");//tried editContext.Validate but that did not get the markup to render 
            input.Change("abc");

            inputComponent.WaitForAssertion(() =>
            {
                // Use FindAll first to debug if it exists at all
                var errorDiv = inputComponent.Find($".{GlobalValues.Numeric_Input_Error_Class}");

                using (new AssertionScope())
                {
                    errorDiv.GetAttribute("tabindex").Should().Be("0");
                    errorDiv.GetAttribute("role").Should().Be("region");

                    input.GetAttribute("aria-invalid").Should().Be("true");
                }
            });
        }


        [Theory]
        [InlineData("My parse error text")]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public async Task Should_be_able_to_set_the_parse_error_message_or_have_use_the_default_used(string? parseErrorText)
        {
            await using var context = new BunitContext();

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);

            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Numeric_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Set_Value, _ => true).SetVoidResult();

            var model = new TestModel();
            var editContext = new EditContext(model);

            var component = context.Render<NumericInput<decimal>>(
                builder => builder
                    .AddCascadingValue(editContext)
                    .Add(p => p.Value, model.DecimalValue)
                    .Add(p => p.ValueChanged, EventCallback.Factory.Create<decimal>(context, v => model.DecimalValue = v))
                    .Add(p => p.ValueExpression, () => model.DecimalValue)
                    .Add(p => p.LabelText, "MyField")
                    .Add(p => p.ParseErrorMessage, parseErrorText));

            var input = component.Find("input");

            await component.InvokeAsync(() => input.Change("999999999999999999999999999999999999999999999999"));

            component.WaitForAssertion(() =>
            {
                var errorItems = component.FindAll($"div.{GlobalValues.Numeric_Input_Error_Class} > ul > li");

                using (new AssertionScope())
                {
                    if (String.IsNullOrWhiteSpace(parseErrorText))
                    {
                        errorItems[0].TextContent.Should().Be($"MyField - {GlobalValues.Input_Parse_Error_Message}");
                        return;
                    }

                    errorItems[0].TextContent.Should().Be($"MyField - {parseErrorText}.");
                }
            });
        }


        [Fact]
        public async Task Should_be_able_to_set_a_number_formatter_()
        {
            await using var context = new BunitContext();

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);

            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Numeric_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Set_Value, _ => true).SetVoidResult();

            var model = new TestModel();
            var editContext = new EditContext(model);

            var component = context.Render<NumericInput<decimal>>(
                builder => builder
                    .AddCascadingValue(editContext)
                    .Add(p => p.Value, model.DecimalValue)
                    .Add(p => p.ValueChanged, EventCallback.Factory.Create<decimal>(context, v => model.DecimalValue = v))
                    .Add(p => p.ValueExpression, () => model.DecimalValue)
                    .Add(p => p.Format, "F2"));

            
            component.Instance.Format.Should().Be("F2");

        }



        [Fact]
        public async Task Should_update_validation_state_when_edit_context_validation_is_requested()
        {
            await using var context = new BunitContext();

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Numeric_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Set_Value, _ => true).SetVoidResult();

            var model = new TestModel();
            var editContext = new EditContext(model);
            editContext.EnableDataAnnotationsValidation(context.Services);

            var component = context.Render<NumericInput<decimal>>(
                builder => builder
                    .AddCascadingValue(editContext)
                    .Add(p => p.Value, model.DecimalValue)
                    .Add(p => p.ValueChanged, EventCallback.Factory.Create<decimal>(context, v => model.DecimalValue = v))
                    .Add(p => p.ValueExpression, () => model.DecimalValue)
                    .Add(p => p.Required, true));


            await component.Find("input").ChangeAsync("abc");
            await component.InvokeAsync(() => editContext.Validate());

            component.WaitForAssertion(() =>
            {
                var errorItems = component.FindAll($"div.{GlobalValues.Numeric_Input_Error_Class} > ul > li");
                errorItems.Should().NotBeEmpty();
            });
        }


    }


    public class Properties()
    {
        [Fact]
        public async Task Should_be_able_to_get_the_control_reference_for_the_underlying_input()
        {
            await using var context = new BunitContext();

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);

            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Numeric_Handlers, _ => true).SetVoidResult();

            var model = new TestModel();
            var editContext = new EditContext(model);

            var component = context.Render<NumericInput<decimal>>(
                builder => builder
                    .AddCascadingValue(editContext)
                    .Add(p => p.Value, model.DecimalValue)
                    .Add(p => p.ValueChanged, EventCallback.Factory.Create<decimal>(context, v => model.DecimalValue = v))
                    .Add(p => p.ValueExpression, () => model.DecimalValue));


            component.Instance.ControlReference.Should().NotBeNull();
        }
    }


    public class HandleOnBlur_Tests
    {
        [Fact]
        public async Task Should_set_current_value_to_default_when_the_string_value_is_empty_and_type_is_non_nullable()
        {
            await using var context = new BunitContext();

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Numeric_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Set_Value, _ => true).SetVoidResult();

            var model = new TestModel();
            var editContext = new EditContext(model);

            var component = context.Render<NumericInput<decimal>>(
                builder => builder
                    .AddCascadingValue(editContext)
                    .Add(p => p.Value, model.DecimalValue)
                    .Add(p => p.ValueChanged, EventCallback.Factory.Create<decimal>(context, v => model.DecimalValue = v))
                    .Add(p => p.ValueExpression, () => model.DecimalValue));

            var input = component.Find("input");


            await input.ChangeAsync("");
            await input.BlurAsync();

            component.WaitForAssertion(() =>
            {
                component.Instance.Value.Should().Be(0);
            });
        }


        [Fact]
        public async Task Should_set_current_value_to_default_when_string_value_is_empty_and_type_is_nullable()
        {
            await using var context = new BunitContext();

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Numeric_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Set_Value, _ => true).SetVoidResult();

            var model = new TestModel();
            var editContext = new EditContext(model);

            var component = context.Render<NumericInput<int?>>(
                builder => builder
                    .AddCascadingValue(editContext)
                    .Add(p => p.Value, model.NullableInt)
                    .Add(p => p.ValueChanged, EventCallback.Factory.Create<int?>(context, v => model.NullableInt = v))
                    .Add(p => p.ValueExpression, () => model.NullableInt));

            var input = component.Find("input");


            await input.ChangeAsync("");
            await input.BlurAsync();

            component.WaitForAssertion(() =>
            {
                component.Instance.Value.Should().BeNull(); ;
            });
        }


        [Fact]
        public async Task Should_apply_format_to_display_value_on_blur_when_format_is_set()
        {
            await using var context = new BunitContext();

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Numeric_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Set_Value, _ => true).SetVoidResult();

            var model = new TestModel();
            var editContext = new EditContext(model);

            var component = context.Render<NumericInput<decimal>>(
                builder => builder
                    .AddCascadingValue(editContext)
                    .Add(p => p.Value, model.DecimalValue)
                    .Add(p => p.ValueChanged, EventCallback.Factory.Create<decimal>(context, v => model.DecimalValue = v))
                    .Add(p => p.ValueExpression, () => model.DecimalValue)
                    .Add(p => p.Format, "F2"));

            var input = component.Find("input");

            await input.ChangeAsync("26.1");
            await input.BlurAsync();


            component.WaitForAssertion(() =>
            {
                moduleInterop.VerifyInvoke(GlobalValues.JS_Inputs_Set_Value).Arguments[1].Should().Be("26.10");
            });
        }

    }

}


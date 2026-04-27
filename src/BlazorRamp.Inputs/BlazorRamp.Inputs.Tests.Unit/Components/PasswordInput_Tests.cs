using BlazorRamp.Inputs.Common.Constants;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using PasswordInputComponent = BlazorRamp.Inputs.Components.PasswordInput;

namespace BlazorRamp.Inputs.Tests.Unit.Components;

public class PasswordInput_Tests
{
    internal class TestModel
    {
        [Required(ErrorMessage = "Needs a value")]
        public string PropertyValue { get; set; } = "test";
    }

    public static (IRenderedComponent<PasswordInputComponent> Component, EditContext EditContext) CreateInputWithParamByName<TValue>(BunitContext context, string paramName, TValue paramValue)
    {
        var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);
        moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();

        var model = new TestModel();
        var editContext = new EditContext(model);

        editContext.EnableDataAnnotationsValidation(context.Services);

        var component = context.Render<PasswordInputComponent>(
            builder => builder
                .AddCascadingValue(editContext)
                .Add(p => p.Value, model.PropertyValue)
                .Add(p => p.ValueChanged, EventCallback.Factory.Create<string>(context, v => model.PropertyValue = v))
                .Add(p => p.ValueExpression, () => model.PropertyValue)
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

            var (inputComponent, _) = CreateInputWithParamByName<bool>(context, nameof(PasswordInputComponent.Required), required);

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

            var (inputComponent, _) = CreateInputWithParamByName<bool>(context, nameof(PasswordInputComponent.ReadOnly), readOnly);

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

            var (inputComponent, _) = CreateInputWithParamByName<bool>(context, nameof(PasswordInputComponent.AriaDisabled), disabled);

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

            var model = new TestModel();
            var editContext = new EditContext(model);

            var component = context.Render<PasswordInputComponent>(
                builder => builder
                    .AddCascadingValue(editContext)
                    .Add(p => p.Value, model.PropertyValue)
                    .Add(p => p.ValueChanged, EventCallback.Factory.Create<string>(context, v => model.PropertyValue = v))
                    .Add(p => p.ValueExpression, () => model.PropertyValue)
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

            var (inputComponent, _) = CreateInputWithParamByName<string?>(context, nameof(PasswordInputComponent.ControlID), controlID);

            var idAttribute = inputComponent.Find("input").GetAttribute("id");

            if (String.IsNullOrWhiteSpace(controlID)) Guid.Parse(idAttribute!).Should().NotBeEmpty();

            if (!String.IsNullOrWhiteSpace(controlID)) idAttribute.Should().Be(controlID);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("MyControl")]
        public async Task Should_be_able_to_set_the_label_text_which_will_used_the_field_identity_if_null_empty_or_whitespace(string? labelText)
        {
            await using var context = new BunitContext();

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);

            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();

            var model = new TestModel();
            var editContext = new EditContext(model);

            var component = context.Render<PasswordInputComponent>(
                builder => builder
                    .AddCascadingValue(editContext)
                    .Add(p => p.Value, model.PropertyValue)
                    .Add(p => p.ValueChanged, EventCallback.Factory.Create<string>(context, v => model.PropertyValue = v))
                    .Add(p => p.ValueExpression, () => model.PropertyValue)
                    .Add(p => p.Required, false) // Setting this to false otherwise a * is appended
                    .Add(p => p.DisplayName, labelText));

            var labelContent = component.Find("label").TextContent;

            if (String.IsNullOrWhiteSpace(labelText)) labelContent.Should().Be("PropertyValue");

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

            var model = new TestModel();
            var editContext = new EditContext(model);

            var component = context.Render<PasswordInputComponent>(
                builder => builder
                    .AddCascadingValue(editContext)
                    .Add(p => p.Value, model.PropertyValue)
                    .Add(p => p.ValueChanged, EventCallback.Factory.Create<string>(context, v => model.PropertyValue = v))
                    .Add(p => p.ValueExpression, () => model.PropertyValue)
                    .Add(p => p.Required, true)
                    .Add(p => p.DisplayName, labelText));

            var labelContent = component.Find("label").TextContent;

            if (String.IsNullOrWhiteSpace(labelText)) labelContent.Should().Be("PropertyValue *");

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

            var (inputComponent, _) = CreateInputWithParamByName<string?>(context, nameof(PasswordInputComponent.HintText), hintText);

            var hints = inputComponent.FindAll($".{GlobalValues.Password_Input_Hint_Class}");

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

            var model = new TestModel();
            var editContext = new EditContext(model);

            var component = context.Render<PasswordInputComponent>(
                builder => builder
                    .AddCascadingValue(editContext)
                    .Add(p => p.Value, model.PropertyValue)
                    .Add(p => p.ValueChanged, EventCallback.Factory.Create<string>(context, v => model.PropertyValue = v))
                    .Add(p => p.ValueExpression, () => model.PropertyValue)
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

            var model = new TestModel();
            var editContext = new EditContext(model);

            var component = context.Render<PasswordInputComponent>(
                builder => builder
                    .AddCascadingValue(editContext)
                    .Add(p => p.Value, model.PropertyValue)
                    .Add(p => p.ValueChanged, EventCallback.Factory.Create<string>(context, v => model.PropertyValue = v))
                    .Add(p => p.ValueExpression, () => model.PropertyValue)
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

            var (inputComponent, _) = CreateInputWithParamByName<bool?>(context, nameof(PasswordInputComponent.UpdateOnInput), updateOnInput);

            var hints = inputComponent.FindAll($".{GlobalValues.Password_Input_Hint_Class}");

            inputComponent.Instance.UpdateOnInput.Should().Be(updateOnInput);
        }

        [Theory]
        [InlineData(PasswordAutoComplete.CurrentPassword)]
        [InlineData(PasswordAutoComplete.NewPassword)]

        public async Task Should_be_able_to_set_the_auto_complete_param(PasswordAutoComplete autoCompleteValue)
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateInputWithParamByName<PasswordAutoComplete?>(context, nameof(PasswordInputComponent.PasswordAutoComplete), autoCompleteValue);

            var inputTypeAttribute = inputComponent.Find("input").GetAttribute("autocomplete");

            using (new AssertionScope())
            {
                var enumText = autoCompleteValue == PasswordAutoComplete.CurrentPassword ? "current-password" : "new-password";
                inputComponent.Instance.PasswordAutoComplete.Should().Be(autoCompleteValue);
                inputTypeAttribute.Should().Be(enumText);
            }
        }

        [Fact]
        public async Task The_input_type_should_be_set_to_password()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateInputWithParamByName<bool?>(context, nameof(PasswordInputComponent.Required), true);

            inputComponent.Find("input").GetAttribute("type").Should().Be("password");

        }



        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("Show It")]
        public async Task Should_be_able_to_set_the_show_password_text_for_the_show_password_button_with_a_default_of_show_password(string? showPasswordText)
        {
            await using var context = new BunitContext();

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);

            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();

            var model = new TestModel();
            var editContext = new EditContext(model);

            var component = context.Render<PasswordInputComponent>(
                builder => builder
                    .AddCascadingValue(editContext)
                    .Add(p => p.Value, model.PropertyValue)
                    .Add(p => p.ValueChanged, EventCallback.Factory.Create<string>(context, v => model.PropertyValue = v))
                    .Add(p => p.ValueExpression, () => model.PropertyValue)
                    .Add(p => p.Required, false) // Setting this to false otherwise a * is appended
                    .Add(p => p.ShowPasswordText, showPasswordText));

            var buttonContent = component.Find("button").TextContent;

            if (String.IsNullOrWhiteSpace(showPasswordText)) buttonContent.Should().Be(GlobalValues.Password_Input_Show_Password_Text);

            if (!String.IsNullOrWhiteSpace(showPasswordText)) buttonContent.Should().Be(showPasswordText);
        }



        [Fact]
        public async Task Should_be_able_to_set_the_error_label_used_for_tabbable_error_regions()
        {
            await using var context = new BunitContext();

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);

            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();

            var model = new TestModel();
            var editContext = new EditContext(model);

            editContext.EnableDataAnnotationsValidation(context.Services);

            var component = context.Render<PasswordInputComponent>(
                builder => builder
                    .AddCascadingValue(editContext)
                    .Add(p => p.Value, model.PropertyValue)
                    .Add(p => p.ValueChanged, EventCallback.Factory.Create<string>(context, v => model.PropertyValue = v))
                    .Add(p => p.ValueExpression, () => model.PropertyValue)
                    .Add(p => p.Required, true)
                    .Add(p => p.ValidationDisplayMode, ValidationDisplayMode.TabbableWithHint)
                    .Add(p => p.ErrorsLabel, "My Errors"));



            var input = component.Find("input");//tried editContext.Validate but that did not get the markup to render 

            input.Change("");

            component.WaitForAssertion(() =>
            {
                // Use FindAll first to debug if it exists at all
                var errorDiv = component.Find($"div.{GlobalValues.Password_Input_Error_Class}");

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

            var (inputComponent, _) = CreateInputWithParamByName<string?>(context, nameof(PasswordInputComponent.SvgIcon), svgIconVariable);

            if (String.IsNullOrWhiteSpace(svgIconVariable))
            {
                inputComponent.FindAll($"span.{GlobalValues.Password_Input_Icon_Class}").Should().BeEmpty();
                return;
            }

            if (svgIconVariable.StartsWith("--"))
            {
                inputComponent.Find($"span.{GlobalValues.Password_Input_Icon_Class}").GetAttribute("style").Should().NotBeEmpty();
                return;
            }

            inputComponent.FindAll($"span.{GlobalValues.Password_Input_Icon_Class}").Should().BeEmpty();
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

            var model = new TestModel();
            var editContext = new EditContext(model);

            var component = context.Render<PasswordInputComponent>(
                builder => builder
                    .AddCascadingValue(editContext)
                    .Add(p => p.Value, model.PropertyValue)
                    .Add(p => p.ValueChanged, EventCallback.Factory.Create<string>(context, v => model.PropertyValue = v))
                    .Add(p => p.ValueExpression, () => model.PropertyValue));


            component.Instance.ControlReference.Should().NotBeNull();
        }
    }

    public class Method()
    {
        [Fact]
        public async Task Clicking_the_show_password_button_should_call_toggle_show_password_and_toggle_the_input_type_text_or_password()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateInputWithParamByName<bool?>(context, nameof(PasswordInputComponent.Required), true);

            using(new AssertionScope())
            {
                inputComponent.Find("input").GetAttribute("type").Should().Be("password");

                var showButton = inputComponent.Find("button");
                showButton.GetAttribute("aria-pressed").Should().Be("false");

                showButton.Click();

                inputComponent.WaitForAssertion(() => {
                
                    inputComponent.Find("input").GetAttribute("type").Should().Be("text");
                    showButton.GetAttribute("aria-pressed").Should().Be("true");
                });

                showButton.Click();

                inputComponent.WaitForAssertion(() => {

                    inputComponent.Find("input").GetAttribute("type").Should().Be("password");
                    showButton.GetAttribute("aria-pressed").Should().Be("false");
                });
            }


        }

    }

}
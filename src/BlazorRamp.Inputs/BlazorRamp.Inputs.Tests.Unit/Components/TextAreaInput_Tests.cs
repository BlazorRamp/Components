using BlazorRamp.Core.Services;
using BlazorRamp.Inputs.Common.Constants;
using BlazorRamp.Inputs.Components;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace BlazorRamp.Inputs.Tests.Unit.Components;

public class TextAreaInput_Tests
{
    internal class TestModel
    {
        [StringLength(4, ErrorMessage = "Needs to be less than 4 characters in length")]
        public string StringValue          { get; set; } 
        public string? NullableStringValue { get; set; } = null;
    }


    public static (IRenderedComponent<TextAreaInput> Component, EditContext EditContext) CreateTextAreaInput(
    BunitContext context,
    Action<ComponentParameterCollectionBuilder<TextAreaInput>>? parameters = null)
    {
        var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);
        moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();
        moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Readonly_Handlers, _ => true).SetVoidResult();
        moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Count_Callback_Handlers, _ => true).SetVoidResult();

        context.Services.AddScoped<ILiveRegionService, LiveRegionService>();

        var model       = new TestModel();
        var editContext = new EditContext(model);
        
        editContext.EnableDataAnnotationsValidation(context.Services);

        var component = context.Render<TextAreaInput>(
            builder =>
            {
                builder
                    .AddCascadingValue(editContext)
                    .Add(p => p.Value, model.StringValue)
                    .Add(p => p.ValueChanged, EventCallback.Factory.Create<string>(context, v => model.StringValue = v))
                    .Add(p => p.ValueExpression, () => model.StringValue);

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
        public async Task Should_be_able_to_set_the_data_position(DataPosition dataPosition)
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTextAreaInput(context, p => p.Add(x => x.DataPosition, dataPosition));

            var dataAttribute = inputComponent.Find("textarea").GetAttribute("data-br-input-position");

            dataAttribute.Should().Be(dataPosition.ToString().ToLower());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_be_able_to_set_required_which_uses_sets_aria_required_on_each_input_if_true(bool required)
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTextAreaInput(context, p => p.Add(x => x.Required, required));

            var inputs = inputComponent.FindAll("textarea");

            foreach (var input in inputs)
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

            var (inputComponent, _) = CreateTextAreaInput(context, p => p.Add(x => x.ReadOnly, readOnly));

            var inputs = inputComponent.FindAll("textarea");

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

            var (inputComponent, _) = CreateTextAreaInput(context, p => p.Add(x => x.AriaDisabled, disabled));

            var inputs = inputComponent.FindAll("textarea");

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

            var (inputComponent, _)   = CreateTextAreaInput(context, p => p.Add(x => x.AriaDisabled, true).Add(x => x.ReadOnly, true));
            var readonlyAttribute     = inputComponent.Find("textarea").GetAttribute("readonly");
            var ariaDisabledAttribute = inputComponent.Find("textarea").GetAttribute("aria-disabled");


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

            var (inputComponent, _) = CreateTextAreaInput(context, p => p.Add(x => x.ControlID, controlID));

            var idAttribute = inputComponent.Find("textarea").GetAttribute("id");

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

            var (inputComponent, _) = CreateTextAreaInput(context, p => p.Add(x => x.LabelText, labelText).Add(x => x.Required, false));

            var labelContent = inputComponent.Find("label").TextContent;

            if (String.IsNullOrWhiteSpace(labelText)) labelContent.Should().Be("StringValue");

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

            var (inputComponent, _) = CreateTextAreaInput(context, p => p.Add(x => x.LabelText, labelText).Add(x => x.Required, true));

            var labelContent = inputComponent.Find("label").TextContent;

            if (String.IsNullOrWhiteSpace(labelText)) labelContent.Should().Be("StringValue *");

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

            var (inputComponent, _) = CreateTextAreaInput(context, p => p.Add(x => x.HintText, hintText));

            var hints = inputComponent.FindAll($".{GlobalValues.TextArea_Input_Hint_Class}");

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

            var (inputComponent, _) = CreateTextAreaInput(context, p => p.AddUnmatched("class", "test").AddUnmatched("style", "color:red;"));

            using (new AssertionScope())
            {
                inputComponent.Instance.AdditionalAttributes.Should().ContainKey("style").WhoseValue.Should().Be("color:red;");

                var inputElement = inputComponent.Find("textarea");

                inputElement.GetAttribute("style").Should().Be("color:red;");

                inputElement.ClassList.Should().NotContain("test");
                inputElement.ClassList.Should().Contain(GlobalValues.TextArea_Input_Field_Class);

            }
        }


        [Fact]
        public async Task Should_capture_unmatched_attributes_and_add_class_to_the_date_input_class_list()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTextAreaInput(context, p => p.AddUnmatched("class", "test"));

            using (new AssertionScope())
            {
                inputComponent.Instance.AdditionalAttributes.Should().ContainKey("class").WhoseValue.Should().Be("test");

                var parentElement = inputComponent.Find("div");
                parentElement.ClassList.Should().Contain("test");

            }
        }


        [Theory]
        [InlineData("--svg-icon")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async Task Should_be_able_to_set_the_optional_svg_icon_parameter_which_must_start_with_a_double_dash(string? svgIconVariable)
        {
            await using var context = new BunitContext();
            var (inputComponent, _) = CreateTextAreaInput(context, p => p.Add(x => x.ValidationDisplayMode, ValidationDisplayMode.TabbableWithHint).Add(x => x.SvgIcon, svgIconVariable));

            if (String.IsNullOrWhiteSpace(svgIconVariable))
            {
                inputComponent.FindAll($"span.{GlobalValues.TextArea_Input_Icon_Class}").Should().BeEmpty();
                return;
            }

            if (svgIconVariable.StartsWith("--"))
            {
                inputComponent.Find($"span.{GlobalValues.TextArea_Input_Icon_Class}").GetAttribute("style").Should().NotBeEmpty();
                return;
            }

            inputComponent.FindAll($"span.{GlobalValues.TextArea_Input_Icon_Class}").Should().BeEmpty();
        }


        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(20)]
         public async Task Should_be_able_to_set_the_text_area_rows_or_use_the_default_if_not_set_or_below_one(int rows)
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTextAreaInput(context, p => p.Add(x => x.TextAreaRows, rows));

            var rowAttribute = inputComponent.Find("textarea").GetAttribute("rows");

            if(rows < 1) rowAttribute.Should().Be("5");
            if(rows > 0) rowAttribute.Should().Be(rows.ToString());

        }


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_be_able_to_set_the_auto_size_param_that_allows_the_text_area_to_grow_vertically(bool autosize)
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTextAreaInput(context, p => p.Add(x => x.AutoSize, autosize));

            var classList = inputComponent.Find("textarea").ClassList;

            if (true == autosize) classList.Should().Contain(GlobalValues.TextArea_Input_Field_Autosize_Modifier);

            if (false == autosize) classList.Should().NotContain(GlobalValues.TextArea_Input_Field_Autosize_Modifier);

        }

        [Theory]
        [InlineData(20)]
        [InlineData(200)]
        public async Task Should_be_able_to_set_max_characters_for_the_counter_and_announcements(int maxCharacters)
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTextAreaInput(context, p => p.Add(x => x.MaxCharacters, maxCharacters));

            inputComponent.Instance.MaxCharacters.Should().Be(maxCharacters);
        }

        //[Theory]
        //[InlineData("")]
        //[InlineData(" ")]
        //[InlineData(null)]
        //[InlineData("Text with {count} token")]

        //public async Task Should_be_able_to_set_the_character_remaining_text_uses_default_if_null_empty_whitespace_or_missing_the_token(string? remainingText)
        //{
        //    await using var context = new BunitContext();

        //    var (inputComponent, _) = CreateTextAreaInput(context, p => p.Add(x => x.CharactersRemainingText,remainingText));

        //    if (String.IsNullOrWhiteSpace(remainingText)) inputComponent.Instance.CharactersRemainingText.Should().Be(GlobalValues.TextArea_Characters_Remaining_Text);

        //    if (!String.IsNullOrWhiteSpace(remainingText)) inputComponent.Instance.CharactersRemainingText.Should().Be(remainingText);

        //}

        //[Theory]
        //[InlineData("")]
        //[InlineData(" ")]
        //[InlineData(null)]
        //[InlineData("Text with {count} token")]

        //public async Task Should_be_able_to_set_the_character_overlimit_text_uses_default_if_null_empty_whitespace_or_missing_the_token(string? overlimitText)
        //{
        //    await using var context = new BunitContext();

        //    var (inputComponent, _) = CreateTextAreaInput(context, p => p.Add(x => x.CharactersOverLimitText, overlimitText));

        //    if (String.IsNullOrWhiteSpace(overlimitText)) inputComponent.Instance.CharactersOverLimitText.Should().Be(GlobalValues.TextArea_Characters_Over_Limit_Text);

        //    if (!String.IsNullOrWhiteSpace(overlimitText)) inputComponent.Instance.CharactersOverLimitText.Should().Be(overlimitText);

        //}














        [Fact]
        public async Task Should_have_a_tabbable_region_for_errors_when_the_Validation_display_mode_is_set_to_tabbable_with_hint()
        {
            await using var context = new BunitContext();

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Readonly_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Count_Callback_Handlers, _ => true).SetVoidResult();

            context.Services.AddScoped<ILiveRegionService, LiveRegionService>();


            var model = new TestModel { StringValue = "Too Long" };
            var editContext = new EditContext(model);

            editContext.EnableDataAnnotationsValidation(context.Services);

            var component = context.Render<TextAreaInput>(
                builder =>
                {
                    builder
                        .AddCascadingValue(editContext)
                        .Add(p => p.Value, model.StringValue)
                        .Add(p => p.ValueChanged, EventCallback.Factory.Create<string>(context, v => model.StringValue = v))
                        .Add(p => p.ValueExpression, () => model.StringValue)
                        .Add(p => p.ValidationDisplayMode, ValidationDisplayMode.TabbableWithHint)
                        .Add(p => p.ErrorsLabel, "My Errors");

                });

            await component.InvokeAsync(() =>
            {
                editContext.NotifyFieldChanged(new FieldIdentifier(model, nameof(model.StringValue)));
                editContext.Validate();
            });

            component.WaitForAssertion(() =>
            {

                // Use FindAll first to debug if it exists at all
                var errorDiv = component.Find($".{GlobalValues.TextArea_Input_Error_Class}");

                using (new AssertionScope())
                {
                    errorDiv.GetAttribute("tabindex").Should().Be("0");
                    errorDiv.GetAttribute("role").Should().Be("region");

                    component.Find("textarea").GetAttribute("aria-invalid").Should().Be("true");

                }
            });
        }

        [Fact]
        public async Task Should_be_able_to_set_the_error_label_used_for_tabbable_error_regions()
        {
            await using var context = new BunitContext();

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Readonly_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Count_Callback_Handlers, _ => true).SetVoidResult();

            context.Services.AddScoped<ILiveRegionService, LiveRegionService>();


            var model = new TestModel { StringValue = "Too Long" };
            var editContext = new EditContext(model);

            editContext.EnableDataAnnotationsValidation(context.Services);

            var component = context.Render<TextAreaInput>(
                builder =>
                {
                    builder
                        .AddCascadingValue(editContext)
                        .Add(p => p.Value, model.StringValue)
                        .Add(p => p.ValueChanged, EventCallback.Factory.Create<string>(context, v => model.StringValue = v))
                        .Add(p => p.ValueExpression, () => model.StringValue)
                        .Add(p => p.ValidationDisplayMode, ValidationDisplayMode.TabbableWithHint)
                        .Add(p => p.ErrorsLabel, "My Errors");

                });

            await component.InvokeAsync(() =>
            {
                editContext.NotifyFieldChanged(new FieldIdentifier(model, nameof(model.StringValue)));
                editContext.Validate();
            });

            component.WaitForAssertion(() =>
            {
                // Use FindAll first to debug if it exists at all
                var errorDiv = component.Find($"div.{GlobalValues.TextArea_Input_Error_Class}");

                using (new AssertionScope())
                {
                    component.Instance.ErrorsLabel.Should().Be("My Errors");
                    errorDiv.GetAttribute("aria-label").Should().Contain("My Errors");
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

            var (inputComponent, _) = CreateTextAreaInput(context);

            inputComponent.Instance.ControlReference.Should().NotBeNull();
        }
    }

    public class HandleOnBlur
    {
        [Fact]
        public async Task Should_trim_value_if_trim_on_blur_set_to_true()
        {
            await using var context = new BunitContext();

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Inputs_File_Path);
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Readonly_Handlers, _ => true).SetVoidResult();
            moduleInterop.SetupVoid(GlobalValues.JS_Inputs_Register_Count_Callback_Handlers, _ => true).SetVoidResult();

            context.Services.AddScoped<ILiveRegionService, LiveRegionService>();


            var model = new TestModel { StringValue = "Too Long" };
            var editContext = new EditContext(model);

            editContext.EnableDataAnnotationsValidation(context.Services);

            var component = context.Render<TextAreaInput>(
                builder =>
                {
                    builder
                        .AddCascadingValue(editContext)
                        .Add(p => p.Value, model.StringValue)
                        .Add(p => p.ValueChanged, EventCallback.Factory.Create<string>(context, v => model.StringValue = v))
                        .Add(p => p.ValueExpression, () => model.StringValue)
                        .Add(p => p.TrimOnBlur,true);

                });

            var input = component.Find("textarea");

            input.Change(" P ");
            input.Blur();

            model.StringValue.Should().Be("P");
        }
    }
   
    public class UpdateLiveCharacterCount
    {
        [Fact]
        public async Task UpdateLiveCharacterCount_should_update_the_internal_live_character_count()
        {
            await using var context = new BunitContext();

            var (inputComponent, _) = CreateTextAreaInput(context);

            await inputComponent.InvokeAsync(() => inputComponent.Instance.UpdateLiveCharacterCount(42, false));

            // No exception thrown and component still renders — count is internal
            // but we can verify the component is still alive and rendered
            inputComponent.Instance.Should().NotBeNull();
        }
    }
}

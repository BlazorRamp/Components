using BlazorRamp.Core.Services;
using BlazorRamp.DebounceFilter.Common.Constants;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;

using DebounceFilterComponent = BlazorRamp.DebounceFilter.Components.DebounceFilter;

namespace BlazorRamp.DebounceFilter.Tests.Unit.Components;

public class DebounceFilter_Tests
{
    public static IRenderedComponent<DebounceFilterComponent> CreateDebounceFilter(
    BunitContext context,
    Action<ComponentParameterCollectionBuilder<DebounceFilterComponent>>? parameters = null)
    {
        var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Debounce_Filter_File_Path);
        moduleInterop.SetupVoid(GlobalValues.JS_Register_Debounce_Filter_Handler, _ => true).SetVoidResult();
        moduleInterop.SetupVoid(GlobalValues.JS_Unregister_Debounce_Filter_Handler, _ => true).SetVoidResult();
        moduleInterop.SetupVoid(GlobalValues.JS_Clear_Debounce_Filter, _ => true).SetVoidResult();

        context.Services.AddScoped<ILiveRegionService, LiveRegionService>();

        var component = context.Render<DebounceFilterComponent>(
        builder =>
        {
            parameters?.Invoke(builder);
        });


        return component;
    }



    public class Parameters
    {
        [Theory]
        [InlineData(FilterDataPosition.End)]
        [InlineData(FilterDataPosition.Centre)]
        [InlineData(FilterDataPosition.Start)]
        public async Task Should_be_able_to_set_the_data_position(FilterDataPosition dataPosition)
        {
            await using var context = new BunitContext();

            var component = CreateDebounceFilter(context, p => p.Add(x => x.FilterDataPosition, dataPosition));

            var dataAttribute = component.Find("input").GetAttribute("data-br-input-position");

            dataAttribute.Should().Be(dataPosition.ToString().ToLower());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("MyControl")]
        public async Task Should_be_able_to_set_the_control_id_param_or_have_a_guid_string_if_null_empty_or_whitespace(string? controlID)
        {
            await using var context = new BunitContext();

            var component = CreateDebounceFilter(context, p => p.Add(x => x.ControlID, controlID));

            var idAttribute = component.Find("input").GetAttribute("id");

            if (String.IsNullOrWhiteSpace(controlID)) Guid.Parse(idAttribute!).Should().NotBeEmpty();

            if (!String.IsNullOrWhiteSpace(controlID)) idAttribute.Should().Be(controlID);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("MyControl")]
        public async Task Should_be_able_to_set_the_filter_label_text_which_defaults_to_the_text_filter_if_null_empty_or_whitespace(string? labelText)
        {
            await using var context = new BunitContext();

            var component = CreateDebounceFilter(context, p => p.Add(x => x.FilterLabelText, labelText));

            var labelContent = component.Find("label").TextContent;

            if (String.IsNullOrWhiteSpace(labelText)) labelContent.Should().Be(GlobalValues.Debounce_Filter_Label_Text);

            if (!String.IsNullOrWhiteSpace(labelText)) labelContent.Should().Be(labelText);
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

            var component = CreateDebounceFilter(context, p => p.Add(x => x.HintText, hintText));

            var hints = component.FindAll($".{GlobalValues.Debounce_Filter_Hint_Class}");

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

            var component = CreateDebounceFilter(context, p => p.AddUnmatched("class", "test").AddUnmatched("style", "color:red;"));

            using (new AssertionScope())
            {
                component.Instance.AdditionalAttributes.Should().ContainKey("style").WhoseValue.Should().Be("color:red;");

                var inputElement = component.Find("input");

                inputElement.GetAttribute("style").Should().Be("color:red;");

                inputElement.ClassList.Should().NotContain("test");
                inputElement.ClassList.Should().Contain(GlobalValues.Debounce_Filter_Field_Class);

            }
        }

        [Fact]
        public async Task Should_capture_unmatched_attributes_and_add_class_to_the_component_class_list()
        {
            await using var context = new BunitContext();

            var component = CreateDebounceFilter(context, p => p.AddUnmatched("class", "test"));

            using (new AssertionScope())
            {
                component.Instance.AdditionalAttributes.Should().ContainKey("class").WhoseValue.Should().Be("test");

                var parentElement = component.Find("div");
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

            var component = CreateDebounceFilter(context, p => p.Add(x => x.SvgIcon, svgIconVariable));

            if (String.IsNullOrWhiteSpace(svgIconVariable))
            {
                component.FindAll($"span.{GlobalValues.Debounce_Filter_Icon_Class}").Should().BeEmpty();
                return;
            }

            if (svgIconVariable.StartsWith("--"))
            {
                component.Find($"span.{GlobalValues.Debounce_Filter_Icon_Class}").GetAttribute("style").Should().NotBeEmpty();
                return;
            }

            component.FindAll($"span.{GlobalValues.Debounce_Filter_Icon_Class}").Should().BeEmpty();
        }




        [Theory]
        [InlineData(500)]
        [InlineData(250)]

        public async Task Should_be_able_to_set_the_debounce_delay_that_defaults_to_250_if_below_1(int delayMS)
        {
            await using var context = new BunitContext();

            var component = CreateDebounceFilter(context, p => p.Add(x => x.DebounceDelayMs, delayMS));

            var result = delayMS < 1 ? GlobalValues.Debounce_DelayMs : delayMS;

            component.Instance.DebounceDelayMs.Should().Be(delayMS);
        }




    }


    public class Properties()
    {
        [Fact]
        public async Task Should_be_able_to_get_the_control_reference_for_the_underlying_input()
        {
            await using var context = new BunitContext();

            var component = CreateDebounceFilter(context);

            component.Instance.ControlReference.Should().NotBeNull();
        }
    }


    public class Methods
    {
        [Fact]
        public async Task Clear_filter_should_clear_the_filter_and_state()
        {
            await using var context = new BunitContext();

            var component = CreateDebounceFilter(context);

            var input = component.Find("input");

        }
    }


}

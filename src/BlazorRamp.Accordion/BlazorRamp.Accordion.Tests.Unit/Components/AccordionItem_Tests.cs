using BlazorRamp.Accordion.Common.Constants;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;
using AccordionComponennt = BlazorRamp.Accordion.Components.Accordion;
using AccordItemComponennt = BlazorRamp.Accordion.Components.AccordionItem;


namespace BlazorRamp.Accordion.Tests.Unit.Components;

public class AccordionItem_Tests
{
    public static IRenderedComponent<AccordionComponennt> CreateAccordionWithParamByName<TValue>(BunitContext context, string paramName, TValue paramValue, ExpandMode expandMode = ExpandMode.Multiple)
    {
        var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Module_File_Path);
        moduleInterop.SetupVoid(GlobalValues.JS_Register_Handler_Func, _ => true).SetVoidResult();

        return context.Render<AccordionComponennt>(paramBuilder =>
        {
            paramBuilder.TryAdd<ExpandMode>(nameof(AccordionComponennt), expandMode);

            paramBuilder.AddChildContent<AccordItemComponennt>(compBuilder => compBuilder.Add(p => p.HeadingText, "Accordion One").TryAdd<TValue>(paramName,paramValue));
            paramBuilder.AddChildContent<AccordItemComponennt>(compBuilder => compBuilder.Add(p => p.HeadingText, "Accordion Two"));
            paramBuilder.AddChildContent<AccordItemComponennt>(compBuilder => compBuilder.Add(p => p.HeadingText, "Accordion Three"));
        });

    }
    public class Paramaters()
    {
        [Theory]
        [InlineData("heading one")]
        [InlineData("missing")]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData(null)]

        public async Task Should_be_able_to_set_the_header_text_param_with_an_exception_if_null_empty_whitespace_or_missing(string? headingText)
        {
            await using var context = new BunitContext();
            
            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Module_File_Path);
            moduleInterop.SetupVoid(GlobalValues.JS_Register_Handler_Func, _ => true).SetVoidResult();
            
            if (String.IsNullOrWhiteSpace(headingText))
            {
                FluentActions.Invoking(() =>
                {
                    context.Render<AccordionComponennt>(paramBuilder =>
                    {
                        paramBuilder.AddChildContent<AccordItemComponennt>(compBuilder => compBuilder.Add(p => p.HeadingText, headingText));
                    });
                }).Should().ThrowExactly<ArgumentNullException>();

                return;
            }

            if (headingText == "missing")
            {
                FluentActions.Invoking(() =>
                {
                    context.Render<AccordionComponennt>(paramBuilder =>
                    {
                        paramBuilder.AddChildContent<AccordItemComponennt>();
                    });
                }).Should().ThrowExactly<ArgumentNullException>();

                return;
            }

            var accComponent = context.Render<AccordionComponennt>(paramBuilder =>
            {
                paramBuilder.AddChildContent<AccordItemComponennt>(compBuilder => compBuilder.Add(p => p.HeadingText, headingText));
            });

            accComponent.Find($"span.{GlobalValues.Accordion_Trigger_Content_Class} > span:last-child").TextContent.Should().Be(headingText);
        }


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_be_able_to_set_a_tab_index_for_the_panel(bool panelHasTabIndex)
        {
            await using var context = new BunitContext();

            var accComponent = CreateAccordionWithParamByName<bool>(context, nameof(AccordItemComponennt.PanelHasTabIndex), panelHasTabIndex);

            var tabIndex = accComponent.Find($"div.{GlobalValues.Accordion_Panel_Class}").GetAttribute("tabindex");

            if (true == panelHasTabIndex)
            {
                tabIndex.Should().Be("0");
            }
            else
            {
                tabIndex.Should().BeNull();
            }
           
        }
    }
}

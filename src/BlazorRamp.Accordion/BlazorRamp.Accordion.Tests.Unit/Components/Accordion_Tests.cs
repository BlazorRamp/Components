using BlazorRamp.Accordion.Common.Constants;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using AccordionComponennt  = BlazorRamp.Accordion.Components.Accordion;
using AccordItemComponennt = BlazorRamp.Accordion.Components.AccordionItem;

namespace BlazorRamp.Accordion.Tests.Unit.Components;

public class Accordion_Tests
{
    public static IRenderedComponent<AccordionComponennt> CreateAccordionWithParamByName<TValue>(BunitContext context, string paramName, TValue paramValue)
    {
        var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Module_File_Path);
        moduleInterop.SetupVoid(GlobalValues.JS_Register_Handler_Func, _ => true).SetVoidResult();

        return context.Render<AccordionComponennt>(paramBuilder =>
        {
            paramBuilder.TryAdd<TValue>(paramName, paramValue);

            paramBuilder.AddChildContent<AccordItemComponennt>(compBuilder => compBuilder.Add(p => p.HeadingText, "Accordion One"));
            paramBuilder.AddChildContent<AccordItemComponennt>(compBuilder => compBuilder.Add(p => p.HeadingText, "Accordion Two"));
            paramBuilder.AddChildContent<AccordItemComponennt>(compBuilder => compBuilder.Add(p => p.HeadingText, "Accordion Three"));
        });

    }

    public class Parameters()
    {
        [Theory]
        [InlineData(ExpandMode.Multiple)]
        [InlineData(ExpandMode.Single)]
        public async Task Should_be_able_to_set_the_expand_mode(ExpandMode expandMode)
        {
            await using var context = new BunitContext();

            var accComponent = CreateAccordionWithParamByName<ExpandMode>(context, nameof(AccordionComponennt.ExpandMode), expandMode);

            accComponent.FindAll($"button.{GlobalValues.Accordion_Trigger_Class}")[0].Click();

            accComponent.WaitForAssertion(() =>
                accComponent.FindAll($"button.{GlobalValues.Accordion_Trigger_Class}")[0]
                    .GetAttribute("aria-expanded").Should().Be("true")
            );

            accComponent.FindAll($"button.{GlobalValues.Accordion_Trigger_Class}")[1].Click();

            if (expandMode == ExpandMode.Single)
            {
                accComponent.WaitForAssertion(() =>
                {
                    using (new AssertionScope())
                    {
                        var btns = accComponent.FindAll($"button.{GlobalValues.Accordion_Trigger_Class}");
                        accComponent.Instance.ExpandMode.Should().Be(expandMode);
                        accComponent.FindAll($"[data-br-panel-expanded=\"true\"]").Count.Should().Be(1);
                        btns[0].GetAttribute("aria-expanded").Should().Be("false");
                        btns[1].GetAttribute("aria-expanded").Should().Be("true");
                    }
                });
            }
            else
            {
                accComponent.WaitForAssertion(() =>
                {
                    using (new AssertionScope())
                    {
                        var btns = accComponent.FindAll($"button.{GlobalValues.Accordion_Trigger_Class}");
                        accComponent.Instance.ExpandMode.Should().Be(expandMode);
                        accComponent.FindAll($"[data-br-panel-expanded=\"true\"]").Count.Should().Be(2);
                        btns[0].GetAttribute("aria-expanded").Should().Be("true");
                        btns[1].GetAttribute("aria-expanded").Should().Be("true");
                    }
                });
            }
        }


    }

}

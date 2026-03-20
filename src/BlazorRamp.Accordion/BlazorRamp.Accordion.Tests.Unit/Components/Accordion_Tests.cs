using BlazorRamp.Accordion.Common.Constants;
using Bunit;
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
        }
    }

}

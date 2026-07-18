using BlazorRamp.Accordion.Common.Constants;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_be_able_to_set_persistence_to_either_keep_or_clear_panel_conent_from_the_dome(bool persistConent)
        {
            await using var context = new BunitContext();
            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Module_File_Path);

            moduleInterop.SetupVoid(GlobalValues.JS_Register_Handler_Func, _ => true).SetVoidResult();

            var accComponent = context.Render<AccordionComponennt>(paramBuilder =>
            {
                paramBuilder.Add(p => p.ExpandMode, ExpandMode.Multiple);
                paramBuilder.AddChildContent<AccordItemComponennt>(compBuilder =>
                {
                    compBuilder.Add(p => p.HeadingText, "Accordion One");
                    compBuilder.Add(p => p.PersistContent, persistConent);
                    compBuilder.Add(p => p.PanelContent, "<p>Panel One Content</p>");
                });
            });
            
            await accComponent.Instance.ExpandAllPanels();

            accComponent.WaitForAssertion(() =>
            {
                var button   = accComponent.Find($"button.{GlobalValues.Accordion_Trigger_Class}");
                var divPanel = accComponent.Find($"div.{GlobalValues.Accordion_Panel_Class}");

                using (new AssertionScope())
                {
                    button.GetAttribute("aria-expanded").Should().Be("true");
                    divPanel.InnerHtml.Should().NotBeEmpty();
                }
            });

            await accComponent.Instance.CollapseAllPanels();

            accComponent.WaitForAssertion(() =>
            {
                var button = accComponent.Find($"button.{GlobalValues.Accordion_Trigger_Class}");
                var divPanel = accComponent.Find($"div.{GlobalValues.Accordion_Panel_Class}");

                using (new AssertionScope())
                {
                    button.GetAttribute("aria-expanded").Should().Be("false");

                    if(true == persistConent)
                    {
                        divPanel.InnerHtml.Should().NotBeEmpty();
                        return;
                    }

                    divPanel.InnerHtml.Should().BeEmpty();
                }
            });

        }


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_be_able_to_set_the_panal_as_an_aria_region(bool panelIsRegion)
        {
            await using var context = new BunitContext();

            var accComponent = CreateAccordionWithParamByName<bool>(context, nameof(AccordItemComponennt.PanelIsRegion), panelIsRegion);

            var role = accComponent.Find($"div.{GlobalValues.Accordion_Panel_Class}").GetAttribute("role");

            if (true == panelIsRegion)
            {
                role.Should().Be("region");
                return;
            }

            role.Should().BeNull();
        }

        [Theory]
        [InlineData("--svg-icon")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async Task Should_be_able_to_set_the_optional_svg_icon_paramater_which_must_start_with_a_double_dash(string? svgIconVariable)
        {
            await using var context = new BunitContext();

            var accComponent = CreateAccordionWithParamByName(context, nameof(AccordItemComponennt.SvgIcon), svgIconVariable);

            if (String.IsNullOrWhiteSpace(svgIconVariable))
            {
                accComponent.FindAll($"span.{GlobalValues.Accordion_Heading_Icon_Class}").Should().BeEmpty();
                return;
            }

            if (svgIconVariable.StartsWith("--"))
            {
                accComponent.Find($"span.{GlobalValues.Accordion_Heading_Icon_Class}").GetAttribute("style").Should().NotBeEmpty();
                return;
            }

            accComponent.FindAll($"span.{GlobalValues.Accordion_Heading_Icon_Class}").Should().BeEmpty();
        }
    }
}

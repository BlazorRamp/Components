using BlazorRamp.Accordion.Common.Constants;
using BlazorRamp.Accordion.Common.Models;
using Bunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Components.Web;
using System.Runtime.CompilerServices;
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

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        public async Task Should_be_able_to_set_a_heading_level_h2_to_h6_outside_of_this_defaults_to_h3(int headingLevelValue)
        {
            await using var context = new BunitContext();

            var accComponent = CreateAccordionWithParamByName<HeadingLevel>(context, nameof(AccordionComponennt.HeadingLevel), (HeadingLevel)headingLevelValue);

            var hTag = (headingLevelValue < 2 || headingLevelValue > 6) ? "h3" : "h" + headingLevelValue;

            var tag = accComponent.Find($"div > {hTag}");

            tag.Should().NotBeNull();

        }

        [Fact]
        public async Task Should_capture_unmatched_attributed_and_apply_to_the_component()
        {
            await using var context = new BunitContext();

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Module_File_Path);
            moduleInterop.SetupVoid(GlobalValues.JS_Register_Handler_Func, _ => true).SetVoidResult();

            var accComponent = context.Render<AccordionComponennt>(parameters => parameters.AddUnmatched("style", "color:red;"));

            accComponent.Instance.AdditionalAttributes.Should().ContainKey("style").WhoseValue.Should().Be("color:red;");
        }
    }

    public class OnAccordionItemHeadingClicked()
    {
        [Fact]
        public async Task Should_be_invoked_with_correct_payload_when_heading_is_clicked()
        {
            await using var context = new BunitContext();

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Module_File_Path);
            moduleInterop.SetupVoid(GlobalValues.JS_Register_Handler_Func, _ => true).SetVoidResult();

            ItemHeadingData? receivedPayload = null;

            var accComponent = context.Render<AccordionComponennt>(paramBuilder =>
            {
                paramBuilder.Add(p => p.OnAccordionItemHeadingClicked, (ItemHeadingData data) => receivedPayload = data);
                paramBuilder.AddChildContent<AccordItemComponennt>(compBuilder => compBuilder.Add(p => p.HeadingText, "Accordion One"));
                paramBuilder.AddChildContent<AccordItemComponennt>(compBuilder => compBuilder.Add(p => p.HeadingText, "Accordion Two"));
                paramBuilder.AddChildContent<AccordItemComponennt>(compBuilder => compBuilder.Add(p => p.HeadingText, "Accordion Three"));
            });

            accComponent.FindAll($"button.{GlobalValues.Accordion_Trigger_Class}")[0].Click();

            accComponent.WaitForAssertion(() =>
            {
                receivedPayload.Should().Match<ItemHeadingData>(r => r.ItemIndex == 0 && r.HeadingText == "Accordion One" & r.IsExpanded == true);
            });
        }
    }

    public class ExpandAllPanels()
    {
        [Fact]
        public async Task Should_expand_all_panels_when_called()
        {
            await using var context = new BunitContext();

            var accComponent = CreateAccordionWithParamByName<ExpandMode>(context, nameof(AccordionComponennt.ExpandMode), ExpandMode.Multiple);

            var buttons = accComponent.FindAll($"button.{GlobalValues.Accordion_Trigger_Class}");

            using (new AssertionScope())
            {
                buttons[0].GetAttribute("aria-expanded").Should().Be("false");
                buttons[1].GetAttribute("aria-expanded").Should().Be("false");
                buttons[2].GetAttribute("aria-expanded").Should().Be("false");
            }
            
            await accComponent.Instance.ExpandAllPanels();

            accComponent.WaitForAssertion(() =>
            {
                var buttons = accComponent.FindAll($"button.{GlobalValues.Accordion_Trigger_Class}");

                using (new AssertionScope())
                {
                    buttons[0].GetAttribute("aria-expanded").Should().Be("true");
                    buttons[1].GetAttribute("aria-expanded").Should().Be("true");
                    buttons[2].GetAttribute("aria-expanded").Should().Be("true");
                }
            });
        }
    }
    public class CollapseAllPanels()
    {
        [Fact]
        public async Task Should_expand_all_panels_when_called()
        {
            await using var context = new BunitContext();

            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Module_File_Path);
            moduleInterop.SetupVoid(GlobalValues.JS_Register_Handler_Func, _ => true).SetVoidResult();

            var accComponent = context.Render<AccordionComponennt>(paramBuilder =>
            {
                paramBuilder.AddChildContent<AccordItemComponennt>(compBuilder => compBuilder.Add(p => p.HeadingText, "Accordion One").Add(p => p.Expanded, true));
                paramBuilder.AddChildContent<AccordItemComponennt>(compBuilder => compBuilder.Add(p => p.HeadingText, "Accordion Two").Add(p => p.Expanded, true));
                paramBuilder.AddChildContent<AccordItemComponennt>(compBuilder => compBuilder.Add(p => p.HeadingText, "Accordion Three").Add(p => p.Expanded, true));
            });

            var buttons = accComponent.FindAll($"button.{GlobalValues.Accordion_Trigger_Class}");

            using (new AssertionScope())
            {
                buttons[0].GetAttribute("aria-expanded").Should().Be("true");
                buttons[1].GetAttribute("aria-expanded").Should().Be("true");
                buttons[2].GetAttribute("aria-expanded").Should().Be("true");
            }

            await accComponent.Instance.CollapseAllPanels();

            accComponent.WaitForAssertion(() =>
            {
                var buttons = accComponent.FindAll($"button.{GlobalValues.Accordion_Trigger_Class}");

                using (new AssertionScope())
                {
                    buttons[0].GetAttribute("aria-expanded").Should().Be("false");
                    buttons[1].GetAttribute("aria-expanded").Should().Be("false");
                    buttons[2].GetAttribute("aria-expanded").Should().Be("false");
                }
            });
        }
    }

    public class ExpandPanel
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(99)]
        public async Task Should_do_nothing_when_index_is_out_of_range(int index)
        {
            await using var context = new BunitContext();

            var accComponent = Accordion_Tests.CreateAccordionWithParamByName<ExpandMode>(context, nameof(AccordionComponennt.ExpandMode), ExpandMode.Multiple);

            await accComponent.Instance.ExpandPanel(index);

            accComponent.WaitForAssertion(() =>
            {
                var buttons = accComponent.FindAll($"button.{GlobalValues.Accordion_Trigger_Class}");

                using (new AssertionScope())
                {
                    buttons[0].GetAttribute("aria-expanded").Should().Be("false");
                    buttons[1].GetAttribute("aria-expanded").Should().Be("false");
                    buttons[2].GetAttribute("aria-expanded").Should().Be("false");
                }
            });
        }

        [Fact]
        public async Task Should_expand_the_panel_at_the_given_index()
        {
            await using var context = new BunitContext();

            var accComponent = Accordion_Tests.CreateAccordionWithParamByName<ExpandMode>(context, nameof(AccordionComponennt.ExpandMode), ExpandMode.Multiple);

            await accComponent.Instance.ExpandPanel(1);

            accComponent.WaitForAssertion(() =>
            {
                var buttons = accComponent.FindAll($"button.{GlobalValues.Accordion_Trigger_Class}");

                using (new AssertionScope())
                {
                    buttons[0].GetAttribute("aria-expanded").Should().Be("false");
                    buttons[1].GetAttribute("aria-expanded").Should().Be("true");
                    buttons[2].GetAttribute("aria-expanded").Should().Be("false");
                }
            });
        }

        [Fact]
        public async Task Should_not_collapse_other_panels_when_expand_mode_is_multiple()
        {
            await using var context = new BunitContext();

            var accComponent = Accordion_Tests.CreateAccordionWithParamByName<ExpandMode>(context, nameof(AccordionComponennt.ExpandMode), ExpandMode.Multiple);

            await accComponent.Instance.ExpandPanel(0);
            await accComponent.Instance.ExpandPanel(1);

            accComponent.WaitForAssertion(() =>
            {
                var buttons = accComponent.FindAll($"button.{GlobalValues.Accordion_Trigger_Class}");

                using (new AssertionScope())
                {
                    buttons[0].GetAttribute("aria-expanded").Should().Be("true");
                    buttons[1].GetAttribute("aria-expanded").Should().Be("true");
                    buttons[2].GetAttribute("aria-expanded").Should().Be("false");
                }
            });
        }

        [Fact]
        public async Task Should_collapse_other_panels_when_expand_mode_is_single()
        {
            await using var context = new BunitContext();

            var accComponent = Accordion_Tests.CreateAccordionWithParamByName<ExpandMode>(context, nameof(AccordionComponennt.ExpandMode), ExpandMode.Single);

            await accComponent.Instance.ExpandPanel(0);

            accComponent.WaitForAssertion(() =>
                accComponent.FindAll($"button.{GlobalValues.Accordion_Trigger_Class}")[0]
                    .GetAttribute("aria-expanded").Should().Be("true")
            );

            await accComponent.Instance.ExpandPanel(1);

            accComponent.WaitForAssertion(() =>
            {
                var buttons = accComponent.FindAll($"button.{GlobalValues.Accordion_Trigger_Class}");

                using (new AssertionScope())
                {
                    buttons[0].GetAttribute("aria-expanded").Should().Be("false");
                    buttons[1].GetAttribute("aria-expanded").Should().Be("true");
                    buttons[2].GetAttribute("aria-expanded").Should().Be("false");
                }
            });
        }

        [Fact]
        public async Task Should_not_re_expand_a_panel_that_is_already_expanded()
        {
            await using var context = new BunitContext();

            var accComponent = Accordion_Tests.CreateAccordionWithParamByName<ExpandMode>(context, nameof(AccordionComponennt.ExpandMode), ExpandMode.Multiple);

            await accComponent.Instance.ExpandPanel(0);
            await accComponent.Instance.ExpandPanel(0);

            accComponent.WaitForAssertion(() =>
            {
                var buttons = accComponent.FindAll($"button.{GlobalValues.Accordion_Trigger_Class}");

                using (new AssertionScope())
                {
                    buttons[0].GetAttribute("aria-expanded").Should().Be("true");
                    buttons[1].GetAttribute("aria-expanded").Should().Be("false");
                    buttons[2].GetAttribute("aria-expanded").Should().Be("false");
                }
            });
        }


    }

    public class KeyboardNavigation
    {
        private static IRenderedComponent<AccordionComponennt> CreateAccordion(BunitContext context)
        {
            var moduleInterop = context.JSInterop.SetupModule(GlobalValues.JS_Module_File_Path);
            moduleInterop.SetupVoid(GlobalValues.JS_Register_Handler_Func, _ => true).SetVoidResult();

            return context.Render<AccordionComponennt>(paramBuilder =>
            {
                paramBuilder.Add(p => p.ExpandMode, ExpandMode.Multiple);
                paramBuilder.AddChildContent<AccordItemComponennt>(compBuilder => compBuilder.Add(p => p.HeadingText, "Accordion One"));
                paramBuilder.AddChildContent<AccordItemComponennt>(compBuilder => compBuilder.Add(p => p.HeadingText, "Accordion Two"));
                paramBuilder.AddChildContent<AccordItemComponennt>(compBuilder => compBuilder.Add(p => p.HeadingText, "Accordion Three"));
            });
        }

        private static void FocusButton(IRenderedComponent<AccordionComponennt> accComponent, int index)
        {
            accComponent.FindAll($"button.{GlobalValues.Accordion_Trigger_Class}")[index].TriggerEvent("onfocusin", new FocusEventArgs());
        }

        private static void PressKey(IRenderedComponent<AccordionComponennt> accComponent, string key)
        {
            accComponent.Find($"div.{GlobalValues.Accordion_Class}").KeyDown(new KeyboardEventArgs { Key = key });
        }

        private static void AssertFocusIndex(IRenderedComponent<AccordionComponennt> accComponent, int expectedIndex)
        {
            accComponent.WaitForAssertion(() =>
                accComponent.Instance._focusIndex.Should().Be(expectedIndex)
            );
        }

        [Fact]
        public async Task Arrow_down_should_move_focus_to_next_item()
        {
            await using var context = new BunitContext();
            var accComponent = CreateAccordion(context);

            FocusButton(accComponent, 0);
            PressKey(accComponent, GlobalValues.KeyBoard_Down_Arrow_Key);

            AssertFocusIndex(accComponent, 1);
        }

        [Fact]
        public async Task Arrow_up_should_move_focus_to_previous_item()
        {
            await using var context = new BunitContext();
            var accComponent = CreateAccordion(context);

            FocusButton(accComponent, 1);
            PressKey(accComponent, GlobalValues.KeyBoard_Up_Arrow_Key);

            AssertFocusIndex(accComponent, 0);
        }

        [Fact]
        public async Task Arrow_down_should_wrap_to_first_item_when_on_last()
        {
            await using var context = new BunitContext();
            var accComponent = CreateAccordion(context);

            FocusButton(accComponent, 2);
            PressKey(accComponent, GlobalValues.KeyBoard_Down_Arrow_Key);

            AssertFocusIndex(accComponent, 0);
        }

        [Fact]
        public async Task Arrow_up_should_wrap_to_last_item_when_on_first()
        {
            await using var context = new BunitContext();
            var accComponent = CreateAccordion(context);

            FocusButton(accComponent, 0);
            PressKey(accComponent, GlobalValues.KeyBoard_Up_Arrow_Key);

            AssertFocusIndex(accComponent, 2);
        }

        [Fact]
        public async Task Home_key_should_move_focus_to_first_item()
        {
            await using var context = new BunitContext();
            var accComponent = CreateAccordion(context);

            FocusButton(accComponent, 2);
            PressKey(accComponent, GlobalValues.KeyBoard_Home_Key);

            AssertFocusIndex(accComponent, 0);
        }

        [Fact]
        public async Task End_key_should_move_focus_to_last_item()
        {
            await using var context = new BunitContext();
            var accComponent = CreateAccordion(context);

            FocusButton(accComponent, 0);
            PressKey(accComponent, GlobalValues.KeyBoard_End_Key);

            AssertFocusIndex(accComponent, 2);
        }

        [Fact]
        public async Task Keys_should_not_fire_when_trigger_does_not_have_focus()
        {
            await using var context = new BunitContext();
            var accComponent = CreateAccordion(context);

            PressKey(accComponent, GlobalValues.KeyBoard_Down_Arrow_Key);

            AssertFocusIndex(accComponent, -1);
        }

        [Fact]
        public async Task Focus_out_should_clear_trigger_has_focus()
        {
            await using var context = new BunitContext();
            var accComponent = CreateAccordion(context);

            FocusButton(accComponent, 0);

            accComponent.FindAll($"button.{GlobalValues.Accordion_Trigger_Class}")[0].TriggerEvent("onfocusout", new FocusEventArgs());

            PressKey(accComponent, GlobalValues.KeyBoard_Down_Arrow_Key);

            AssertFocusIndex(accComponent, 0); // unchanged — guard blocked the key
        }
    }
}

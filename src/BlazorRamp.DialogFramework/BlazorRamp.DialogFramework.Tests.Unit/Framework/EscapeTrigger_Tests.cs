using BlazorRamp.DialogFramework.Framework;
using FluentAssertions;
using FluentAssertions.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.DialogFramework.Tests.Unit.Framework;

public class EscapeTrigger_Tests
{
    [Fact]
    public void Should_be_able_to_subscribe_to_the_escape_trigger()
    {
        var escapeTrigger = new EscapeTrigger();
        Func<Task> handler = () => Task.CompletedTask;

        bool hasHandlersBefore = escapeTrigger.HasHandlers;

        escapeTrigger.Subscribe(handler);

        bool hasHandlersAfter = escapeTrigger.HasHandlers;

        using (new AssertionScope())
        {
            hasHandlersBefore.Should().BeFalse();
            hasHandlersAfter.Should().BeTrue();
        }

    }
    [Fact]
    public void Should_be_able_to_unsubscribe_to_the_escape_trigger()
    {
        var escapeTrigger = new EscapeTrigger();
        
        Func<Task> handler = () => Task.CompletedTask;

        bool hasHandlersBefore = escapeTrigger.HasHandlers;

        escapeTrigger.Subscribe(handler);

        bool hasHandlersAfterAdd = escapeTrigger.HasHandlers;

        escapeTrigger.Unsubscribe(handler);

        bool hasHandlersAfterRemove = escapeTrigger.HasHandlers;

        using (new AssertionScope())
        {
            hasHandlersBefore.Should().BeFalse();
            hasHandlersAfterAdd.Should().BeTrue();
            hasHandlersAfterRemove.Should().BeFalse();
        }

    }
    [Fact]
    public async Task Raise_escape_key_pressed_Should_invoke_all_subscribed_handlers()
    {
        var escapeTrigger = new EscapeTrigger();
        int invokeCount = 0;

        Func<Task> handlerOne = () => { invokeCount++; return Task.CompletedTask; };
        Func<Task> handlerTwo = () => { invokeCount++; return Task.CompletedTask; };

        escapeTrigger.Subscribe(handlerOne);
        escapeTrigger.Subscribe(handlerTwo);

        await escapeTrigger.RaiseEscapeKeyPressed();

        invokeCount.Should().Be(2);
    }

    [Fact]
    public async Task Raise_escape_key_pressed_Should_squash_exceptions_from_handlers()
    {
        var escapeTrigger = new EscapeTrigger();
        Func<Task> badHandler = () => throw new InvalidOperationException("boom");

        escapeTrigger.Subscribe(badHandler);

        var raisedExceptions = await Record.ExceptionAsync(() => escapeTrigger.RaiseEscapeKeyPressed());

        raisedExceptions.Should().BeNull();
    }

}

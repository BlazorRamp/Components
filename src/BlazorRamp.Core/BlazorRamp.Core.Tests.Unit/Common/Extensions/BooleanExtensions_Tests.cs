using BlazorRamp.Core.Common.Extensions;
using FluentAssertions;

namespace BlazorRamp.Core.Tests.Unit.Common.Extensions;

public class BooleanExtensions_Tests
{

    public class WhenTrue
    {
        [Fact]
        public void Should_execute_the_action_when_true()
        {
            var actionExecuted = false;

            true.WhenTrue(() => actionExecuted = true);
            
            actionExecuted.Should().BeTrue();
        }
        [Fact]
        public void Should_not_execute_the_action_when_false()
        {
            var actionExecuted = false;

            false.WhenTrue(() => actionExecuted = true);

            actionExecuted.Should().BeFalse();
        }

        [Fact]
        public async Task Should_execute_the_task_action_when_true()
        {
            var actionExecuted = false;

            Func<Task> action = async () => { actionExecuted = true; await Task.CompletedTask; };

            await true.WhenTrue(action);

            actionExecuted.Should().BeTrue();
        }
        [Fact]
        public async Task Should_not_execute_the_task_action_when_false()
        {
            var actionExecuted = false;
            
            Func<Task> action = async () => { actionExecuted = true; await Task.CompletedTask; };

            await false.WhenTrue(action);

            actionExecuted.Should().BeFalse();
        }
        [Fact]
        public async Task Should_execute_the_value_task_action_when_true()
        {
            var actionExecuted = false;

            Func<ValueTask> action = async () => { actionExecuted = true; await ValueTask.CompletedTask; };

            await true.WhenTrue(action);

            actionExecuted.Should().BeTrue();
        }
        [Fact]
        public async Task Should_not_execute_the_value_task_action_when_false()
        {
            var actionExecuted = false;

            Func<Task> action = async () => { actionExecuted = true; await ValueTask.CompletedTask; };

            await false.WhenTrue(action);

            actionExecuted.Should().BeFalse();
        }
    }
    public class WhenFalse
    {
        [Fact]
        public void Should_execute_the_action_when_false()
        {
            var actionExecuted = false;

            false.WhenFalse(() => actionExecuted = true);

            actionExecuted.Should().BeTrue();
        }
        [Fact]
        public void Should_not_execute_the_action_when_true()
        {
            var actionExecuted = false;

            true.WhenFalse(() => actionExecuted = true);

            actionExecuted.Should().BeFalse();
        }

        [Fact]
        public async Task Should_execute_the_task_action_when_false()
        {
            var actionExecuted = false;

            Func<Task> action = async () => { actionExecuted = true; await Task.CompletedTask; };

            await false.WhenFalse(action);

            actionExecuted.Should().BeTrue();
        }
        [Fact]
        public async Task Should_not_execute_the_task_action_when_true()
        {
            var actionExecuted = false;

            Func<Task> action = async () => { actionExecuted = true; await Task.CompletedTask; };

            await true.WhenFalse(action);

            actionExecuted.Should().BeFalse();
        }
        [Fact]
        public async Task Should_execute_the_value_task_action_when_false()
        {
            var actionExecuted = false;

            Func<ValueTask> action = async () => { actionExecuted = true; await ValueTask.CompletedTask; };

            await false.WhenFalse(action);

            actionExecuted.Should().BeTrue();
        }
        [Fact]
        public async Task Should_not_execute_the_value_task_action_when_true()
        {
            var actionExecuted = false;

            Func<ValueTask> action = async () => { actionExecuted = true; await ValueTask.CompletedTask; };

            await true.WhenFalse(action);

            actionExecuted.Should().BeFalse();
        }

    }
    public class WhenTrueElse
    {
        [Fact]
        public void Should_execute_the_true_action_when_true()
        {
            var actionExecuted = false;

            true.WhenTrueElse(() => actionExecuted = true, () => actionExecuted = false);

            actionExecuted.Should().BeTrue();
        }
        [Fact]
        public void Should_execute_the_else_action_when_false()
        {
            var actionExecuted = true;

            false.WhenTrueElse(() => actionExecuted = true, () => actionExecuted = false);

            actionExecuted.Should().BeFalse();
        }

        [Fact]
        public async Task Should_execute_the_true_task_action_when_true()
        {
            var actionExecuted = false;

            Func<Task> trueAction = async () => { actionExecuted = true; await Task.CompletedTask; };
            Func<Task> falseAction = async () => { actionExecuted = false; await Task.CompletedTask; };

            await true.WhenTrueElse(trueAction,falseAction);

            actionExecuted.Should().BeTrue();
        }
        [Fact]
        public async Task Should_execute_the_else_task_action_when_false()
        {
            var actionExecuted = true;

            Func<Task> trueAction = async () => { actionExecuted = true; await Task.CompletedTask; };
            Func<Task> falseAction = async () => { actionExecuted = false; await Task.CompletedTask; };

            await false.WhenTrueElse(trueAction, falseAction);

            actionExecuted.Should().BeFalse();
        }

        [Fact]
        public async Task Should_execute_the_true_value_task_action_when_true()
        {
            var actionExecuted = false;

            Func<ValueTask> trueAction = async () => { actionExecuted = true; await Task.CompletedTask; };
            Func<ValueTask> falseAction = async () => { actionExecuted = false; await Task.CompletedTask; };

            await true.WhenTrueElse(trueAction, falseAction);

            actionExecuted.Should().BeTrue();
        }
        [Fact]
        public async Task Should_execute_the_else_value_task_action_when_false()
        {
            var actionExecuted = true;

            Func<ValueTask> trueAction = async () => { actionExecuted = true; await ValueTask.CompletedTask; };
            Func<ValueTask> falseAction = async () => { actionExecuted = false; await ValueTask.CompletedTask; };

            await false.WhenTrueElse(trueAction, falseAction);

            actionExecuted.Should().BeFalse();
        }
    }
}

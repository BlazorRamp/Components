using BlazorRamp.DialogFramework.Framework;
using FluentAssertions;

namespace BlazorRamp.DialogFramework.Tests.Unit.Framework;

public class NoReturnValue_Tests
{

    [Fact]
    public void To_string_should_return_the_empty_set_character()
    {
        NoReturnValue noValue = NoReturnValue.Value;

        noValue.ToString().Should().Be("Ø");
    }

    [Fact]
    public void No_return_value_is_singleton()
    {
        NoReturnValue noValue = NoReturnValue.Value;
        var noValueTwo = noValue with { };

        noValue.Should().Be(noValueTwo);
    }
    

}

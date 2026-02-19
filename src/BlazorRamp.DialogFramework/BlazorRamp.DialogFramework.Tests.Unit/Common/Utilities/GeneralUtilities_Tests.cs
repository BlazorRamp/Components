using BlazorRamp.DialogFramework.Common.Utilities;
using BlazorRamp.DialogFramework.Tests.SharedDataFixtures.Common.Models;
using BlazorRamp.DialogFramework.Tests.SharedDataFixtures.Components;
using FluentAssertions;
using FluentAssertions.Execution;

namespace BlazorRamp.DialogFramework.Tests.Unit.Common.Utilities;

public class GeneralUtilities_Tests
{
    public class GetModalDialogParamType()
    {
        [Fact]
        public void Should_return_the_correct_type_for_the_paramter()
        {
            var (paramName, paramType) = GeneralUtilities.GetModalDialogParamType<SomeForm>(s => s.SomePersonData);

            using(new AssertionScope())
            {
                paramName.Should().Be(nameof(SomeForm.SomePersonData));

                paramType.Should().Be(typeof(SomePersonData));
            }
    
        }
        [Fact]
        public void Should_throw_an_exception_for_an_invalid_expression()
        {
            var (paramName, paramType) = GeneralUtilities.GetModalDialogParamType<SomeForm>(s => s.SomePersonData);

            FluentActions.Invoking(() => GeneralUtilities.GetModalDialogParamType<SomeForm>(s => "literal")).Should().Throw<ArgumentException>();
        }
    }

    public class ThrowIfNullEmptyOrWhitespace()
    {
        [Fact]
        public void _Should_return_value_when_valid_non_string()

            => GeneralUtilities.ThrowIfNullEmptyOrWhitespace(42).Should().Be(42);

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]

        public void Should_throw_an_exception_when_null_empty_or_whitespace(string? value)

            => FluentActions.Invoking(() => GeneralUtilities.ThrowIfNullEmptyOrWhitespace(value)).Should().Throw<ArgumentException>();
    }
}

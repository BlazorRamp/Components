using BlazorRamp.Core.Common.Constants;
using BlazorRamp.Core.Common.Utilities;
using FluentAssertions;

namespace BlazorRamp.Core.Tests.Unit.Common.Utilities;

public class CoreUtilities_Tests
{
    public class GetStyleAsValue 
    {
        [Theory]
        [InlineData(StyleAs.OnDark)]
        [InlineData(StyleAs.OnLight)]
        [InlineData(StyleAs.Dynamic)]
        public void Should_return_the_correct_string_value_from_the_style_as_enum(StyleAs styleAs)
        {
            switch (styleAs)
            {
                case StyleAs.OnDark:  CoreUtilities.GetStyleAsValue(styleAs).Should().Be(CoreGlobalValues.Style_As_Dark); break;
                case StyleAs.OnLight: CoreUtilities.GetStyleAsValue(styleAs).Should().Be(CoreGlobalValues.Style_As_Light); break;
                case StyleAs.Dynamic: CoreUtilities.GetStyleAsValue(styleAs).Should().BeNull(); break;
                default:              CoreUtilities.GetStyleAsValue(styleAs).Should().BeNull(); break;
            }

        }
    }
}

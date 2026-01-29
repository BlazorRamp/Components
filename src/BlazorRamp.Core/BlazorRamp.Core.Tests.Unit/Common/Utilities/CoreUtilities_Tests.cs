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

    public class CreateClassList
    {
        [Theory]
        [InlineData("  class-with-space  ", 1)]
        [InlineData("  ", 0)]
        [InlineData(null, 0)]
        [InlineData("one-class", 1)]
        [InlineData("one-class, two-class", 2)]
        [InlineData("one-class, two-class three-class", 3)]

        public void Should_get_a_space_separated_list_of_classes_or_return_null(string classes, int expectedClasses)
        {
            string[]? classList = null;
            
            if (classes is not null) classList = classes.Split(',');

            var result = CoreUtilities.CreateClassList(classList!);

            if (String.IsNullOrWhiteSpace(classes))
            {
                result.Should().BeNull();
                return;
            }
            
            result!.Split(' ').Count().Should().Be(expectedClasses);

        }
    }

}

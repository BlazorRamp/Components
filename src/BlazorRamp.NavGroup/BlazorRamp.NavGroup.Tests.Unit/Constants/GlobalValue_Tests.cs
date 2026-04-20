using BlazorRamp.NavGroup.Common.Constants;
using FluentAssertions;
using FluentAssertions.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.NavGroup.Tests.Unit.Constants;

public class GlobalValue_Tests
{
    public class CheckSetSvgVariable()
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("-a")]
        public void Should_return_null_if_the_value_is_null_empty_whitespace_or_does_not_start_with_a_double_dash(string? svgIcon)
            
            => GlobalValues.CheckSetSvgVariable(svgIcon).Should().BeNull();

        [Theory]
        [InlineData("--svgIcon")]
        [InlineData("--svgIcon:")]
        public void Should_return_the_internal_variable_with_the_entered_svg_varialbe_in_a_var_if_starts_with_a_double_dash_removing_any_errant_trailing_colon(string svgIcon)
        
            =>  GlobalValues.CheckSetSvgVariable(svgIcon).Should().Be($"{GlobalValues.Nav_Group_Svg_Css_Variable_Name}:var(--svgIcon);");
    }


    public class GetDepthStyleVariable
    {
        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public void Should_return_the_internal_variable_with_the_depth_value_provided(int depthValue)

            => GlobalValues.GetDepthStyleVariable(depthValue).Should().Be($"{GlobalValues.Nav_Group_Depth_CSS_Variable_Name}:{depthValue};");
        
    }
}

using BlazorRamp.CssClasses.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.CssClasses.Common.Utilities;


public static class Margin
{
    public static string All(UnitSpace space)

        => GenerateClass(space, "br-margin");

    public static string Inline(UnitSpace space)

        => GenerateClass(space, "br-margin-inline");

    public static string InlineStart(UnitSpace space)

        => GenerateClass(space, "br-margin-inline-start");

    public static string InlineEnd(UnitSpace space)

        => GenerateClass(space, "br-margin-inline-end");

    public static string Block(UnitSpace space)

        => GenerateClass(space, "br-margin-block");
    public static string BlockStart(UnitSpace space)

        => GenerateClass(space, "br-margin-block-start");
    public static string BlockEnd(UnitSpace space)

        => GenerateClass(space, "br-margin-block-end");

    private static string GenerateClass(UnitSpace spaceUnit, string className)

        => spaceUnit switch 
        { 
            UnitSpace.None  => $"{className}-0",
            UnitSpace.One   => $"{className}-1",
            UnitSpace.Two   => $"{className}-2",
            UnitSpace.Three => $"{className}-3",
            UnitSpace.Four  => $"{className}-4",
            UnitSpace.Five  => $"{className}-5",
            UnitSpace.Six   => $"{className}-6",
            UnitSpace.Seven => $"{className}-7",
            UnitSpace.Eight => $"{className}-8",
            UnitSpace.Nine  => $"{className}-9",
            _               => $"{className}-0"
        };

}

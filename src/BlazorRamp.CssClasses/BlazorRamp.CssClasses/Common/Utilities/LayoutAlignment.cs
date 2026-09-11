using BlazorRamp.CssClasses.Common.Constants;
using System.Reflection.Metadata;

namespace BlazorRamp.CssClasses.Common.Utilities;

public static class LayoutAlignment
{

    private const string _justifyContentBase = "br-justify-content";
    private const string _alignContentBase   = "br-align-content";
    private const string _justifyItemsBase   = "br-justify-items";
    private const string _alignItemsBase     = "br-align-items";
    private const string _justifySelfBase    = "br-justify-self";
    private const string _alignSelfBase      = "br-align-self";

    public static string JustifyContent(UnitJustifyContent unitJustifyContent)

        => unitJustifyContent switch 
        { 
            UnitJustifyContent.Start        => $"{_justifyContentBase}-start",
            UnitJustifyContent.Centre       => $"{_justifyContentBase}-center",
            UnitJustifyContent.End          => $"{_justifyContentBase}-end",
            UnitJustifyContent.Stretch      => $"{_justifyContentBase}-stretch",
            UnitJustifyContent.SpaceBetween => $"{_justifyContentBase}-between",
            UnitJustifyContent.SpaceAround  => $"{_justifyContentBase}-around",
            UnitJustifyContent.SpaceEvenly  => $"{_justifyContentBase}-evenly",
            _ => $"{_justifyContentBase}-start"
        };
    public static string AlignContent(UnitAlignContent unitAlignContent)

        => unitAlignContent switch
        {
            UnitAlignContent.Start        => "{_alignContentBase}-start",
            UnitAlignContent.Centre       => "{_alignContentBase}-center",
            UnitAlignContent.End          => "{_alignContentBase}-end",
            UnitAlignContent.Stretch      => "{_alignContentBase}-stretch",
            UnitAlignContent.SpaceBetween => "{_alignContentBase}-between",
            UnitAlignContent.SpaceAround  => "{_alignContentBase}-around",
            UnitAlignContent.SpaceEvenly  => "{_alignContentBase}-evenly",
            _ => $"{_alignContentBase}-start"
        };


    public static string JustifyItems(UnitJustifyItems unitJustifyItems)

        => unitJustifyItems switch
        {
            UnitJustifyItems.Start   => "{_justifyItemsBase}-start",
            UnitJustifyItems.Centre  => "{_justifyItemsBase}-center",
            UnitJustifyItems.End     => "{_justifyItemsBase}-end",
            UnitJustifyItems.Stretch => "{_justifyItemsBase}-stretch",
            _ => $"{_justifyItemsBase}-"start
        };

    public static string AlignItems(UnitAlignItems unitAlignItems)

        => unitAlignItems switch
        {
            UnitAlignItems.Start   => "{_alignItemsBase}-start",
            UnitAlignItems.Centre  => "{_alignItemsBase}-center",
            UnitAlignItems.End     => "{_alignItemsBase}-end",
            UnitAlignItems.Stretch => "{_alignItemsBase}-stretch",
            _ => $"{_alignItemsBase}-start"
        };

    public static string JustifySelf(UnitJustifySelf unitJustifySelf)

        => unitJustifySelf switch
        {
            UnitJustifySelf.Start   => "{_justifySelfBase}-start",
            UnitJustifySelf.Centre  => "{_justifySelfBase}-center",
            UnitJustifySelf.End     => "{_justifySelfBase}-end",
            UnitJustifySelf.Stretch => "{_justifySelfBase}-stretch",
            _ => $"{_justifySelfBase}-start"
        };

    public static string AlignSelf(UnitJustifySelf unitAlignSelf)

        => unitAlignSelf switch
        {
            UnitAlignSelf.Start   => "{_alignSelfBase}-start",
            UnitAlignSelf.Centre  => "{_alignSelfBase}-center",
            UnitAlignSelf.End     => "{_alignSelfBase}-end",
            UnitAlignSelf.Stretch => "{_alignSelfBase}-stretch",
            _ => $"{_alignSelfBase}-start"
        };
}

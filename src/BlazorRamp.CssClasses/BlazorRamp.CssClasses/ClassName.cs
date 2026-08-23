using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.CssClasses;

public class ClassName
{
    public const string Section = $"br-section";
    public const string Section_Container = $"{Section}--container";
    public const string Section_Dialog     = $"{Section}--dialog";
    public const string Section_Bordered   = $"{Section}--bordered";
    public const string Section_Padding_Of = $"{Section}--no-padding";

    public const string Section_Neutral_10        = $"{Section}--neutral-10";
    public const string Section_Neutral_20        = $"{Section}--neutral-20";
    public const string Section_Secondary_Lighter = $"{Section}--secondary-lighter";
    public const string Section_Accent_Lighter    = $"{Section}--accent-lighter";
    public const string Section_Code_Font         = $"{Section}--code-font";
    public const string Section_Accent_Text       = $"{Section}--accent-text";
    public const string Section_Secondary_Text    = $"{Section}--secondary-text";
}

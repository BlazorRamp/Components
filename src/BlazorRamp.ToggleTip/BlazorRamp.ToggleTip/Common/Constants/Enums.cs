using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.ToggleTip.Common.Constants;

public enum ToggleTipPosition: int
{
    TopCentre    = 0,
    TopLeft      = 1,
    TopRight     = 2,
    CentreLeft   = 3,
    CentreRight  = 4,
    BottomCentre = 5,
    BottomLeft   = 6,
    BottomRight  = 7,
}
public enum ToggleTipLabelOrder: int
{
    LabelFirst = 0,
    IconFirst = 1
}
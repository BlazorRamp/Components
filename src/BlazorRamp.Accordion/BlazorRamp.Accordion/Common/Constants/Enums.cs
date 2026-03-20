using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.Accordion.Common.Constants;

public enum HeadingLevel : int
{
   H2 = 2, 
   H3 = 3, 
   H4 = 4, 
   H5 = 5, 
   H6 = 6,
}

public enum ExpandMode : int 
{ 
    Single   = 1, 
    Multiple = 2, 
}

internal enum Direction : int { Up, Down }
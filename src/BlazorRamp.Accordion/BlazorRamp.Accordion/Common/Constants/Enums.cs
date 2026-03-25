using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.Accordion.Common.Constants;

/// <summary>
/// Specifies the semantic heading level rendered for each accordion item trigger.
/// </summary>
public enum HeadingLevel : int
{
    /// <summary>
    /// Renders the trigger inside an <c>&lt;h2&gt;</c> element.
    /// </summary>
    H2 = 2,
    /// <summary>
    /// Renders the trigger inside an <c>&lt;h3&gt;</c> element.
    /// </summary>
    H3 = 3,
    /// <summary>
    /// Renders the trigger inside an <c>&lt;h4&gt;</c> element.
    /// </summary>
    H4 = 4,
    /// <summary>
    /// Renders the trigger inside an <c>&lt;h5&gt;</c> element.
    /// </summary>
    H5 = 5,
    /// <summary>
    /// Renders the trigger inside an <c>&lt;h6&gt;</c> element.
    /// </summary>
    H6 = 6,
}

/// <summary>
/// Specifies whether one or multiple accordion panels can be open simultaneously.
/// </summary>
public enum ExpandMode : int
{
    /// <summary>
    /// Only one panel can be open at a time. Opening a panel collapses all others.
    /// </summary>
    Single = 1,
    /// <summary>
    /// Any number of panels can be open at the same time.
    /// </summary>
    Multiple = 2,
}

/// <summary>
/// Specifies the direction of keyboard arrow key navigation within the accordion.
/// </summary>
internal enum Direction : int
{
    /// <summary>
    /// Navigate to the previous accordion item.
    /// </summary>
    Up = 0,
    /// <summary>
    /// Navigate to the next accordion item.
    /// </summary>
    Down = 1
}
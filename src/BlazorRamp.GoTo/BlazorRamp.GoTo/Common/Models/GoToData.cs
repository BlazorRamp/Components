namespace BlazorRamp.GoTo.Common.Models;

/// <summary>
/// Describes one marker found on the page.
/// </summary>
/// <param name="MarkerID">The id of the marker element, which receives focus.</param>
/// <param name="PopoverID">The id of the popover that shows the marker's label.</param>
/// <param name="GoToText">The label text.</param>
/// <param name="GoToNumber">The two-digit number shown beside the label, such as <c>01</c>.</param>
public record GoToData(string MarkerID, string PopoverID, string GoToText, string GoToNumber);

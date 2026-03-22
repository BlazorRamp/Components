namespace BlazorRamp.Accordion.Common.Models;

/// <summary>
/// Represents the payload delivered by the <c>OnAccordionItemHeadingClicked</c> callback
/// when an accordion item trigger is activated.
/// </summary>
/// <param name="ItemIndex">The zero-based index of the accordion item that was clicked.</param>
/// <param name="HeadingText">The heading text of the accordion item that was clicked.</param>
/// <param name="IsExpanded">
/// <see langword="true"/> if the panel is expanded after the click; 
/// <see langword="false"/> if it was collapsed.
/// </param>
public record ItemHeadingData(int ItemIndex, string HeadingText, bool IsExpanded);


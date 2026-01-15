using BlazorRamp.Core.Common.Constants;
using System.Text.Json.Serialization;

namespace BlazorRamp.Core.Common.Models;

/// <summary>
/// Represents a single accessibility announcement to be processed by the live region service and stored in history.
/// </summary>
/// <param name="Message">The text content to be spoken by the screen reader.</param>
/// <param name="AnnouncementType">The semantic classification of the message (e.g., Info, Success, Error).</param>
/// <param name="AnnouncementTrigger">Optional text describing what action or component triggered this announcement.</param>
/// <param name="LiveRegionType">The ARIA priority level, determining if the screen reader interrupts current speech.</param>
public record Announcement(
    [property: JsonPropertyName("message")] string Message,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    [property: JsonPropertyName("announcementType")] AnnouncementType AnnouncementType = AnnouncementType.Info,
    [property: JsonPropertyName("announcementTrigger")] string? AnnouncementTrigger = "",
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    [property: JsonPropertyName("liveRegionType")] LiveRegionType LiveRegionType = LiveRegionType.Polite
);
using BlazorRamp.Core.Common.Constants;
using System.Text.Json.Serialization;

namespace BlazorRamp.Core.Common.Models;

//public record Announcement([property: JsonPropertyName("message")] string Message, 
//                           [property: JsonPropertyName("announcementType")] string AnnouncementType,
//                           [property: JsonPropertyName("announcementTrigger")] string? AnnouncementTrigger = "",
//                           [property: JsonPropertyName("liveRegionType")] string LiveRegionType = "Polite");





public record Announcement(
    [property: JsonPropertyName("message")] string Message,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    [property: JsonPropertyName("announcementType")] AnnouncementType AnnouncementType,
    [property: JsonPropertyName("announcementTrigger")] string? AnnouncementTrigger = "",
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    [property: JsonPropertyName("liveRegionType")] LiveRegionType LiveRegionType = LiveRegionType.Polite
);
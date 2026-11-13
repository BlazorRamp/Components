using BlazorRamp.Core.Common.Constants;
using BlazorRamp.Notifications.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorRamp.Notifications.Common.Models;

public sealed record NotificationItem
{
    [JsonPropertyName("id")]
    public string ID { get; init; } = Guid.NewGuid().ToString();

    [JsonPropertyName("title")]
    public required string Title { get; init; }

    [JsonPropertyName("message")]
    public string Message { get; init; } = "";

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonPropertyName("category")]
    public NotificationType Category { get; init; } = NotificationType.Info;

    [JsonPropertyName("actionUrl")]
    public string? ActionUrl { get; init; }

    [JsonPropertyName("actionText")]
    public string? ActionText { get; init; }

    [JsonPropertyName("createdAt")]
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;

    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; init; } = false;
}

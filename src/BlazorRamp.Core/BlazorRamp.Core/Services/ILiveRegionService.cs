using BlazorRamp.Core.Common.Constants;
using BlazorRamp.Core.Common.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorRamp.Core.Services;

/// <summary>
/// Defines a service for sending accessible announcements to ARIA live regions.
/// </summary>
public interface ILiveRegionService
{
    /// <summary>
    /// Dispatches an announcement to be spoken by a screen reader.
    /// </summary>
    /// <param name="announcement">The <see cref="Announcement"/> object containing the message and priority details.</param>
    /// <param name="replayable">Determines if the announcement is added to the announcment history dialog, defauts to <c>true</c></param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task MakeAnnouncement(Announcement announcement, bool replayable = true);

    /// <summary>
    /// Dispatches an announcement to be spoken by a screen reader.
    /// </summary>
    /// <param name="announcement">The <see cref="Announcement"/> object containing the message and priority details.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task MakeAnnouncement(Announcement announcement);
}
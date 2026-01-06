using BlazorRamp.Core.Common.Constants;
using BlazorRamp.Core.Common.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorRamp.Core.Services;
public interface ILiveRegionService
{
    Task MakeAnnouncement(Announcement announcement);
}
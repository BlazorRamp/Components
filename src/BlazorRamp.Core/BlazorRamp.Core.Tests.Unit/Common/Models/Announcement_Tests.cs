using BlazorRamp.Core.Common.Constants;
using BlazorRamp.Core.Common.Models;
using FluentAssertions;

namespace BlazorRamp.Core.Tests.Unit.Common.Models;

public class Announcement_Tests
{
    [Fact]
    public void Constructor_should_set_all_properties()
    {
        var announcement = new Announcement("Message", AnnouncementType.OperationStarted,  "AnnouncementTrigger", LiveRegionType.Assertive);

        announcement.Should().Match<Announcement>(a => a.Message == nameof(announcement.Message) && a.AnnouncementType == AnnouncementType.OperationStarted 
                                             && a.AnnouncementTrigger == nameof(announcement.AnnouncementTrigger) && a.LiveRegionType == LiveRegionType.Assertive);
    }

    [Fact]
    public void Should_be_able_to_set_all_properties_using_with()
    {
        var announcement = new Announcement("Message", AnnouncementType.OperationStarted, "AnnouncementTrigger", LiveRegionType.Assertive);

        var clone = announcement with {Message = announcement.Message, AnnouncementTrigger = announcement.AnnouncementTrigger, 
                                       AnnouncementType = announcement.AnnouncementType, LiveRegionType = announcement.LiveRegionType};
 
        clone.Should().Match<Announcement>(a => a.Message == announcement.Message && a.AnnouncementType == announcement.AnnouncementType
                                     && a.AnnouncementTrigger == announcement.AnnouncementTrigger && a.LiveRegionType == announcement.LiveRegionType);
    }
}

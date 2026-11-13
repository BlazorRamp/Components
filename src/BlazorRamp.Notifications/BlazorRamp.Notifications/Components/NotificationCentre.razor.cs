using BlazorRamp.Notifications.Services;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;

namespace BlazorRamp.Notifications.Components;

public partial class NotificationCentre
{

    [Inject] private INotificationService NotificationService { get; set; }


    protected override Task OnInitializedAsync()
    {
        return base.OnInitializedAsync();
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(true == firstRender)
        {
            await ((NotificationService)NotificationService).RegisterService();
        }

    }
}

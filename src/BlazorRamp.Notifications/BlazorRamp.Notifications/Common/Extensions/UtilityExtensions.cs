namespace BlazorRamp.Notifications.Common.Extensions;

internal static class UtilityExtensions
{

    public static async void AwaitTask(this Task @this, Action<Exception> exceptionHander)
    {
        try
        {
            await @this;
        }
        catch (Exception ex) { exceptionHander(ex); }
    }
}

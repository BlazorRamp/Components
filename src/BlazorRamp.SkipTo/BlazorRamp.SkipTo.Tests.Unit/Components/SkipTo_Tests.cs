using BlazorRamp.SkipTo.Components;
using Bunit;

using SkipToComponent = BlazorRamp.SkipTo.Components.SkipTo;

namespace BlazorRamp.SkipTo.Tests.Unit.Components;


public class SkipTo_Tests
{

    private static IRenderedComponent<SkipToComponent> CreateBusyIndicatorWithoutParams(BunitContext context)
    {
        //context.Services.AddScoped<ILiveRegionService, LiveRegionService>();
        //context.JSInterop.SetupModule(GlobalValues.JS_File_Path);

        //return context.Render<SkipTo>();
        return null;
    }
    //private static IRenderedComponent<BusyIndicatorComponent> CreateBusyIndicatorWithParamByName<TValue>(BunitContext context, string paramName, TValue paramValue, bool showIndicator = false)
    //{
    //    context.Services.AddScoped<ILiveRegionService, LiveRegionService>();
    //    context.JSInterop.SetupModule(GlobalValues.JS_File_Path);

    //    return context.Render<BusyIndicatorComponent>(paramBuilder => paramBuilder.Add(p => p.ShowIndicator, showIndicator)
    //                                                  .TryAdd(paramName, paramValue));
    //}
    public class OnParametersSet
    {

    }
}

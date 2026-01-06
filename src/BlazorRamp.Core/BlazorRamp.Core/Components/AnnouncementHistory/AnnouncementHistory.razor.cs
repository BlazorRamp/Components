using BlazorRamp.Core.Common.Constants;
using BlazorRamp.Core.Common.Models;
using BlazorRamp.Core.Common.Utilities;
using BlazorRamp.Core.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Globalization;
using System.Text.Json;

namespace BlazorRamp.Core.Components.AnnouncementHistory;

public partial class AnnouncementHistory
{
    [Parameter] public string Title          { get; set; } = CoreGlobalValues.AH_Text_For_Heading;
    [Parameter] public string CloseText      { get; set; } = CoreGlobalValues.AH_Text_For_Close_Btn;
    [Parameter] public string ClearCloseText { get; set; } = CoreGlobalValues.AH_Text_For_Clear_Btn;
    [Parameter] public string RefreshText    { get; set; } = CoreGlobalValues.AH_Text_For_Refresh_Btn;
    [Parameter] public string NoDataText     { get; set; } = CoreGlobalValues.AH_Text_No_Content;
    [Parameter] public string TriggerText    { get; set; } = CoreGlobalValues.AH_Text_For_Trigger_Btn;
    [Parameter] public bool   TriggerVisible { get; set; } = true;
    [Inject] private ILiveRegionService  LiveRegionService  { get; set; } = default!;//just forces service to load.

    private string  _historyTitle      = String.Empty;
    private string  _closeButtonText   = String.Empty;
    private string  _clearButtonText   = String.Empty;
    private string  _refreshButtonText = String.Empty;
    private string  _triggerButtonText = String.Empty;
    private string  _noDataText        = String.Empty;
    private string  _locale            = String.Empty;
    private string? _triggerClasses    = null;
    protected override void OnInitialized()
    {
        _locale            = CultureInfo.CurrentUICulture.Name; ;
        _historyTitle      = String.IsNullOrWhiteSpace(Title)          ? CoreGlobalValues.AH_Text_For_Heading     : Title.Trim();
        _closeButtonText   = String.IsNullOrWhiteSpace(CloseText)      ? CoreGlobalValues.AH_Text_For_Close_Btn   : CloseText.Trim();
        _clearButtonText   = String.IsNullOrWhiteSpace(ClearCloseText) ? CoreGlobalValues.AH_Text_For_Clear_Btn   : ClearCloseText.Trim();
        _refreshButtonText = String.IsNullOrWhiteSpace(RefreshText)    ? CoreGlobalValues.AH_Text_For_Refresh_Btn : RefreshText.Trim();
        _triggerButtonText = String.IsNullOrWhiteSpace(TriggerText)    ? CoreGlobalValues.AH_Text_For_Trigger_Btn : TriggerText.Trim();
        _noDataText        = String.IsNullOrWhiteSpace(NoDataText)     ? CoreGlobalValues.AH_Text_No_Content      : NoDataText.Trim();

        _triggerClasses    = TriggerVisible ? CoreGlobalValues.AH_Trigger_Class : CoreUtilities.CreateClassList(CoreGlobalValues.AH_Trigger_Class, CoreGlobalValues.AH_Trigger_Modifier);

    }
    
}

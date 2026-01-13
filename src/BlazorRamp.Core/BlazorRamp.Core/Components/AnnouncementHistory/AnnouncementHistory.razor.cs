using BlazorRamp.Core.Common.Constants;
using BlazorRamp.Core.Common.Utilities;
using BlazorRamp.Core.Services;
using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace BlazorRamp.Core.Components.AnnouncementHistory;

/// <summary>
/// Interaction logic for the AnnouncementHistory component, which displays a chronological list 
/// of past accessibility announcements and manages the history dialog state.
/// </summary>
public partial class AnnouncementHistory
{
    /// <summary>Gets or sets the h2 heading text displayed for announcement history dialog as well as being used for the dialogs aria-labelledby attribute.
    /// A default value of "Recent Announcements" is used if the parameter value is not specified.
    /// </summary>
    [Parameter] public string Title          { get; set; } = CoreGlobalValues.AH_Text_For_Heading;

    /// <summary>Gets or sets the text for the button that closes the history dialog.
    /// A default value of "Close" is used if the parameter value is not specified.
    /// </summary>
    [Parameter] public string CloseText      { get; set; } = CoreGlobalValues.AH_Text_For_Close_Btn;

    /// <summary>Gets or sets the text for the button that clears the announcement history logs and closes the dialog.
    /// A default value of "Clear & Close" is used if the parameter value is not specified.
    /// </summary>
    [Parameter] public string ClearCloseText { get; set; } = CoreGlobalValues.AH_Text_For_Clear_Btn;

    /// <summary>Gets or sets the text for the button that refreshes the history list that is displayed in the dialog.
    /// A default value of "Refresh" is used if the parameter value is not specified.
    /// </summary>
    [Parameter] public string RefreshText    { get; set; } = CoreGlobalValues.AH_Text_For_Refresh_Btn;

    /// <summary>Gets or sets the text displayed when there are no announcements in the history.
    /// A default value of "No Announcements" is used if the parameter value is not specified.
    /// </summary>
    [Parameter] public string NoDataText     { get; set; } = CoreGlobalValues.AH_Text_No_Content;

    /// <summary>Gets or sets the text for the trigger button that opens the history dialog.
    /// A default value of "Alerts" is used if the parameter value is not specified.
    /// </summary>
    [Parameter] public string TriggerText    { get; set; } = CoreGlobalValues.AH_Text_For_Trigger_Btn;

    /// <summary>Gets or sets a value indicating whether the trigger button is visible or only when its focused via the keyboard.
    /// The default value is true, always visible.    /// </summary>
    [Parameter] public bool   TriggerVisible { get; set; } = true;

    /// <summary>Injected service to manage and dispatch live region announcements.</summary>
    [Inject] private ILiveRegionService  LiveRegionService  { get; set; } = default!;//just forces service to load.

    private string  _historyTitle      = String.Empty;
    private string  _closeButtonText   = String.Empty;
    private string  _clearButtonText   = String.Empty;
    private string  _refreshButtonText = String.Empty;
    private string  _triggerButtonText = String.Empty;
    private string  _noDataText        = String.Empty;
    private string  _locale            = String.Empty;
    private string? _triggerClasses    = null;


    /// <summary>
    /// Initializes the component by setting up localized strings and determining the visibility classes 
    /// for the trigger button based on the provided parameters.
    /// </summary>
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

namespace BlazorRamp.DocSite.Common.Constants;

public class CodeSnippets
{
    public const string Add_Core_Style_Sheet = """
        <head>
            <link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
        </head>
        """;
    public const string Add_Busy_Indicator_Style_Sheet = """
        <head>
            <link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
            <link rel="stylesheet" href="_content/BlazorRamp.BusyIndicator/assets/css/busy-indicator.min.css" />
        </head>
        """;
    public const string Add_Skip_To_Style_Sheet = """
        <head>
            <link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
            <link rel="stylesheet" href="_content/BlazorRamp.SkipTo/assets/css/skip-to.min.css" />
        </head>
        """;
    public const string Add_Dialog_Framework_Style_Sheet = """
        <head>
            <link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
            <link rel="stylesheet" href="_content/BlazorRamp.DialogFramework/assets/css/dialog-framework.min.css" />
        </head>
        """;
    public const string Add_Switch_Style_Sheet = """
        <head>
            <link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
            <link rel="stylesheet" href="_content/BlazorRamp.Switch/assets/css/switch.min.css" />
        </head>
        """;

    public const string Add_Core_Package = "dotnet add package BlazorRamp.Core";

    public const string Add_Core_Script = """
        <script src="_framework/blazor.web.js"></script>
        <script type="module" src="_content/BlazorRamp.Core/assets/js/core-live-region.js"></script>
        """;

    public const string Add_Live_Region_Service = """
        @using BlazorRamp.Core.Common.Extensions;

        builder.Services.AddBlazorRampCore();
        """;
    public const string Add_Dialog_Framework_Service = """
        @using BlazorRamp.DialogFramew.Common.Extensions;

        builder.Services.AddBlazorRampDialogService();
        """;

    public const string Add_Announcement_History_Component = """
        <AnnouncementHistory RefreshText="Refresh" ClearCloseText="Clear & Close" CloseText="Close" 
        NoDataText="No announcements" Title="Recent Announcements" TriggerVisible="true" TriggerText="Alerts" />

        <Router AppAssembly . . .
        """;

    public const string Make_Announcement = """
        var announcement = new Announcement("The site is now using a dark coloured theme.", AnnouncementType.Info,  "Dark Theme Switch", LiveRegionType.Polite);

        await _liveRegionService.MakeAnnouncement(announcement);
        """;

    public const string Add_Configure_Announcement_History_Component = """
        <AnnouncementHistory Title="Recent Announcements" CloseText="Close" ClearCloseText="Clear & Close" 
         RefreshText="Refresh" TriggerText="Alerts" TriggerVisible="false" />
        /*
            * Which is the same as
        */
        <AnnouncementHistory />
        """;


    public const string Dark_Theme_Setting = """
        :root:has(#theme-toggler[aria-checked="true"]) {
         --br-comp-switch-thumb-Hover-colour:                var(--br-unit-colour-primary-20);
         --br-comp-switch-thumb-text:                        var(--br-unit-colour-primary-30);
         --br-comp-all-thumb-colour:                         var(--br-unit-colour-neutral-80);
         --br-comp-switch-on-track-colour:                   var(--br-unit-colour-primary-30);
         --br-comp-all-track-colour:                         var(--br-unit-colour-neutral-70);
         --br-comp-history-trigger-pane-surface-background:  var(--br-unit-colour-neutral-5);
         --br-comp-history-trigger-pane-surface-text:        var(--br-unit-colour-neutral-90);
         --br-comp-all-pane-base-background:                 var(--br-unit-colour-neutral-80);
         --br-comp-all-focus-indicator-colour:               var(--br-unit-colour-primary-30);
         --br-comp-all-pane-surface-background:              var(--br-unit-colour-neutral-70);
         --br-comp-all-area-header-background:               var(--br-unit-colour-neutral-70);
         --br-comp-all-area-content-text:                    var(--br-unit-colour-neutral-5);
         --br-unit-colour-canvas:                            var(--br-unit-colour-neutral-90);
         --br-unit-colour-canvas-text:                       var(--br-unit-colour-neutral-5);
         --br-comp-all-button-text:                          var(--br-unit-colour-primary-text-light);
         --br-comp-all-area-header-text:                     var(--br-unit-colour-primary-10);
         --br-comp-all-button-state-hover:                   hsl(from var(--br-unit-colour-primary) h s l / var(--br-unit-opacity-val-30));
         --br-comp-all-link-current-page-background-colour:  var(--br-unit-colour-secondary-darker);
         --br-comp-all-link-hover-background-colour:         hsl(from var(--br-unit-colour-secondary) h s l / 0.15);
         --br-comp-all-link-active-background-colour:        hsl(from var(--br-unit-colour-secondary) h s l / 0.3);
         --br-comp-all-link-focused-background-colour:       var(--br-unit-colour-secondary-darker);
         --br-info-box-alternate-heading-text:               var(--br-unit-colour-accent-light);
         --br-kbd-background-colour:                         var(--br-unit-colour-info-darker);
         --br-unit-colour-canvas-inverted:                   var(--br-unit-colour-neutral-5);
         --br-unit-colour-canvas-text-inverted:              var(--br-unit-colour-neutral-90);
         --br-comp-all-button-background:                    var(--br-unit-colour-primary-70);
         --br-comp-all-button-text:                          var(--br-unit-colour-primary-text-light);
         --br-comp-all-button-border-colour:                 var(--br-unit-colour-primary-30);
         --br-code-block-background-colour:                  var(--br-unit-colour-secondary-darker);
         --br-code-block-text:                               var(--br-unit-colour-info-lighter);
        }
        """;


    public const string Busy_Indicator_Example = """
        <BusyIndicator AriaStartText="Saving, please wait" AriaEndText=@_endMessage ShowIndicator="@_showIndicator" 
                OverlayPosition="OverlayPosition.Screen" BusyText=". . . Saving . . . ."  ContentPosition="ContentPosition.Top" 
                EndStatus="@_announcementType" DisplayTimeoutMS="15_000" OnBusyCompleted="HandleOnBusyCompleted" />

        public async Task SaveCustomer(CustomerData customerData)
        {
            _showIndicator = true;

            try
            {
                await _customerService.UpdateCustomer(customerData);
                _announcementType = AnnouncementType.OperationCompleted;
                _endMessage = "Saved Successfully";

            }
            catch(Exception ex)
            {
                /*
                    * Perhaps show some dialog or set a summary panel with the details etc
                 */

                _announcementType = AnnouncementType.SystemWarning;
                _endMessage = "You data was not saved, please review the details in the summary panel.";
            }
            finally
            {
                _showIndicator = false;
            }
        }
        """;


    public const string Busy_Indicator_Content = """
        <BusyIndicator>

        <!-- Your content in here -->

        </BusyIndicator>

        """;

    public const string Skip_To_Params_Example = """
        <SkipTo IconVisible="true" SkipToText="Skip to content" 
            SkipToType="SkipToType.Site" TargetID="app__main" />
        """;

    public const string Skip_To_Section_Example = """
        <SkipTo IconVisible="true" SkipToText="Skip to section content" 
            SkipToType="SkipToType.Section" TargetID="section-one" />
        """;


    public const string Switch_Two_Way_Bind_Example = """
        <Switch @bind-SwitchState="@_switchState" Label="Airplane mode:" AriaDisabled="@_switchDisabled" SpaceBetween="false" />

        @code {

            private bool _switchState    = false;
            private bool _switchDisabled = false;
        }
        """;

    public const string Switch_One_Way_Bind_Event_Example = """
        <Switch SwitchState="@_switchState" Label="Airplane mode:" AriaDisabled="@_switchDisabled" SpaceBetween="false" 
        SwitchStateChanged="HandleSwitchChange" />

        @code {

            private bool _switchState    = false;
            private bool _switchDisabled = false;

            private void HandleSwitchChange(bool switchState)
            {
                _switchState = switchState;
            }
        }
        """;


    public const string Modal_Dialog_Starting = """
        @inject ModalDialogService _dialogService

        <button @ref="ButtonRef" class="test-button" @onclick="ShowDialog">Show the Popup Form</button>


        @if (_outputVisible)
        {
            <p style="margin-top:2rem;">
                <b>The result: </b> @_outputMessage
            </p>
        }


        @code {

            private ElementReference ButtonRef { get; set; }

            private bool _outputVisible = false;

            private string _outputMessage = String.Empty;
            private string _location = String.Empty;

            public async Task ShowDialog()
            {
                SomePersonData somePersonData = new("John","Doe", 42,"United Kingdom");

                var dialogOptions     = new ModalDialogOptions(HorizontalAlignment.Centre, VerticalAlignment.Top);
                var dialogParameters  = new ModalDialogParameters<SomeForm>();

                dialogParameters.Add(x => x.SomePersonData, somePersonData);//The parameter/type and the data

                var dialogResult = await _dialogService.ShowDialog<SomeForm>(dialogParameters, dialogOptions);

                if (dialogResult.ButtonClicked == DialogResultButtons.Ok)
                {
                    _outputMessage = "The data returned was:" + dialogResult.Data.ToString();
                }
                else
                {
                    _outputMessage = "The operation was cancelled.";
                }

                await ButtonRef.FocusAsync();
            }
        }
        
        """;


    public const string Modal_Dialog_Some_Form = """
        @inject ModalDialogService _dialogService
        @implements IDisposable

        <div class="demo-dialog">
            <h2 class="demo-dialog__header" id="@_dialogTitleID">Some Form</h2>
            <div class="demo-dialog__content">
                <p>Your form fields in here.</p>
                <p>The object passed in: @(SomePersonData is null ? "[Null]" : SomePersonData)</p>
                <p>
                    <label for="checkboxSave">Check to dirty the form (our fake edit):</label>
                    <input id="checkboxSave" type="checkbox" @bind="_isDirty" />
                </p>
                <p>The confirmation dialog is set to open bottom right</p>
            </div>

            <div class="demo-dialog__footer">
                <button @ref="ButtonCancelRef" class="demo-dialog__button" @onclick="async () => await HandleCancelRequest(_isDirty, _dialogService, ButtonCancelRef)" type="button">
                    Cancel
                </button>
                <button class="demo-dialog__button" @onclick="async () => await HandleSaveChanges()" aria-disabled="@((!_isDirty).ToString().ToLower())" type="button">
                    Save Changes
                </button>
            </div>
        </div>
        @code {

            [Parameter] public SomePersonData SomePersonData { get; set; } = default!;

            private ElementReference ButtonCancelRef { get; set; }
            private string _dialogTitleID = Guid.Empty.ToString();
            private bool   _isDirty       = false;

            private string _dialogMessage = "<p>You pressed the escape or cancel key to close the dialog with unsaved changes.</p><p>Do you wish to continue and lose these changes?</p>";
            protected override void OnInitialized()
            {
                _dialogTitleID = _dialogService is null ? _dialogTitleID : _dialogService.GetAriaLabelledByID();
                _dialogService?.RegisterEscapeHandler(HandleEscapeKey);
            }

            private async Task HandleEscapeKey()

                => await HandleCancelRequest(_isDirty, _dialogService, ButtonCancelRef);

            public async Task HandleSaveChanges()
            {
                if (_isDirty == false) return;
                var returnData = new SomePersonView(SomePersonData.FirstName + " " + SomePersonData.Surname, "Jane " + SomePersonData.Surname);

                await _dialogService.CloseDialog(ModalDialogResult.OK(returnData));
            }

            public async Task HandleCancelRequest(bool isDirty, ModalDialogService dialogService, ElementReference? buttonCancelRef)
            {
                if (false == isDirty)
                {
                    await dialogService.CloseDialog(ModalDialogResult.Cancel());
                    return;
                }

                var dialogParameters = new ModalDialogParameters<ConfirmationDialog>();
                var dialogOptions   = new ModalDialogOptions(HorizontalAlignment.Centre, VerticalAlignment.Centre, maxWidthPercent: 50);

                dialogParameters.Add<string>(s => s.DialogMessage, _dialogMessage);//Param name/type and its data;

                var closeDialog = await dialogService.ShowDialog<ConfirmationDialog>(dialogParameters, dialogOptions);

                if (closeDialog.ButtonClicked == DialogResultButtons.Ok)
                {
                    await dialogService.CloseDialog(ModalDialogResult.Cancel());
                    return;
                }

                if (buttonCancelRef.HasValue) await buttonCancelRef.Value.FocusAsync();
            }


            public void Dispose()

                => _dialogService?.UnregisterEscapeHandler(HandleEscapeKey);
        }
        
        """;

    public const string Modal_Dialog_Confirmation_Form = """
        @inject ModalDialogService _dialogService
        @implements IDisposable

        <div class="demo-dialog">
            <h3 class="demo-dialog__header" id="@_dialogTitleID">@_title</h3>

            <div class="demo-dialog__content">
                @((MarkupString)_dialogMessage)
            </div>

            <div class="demo-dialog__footer">
                <button class="demo-dialog__button" @onclick="async () => await HandleCloseDialog(true)">@_buttonCancelText</button>
                <button class="demo-dialog__button" @onclick="async () => await HandleCloseDialog(false)">@_buttonOKText</button>
            </div>

        </div>
        @code {

            [Parameter] public string DialogTitle   { get; set; } = default!;
            [Parameter] public string DialogMessage { get; set; } = default!; 
            [Parameter] public string ButtonYesText { get; set; } = default!;
            [Parameter] public string ButtonNoText  { get; set; } = default!;

            private string _title            = "Your confirmation is required.";
            private string _buttonOKText     = "Yes, I would like to continue";
            private string _buttonCancelText = "No, do not continue";
            private string _dialogTitleID    = Guid.Empty.ToString();
            private string _dialogMessage    = "In order to continue with the selected action you must provide confirmation.";

            protected override void OnInitialized()
            {
                _title            = String.IsNullOrWhiteSpace(DialogTitle)   ? _title : DialogTitle.Trim();
                _buttonOKText     = String.IsNullOrWhiteSpace(ButtonYesText) ? _buttonOKText : ButtonYesText.Trim();
                _buttonCancelText = String.IsNullOrWhiteSpace(ButtonNoText)  ? _buttonCancelText : ButtonNoText.Trim();
                _dialogMessage    = String.IsNullOrWhiteSpace(DialogMessage) ? _dialogMessage : DialogMessage.Trim();

                _dialogTitleID       = _dialogService is null ? _dialogTitleID : _dialogService.GetAriaLabelledByID();

                _dialogService?.RegisterEscapeHandler(HandleEscapeKey);
            }

            private async Task HandleEscapeKey()

                => await HandleCloseDialog(true);

            public async Task HandleCloseDialog(bool cancelled = false)
            {
                if (false == cancelled)
                {
                    await _dialogService.CloseDialog(ModalDialogResult.OK()); 
                    return;
                }

                await _dialogService.CloseDialog(ModalDialogResult.Cancel());
            }

            public void Dispose()

                => _dialogService?.UnregisterEscapeHandler(HandleEscapeKey);

        }
        
        """;
}

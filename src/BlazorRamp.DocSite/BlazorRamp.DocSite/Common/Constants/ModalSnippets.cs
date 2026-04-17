namespace BlazorRamp.DocSite.Common.Constants;

public class ModalSnippets
{


    public const string Add_Dialog_Framework_Style_Sheet = """
        <head>
            <link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
            <link rel="stylesheet" href="_content/BlazorRamp.DialogFramework/assets/css/dialog-framework.min.css" />
        </head>
        """;




    public const string Add_Dialog_Framework_Service = """
        @using BlazorRamp.DialogFramew.Common.Extensions;

        builder.Services.AddBlazorRampDialogService();
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

            private bool   _outputVisible = false;
            private string _outputMessage = String.Empty;


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
